using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class EnemyPlayerHand : NetworkBehaviour
{

    [SerializeField] Transform handLayout;
    [SerializeField] GameObject cardPrefab;


    [TargetRpc]
    public void DrawCard()
    {
        GameObject cardObject = Instantiate(cardPrefab);
        cardObject.transform.SetParent(handLayout.transform, false);
    }
}
