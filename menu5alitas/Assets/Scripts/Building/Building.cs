using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] ResourceCounterList productionPerTurn = new ResourceCounterList(ResourceCounterType.Production);
    [SerializeField] ResourceCounterList costPerEvent = new ResourceCounterList(ResourceCounterType.Cost);

    public ResourceCounterList getProduction()
    {
        ResourceCounterList production = productionPerTurn.copy();

        return production;
    }

    public ResourceCounterList getCost()
    {
        ResourceCounterList cost = costPerEvent.copy();

        return cost;
    }
}
