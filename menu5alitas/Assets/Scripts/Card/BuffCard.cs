using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffCard : Card
{
    public Buff[] buffs;

    public int HappinessAmount;

    public override bool tryPlayCard(Tile tile)
    {
        if (ResourceManager.Instance.subtractResources(cost.AdjustAllResources()))
        {
            foreach (Buff buff in buffs)
                ResourceManager.Instance.addBuff(Instantiate(buff));

            TileMap.Instance.UpdateMap();

            ResourceManager.Instance.Happiness += HappinessAmount;

            endCard();
            return true;
        }
        return false;
    }
}
