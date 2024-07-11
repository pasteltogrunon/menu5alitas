using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Esta clase es un contador de recursos individual
[System.Serializable]
public class ResourceCounter
{
    public ResourceType resource;
    public int amount;

    public ResourceCounterType counterType;

    public ResourceCounter(ResourceType resource, int amount, ResourceCounterType counterType)
    {
        this.resource = resource;
        this.amount = amount;
        this.counterType = counterType;
    }

    public int AdjustedAmount()
    {
        return ResourceManager.Instance.adjustedAmount(this);
    }


}

//Contador de recursos de todo tipo
[System.Serializable]
public class ResourceCounterList
{
    Dictionary<ResourceType, ResourceCounter> resourcesDictionary = new Dictionary<ResourceType, ResourceCounter>();

    public ResourceCounter metalResourceCounter;
    public ResourceCounter waterResourceCounter;
    public ResourceCounter workerResourceCounter;
    public ResourceCounter scienceResourceCounter;

    public ResourceCounterList(ResourceCounterType counterType)
    {
        metalResourceCounter = new ResourceCounter(ResourceType.Metal, 0, counterType);
        waterResourceCounter = new ResourceCounter(ResourceType.Water, 0, counterType);
        workerResourceCounter = new ResourceCounter(ResourceType.Worker, 0, counterType);
        scienceResourceCounter = new ResourceCounter(ResourceType.Science, 0, counterType);

        resourcesDictionary.Add(ResourceType.Metal, metalResourceCounter);
        resourcesDictionary.Add(ResourceType.Water, waterResourceCounter);
        resourcesDictionary.Add(ResourceType.Worker, workerResourceCounter);
        resourcesDictionary.Add(ResourceType.Science, scienceResourceCounter);
    }

    public int AdjustedAmount(ResourceType resource)
    {
        return resourcesDictionary[resource].AdjustedAmount();
    }

    public int amount(ResourceType resource)
    {
        return resourcesDictionary[resource].amount;
    }

    public void SetAmount(ResourceType resource, int amount)
    {
        resourcesDictionary[resource].amount = amount;
    }
}

public enum ResourceCounterType
{
    Production,
    Cost,
    Storage,
    AlreadyAdjusted
}

public enum ResourceType
{
    Metal,
    Water,
    Worker,
    Science
}
