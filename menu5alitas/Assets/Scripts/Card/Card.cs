using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public abstract class Card : MonoBehaviour
{
    public ResourceCounterList cost = new ResourceCounterList(ResourceCounterType.Cost);
    public Sprite sprite;

    bool isDragged = false;

    Vector3 originPosition;

    public abstract bool tryPlayCard(Tile tile);

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

    public void Select(Vector3 handPosition)
    {
        originPosition = handPosition;
    }

    public void endDrag()
    {
        isDragged = false;

        if(TileMap.Instance.SelectedTile != null)
        {
            if(!tryPlayCard(TileMap.Instance.SelectedTile))
            {
                if (transform)
                    transform.position = originPosition;
            }
        }
        else
        {
            if (transform)
                transform.position = originPosition;
        }
    }

    protected void DeleteFromHandManager()
    {
        var handManagerParent = transform.parent.GetComponent<HandManager>();
        handManagerParent.DeleteCard(this);
    }
}

