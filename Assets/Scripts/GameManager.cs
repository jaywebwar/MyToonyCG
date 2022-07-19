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

    [Header("Client References")]
    [SerializeField] LocalPlayer localPlayer;
    [SerializeField] EnemyPlayer remotePlayer;
    [SerializeField] PlayerHand localHand;

    [SerializeField] EnemyPlayerHand remoteHand;
    [SerializeField] GameObject attackButton;
    [SerializeField] GameObject endTurnButton;

    //server properties
    NetworkConnectionToClient player1;
    NetworkConnectionToClient player2;

    //client properties
    bool isLocalPlayerTurn = false;
    int playerCount = 0;
    const int maxPlayerCount = 2;
    List<Card> deck = new List<Card>();
    List<int> discardPile = new List<int>();
    
    NetworkIdentity _ni;

    [Server]
    public void PassConnections(NetworkConnectionToClient conn1, NetworkConnectionToClient conn2)
    {
        //Set connections
        player1 = conn1;
        player2 = conn2;

        _ni = GetComponent<NetworkIdentity>();

        _ni.AssignClientAuthority(player1);
        StartGame(_ni.connectionToClient);
        _ni.RemoveClientAuthority();

        _ni.AssignClientAuthority(player2);
        StartGame(_ni.connectionToClient);
        _ni.RemoveClientAuthority();
    }

    [TargetRpc]
    public void StartGame(NetworkConnection conn)
    {
        //Show enemy player
        remotePlayer.InitBuildings();

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
        PrintDeckToConsole();

        //Deal cards to hand
        DealHand();
    }

    [Client]
    void DealHand()
    {
        for (int i = localHand.Size(); i < handSize; i++)
        {
            localHand.DrawCard(deck[i]);
            deck.RemoveAt(i);
            CmdMakeEnemyDraw();
        }
    }

    [Command(requiresAuthority = false)]
    private void CmdMakeEnemyDraw()
    {
        Debug.Log("Make it");
        RpcMakeEnemyDraw();
    }

    [TargetRpc]
    void RpcMakeEnemyDraw()
    {
        remoteHand.DrawCard();
    }

    [Client]
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
        }
    }

    [Client]
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
        if(isLocalPlayerTurn)
        {
            ActivateUIButtons();
        }
    }

    private void ActivateUIButtons()
    {
        throw new NotImplementedException();
    }
}
