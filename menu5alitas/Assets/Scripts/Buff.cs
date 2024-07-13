using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Buff")]
public class Buff : ScriptableObject
{
    public ResourceCounterList value;
    public uint turnsLeft;

    public Buff(Buff b)
    {
        turnsLeft = b.turnsLeft;
        value = b.value;
    }
    

}
