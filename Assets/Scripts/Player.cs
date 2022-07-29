using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;

public class Player : NetworkBehaviour
{


    GameManager _gm;
    RTSManager _rtsm;
    [SerializeField] int playerNumber = 0;

    bool allPlayersAreConnected = false;

    public override void OnStartClient()
    {
        base.OnStartClient();

        //If we are not this player, disable the camera attached to this player.
        if(!isLocalPlayer)
        {
            transform.GetComponentInChildren<Camera>().gameObject.SetActive(false);
        }

        //Get references for later
        _gm = FindObjectOfType<GameManager>();
        _rtsm = gameObject.GetComponent<RTSManager>();

        //Wait for everyone to connect...
        StartCoroutine(WaitForPlayersToConnect());
        _rtsm.InitBuildings();//Set locally while waiting.
    }

    IEnumerator WaitForPlayersToConnect()
    {
        while(!allPlayersAreConnected)
        {
            yield return null;
        }
        //Once all players connect, set buildings to their initial state.
        _rtsm.CmdInitBuildings();
    }

    public void SetAllPlayersAreConnected(bool b)
    {
        allPlayersAreConnected = b;
    }

    public void AssignLeader(string name)
    {
        _rtsm.AssignLeader(name);
    }

    public void SpawnLeader()
    {
        Debug.Log("Spawning Leader");
        _rtsm.SpawnLeader();
    }

    public void SetPlayerNumber(int number)
    {
        playerNumber = number;
    }

    public int GetPlayerNumber()
    {
        return playerNumber;
    }
}
