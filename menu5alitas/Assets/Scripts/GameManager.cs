using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public uint turn = 1;
    public uint turnsPerEvent = 5;
    public uint turnsPerGolemScreen = 20;
    public List<Buff> worldEvents = new List<Buff>();
    private HardBuff currentCatastrofe = HardBuff.None;

    private HandManager handManager;
    private ResourceManager resourceManager;
    private UIManager uiManager;

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
        currentCatastrofe = HardBuff.None;
        var nextBuff = worldEvents[Random.Range(0, worldEvents.Count)];
        resourceManager.ApplyWorldEvent(nextBuff);

        if (nextBuff.HardBuffId != HardBuff.None)
            currentCatastrofe = nextBuff.HardBuffId;
        if (worldEvents.Count > 0)
            resourceManager.ApplyWorldEvent(worldEvents[Random.Range(0, worldEvents.Count)]);
    }


    public int GolemScore(ResourceCounterList spentResources)
    {
        uint golemPiece = turn % turnsPerGolemScreen;

        resourceManager.storedResources -= spentResources;

        return 0;
    }

    private void ApplyEndEvents()
    {
        switch (currentCatastrofe)
        {
            case HardBuff.TAXES_FROM_ABOVE:
                resourceManager.storedResources.metalResourceCounter.amount -= 12;
                return;

            case HardBuff.RATS_IN_FOOD:
                resourceManager.storedResources.waterResourceCounter.amount -= 10;
                return;

            case HardBuff.PANDEMIC_INCOMING:
                resourceManager.storedResources.workerResourceCounter.amount -= 8;
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
