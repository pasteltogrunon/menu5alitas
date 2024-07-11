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

    //Ajusta la cantidad segun si es coste o produccion, depender� de los eventos
    public int adjustedAmount(ResourceCounter resourceCounter)
    {
        switch(resourceCounter.counterType)
        {
            default:
                return resourceCounter.amount;
        }
    }
}