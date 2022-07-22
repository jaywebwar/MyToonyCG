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
    [SerializeField] int handSize = 8;

    [Header("Client References")]
    [SerializeField] LocalPlayer localPlayer;
    [SerializeField] EnemyPlayer remotePlayer;
    [SerializeField] PlayerHand localHand;
    [SerializeField] EnemyPlayerHand remoteHand;
    [SerializeField] GameObject attackButton;
    [SerializeField] GameObject endTurnButton;

    //server properties
    NetworkConnection player1Connection;
    NetworkConnection player2Connection;
    int leaderIndexToIgnore = -1;

    //client properties
    bool isLocalPlayerTurn = false;
    int playerCount = 0;
    const int maxPlayerCount = 2;
    List<Card> deck = new List<Card>();
    List<int> discardPile = new List<int>();
    NetworkIdentity _ni;
    bool RpcReceived;



    [Server]
    public IEnumerator PassConnections(NetworkConnectionToClient conn1, NetworkConnectionToClient conn2)
    {
        //Set connections
        _ni = GetComponent<NetworkIdentity>();

        Debug.Log("Set Player1");
        _ni.AssignClientAuthority(conn1);
        player1Connection = _ni.connectionToClient;
        RpcReceived = false;
        RpcSetPlayer1(player1Connection);
        while(!RpcReceived)
        {
            yield return null;
        }
        _ni.RemoveClientAuthority();
    

        Debug.Log("Set player2Connection");
        _ni.AssignClientAuthority(conn2);
        player2Connection = _ni.connectionToClient;
        RpcReceived = false;
        RpcSetPlayer2(player2Connection);
        while(!RpcReceived)
        {
            yield return null;
        }
        _ni.RemoveClientAuthority();
        

        
        Debug.Log("Start p1 game");
        RpcReceived = false;
        RpcStartGame(player1Connection);
        while(!RpcReceived)
        {
            yield return null;
        }

        Debug.Log("Start p2 game");
        RpcReceived = false;
        RpcStartGame(player2Connection);
        while(!RpcReceived)
        {
            yield return null;
        }
        
    }

    [Command]
    public void CmdMakeLeaderChoice(int index)
    {
        if(leaderIndexToIgnore < 0)
        {
            leaderIndexToIgnore = index;
        }
        else
        {

        }
    }

    [Client]
    public int GetPlayerNumber()
    {
        return player1Connection == null ? 2 : 1;
    }

    [Command(requiresAuthority=false)]
    public void CmdHighlightEnemyHandCard(int index, bool shouldHighlight, int playerNumber)
    {
        Debug.Log("Server determines which player to RPC.");
        Debug.Log("Server says index = " + index.ToString());
        if(playerNumber == 1)
        {
            RpcHighlightEnemyHandCard(player2Connection, index, shouldHighlight);
        }
        else
        {
            RpcHighlightEnemyHandCard(player1Connection, index, shouldHighlight);
        }
    }

    [TargetRpc]
    public void RpcHighlightEnemyHandCard(NetworkConnection conn, int index, bool shouldHighlight)
    {
        Debug.Log("Client Receives index = " + index.ToString());
        GameObject card = remoteHand.transform.GetChild(0).GetChild(0).GetChild(index).gameObject;
        Debug.Log("Card = " + card.ToString());
        remoteHand.EmphasizeCard(card, shouldHighlight);
    }

    [Command(requiresAuthority=false)]
    public void RpcWasReceived()
    {
        RpcReceived = true;
        Debug.Log("Rpc Received");
    }

    [TargetRpc]
    public void RpcSetPlayer1(NetworkConnection conn)
    {
        player1Connection = conn;
        Debug.Log("p1 = " + player1Connection.ToString());

        RpcWasReceived();
    }

    [TargetRpc]
    public void RpcSetPlayer2(NetworkConnection conn)
    {
        player2Connection = conn;
        Debug.Log("p1 = " + player2Connection.ToString());

        RpcWasReceived();
    }

    [TargetRpc]
    public void RpcStartGame(NetworkConnection conn)
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
        DealHand(conn);

        //Make it Player 1's turn.
        isLocalPlayerTurn = conn == player1Connection ? true : false;
        RpcWasReceived();
    }

    [Client]
    void DealHand(NetworkConnection conn)
    {
        for (int i = localHand.Size(); i < handSize; i++)
        {
            localHand.DrawCard(deck[i]);
            deck.RemoveAt(i);
            if (conn == player1Connection)
            {
                CmdMakePlayerDraw(2);
            }
            else
            {
                CmdMakePlayerDraw(1);
            }
                
        }
    }

    [Command(requiresAuthority=false)]
    public void CmdMakePlayerDraw(int playerNumber)
    {
        if(playerNumber == 1)
        {
            RpcMakeEnemyDraw(player1Connection);
        }
        else
        {
            RpcMakeEnemyDraw(player2Connection);
        }
        
    }

    [TargetRpc]
    public void RpcMakeEnemyDraw(NetworkConnection conn)
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
            ActivateUIButtons(true);
        }
        else
        {
            ActivateUIButtons(false);
        }
    }

    void ActivateUIButtons(bool activate)
    {
        attackButton.SetActive(activate);
        endTurnButton.SetActive(activate);
    }
}
