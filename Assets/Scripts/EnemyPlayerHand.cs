using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayerHand : MonoBehaviour
{

    [SerializeField] Transform handLayout;
    [SerializeField] GameObject cardPrefab;


    public void DrawCard()
    {
        GameObject cardObject = Instantiate(cardPrefab);
        cardObject.transform.SetParent(handLayout.transform, false);
    }
}
