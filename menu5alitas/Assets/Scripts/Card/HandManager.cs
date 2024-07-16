using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class HandManager : MonoBehaviour
{
    public static HandManager Instance;

    private GameManager gameManager;

    [SerializeField] string[] startingCardsIds;
    [SerializeField] uint MaxCardsAmount = 10;

    public List<Card> Hand = new List<Card>();
    public int CurrentTier = 1;

    Deck handDeck;
    [SerializeField] Deck handDeckReference;
    [SerializeField] Collection cardCollection;
    public AudioClip cannotPlayCardSound;

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

                if(value != null && value.hoverCooldown <= 0)
                {
                    SFXManager.PlayRandomSoundFromArray(hoverSounds, bajito);
                    _hoveredCard = value;
                    UIManager.Instance.updateCostText(_hoveredCard.getCost());

                }
                else
                {
                    _hoveredCard = null;

                }

                _hoveredCard?.startHover();
            }
        }
    }

    [SerializeField] AudioClip cardPickingSound;
    [SerializeField] AudioClip cardDragging;
    public AudioClip cartaQuemada;
    [SerializeField] AudioClip[] cardAppearingSounds;
    [SerializeField] AudioClip[] hoverSounds;

    [SerializeField] AudioMixerGroup bajito;

    [SerializeField] LayerMask cardLayer;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        gameManager = GameManager.Instance;

        handDeck = Instantiate(handDeckReference);

        AddStartingHand();
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
                        HoveredCard.Select();
                        HoveredCard = null;
                    }
                }
                else
                {
                    HoveredCard?.startDrag();
                    if (HoveredCard != null)
                        SFXManager.PlaySound(cardDragging);
                }
            }
        }
    }

    private void AddStartingHand()
    {
        /*for (int i = 0; i < StartingCardsAmount; i++)
        {
            AddRandomCard();
        }*/

        foreach(string id in startingCardsIds)
        {
            Hand.Add(Instantiate(cardCollection.GetCardPrefab(id, CurrentTier), transform).GetComponent<Card>());
        }

        recomputeHandPositions();
    }

    public void StealCard()
    {
        StartCoroutine(SpawnCardChoice());
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

            if(choosingCards.CardCenter != null)
                choosingCards.CardCenter.GetComponent<Card>().endCard();
            choosingCards.CardRight.GetComponent<Card>().endCard();
        }
        else if(isCenter)
        {
            chosenCardPrefab = choosingCards.CardCenter;

            choosingCards.CardLeft.GetComponent<Card>().endCard();
            choosingCards.CardRight.GetComponent<Card>().endCard();
        }
        else if (isRight)
        {
            chosenCardPrefab = choosingCards.CardRight;

            if (choosingCards.CardCenter != null)
                choosingCards.CardCenter.GetComponent<Card>().endCard();
            choosingCards.CardLeft.GetComponent<Card>().endCard();
        }
        else
        {
            return Vector3.zero;
        }

        if (chosenCardPrefab == null)//redundant
        {
            return Vector3.zero;
        }

        isChoosingCard = false;

        SFXManager.PlaySound(cardPickingSound);

        return card.targetPosition;
    }

    private IEnumerator SpawnCardChoice()
    {
        isChoosingCard = true;


        if(gameManager.GetCurrentCatastrofeId() == HardBuff.PESIMISM)
        {
            choosingCards.CardLeft = Instantiate(GetRandomCard(), transform.position + Vector3.up * 10, Quaternion.identity, transform);
            choosingCards.CardLeft.GetComponent<Card>().targetPosition = transform.position + Vector3.up * 3.5f + Vector3.left * 1f;
            SFXManager.PlayRandomSoundFromArray(cardAppearingSounds);
            yield return new WaitForSeconds(0.3f);
            choosingCards.CardRight = Instantiate(GetRandomCard(), transform.position + Vector3.up * 10, Quaternion.identity, transform);
            choosingCards.CardRight.GetComponent<Card>().targetPosition = transform.position + Vector3.up * 3.5f + Vector3.right * 1f;
            SFXManager.PlayRandomSoundFromArray(cardAppearingSounds);

        }
        else
        {
            choosingCards.CardLeft = Instantiate(GetRandomCard(), transform.position + Vector3.up * 10, Quaternion.identity, transform);
            choosingCards.CardLeft.GetComponent<Card>().targetPosition = transform.position + Vector3.up * 3.5f + Vector3.left * 2f;
            SFXManager.PlayRandomSoundFromArray(cardAppearingSounds);
            yield return new WaitForSeconds(0.3f);
            choosingCards.CardCenter = Instantiate(GetRandomCard(), transform.position + Vector3.up * 10, Quaternion.identity, transform);
            choosingCards.CardCenter.GetComponent<Card>().targetPosition = transform.position + Vector3.up * 3.5f;
            SFXManager.PlayRandomSoundFromArray(cardAppearingSounds);
            yield return new WaitForSeconds(0.3f);
            choosingCards.CardRight = Instantiate(GetRandomCard(), transform.position + Vector3.up * 10, Quaternion.identity, transform);
            choosingCards.CardRight.GetComponent<Card>().targetPosition = transform.position + Vector3.up * 3.5f + Vector3.right * 2f;
            SFXManager.PlayRandomSoundFromArray(cardAppearingSounds);

        }
    }

    private void AddRandomCard()
    {
        GameObject cardPrefab = GetRandomCard();
        Hand.Add(Instantiate(cardPrefab, transform).GetComponent<Card>());
    }

    private GameObject GetRandomCard()
    {
        string id = handDeck.takeRandomCard();
        return cardCollection.GetCardPrefab(id, CurrentTier);
    }

    public void DeleteCard(Card card)
    {
        Hand.Remove(card);
        recomputeHandPositions();
    }

    public void recomputeHandPositions()
    {
        var c = 0;
        foreach (Card handCard in Hand)
        {
            handCard.targetPosition = transform.position + cardPositionOffset(c);
            Vector3 euler = handCard.targetRotation.eulerAngles;
            handCard.targetRotation = Quaternion.Euler(new Vector3(euler.x, euler.y, cardAngleInHand(c)));
            c++;
        }
    }

    Vector3 cardPositionOffset(int cardNum)
    {
        float cardInHandOffset = 4.0f/Hand.Count;
        float x = (2 * cardNum + 1) * cardInHandOffset - 4;
        return Vector3.left * x + (Vector3.up * (Mathf.Sqrt(1 - Mathf.Pow(x / 4, 2)) - 0.4f));
    }

    float cardAngleInHand(int cardNum)
    {
        float cardInHandOffset = 4.0f / Hand.Count;
        float x = (2 * cardNum + 1) * cardInHandOffset - 4;

        return 4 * x;
    }

    public void addCardToHandByXPosition(Card card)
    {
        if (Hand.Count >= MaxCardsAmount)
        {
            card.endCard();
        }
        else
        {
            Hand.Add(card);
            Hand.Sort(CompareCardPosition);
            recomputeHandPositions();
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

    static int CompareCardPosition(Card card1, Card card2)
    {
        Vector2 screenSpace1 = Camera.main.WorldToScreenPoint(card1.transform.position);
        Vector2 screenSpace2 = Camera.main.WorldToScreenPoint(card2.transform.position);

        return Mathf.FloorToInt(Mathf.Sign(screenSpace2.x - screenSpace1.x));
    }
}
