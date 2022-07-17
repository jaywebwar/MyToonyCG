using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class Player : NetworkBehaviour
{

    public static Player localPlayer;

    [SerializeField] List<GameObject> primaryBuildings;
    [SerializeField] List<GameObject> secondaryBuildings;
    [SerializeField] List<GameObject> keepTowers;
    [SerializeField] GameObject keep;

    bool localPlayerHasEnemy = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        for (int i = 0; i < primaryBuildings.Count; i++)
        {
            primaryBuildings[i].SetActive(false);
            secondaryBuildings[i].SetActive(false);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if(!localPlayerHasEnemy)
        {
            UpdateEnemyInfo();
        }
        
    }

    private void UpdateEnemyInfo()
    {
        // Find all Players and add them to the list.
        Player[] onlinePlayers = FindObjectsOfType<Player>();

        // Loop through all online Players (should just be one other Player)
        foreach (Player players in onlinePlayers)
        {
            // There should only be one other Player online, so if it's not us then it's the enemy.
            if (players != this)
            {
                localPlayerHasEnemy = true;
                Debug.Log("Enemy Connected!");
            }
        }
    }
}
