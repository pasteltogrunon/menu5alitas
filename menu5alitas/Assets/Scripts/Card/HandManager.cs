using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public static HandManager Instance;

    [SerializeField] uint StartingCardsAmount = 5;
    [SerializeField] GameObject CardPrefab;

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

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        AddStartingHand();
    }

    private void Update()
    {
        //Si sueltas, acabar el drag
        if (Input.GetMouseButtonUp(0))
        {
            if (HoveredCard != null)
            {
                HoveredCard?.endDrag();
                HoveredCard = null;
            }
        }

        //Si no se esta dragueando nada, hoverear cartas
        if (!Input.GetMouseButton(0))
        {
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo, 100, cardLayer);
            HoveredCard = (hitInfo.collider != null && hitInfo.collider.TryGetComponent(out Card hitCard))
                ? hitCard : null;
        }

        //Si apretas, comenzar el drag
        if (Input.GetMouseButtonDown(0))
        {
            if (HoveredCard != null)
            {
                HoveredCard?.startDrag();
            }
        }
    }

    private void AddStartingHand()
    {
        for (int i = 0; i < StartingCardsAmount; i++)
        {
            AddCard();
        }

    }

    public void StealCard()
    {
        AddCard();
    }

    private void AddCard()
    {
        //TODO: La carta tiene que ser sacada del mazo (aleatoriamente?)
        var cardInHandOffset = 1.25f;
        var gameObject = Instantiate(CardPrefab, transform.position + Vector3.left * Hand.Count * cardInHandOffset, Quaternion.identity, transform);
        Hand.Add(gameObject.GetComponent<Card>());
    }

    public void DeleteCard(Card card)
    {
        Hand.Remove(card);
        var cardInHandOffset = 1.25f;
        var c = 0;
        foreach (Card handCard in Hand) {
            handCard.transform.position = transform.position + Vector3.left * c * cardInHandOffset;
            c++;
        }
    }
}
