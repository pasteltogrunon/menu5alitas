using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public uint turn = 1;
    public uint turnsPerEvent = 5;
    public uint turnsPerGolemScreen = 20;

    public uint turnTier2 = 10;
    public uint turnTier3 = 25;

    public List<Buff> worldEvents = new List<Buff>();
    private HardBuff currentCatastrofe = HardBuff.None;

    private HandManager handManager;
    private ResourceManager resourceManager;
    private UIManager uiManager;

    [SerializeField] AudioClip newCatastropheSound;

    private void Awake()
    {
        Instance = this;
    }


    void Start()
    {
        handManager = HandManager.Instance;
        resourceManager = ResourceManager.Instance;
        uiManager = UIManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextTurn();
        }
    }

    private void NextTurn()
    {
        if (handManager.isChoosingCard) return;

        turn++;
        resourceManager.NextTurn();
        uiManager.updateTurnUI(turn);

        if(turn == turnTier2)
        {
            handManager.AddCardToDeck("tier2");
        }
        else if(turn == turnTier3)
        {
            handManager.AddCardToDeck("tier3");
        }

        if (turn % turnsPerEvent == 0)
        {
            ApplyEndEvents();
            NextEvent();
        }

        if (turn % turnsPerGolemScreen == 0)
        {
            handManager.isInGolemScreen = true;
            uiManager.ShowGolemScreen();
        }

        handManager.StealCard();
    }

    public HardBuff GetCurrentCatastrofeId()
    {
        return currentCatastrofe;
    }

    private void NextEvent()
    {
        if (worldEvents.Count == 0) return;

        currentCatastrofe = HardBuff.None;

        var nextBuff = worldEvents[Random.Range(0, worldEvents.Count)];
        resourceManager.ApplyWorldEvent(nextBuff);

        SFXManager.PlaySound(newCatastropheSound);

        UIManager.Instance.updateCatastropheText(nextBuff.HardBuffId.ToString());

        if (nextBuff.HardBuffId != HardBuff.None)
            currentCatastrofe = nextBuff.HardBuffId;
    }


    public int GolemScore(ResourceCounterList spentResources)
    {
        uint golemPiece = turn % turnsPerGolemScreen;

        resourceManager.subtractResources(spentResources);

        return 0;
    }

    private void ApplyEndEvents()
    {
        ResourceCounterList cost = new ResourceCounterList(ResourceCounterType.Cost);
        switch (currentCatastrofe)
        {
            case HardBuff.TAXES_FROM_ABOVE:
                cost.GetResourceCounterByResourceType(ResourceType.Metal).amount = 12;
                resourceManager.subtractResources(cost);
                return;

            case HardBuff.RATS_IN_FOOD:
                cost.GetResourceCounterByResourceType(ResourceType.Water).amount = 10;
                resourceManager.subtractResources(cost);
                return;

            case HardBuff.PANDEMIC_INCOMING:
                cost.GetResourceCounterByResourceType(ResourceType.Worker).amount = 8;
                resourceManager.subtractResources(cost);
                return;

            default:
                return;
        }
    }
}

public enum HardBuff
{
    None,
    PESIMISM,
    TAXES_FROM_ABOVE,
    I_FUCKED_UP,
    OVERLOAD,
    RATS_IN_FOOD,
    PANDEMIC_INCOMING,
    BREAD_YES_BUT_NO_WINE
}
