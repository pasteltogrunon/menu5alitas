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

    public int HappinessPerTurn = 0;

    private int _happiness;
    public int Happiness
    {
        get => _happiness;
        set
        {
            //Preparado para cambiar la UI cuando haga falta
            _happiness = value;
            updateUI();
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Happiness = 50;
    }

    void Update()
    {
        
    }

    public void updateResourceCounters(ResourceCounterList production, ResourceCounterList cost)
    {
        resourceProductionPerTurn = production.AdjustAllResources();
        resourceCostPerEvent = cost.AdjustAllResources();

        updateUI();
    }

    public void NextTurn()
    {
        storagedResources += resourceProductionPerTurn;

        Happiness = Mathf.Clamp(Happiness + HappinessPerTurn, 0, 100);
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

    void updateUI()
    {
        ResourcesUI.Instance.UpdateUI(storagedResources, Happiness);
    }

    //Ajusta la cantidad segun si es coste o produccion, dependerá de los eventos
    public int ResourceAmountAdjustedFromBuffs(ResourceCounter resourceCounter)
    {
        switch(resourceCounter.counterType)
        {
            default:
                return resourceCounter.amount;
        }
    }
}
