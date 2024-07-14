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


    public float GolemScore(ResourceCounterList spentResources)
    {
        uint golemPiece = turn % turnsPerGolemScreen;

        resourceManager.subtractResources(spentResources);
        
        float metal = spentResources.AdjustedAmountOfResource(ResourceType.Metal);
        float water = spentResources.AdjustedAmountOfResource(ResourceType.Water);
        float workers = spentResources.AdjustedAmountOfResource(ResourceType.Worker);
        float science = spentResources.AdjustedAmountOfResource(ResourceType.Science);
        
        float weight_metal = 0.25f;
        float weight_water = 0.1f;
        float weight_workers = 0.15f;
        float weight_science = 0.5f;

        float cost_metal = 1;
        float cost_water = 1;
        float cost_workers = 1;
        float cost_science = 1;

        switch(golemPiece)
        {
            case 1:
                cost_metal = 15;
                cost_water = 15;
                cost_workers = 15;
                cost_science = 20;
                break;
            case 2:
                cost_metal = 20;
                cost_water = 20;
                cost_workers = 20;
                cost_science = 40;
                break;
            case 3:
                cost_metal = 30;
                cost_water = 30;
                cost_workers = 30;
                cost_science = 55;
                break;
            case 4:
                cost_metal = 40;
                cost_water = 40;
                cost_workers = 40;
                cost_science = 70;
                break;
            case 5:
                cost_metal = 50;
                cost_water = 50;
                cost_workers = 50;
                cost_science = 80;
                break;
        }

        if (metal > cost_metal) metal = cost_metal;
        if (water > cost_water) water = cost_water;
        if (workers > cost_workers) workers = cost_workers;
        if (science > cost_science) science = cost_science;

        float rating = Mathf.Clamp(5 * (weight_metal * metal/cost_metal + weight_water * water/cost_water+ weight_workers * workers/cost_workers + weight_science * science/cost_science),0,5);

        return rating;
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
