using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2Int uvCoords;

    public Building building;

    //Se guardan sus coordenadas y se le cambia el nombre
    public void Initialize(int uCoord, int vCoord)
    {
        uvCoords = new Vector2Int(uCoord, vCoord);
        gameObject.name = "Tile " + "(" + uCoord + ", " + vCoord + ")"; 
    }


    //Gestionan el hover con el ratón

    public void startHover()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
    }

    public void endHover()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
