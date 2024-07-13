using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public uint turn = 1;
    public uint turnsPerEvent = 5;
    
    private HandManager handManager;
    // Start is called before the first frame update

    void Start()
    {
        handManager = HandManager.Instance;
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
        if (HandManager.Instance.isChoosingCard) return;

        turn++;
        handManager.StealCard();
        ResourceManager.Instance.NextTurn();
        UIManager.Instance.updateTurnUI(turn);
        if(turn % turnsPerEvent == 0)
        {
            NextEvent();
        }
    }

    private void NextEvent()
    {
        ResourceManager.Instance.NextEvent();

    }
}
