using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Collection")]
public class Collection : ScriptableObject
{
    public CardGroup[] collection;

    public CardGroup GetCardGroup(string id)
    {
        foreach(CardGroup cardGroup in collection)
        {
            if(cardGroup.Id == id)
            {
                return cardGroup;
            }
        }
        return null;
    }

    public GameObject GetCardPrefab(string id, int tier)
    {
        foreach(CardGroup cardGroup in collection)
        {
            if(cardGroup.Id == id)
            {
                if(cardGroup.HasTier)
                    return cardGroup.Cards[tier-1];
                return cardGroup.Cards[0];
            }
        }
        return null;
    }
}
