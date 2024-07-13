using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Buff")]
public class Buff : ScriptableObject
{
    public ResourceType resource;
    public float factor;
    public ResourceCounterType counterType;

    public uint turnsLeft;

    public Buff(Buff b)
    {
        turnsLeft = b.turnsLeft;
        factor = b.factor;

        resource = b.resource;
        counterType = b.counterType;
    }
    

}
