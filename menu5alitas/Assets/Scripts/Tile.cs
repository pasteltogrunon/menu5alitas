using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2Int uvCoords;

    public Building building;

    public int energyLevel = 0;

    [SerializeField] Material lockedMat;
    [SerializeField] Material unlockedMat;

    float hoverPhase = 0;

    private bool _unlocked = false;
    public bool Unlocked
    {
        get => _unlocked;
        set
        {
            if (value)
            {
                unlock();
            }
            else
            {
                relock();
            }

            _unlocked = value;
        }
    }


    //Se guardan sus coordenadas y se le cambia el nombre
    public void Initialize(int uCoord, int vCoord)
    {
        uvCoords = new Vector2Int(uCoord, vCoord);
        gameObject.name = "Tile " + "(" + uCoord + ", " + vCoord + ")";
    }


    private void Update()
    {
        if(hoverPhase > 0 && hoverPhase < 0.75f)
        {
            hoverPhase = Mathf.Clamp01(hoverPhase - Time.deltaTime);
            GetComponent<Renderer>().material.SetFloat("_HoverPhase", hoverPhase);
        }
    }

    //Gestionan el hover con el ratón

    public void startHover()
    {
        if (Unlocked)
        {
            hoverPhase = 1;
            GetComponent<Renderer>().material.SetFloat("_HoverPhase", hoverPhase);
        }
    }

    public void endHover()
    {
        if (Unlocked)
        {
            hoverPhase = 0.5f;
        }
    }

    void unlock()
    {
        GetComponent<SpriteRenderer>().material = unlockedMat;
    }

    void relock()
    {
        GetComponent<SpriteRenderer>().material = lockedMat;
    }

    //Returns 0 if cannot place, else returns the price reduction factor
    public float canPlace(Building newBuilding)
    {
        if (!Unlocked)
            return 0;

        //Can be placed if no building is present
        if (building == null)
            return 1;

        //Can be placed if the building is the same
        if (newBuilding.id == building.id)
            return (float) (newBuilding.tier - building.tier) / newBuilding.tier;

        return 0;
    }

    public ResourceCounterList getProduction()
    {
        if (building == null)
            return new ResourceCounterList(ResourceCounterType.Production);
        
        return building.getProduction();
    }

    public ResourceCounterList getCost()
    {
        if (building == null)
            return new ResourceCounterList(ResourceCounterType.Cost);

        return building.getCost();
    }

    public int getHappinessProduction()
    {
        if (building == null)
            return 0;

        return building.happinessPerTurn;
    }
}
