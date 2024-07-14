using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public abstract class Card : MonoBehaviour
{
    public ResourceCounterList cost = new ResourceCounterList(ResourceCounterType.Cost);
    public Sprite sprite;

    bool isDragged = false;
    bool isHovered = false;

    public Vector3 targetPosition;
    public Quaternion targetRotation;

    public float hoverCooldown;

    [SerializeField] protected AudioClip cardPlaySound;

    public abstract bool tryPlayCard(Tile tile);

    //Se utiliza el update si está siendo drageado
    private void Update()
    {
        if (hoverCooldown > 0)
            hoverCooldown -= Time.deltaTime;

        if(isDragged)
        {
            targetPosition = TileMap.mapPosition + Vector3.back;
            targetRotation = Quaternion.identity;
        }

        if(isHovered)
        {
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo);
            Vector3 turningAxis = Vector3.Cross(hitInfo.point - transform.position, Vector3.back).normalized;
            targetRotation = Quaternion.Euler(5 * turningAxis.x, 5 * turningAxis.y, targetRotation.z);
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, Mathf.Clamp01(10.0f * Time.deltaTime));
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Mathf.Clamp01(10.0f * Time.deltaTime));
    }


    //Gestionan los hovers

    public void startHover()
    {
        isHovered = true;

        targetPosition = targetPosition + 1.5f * Vector3.up + 0.5f * Vector3.back;
        transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 1;
        transform.localScale = 3f * Vector3.one;
    }

    public void endHover()
    {
        isHovered = false;

        targetPosition = targetPosition - 1.5f * Vector3.up - 0.5f * Vector3.back;
        transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 0;
        hoverCooldown = 0.2f;
        transform.localScale = Vector3.one;

        HandManager.Instance.recomputeHandPositions();
    }

    //Gestionan el drag and drop aqui en la clase de carta como a ti te gusta guarro

    public void startDrag()
    {
        if(HandManager.Instance.Hand.Contains(this))
        {
            isHovered = false;
            isDragged = true;
            DeleteFromHandManager();
            transform.localScale = Vector3.one;
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

    protected void endCard()
    {
        StartCoroutine(destroyCard());
    }

    IEnumerator destroyCard()
    {
        float time = 1.0f;
        GetComponent<Collider>().enabled = false;
        Material mat = transform.GetChild(0).GetComponent<SpriteRenderer>().material;
        transform.GetChild(1).gameObject.SetActive(false);
        for(float t = 0; t < time; t+=Time.deltaTime)
        {
            mat.SetFloat("_DissolvePhase", t/time);
            yield return null;
        }
        mat.SetFloat("_DissolvePhase", 0);
        DeleteFromHandManager();
        Destroy(gameObject);
    }

    protected void DeleteFromHandManager()
    {
        var handManagerParent = transform.parent.GetComponent<HandManager>();
        handManagerParent.DeleteCard(this);
    }
}

