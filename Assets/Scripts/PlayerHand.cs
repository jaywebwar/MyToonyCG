using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] Transform handLayout;
    [SerializeField] GameObject cardPrefab;

    public void DrawCard(Card card)
    {
        GameObject cardObject = Instantiate(cardPrefab.gameObject);
        cardObject.transform.SetParent(handLayout, false);

        cardObject.GetComponent<CardDisplay>().SetThisCard(card);
    }

    void Update()
    {
        
    }
}
