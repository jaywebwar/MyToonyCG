using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class EnemyPlayer : NetworkBehaviour
{
    [SerializeField] List<GameObject> primaryBuildings;
    [SerializeField] List<GameObject> secondaryBuildings;
    [SerializeField] List<GameObject> keepTowers;
    [SerializeField] GameObject keep;
    EnemyPlayerHand _ph;

    [Client]
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
}
