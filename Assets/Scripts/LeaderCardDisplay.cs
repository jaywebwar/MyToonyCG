using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;

public class LeaderCardDisplay : CardDisplay, IPointerClickHandler
{

    GameManager _gm;
    LocalPlayer _localPlayer;

    void Start()
    {
        _gm = FindObjectOfType<GameManager>();
        _localPlayer = FindObjectOfType<LocalPlayer>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerClick.transform.GetSiblingIndex().ToString() + " is the index of the leader chosen.");
        _gm.CmdMakeLeaderChoice(eventData.pointerClick.transform.GetSiblingIndex());
        _localPlayer.AssignLeader(eventData.pointerClick.GetComponent<LeaderCardDisplay>().cardName.text);

        //remove cards from screen
        transform.parent.gameObject.SetActive(false);
    }
}
