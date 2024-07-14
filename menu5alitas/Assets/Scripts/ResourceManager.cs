using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    public ResourceCounterList storedResources = new ResourceCounterList(ResourceCounterType.Storage);
    //Estos de aqui ya recibiran la produccion y el coste ajustados, asi que no habra que ajustarlo por evento.
    public ResourceCounterList resourceProductionPerTurn = new ResourceCounterList(ResourceCounterType.AlreadyAdjusted);
    public ResourceCounterList resourceCostPerEvent = new ResourceCounterList(ResourceCounterType.AlreadyAdjusted);

    private List<Buff> buffs = new List<Buff>();

    public int HappinessPerTurn = 0;

    private int _happiness;
    public int Happiness
    {
        get => _happiness;
        set
        {
            //Preparado para cambiar la UI cuando haga falta
            _happiness = Mathf.Clamp(value, 0, 100);
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
        TileMap.Instance.UpdateMap();

        storedResources += resourceProductionPerTurn;

        Happiness = Mathf.Clamp(Happiness + HappinessPerTurn, 0, 100);

    }

    public void ApplyWorldEvent(Buff b)
    {
        Happiness -= 5;
        if (b.HardBuffId != HardBuff.None)
            return;
        subtractResources(resourceCostPerEvent);
        buffs.Add(b);
    }

    public bool canAfford(ResourceCounterList cost)
    {
        return storedResources >= cost;
    }

    public bool subtractResources(ResourceCounterList cost)
    {
        if (storedResources >= cost)
        {
            storedResources -= cost;
            updateUI();
            return true;
        }
        return false;
    }

    void updateUI()
    {
        UIManager.Instance.UpdateResourcesUI(storedResources, resourceProductionPerTurn, resourceCostPerEvent, Happiness);
    }

    public void addBuff(Buff buff)
    {
        buffs.Add(buff);
    }

    //Ajusta la cantidad segun si es coste o produccion, dependerá de los eventos
    public int ResourceAmountAdjustedFromBuffs(ResourceCounter resourceCounter)
    {
        if (resourceCounter == null)
            return 0;

        if (resourceCounter.counterType == ResourceCounterType.AlreadyAdjusted)
            return resourceCounter.amount;

        List<Buff> buffsCopy = new List<Buff>(buffs);
        foreach (Buff buff in buffsCopy)
        {
            if (buff.turnsLeft > 0)
            {
                if (buff.counterType == resourceCounter.counterType && buff.resource == resourceCounter.resource)
                    resourceCounter.amount = Mathf.CeilToInt(buff.factor * resourceCounter.amount);

                buff.turnsLeft--;
                return resourceCounter.amount;
            }
            else
            {
                buffs.Remove(buff);
            }
        }

        return resourceCounter.amount;
    }
}
