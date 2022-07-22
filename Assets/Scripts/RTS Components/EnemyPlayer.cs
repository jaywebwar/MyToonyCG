using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayer : MonoBehaviour
{
    [SerializeField] List<GameObject> primaryBuildings;
    [SerializeField] List<GameObject> secondaryBuildings;
    [SerializeField] List<GameObject> keepTowers;
    [SerializeField] GameObject keep;
    EnemyPlayerHand _ph;

    [SerializeField] RTSUnit leader;

    public void HideAllBuildings()
    {
        //Hides all buildings and building mat
        for (int i = 0; i < primaryBuildings.Count; i++)
        {
            primaryBuildings[i].SetActive(false);
            secondaryBuildings[i].SetActive(false);
        }
        keepTowers[0].SetActive(false);
        keepTowers[1].SetActive(false);
        keep.SetActive(false);
    }

    public void InitBuildings()
    {
        keepTowers[0].SetActive(true);
        keepTowers[1].SetActive(true);
        keep.SetActive(true);
    }

    public void AssignLeader(string name)
    {
        Instantiate(leader,keep.transform.GetChild(0));
        foreach (var unit in keep.GetComponent<RTSBuilding>().thisBuilding.spawnableUnits)
        {
            if(unit.unitName == name)
            {
                leader.SetThisUnit(unit);
            }
        }
    }
}
