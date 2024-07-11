using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCard : Card
{
    public GameObject buildingPrefab;

    //De momento poco, para el testeo
    public override bool tryPlayCard(Tile tile)
    {
        float canPlaceFactor = tile.canPlace(buildingPrefab.GetComponent<Building>());
        if(canPlaceFactor != 0)
        {
            if(ResourceManager.Instance.subtractResources(cost * canPlaceFactor))
            {
                if(canPlaceFactor != 1)
                {
                    Destroy(tile.building.gameObject);
                }

                Building newBuilding = Instantiate(buildingPrefab, tile.transform).GetComponent<Building>();
                tile.building = newBuilding;
                newBuilding.onPlace(tile);
                TileMap.Instance.UpdateMap();


                //tile.GetComponent<SpriteRenderer>().sprite = sprite;
                Destroy(gameObject);
                DeleteFromHandManager();
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
