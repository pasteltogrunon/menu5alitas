using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCard : Card
{
    public GameObject buildingPrefab;

    //De momento poco, para el testeo
    public override bool tryPlayCard(Tile tile)
    {
        if(GameManager.Instance.GetCurrentCatastrofeId() == HardBuff.OVERLOAD)
        {
            SFXManager.PlaySound(GameManager.Instance.overloadSound);
            return false;
        }

        float canPlaceFactor = tile.canPlace(buildingPrefab.GetComponent<Building>());
        if(canPlaceFactor != 0)
        {
            if(ResourceManager.Instance.subtractResources(cost.AdjustAllResources() * canPlaceFactor))
            {
                if(canPlaceFactor != 1)
                {
                    Destroy(tile.building.gameObject);
                }

                Building newBuilding = Instantiate(buildingPrefab, tile.transform).GetComponent<Building>();
                tile.building = newBuilding;
                newBuilding.onPlace(tile);
                TileMap.Instance.UpdateMap();


                endCard();
                return true;
            }
            else
            {
                SFXManager.PlaySound(HandManager.Instance.cannotPlayCardSound);
                return false;
            }
        }
        else
        {
            SFXManager.PlaySound(HandManager.Instance.cannotPlayCardSound);
            return false;
        }
    }

    public override ResourceCounterList getCost()
    {
        if(TileMap.Instance.SelectedTile != null)
        {
            float factor = TileMap.Instance.SelectedTile.canPlace(buildingPrefab.GetComponent<Building>());
            if(factor != 0)
                return base.getCost() * TileMap.Instance.SelectedTile.canPlace(buildingPrefab.GetComponent<Building>());
        }
        return base.getCost();
    }
}
