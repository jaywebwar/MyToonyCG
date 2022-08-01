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

    [SerializeField] List<Building> possibleBuildings;

    [SerializeField] TeamColor selfColor;
    [SerializeField] TeamColor enemyColor;

    [SyncVar] public string leaderName;

    [Command(requiresAuthority = false)]
    public void CmdInitBuildings()
    {
        Debug.Log("Server Init Buildings.");
        InitBuildings();
    }

    public void InitBuildings()
    {
        //Hides all buildings
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
        //Hide player mat
        foreach (var item in playerMatArray)
        {
            item.SetActive(false);
        }

        //ensures keep and towers are active and sets color
        if (isServer) { }
        else if (isLocalPlayer)
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
        //Disable keep and towers
        keepTowers[0].SetActive(false);
        keepTowers[1].SetActive(false);
        keep.SetActive(false);
        //Ensure they are set properly
        keepTowers[0].GetComponent<RTSBuilding>().SetThisBuilding(possibleBuildings[GetBuildingIndexFromPossibleList("Keep Tower 1")]);
        keepTowers[1].GetComponent<RTSBuilding>().SetThisBuilding(possibleBuildings[GetBuildingIndexFromPossibleList("Keep Tower 1")]);
        keep.GetComponent<RTSBuilding>().SetThisBuilding(possibleBuildings[GetBuildingIndexFromPossibleList("Keep 1")]);
        //Enable Initialized buildings
        keepTowers[0].SetActive(true);
        keepTowers[1].SetActive(true);
        keep.SetActive(true);
    }

    private int GetBuildingIndexFromPossibleList(string buildingName)
    {
        for (int i = 0; i < possibleBuildings.Count; i++)
        {
            if (possibleBuildings[i].buildingName == buildingName)
            {
                return i;
            }
        }

        return -1;
    }

    public GameObject GetKeep()
    {
        return keep;
    }

    [Server]
    public void AssignLeader(string name)
    {
        leaderName = name;
        Debug.Log("Leader name set to " + name);
    }

    [Server]
    public void SpawnLeader(int playerNumber)
    {
        Debug.Log("Running SpawnLeader() in RTSM");
        foreach (var unit in keep.GetComponent<RTSBuilding>().thisBuilding.spawnableUnits)
        {

            if (unit.unitName == leaderName)
            {
                Debug.Log("Instantiate gets called on leader unit");
                Transform spawnPoint = keep.transform.GetChild(0);  //Important to have SpawnPoint as first child of buildings
                GameObject leaderGO = Instantiate(unit.unitPrefab, spawnPoint.position, spawnPoint.rotation);

                RTSUnit _rtsUnit = leaderGO.GetComponent<RTSUnit>();
                _rtsUnit.SetOwningPlayerNumber(playerNumber);
                _rtsUnit.SetOwningBuildingIndex(-1);

                NetworkServer.Spawn(leaderGO, GetComponent<Player>().gameObject);
            }
        }
    }

    public TeamColor GetSelfColor()
    {
        return selfColor;
    }

    public TeamColor GetEnemyColor()
    {
        return enemyColor;
    }

    public GameObject GetPrimaryBuilding(int owningBuildingIndex)
    {
        return primaryBuildings[owningBuildingIndex];
    }
}