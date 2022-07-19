using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerHand : NetworkBehaviour, 
                          IDragHandler, IDropHandler, IPointerDownHandler, 
                          IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameManager gameManager;
    [SerializeField] Transform handLayout;
    [SerializeField] GameObject cardPrefab;

    int size = 0;
    [SerializeField] Vector3 emphasizePosition = new Vector3(0, 50, 0);
    [SerializeField] Vector3 emphasizeScale = new Vector3(1.2f, 1.2f, 1.2f);

    GameObject  lastPointerTarget;

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

    public int Size()
    {
        return size;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("pointer enters");
        
        lastPointerTarget = eventData.pointerEnter;
        EmphasizeCard(eventData.pointerEnter, true);
    }

    private void EmphasizeCard(GameObject cardUI, bool shouldEmphasize)
    {
        if(cardUI.GetComponent<CardDisplay>() != null)
        {
            RectTransform _rt = cardUI.GetComponent<RectTransform>();
            if(shouldEmphasize)
            {
                _rt.localPosition = _rt.localPosition + emphasizePosition;
                _rt.localScale = emphasizeScale;
            }
            else
            {
                _rt.localPosition = _rt.localPosition - emphasizePosition;
                _rt.localScale = Vector3.one;
            }

            int index = -1;
            index = cardUI.transform.GetSiblingIndex();
            int playerNumber = gameManager.GetPlayerNumber();
            if (index != -1)
            {
                Debug.Log("Sending which card to highlight");
                gameManager.CmdHighlightEnemyHandCard(index, shouldEmphasize, playerNumber);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("pointer exits");
        EmphasizeCard(lastPointerTarget, false);
        // lastPointerTarget = null;
    }

    public void OnDrag(PointerEventData eventData)
    {
        throw new NotImplementedException();
    }

    public void OnDrop(PointerEventData eventData)
    {
        throw new NotImplementedException();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        throw new NotImplementedException();
    }
}
