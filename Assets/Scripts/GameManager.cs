using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class GameManager : NetworkBehaviour
{
    //List of unique cards that are in the player deck
    [Header("Player Deck")]
    [SerializeField] List<Card> deckCards;
    [SerializeField] List<int> cardQuantities;//These must be in the same order as the List of cards to be associated properly!

    [Header("Game Parameters")]
    [SerializeField] int shuffleCount = 7;
    [SerializeField] int handSize = 10;

    [Header("References")]
    [SerializeField] GameObject playerHand;
    PlayerHand _ph;

    List<Card> deck = new List<Card>();
    List<int> discardPile = new List<int>();


    // Start is called before the first frame update
    void Start()
    {
        //Get references needed
        _ph = playerHand.GetComponent<PlayerHand>();

        //Assemble Deck
        for (int i = 0; i < deckCards.Count; i++)
        {
            for (int j = 0; j < cardQuantities[i]; j++)
            {
                deck.Add(deckCards[i]);
            }
        }
        PrintDeckToConsole();

        //Shuffle Deck
        ShuffleDeck();

        //Deal cards to hand
        DealHand();
    }

    void DealHand()
    {
        for (int i = 0; i < handSize; i++)
        {
            _ph.DrawCard(deck[i]);
            deck.RemoveAt(i);
        }
    }

    void ShuffleDeck()
    {
        for (int i = 0; i < shuffleCount; i++)
        {
            //cut deck in half
            List<Card> firstHalf = new List<Card>();
            List<Card> secondHalf = new List<Card>();

            for (int j = 0; j < deck.Count/2;j++)
            {
                firstHalf.Add(deck[j]);
            }
            for (int k = deck.Count/2; k < deck.Count;k++)
            {
                secondHalf.Add(deck[k]);
            }
            //mix halves together randomizing whether or not the card goes on top or bottom
            for (int j = 0; j < deck.Count/2;j++)
            {
                if ( UnityEngine.Random.Range(0, 2) == 0 )
                {
                    deck[2*j] = firstHalf[j];
                    deck[2*j+1] = secondHalf[j];
                }
                else
                {
                    deck[2*j] = secondHalf[j];
                    deck[2*j+1] = firstHalf[j];
                }
            }
            PrintDeckToConsole();
        }
    }

    void PrintDeckToConsole()
    {
        string str = "Deck: ";
        for (int i = 0; i < deck.Count; i++)
        {
            str += deck[i].ToString() + "; ";
        }
        Debug.Log(str);
    }

    public int GetHandSize()
    {
        return handSize;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
