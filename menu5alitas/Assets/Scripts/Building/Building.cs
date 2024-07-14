using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] ResourceCounterList productionPerTurn = new ResourceCounterList(ResourceCounterType.Production);
    [SerializeField] ResourceCounterList costPerEvent = new ResourceCounterList(ResourceCounterType.Cost);

    public string id;
    public int tier;

    public int happinessPerTurn;

    public bool requiresEnergy;

    public ResourceCounterList getProduction()
    {
        return productionPerTurn * getEneryFactor() * getHappinessFactor();
    }

    float getEneryFactor()
    {
        //If placed and requires energy
        if(transform.parent && requiresEnergy)
        {
            float energyLevel = transform.parent.GetComponent<Tile>().energyLevel;

            return energyLevel / tier;
        }
        return 1;
    }

    float getHappinessFactor()
    {
        if (transform.parent)
        {
            return Mathf.Pow((float) (ResourceManager.Instance.Happiness) / 100, 2);
        }
        return 1;
    }

    public ResourceCounterList getCost()
    {
        ResourceCounterList cost = costPerEvent.copy();

        return cost;
    }

    public virtual void onPlace(Tile tile)
    {

    }
}
