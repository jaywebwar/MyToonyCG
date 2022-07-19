using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class LocalPlayer : NetworkBehaviour
{

    [SerializeField] List<GameObject> primaryBuildings;
    [SerializeField] List<GameObject> secondaryBuildings;
    [SerializeField] List<GameObject> keepTowers;
    [SerializeField] GameObject keep;
    [SerializeField] List<GameObject> playerMatArray = new List<GameObject>();
    GameManager _gm;

    
    bool localPlayerHasEnemy = false;



    [Client]
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

}
