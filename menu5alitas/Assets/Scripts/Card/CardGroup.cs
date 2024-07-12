using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/CardGroup")]
public class CardGroup : ScriptableObject
{
    public string Id;
    public bool HasTier;

    public GameObject[] Cards;

}