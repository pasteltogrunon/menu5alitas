using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public uint turn = 1;
    public uint turnsPerEvent = 5;
    public List<Buff> worldEvents = new List<Buff>();

    private HandManager handManager;
    private ResourceManager resourceManager;
    private UIManager uiManager;
    // Start is called before the first frame update

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
        handManager.StealCard();
        resourceManager.NextTurn();
        uiManager.updateTurnUI(turn);
        if(turn % turnsPerEvent == 0)
        {
            NextEvent();
        }
    }

    private void NextEvent()
    {
        resourceManager.ApplyWorldEvent(worldEvents[Random.Range(0, worldEvents.Count)]);

    }
}
