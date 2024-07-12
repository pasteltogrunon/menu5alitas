using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TierCard : Card
{
    [SerializeField] int tier;

    public override bool tryPlayCard(Tile tile)
    {
        if (ResourceManager.Instance.subtractResources(cost.AdjustAllResources()))
        {
            HandManager.Instance.CurrentTier = Mathf.Clamp(tier, 1, 3);
            HandManager.Instance.RemoveCardFromDeck("tier" + tier);

            //tile.GetComponent<SpriteRenderer>().sprite = sprite;
            Destroy(gameObject);
            DeleteFromHandManager();
            return true;
        }
        return false;
    }
}