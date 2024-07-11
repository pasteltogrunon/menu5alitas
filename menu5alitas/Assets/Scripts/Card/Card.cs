using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card : MonoBehaviour
{
    public ResourceCounterList cost = new ResourceCounterList(ResourceCounterType.Cost);
    public Sprite sprite;

    bool isDragged = false;

    Vector3 originPosition;

    public abstract void tryPlayCard(Tile tile);

    //Se utiliza el update si está siendo drageado
    private void Update()
    {
        if(isDragged)
        {
            transform.position = TileMap.mapPosition + Vector3.back;
        }
    }


    //Gestionan los hovers

    public void startHover()
    {

    }

    public void endHover()
    {

    }

    //Gestionan el drag and drop aqui en la clase de carta como a ti te gusta guarro

    public void startDrag()
    {
        isDragged = true;
        originPosition = transform.position;
    }

    public void endDrag()
    {
        isDragged = false;

        if(TileMap.Instance.SelectedTile != null)
        {
            tryPlayCard(TileMap.Instance.SelectedTile);
        }
        else
        {
            transform.position = originPosition;
        }
    }
}
