using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public uint turn = 1;
    
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
        turn++;
        handManager.StealCard();
    }
}
