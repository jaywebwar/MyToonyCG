using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RTSManager : NetworkBehaviour
{
    [SerializeField] List<GameObject> primaryBuildings;
    [SerializeField] List<GameObject> secondaryBuildings;
    [SerializeField] List<GameObject> keepTowers;
    [SerializeField] GameObject keep;
    [SerializeField] List<GameObject> playerMatArray;

    [SerializeField] TeamColor selfColor;
    [SerializeField] TeamColor enemyColor;

    string leaderName;

    [Command(requiresAuthority = false)]
    public void CmdInitBuildings()
    {
        Debug.Log("Server Init Buildings.");
        InitBuildings();
        //RpcInitBuildings();
    }

    [ClientRpc]
    public void RpcInitBuildings()
    {
        Debug.Log("ClientRpc for init buildings.");
        InitBuildings();
    }

    public void InitBuildings()
    {
        //Hides all buildings and building mat
        for (int i = 0; i < primaryBuildings.Count; i++)
        {
            primaryBuildings[i].SetActive(false);
            secondaryBuildings[i].SetActive(false);
            if (isServer) { }
            else if (isLocalPlayer)
            {
                primaryBuildings[i].GetComponent<RTSBuilding>().SetThisTeamColor(selfColor);
                secondaryBuildings[i].GetComponent<RTSBuilding>().SetThisTeamColor(selfColor);
            }
            else
            {
                primaryBuildings[i].GetComponent<RTSBuilding>().SetThisTeamColor(enemyColor);
                secondaryBuildings[i].GetComponent<RTSBuilding>().SetThisTeamColor(enemyColor);
            }
        }
        foreach (var item in playerMatArray)
        {
            item.SetActive(false);
        }

        //ensures keep and towers are active and sets color
        if (isServer) { }
        else if(isLocalPlayer)
        {
            keepTowers[0].GetComponent<RTSBuilding>().SetThisTeamColor(selfColor);
            keepTowers[1].GetComponent<RTSBuilding>().SetThisTeamColor(selfColor);
            keep.GetComponent<RTSBuilding>().SetThisTeamColor(selfColor);
        }
        else
        {
            keepTowers[0].GetComponent<RTSBuilding>().SetThisTeamColor(enemyColor);
            keepTowers[1].GetComponent<RTSBuilding>().SetThisTeamColor(enemyColor);
            keep.GetComponent<RTSBuilding>().SetThisTeamColor(enemyColor);
        }
        keepTowers[0].SetActive(false);
        keepTowers[1].SetActive(false);
        keep.SetActive(false);
        keepTowers[0].SetActive(true);
        keepTowers[1].SetActive(true);
        keep.SetActive(true);
    }

    public void AssignLeader(string name)
    {
        leaderName = name;
    }

    public void SpawnLeader()
    {
        foreach (var unit in keep.GetComponent<RTSBuilding>().thisBuilding.spawnableUnits)
        {
            if (unit.unitName == leaderName)
            {
                Instantiate(unit.unitPrefab, keep.transform.GetChild(0));
            }
        }
    }
}