using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Deck")]
public class Deck : ScriptableObject
{
    public List<string> cardIds;

    public List<string> remainingCardIds;

    public void shuffleDeck()
    {
        //De momento no se tiene en cuenta las cartas que tienes en la mano
        //Podría ser iterar sobre la mano y quitar los ids que toquen
        remainingCardIds = new List<string>(cardIds);
    }

    public string takeRandomCard()
    {
        if (remainingCardIds.Count == 0)
            shuffleDeck();

        int number = Random.Range(0, remainingCardIds.Count);

        string id = remainingCardIds[number];

        remainingCardIds.RemoveAt(number);

        return id;
    }
}
