using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MyNetworkManager : NetworkManager
{
    [SerializeField] GameManager gameManager;

    NetworkConnectionToClient conn1 = null;
    NetworkConnectionToClient conn2 = null;

    //Server side Overrides
    public override void OnStartServer()
    {
        base.OnStartServer();
        Debug.Log("Server Started!");
    }

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn); 
        Debug.Log("Client " + conn.ToString() + " connected to server.");
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);
        Debug.Log("Client " + conn.ToString() + " Disconnected.");
    }

    public override void OnServerReady(NetworkConnectionToClient conn)
    {
        base.OnServerReady(conn);
        Debug.Log("Client " + conn.ToString() + " is loaded and ready!");
        if(conn1 == null)
        {
            //first player connects
            conn1 = conn;
        }
        else if (conn2 == null)
        {
            //second player connects
            conn2 = conn;
            Debug.Log("Both players connected & ready!");
            gameManager.PassConnections(conn1, conn2);
        }
    }


    //Client side Overrides
    public override void OnStartClient()
    {
        base.OnStartClient();
        Debug.Log("Client is starting...");
    }
    public override void OnClientConnect()
    {
        base.OnClientConnect();
        Debug.Log("Client has connected to server.");
    }
    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        Debug.Log("Client disconnected from server.");
    }
    public override void OnStopClient()
    {
        base.OnStopClient();
        Debug.Log("Your client has stopped.");
    }
}
