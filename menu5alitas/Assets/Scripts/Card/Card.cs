using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public abstract class Card : MonoBehaviour
{
    public ResourceCounterList cost = new ResourceCounterList(ResourceCounterType.Cost);
    public Sprite sprite;

    bool isDragged = false;

    public Vector3 targetPosition;
    public Quaternion targetRotation;

    public float hoverCooldown;

    [SerializeField] AudioClip cardPlaySound;

    public abstract bool tryPlayCard(Tile tile);

    //Se utiliza el update si está siendo drageado
    private void Update()
    {
        if(isDragged)
        {
            targetPosition = TileMap.mapPosition + Vector3.back;
            targetRotation = Quaternion.identity;
        }
        if (hoverCooldown > 0)
            hoverCooldown -= Time.deltaTime;

        transform.position = Vector3.Lerp(transform.position, targetPosition, Mathf.Clamp01(10.0f * Time.deltaTime));
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Mathf.Clamp01(10.0f * Time.deltaTime));
    }


    //Gestionan los hovers

    public void startHover()
    {
        targetPosition = targetPosition + 0.1f * Vector3.up;
        transform.localScale = 1.2f * Vector3.one;
    }

    public void endHover()
    {
        targetPosition = targetPosition - 0.1f * Vector3.up;
        hoverCooldown = 0.2f;
        transform.localScale = Vector3.one;

    }

    //Gestionan el drag and drop aqui en la clase de carta como a ti te gusta guarro

    public void startDrag()
    {
        if(HandManager.Instance.Hand.Contains(this))
        {
            isDragged = true;
            DeleteFromHandManager();
        }
    }

    public void Select()
    {
        HandManager.Instance.addCardToHandByXPosition(this);
    }

    public void endDrag()
    {
        isDragged = false;

        if(TileMap.Instance.SelectedTile != null)
        {
            if(!tryPlayCard(TileMap.Instance.SelectedTile))
            {
                if (transform)
                {
                    HandManager.Instance.addCardToHandByXPosition(this);
                }
            }
            else
            {
                if(cardPlaySound != null)
                {
                    SFXManager.PlaySound(cardPlaySound);
                }
            }
        }
        else
        {
            if (transform)
            {
                HandManager.Instance.addCardToHandByXPosition(this);
            }

        }
    }

    protected void DeleteFromHandManager()
    {
        var handManagerParent = transform.parent.GetComponent<HandManager>();
        handManagerParent.DeleteCard(this);
    }
}

