using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerHand : NetworkBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] Transform handLayout;
    [SerializeField] GameObject cardPrefab;

    int size = 0;

    [Client]
    public void DrawCard(Card card)
    {
        GameObject cardObject = Instantiate(cardPrefab.gameObject);
        cardObject.transform.SetParent(handLayout, false);

        cardObject.GetComponent<CardDisplay>().SetThisCard(card);
        size++;
    }

    [Client]
    public void PlayCard(Card card)
    {
        size--;
    }

    [Client]
    public void DiscardCard(Card card)
    {
        size--;
    }

    void Update()
    {
        
    }

    public int Size()
    {
        return size;
    }
}
