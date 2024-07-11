using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    public ResourceCounterList storagedResources = new ResourceCounterList(ResourceCounterType.Storage);
    //Estos de aqui ya recibiran la produccion y el coste ajustados, asi que no habra que ajustarlo por evento.
    public ResourceCounterList resourceProductionPerTurn = new ResourceCounterList(ResourceCounterType.AlreadyAdjusted);
    public ResourceCounterList resourceCostPerEvent = new ResourceCounterList(ResourceCounterType.AlreadyAdjusted);

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void updateResourceCounters(ResourceCounterList production, ResourceCounterList cost)
    {
        resourceProductionPerTurn = production;
        resourceCostPerEvent = cost;
    }

    public void NextTurn()
    {
        storagedResources += resourceProductionPerTurn;
    }

    public bool canAfford(ResourceCounterList cost)
    {
        return storagedResources >= cost;
    }

    public bool subtractResources(ResourceCounterList cost)
    {
        if(storagedResources >= cost)
        {
            storagedResources -= cost;
            return true;
        }
        return false;
    }

    //Ajusta la cantidad segun si es coste o produccion, dependerá de los eventos
    public int adjustedAmount(ResourceCounter resourceCounter)
    {
        switch(resourceCounter.counterType)
        {
            default:
                return resourceCounter.amount;
        }
    }
}
