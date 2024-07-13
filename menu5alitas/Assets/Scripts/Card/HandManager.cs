using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public static HandManager Instance;

    private GameManager gameManager;

    [SerializeField] uint StartingCardsAmount = 5;

    public List<Card> Hand = new List<Card>();
    public int CurrentTier = 1;

    [SerializeField] Deck handDeck;
    [SerializeField] Collection cardCollection;

    public bool isInGolemScreen = false;
    public bool isChoosingCard = false;
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
        gameManager = GameManager.Instance;
    }

    private void Update()
    {
        if (isInGolemScreen) return;

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
                    if (newPositon != Vector3.zero)
                    {
                        HoveredCard.Select(newPositon);
                        HoveredCard?.startDrag();
                    }
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
        bool isLeft = choosingCards.CardLeft != null && choosingCards.CardLeft.GetComponent<Card>() == card;
        bool isCenter = choosingCards.CardCenter != null && choosingCards.CardCenter.GetComponent<Card>() == card;
        bool isRight = choosingCards.CardRight != null && choosingCards.CardRight.GetComponent<Card>() == card;

        if (isLeft)
        {
            chosenCardPrefab = choosingCards.CardLeft;
            Destroy(choosingCards.CardCenter);
            Destroy(choosingCards.CardRight);
        }
        else if(isCenter)
        {
            chosenCardPrefab = choosingCards.CardCenter;
            Destroy(choosingCards.CardLeft);
            Destroy(choosingCards.CardRight);
        }else if (isRight)
        {
            chosenCardPrefab = choosingCards.CardRight;
            Destroy(choosingCards.CardCenter);
            Destroy(choosingCards.CardLeft);
        }
        else
        {
            return Vector3.zero;
        }

        if (chosenCardPrefab == null)//redundant
        {
            return Vector3.zero;
        }

        var newPosition = transform.position + Vector3.left * (Hand.Count - 3) * 1.25f;

        chosenCardPrefab.transform.position = newPosition; //Estaria guay animarlo
        Hand.Add(card);
        isChoosingCard = false;

        return newPosition;
    }

    private void SpawnCardChoice()
    {
        isChoosingCard = true;

        if(gameManager.GetCurrentCatastrofeId() == HardBuff.PESIMISM)
        {
            choosingCards.CardLeft = Instantiate(GetRandomCard(), transform.position + Vector3.up * 2 + Vector3.left * 0.75f, Quaternion.identity, transform);
            choosingCards.CardRight = Instantiate(GetRandomCard(), transform.position + Vector3.up * 2 + Vector3.right * 0.75f, Quaternion.identity, transform);
        }
        else
        {
            choosingCards.CardLeft = Instantiate(GetRandomCard(), transform.position + Vector3.up * 2 + Vector3.left * 1.25f, Quaternion.identity, transform);
            choosingCards.CardCenter = Instantiate(GetRandomCard(), transform.position + Vector3.up * 2, Quaternion.identity, transform);
            choosingCards.CardRight = Instantiate(GetRandomCard(), transform.position + Vector3.up * 2 + Vector3.right * 1.25f, Quaternion.identity, transform);
        }
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
        string id = handDeck.takeRandomCard();
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

    public void RemoveCardFromDeck(string id)
    {
        handDeck.RemoveCard(id);
    }

    public void AddCardToDeck(string id)
    {
        handDeck.AddCard(id);
    }
}
