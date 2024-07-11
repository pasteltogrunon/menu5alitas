using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2Int uvCoords;

    public Building building;

    public int energyLevel = 0;

    private bool _unlocked = false;
    public bool Unlocked
    {
        get => _unlocked;
        set
        {
            if (value)
            {
                unlock();
            }
            else
            {
                relock();
            }

            _unlocked = value;
        }
    }


    //Se guardan sus coordenadas y se le cambia el nombre
    public void Initialize(int uCoord, int vCoord)
    {
        uvCoords = new Vector2Int(uCoord, vCoord);
        gameObject.name = "Tile " + "(" + uCoord + ", " + vCoord + ")";
    }


    //Gestionan el hover con el ratón

    public void startHover()
    {
        if(Unlocked)
            GetComponent<SpriteRenderer>().color = Color.red;
    }

    public void endHover()
    {
        if(Unlocked)
            GetComponent<SpriteRenderer>().color = Color.white;
    }

    void unlock()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    void relock()
    {
        GetComponent<SpriteRenderer>().color = Color.gray;
    }

    //Returns 0 if cannot place, else returns the price reduction factor
    public float canPlace(Building newBuilding)
    {
        if (!Unlocked)
            return 0;

        //Can be placed if no building is present
        if (building == null)
            return 1;

        //Can be placed if the building is the same
        if (newBuilding.id == building.id)
            return (float) (newBuilding.tier - building.tier) / newBuilding.tier;

        return 0;
    }

    public ResourceCounterList getProduction()
    {
        if (building == null)
            return new ResourceCounterList(ResourceCounterType.Production);
        
        return building.getProduction();
    }

    public ResourceCounterList getCost()
    {
        if (building == null)
            return new ResourceCounterList(ResourceCounterType.Cost);

        return building.getCost();
    }
}
