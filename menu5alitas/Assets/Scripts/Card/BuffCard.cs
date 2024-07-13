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

            ResourceManager.Instance.Happiness += HappinessAmount;

            //tile.GetComponent<SpriteRenderer>().sprite = sprite;
            Destroy(gameObject);
            DeleteFromHandManager();
            return true;
        }
        return false;
    }
}
