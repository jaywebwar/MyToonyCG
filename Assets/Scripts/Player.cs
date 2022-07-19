using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Player : NetworkBehaviour
{
    GameManager _gm;

    [Client]
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        //Hide our starting buildings
        FindObjectOfType<LocalPlayer>().InitBuildings();
        FindObjectOfType<EnemyPlayer>().HideAllBuildings();
    }
}
