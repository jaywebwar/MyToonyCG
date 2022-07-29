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

    [SerializeField] Transform leaderCardUILayout;
    [SerializeField] GameObject leaderCardPrefab;
    [SerializeField] Card[] leaderCards;

    [Header("Game Parameters")]
    [SerializeField] int shuffleCount = 7;
    [SerializeField] int handSize = 8;

    [Header("Server References")]
    [SerializeField] Player player1;
    [SerializeField] Player player2;


    [Header("Client References")]
    [SerializeField] PlayerHand localHand;
    [SerializeField] EnemyPlayerHand remoteHand;
    [SerializeField] GameObject attackButton;
    [SerializeField] GameObject endTurnButton;

    //server properties
    NetworkConnection player1Connection;
    NetworkConnection player2Connection;
    bool rpcReceived;

    int leaderIndexToIgnore = -1;
    bool p2PickedLeader = false;
    Card p2Leader;
    bool p1PickedLeader = false;
    Card p1Leader;

    //client properties
    bool isLocalPlayerTurn = false;
    int playerCount = 0;
    const int maxPlayerCount = 2;
    List<Card> deck = new List<Card>();
    List<int> discardPile = new List<int>();

    

    [Server]
    public IEnumerator PassConnections(NetworkConnectionToClient conn1, NetworkConnectionToClient conn2)
    {
        //Set connections
        NetworkIdentity _ni = GetComponent<NetworkIdentity>();

        Debug.Log("Set Player1");
        _ni.AssignClientAuthority(conn1);
        player1Connection = _ni.connectionToClient;
        rpcReceived = false;
        RpcSetPlayerNumber(player1Connection, 1);
        while(!rpcReceived)
        {
            yield return null;
        }
        _ni.RemoveClientAuthority();
    

        Debug.Log("Set player2Connection");
        _ni.AssignClientAuthority(conn2);
        player2Connection = _ni.connectionToClient;
        rpcReceived = false;
        RpcSetPlayerNumber(player2Connection, 2);
        while(!rpcReceived)
        {
            yield return null;
        }
        _ni.RemoveClientAuthority();

        AssignPlayersOnServer();
        Debug.Log("Players assigned.");

        //Alert all players that all players have connected.
        AlertPlayersOfConnection();

        StartCoroutine(HandlePlayerPreGameChoices());
        
    }

    [ClientRpc]
    public void AlertPlayersOfConnection()
    {
        foreach (Player player in FindObjectsOfType<Player>())
        {
            if(player.isLocalPlayer)
            {
                player.SetAllPlayersAreConnected(true);
            }
        }
    }

    [TargetRpc]
    public void RpcSetPlayerNumber(NetworkConnection player1Connection, int playerNumber)
    {
        Player[] players = FindObjectsOfType<Player>();
        foreach(Player player in players)
        {
            if(player.isLocalPlayer)
            {
                //Set playerNumber on the player and set the appropriate player object in the client's GM.
                player.SetPlayerNumber(playerNumber);

                if (playerNumber == 1)
                {
                    Debug.Log("This is P1.");
                    player1 = player;
                }
                else
                {
                    Debug.Log("This is P2.");
                    player2 = player;
                }
            }
        }
        CmdRpcReceived();
    }

    [Command(requiresAuthority = false)]
    public void CmdRpcReceived()
    {
        rpcReceived = true;
    }

    [Server]
    void AssignPlayersOnServer()
    {
        Player[] players = FindObjectsOfType<Player>();
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetPlayerNumber() == 1)
            {
                player1 = players[i];
            }
            else if (players[i].GetPlayerNumber() == 2)
            {
                player2 = players[i];
            }
        }
    }

    [Server]
    private IEnumerator HandlePlayerPreGameChoices()
    {
        //TODO: Let player choose their color and their enemy's color
        //TODO: Let player choose card style and enemy card style

        //Assign Leaders
        Debug.Log("Showing Leader options to P2.");
        RpcShowLeaderOptions(player2Connection, leaderIndexToIgnore);
        while(!p2PickedLeader)
        {
            yield return null;
        }

        Debug.Log("Showing Leader options to P1.");
        RpcShowLeaderOptions(player1Connection, leaderIndexToIgnore);
        while(!p1PickedLeader)
        {
            yield return null;
        }

        Debug.Log("Server says to spawn leaders.");
        SpawnLeaders();
        //Start Game loop
        //Debug.Log("Start p1 game");
        //RpcReceived = false;
        //RpcStartGame(player1Connection);
        //while (!RpcReceived)
        //{
        //    yield return null;
        //}

        //Debug.Log("Start p2 game");
        //RpcReceived = false;
        //RpcStartGame(player2Connection);
        //while (!RpcReceived)
        //{
        //    yield return null;
        //}
    }

    [TargetRpc]
    public void RpcShowLeaderOptions(NetworkConnection conn, int indexToIgnore)
    {
        Debug.Log("Showing leader options...");
        for (int i = 0; i < leaderCards.Length; i++)
        {
            if (i != indexToIgnore)
            {
                GameObject cardObj = Instantiate(leaderCardPrefab.gameObject);
                cardObj.transform.SetParent(leaderCardUILayout, false);

                cardObj.GetComponent<LeaderCardDisplay>().SetThisCard(leaderCards[i]);
            }
        }
    }

    [Command(requiresAuthority = false)]
    public void CmdMakeLeaderChoice(int index)
    {
        if (leaderIndexToIgnore < 0)
        {
            Debug.Log("P2 picked " + leaderCards[index].cardName);
            p2PickedLeader = true;
            p2Leader = leaderCards[index];
            leaderIndexToIgnore = index;
        }
        else
        {
            if(index < leaderIndexToIgnore)
            {
                Debug.Log("P1 picked " + leaderCards[index]);
                p1Leader = leaderCards[index];
            }
            else
            {
                Debug.Log("P1 picked " + leaderCards[index+1]);
                p1Leader = leaderCards[index + 1];
            }
            p1PickedLeader = true;
        }
    }

    [Server]
    void SpawnLeaders()
    {
        RpcSpawnLeader(player1Connection, 1);
        RpcSpawnLeader(player2Connection, 2);
    }

    [TargetRpc]
    public void RpcSpawnLeader(NetworkConnection player1Connection, int playerNumber)
    {
        if(playerNumber == 1)
        {
            player1.SpawnLeader();
        }
        else
        {
            player2.SpawnLeader();
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

    [TargetRpc]
    public void RpcStartGame(NetworkConnection conn)
    {

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
