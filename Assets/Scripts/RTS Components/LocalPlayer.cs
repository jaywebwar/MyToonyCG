using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LocalPlayer : MonoBehaviour
{

    [SerializeField] protected List<GameObject> primaryBuildings;
    [SerializeField] protected List<GameObject> secondaryBuildings;
    [SerializeField] protected List<GameObject> keepTowers;
    [SerializeField] protected GameObject keep;
    [SerializeField] List<GameObject> playerMatArray = new List<GameObject>();
    GameManager _gm;
    [SerializeField] RTSUnit leader;
    List<RTSUnit> army = new List<RTSUnit>();



    public void InitBuildings()
    {
        //Hides all buildings and building mat
        for (int i = 0; i < primaryBuildings.Count; i++)
        {
            primaryBuildings[i].SetActive(false);
            secondaryBuildings[i].SetActive(false);
        }
        foreach (var item in playerMatArray)
        {
            item.SetActive(false);
        }

        //ensures keep and towers are active
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
