using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public List<Card> Hand = new List<Card>();

    //Carta hovereada con su getter y setter
    private Card _hoveredCard;
    public Card HoveredCard
    {
        get => _hoveredCard;
        set
        {
            if (value != _hoveredCard)
            {
                _hoveredCard?.endHover();
                _hoveredCard = value;
                _hoveredCard?.startHover();
            }
        }
    }

    [SerializeField] LayerMask cardLayer;

    private void Start()
    {
        //De momento esto para poder testear cartas en la mano, pero no se usan
        for(int i = 0; i< transform.childCount; i++)
        {
            if(transform.GetChild(i).TryGetComponent(out Card childCard))
            {
                Hand.Add(childCard);
            }
        }
    }

    private void Update()
    {
        //Si sueltas, acabar el drag
        if(Input.GetMouseButtonUp(0))
        {
            HoveredCard?.endDrag();
            HoveredCard = null;
        }

        //Si no se esta dragueando nada, hoverear cartas
        if(!Input.GetMouseButton(0))
        {
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo, 100, cardLayer);
            HoveredCard = (hitInfo.collider != null && hitInfo.collider.TryGetComponent(out Card hitCard))
                ? hitCard : null;
        }

        //Si apretas, comenzar el drag
        if(Input.GetMouseButtonDown(0))
        {
            HoveredCard?.startDrag();
        }

    }

}
