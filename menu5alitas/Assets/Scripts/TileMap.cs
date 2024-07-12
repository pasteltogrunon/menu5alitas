using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour
{
    public static TileMap Instance;

    [SerializeField] int size = 10;
    [SerializeField] int initialRadius = 2;
    [SerializeField] GameObject tilePrefab;
    [SerializeField] LayerMask planeLayer;

    //Diccionario de las coordenadas con el Tile, para que el 0, 0 pueda ser el centro
    Dictionary<Vector2Int, Tile> tiles;

    //Base hexagonal
    static readonly Vector2 u = new Vector2(1, 0);
    static readonly Vector2 v = new Vector2(0.5f, Mathf.Sqrt(3) * 0.5f);

    //Base canónica en hexagonal
    static readonly Vector2 x = new Vector2(1, 0);
    static readonly Vector2 y = new Vector2(-Mathf.Sqrt(3)/3, Mathf.Sqrt(3) * 2/3);

    //Posicion en el mapa, expuesto para la colocación de la carta
    public static Vector3 mapPosition = Vector3.zero;

    //Tile seleccionado con su getter y setter
    private Tile _selectedTile;
    public Tile SelectedTile
    {
        get => _selectedTile;
        set
        {
            if(value != _selectedTile)
            {
                _selectedTile?.endHover();
                _selectedTile = value;
                _selectedTile?.startHover();
            }
        }
    }

    Transform tr;

    //Antes que cualquier Start
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        tr = transform;

        GenerateMap();

        UnlockInitialTiles();
    }

    void Update()
    {
        hoverMap();
    }

    //Obtiene el Tile en coordenadas hexagonales
    public Tile GetTile(int uCoord, int vCoord)
    {
        if (tiles.ContainsKey(new Vector2Int(uCoord, vCoord)))
            return tiles[new Vector2Int(uCoord, vCoord)];
        return null;
    }

    //Obtiene el Tile mas cercano al punto en el plano especificado
    public Tile GetClosestTile(Vector2 planePosition)
    {
        Vector2 uvPosition = worldSpaceToUv(planePosition);

        int uCoord = Mathf.RoundToInt(uvPosition.x);
        int vCoord = Mathf.RoundToInt(uvPosition.y);

        return GetTile(uCoord, vCoord);
    }

    //Inicializa el diccionario y genera el mapa con esta forma hexagonal
    void GenerateMap()
    {
        tiles = new Dictionary<Vector2Int, Tile>();

        for (int i = -size; i < 0; i++)
        {
            for (int j = -size + Mathf.Abs(i); j <= size; j++)
            {
                CreateTile(i, j);
            }
        }

        for (int i = 0; i <= size; i++)
        {
            for (int j = -size; j <= size - Mathf.Abs(i); j++)
            {
                CreateTile(i, j);
            }
        }
        
    }

    //Crea el Tile y se hacen las gestiones necesarias, como ponerlo en el diccionario e inicializarlo
    void CreateTile(int uCoord, int vCoord)
    {
        tiles.Add(new Vector2Int(uCoord, vCoord), Instantiate(tilePrefab, uvToWorldSpace(uCoord, vCoord), Quaternion.identity, tr).GetComponent<Tile>());
        tiles[new Vector2Int(uCoord, vCoord)].Initialize(uCoord, vCoord);
    }

    void UnlockInitialTiles()
    {
        for (int i = -initialRadius; i < 0; i++)
        {
            for (int j = -initialRadius + Mathf.Abs(i); j <= initialRadius; j++)
            {
                Tile neighbor = GetTile(i, j);
                if (neighbor)
                    neighbor.Unlocked = true;
            }
        }

        for (int i = 0; i <= initialRadius; i++)
        {
            for (int j = -initialRadius; j <= initialRadius - Mathf.Abs(i); j++)
            {
                Tile neighbor = GetTile(i, j);
                if (neighbor)
                    neighbor.Unlocked = true;
            }
        }
    }

    //Chequea en que punto del mapa hace contacto el mouse (lanzando un raycast)
    void hoverMap()
    {
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo, 100, planeLayer);
        Tile closestTile = GetClosestTile(new Vector2(hitInfo.point.x, hitInfo.point.y));
        mapPosition = hitInfo.point;
        SelectedTile = closestTile;
    }

    public void UpdateMap()
    {
        ResourceCounterList production = new ResourceCounterList(ResourceCounterType.Production);
        ResourceCounterList cost = new ResourceCounterList(ResourceCounterType.Cost);

        int happinessProduction = 0;

        for (int i = -size; i < 0; i++)
        {
            for (int j = -size + Mathf.Abs(i); j <= size; j++)
            {
                Tile tile = GetTile(i, j);
                production += tile.getProduction();
                cost += tile.getCost();
                happinessProduction += tile.getHappinessProduction();
            }
        }

        for (int i = 0; i <= size; i++)
        {
            for (int j = -size; j <= size - Mathf.Abs(i); j++)
            {
                Tile tile = GetTile(i, j);
                production += tile.getProduction();
                cost += tile.getCost();
                happinessProduction += tile.getHappinessProduction();
            }
        }

        ResourceManager.Instance.updateResourceCounters(production, cost);

        ResourceManager.Instance.HappinessPerTurn = happinessProduction;
    }

    /* TRANSFORMACIONES de la base hexagonal a la canónica y viceversa, con Vector2 y con coordenadas sueltas */

    public static Vector2 uvToWorldSpace(float uCoord, float vCoord)
    {
        return uCoord * u + vCoord * v;
    }

    public static Vector2 uvToWorldSpace(Vector2 uvCoords)
    {
        return uvToWorldSpace(uvCoords.x, uvCoords.y);
    }

    public static Vector2 worldSpaceToUv(float xCoord, float yCoord)
    {
        return xCoord * x + yCoord * y;
    }

    public static Vector2 worldSpaceToUv(Vector2 xyCoords)
    {
        return worldSpaceToUv(xyCoords.x, xyCoords.y);
    }
}
