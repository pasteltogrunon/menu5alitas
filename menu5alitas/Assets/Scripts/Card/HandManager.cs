using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public static HandManager Instance;

    [SerializeField] uint StartingCardsAmount = 5;

    public List<Card> Hand = new List<Card>();
    public int CurrentTier = 1;

    [SerializeField] Deck deck;
    [SerializeField] Collection cardCollection;

    private bool isChoosingCard = false;
    private struct ChoosingCards
    {
        public GameObject CardLeft { get; set; }
        public GameObject CardCenter { get; set; }
        public GameObject CardRight { get; set; }
    }

    private ChoosingCards choosingCards;

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
            if (HoveredCard != null && !isChoosingCard)
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
                if (isChoosingCard)
                {
                    var newPositon = ChooseCard(HoveredCard);
                    HoveredCard?.Select(newPositon);
                }
                else
                {
                    HoveredCard?.startDrag();
                }
            }
        }
    }

    private void AddStartingHand()
    {
        for (int i = 0; i < StartingCardsAmount; i++)
        {
            AddRandomCard();
        }

    }

    public void StealCard()
    {
        SpawnCardChoice();
        //AddRandomCard();
    }

    private Vector3 ChooseCard(Card card)
    {
        if (card == null)
        {
            return Vector3.zero;
        }
        GameObject chosenCardPrefab = null;
        if (choosingCards.CardLeft != null && choosingCards.CardLeft.GetComponent<Card>() != card)
        {
            Debug.Log("Destroyed Left Choosing Card");
            Destroy(choosingCards.CardLeft);
        }
        else
        {
            chosenCardPrefab = choosingCards.CardLeft;
        }

        if (choosingCards.CardCenter != null && choosingCards.CardCenter.GetComponent<Card>() != card)
        {
            Debug.Log("Destroyed Center Choosing Card");
            Destroy(choosingCards.CardCenter);
        }
        else
        {
            chosenCardPrefab = choosingCards.CardCenter;
        }

        if (choosingCards.CardRight != null && choosingCards.CardRight.GetComponent<Card>() != card)
        {
            Debug.Log("Destroyed Right Choosing Card");
            Destroy(choosingCards.CardRight);
        }
        else
        {
            chosenCardPrefab = choosingCards.CardRight;
        }

        //Debug.Log($"Hand Manager position: {transform.position}");
        //Debug.Log($"Parent position: {transform.position}");
        var newPosition = transform.position + Vector3.left * (Hand.Count - 3) * 1.25f;

        chosenCardPrefab.transform.position = newPosition; //Estaria guay animarlo
        Hand.Add(card);
        isChoosingCard = false;

        return newPosition;
    }

    private void SpawnCardChoice()
    {
        isChoosingCard = true;

        choosingCards.CardLeft = Instantiate(GetRandomCard(), transform.position + Vector3.up * 2 + Vector3.left * 1.25f, Quaternion.identity, transform);
        choosingCards.CardCenter = Instantiate(GetRandomCard(), transform.position + Vector3.up * 2, Quaternion.identity, transform);
        choosingCards.CardRight = Instantiate(GetRandomCard(), transform.position + Vector3.up * 2 + Vector3.right * 1.25f, Quaternion.identity, transform);
    }

    private void AddRandomCard()
    {
        var cardInHandOffset = 1.25f;
        GameObject cardPrefab = GetRandomCard();
        var gameObject = Instantiate(cardPrefab, transform.position + Vector3.left * (Hand.Count - 3) * cardInHandOffset, Quaternion.identity, transform);
        Hand.Add(gameObject.GetComponent<Card>());
    }

    private GameObject GetRandomCard()
    {
        string id = deck.takeRandomCard();
        return cardCollection.GetCardPrefab(id, CurrentTier);
    }

    public void DeleteCard(Card card)
    {
        Hand.Remove(card);
        var cardInHandOffset = 1.25f;
        var c = -3;
        foreach (Card handCard in Hand)
        {
            handCard.transform.position = transform.position + Vector3.left * c * cardInHandOffset;
            c++;
        }
    }
}
