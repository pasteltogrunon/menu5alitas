using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCard : Card
{
    public GameObject buildingPrefab;

    //De momento poco, para el testeo
    public override bool tryPlayCard(Tile tile)
    {
        GameObject newBuilding = Instantiate(buildingPrefab, tile.transform);
        tile.building = newBuilding.GetComponent<Building>();
        TileMap.Instance.UpdateMap();
        tile.GetComponent<SpriteRenderer>().sprite = sprite;
        Destroy(gameObject);
        DeleteFromHandManager();
        return true;
    }
}
