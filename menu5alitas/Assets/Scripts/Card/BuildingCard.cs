using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCard : Card
{
    //De momento poco, para el testeo
    public override void tryPlayCard(Tile tile)
    {
        tile.GetComponent<SpriteRenderer>().sprite = sprite;
        Destroy(gameObject);
    }
}