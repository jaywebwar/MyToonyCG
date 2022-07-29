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
    Player _localPlayer;


    void Start()
    {
        _gm = FindObjectOfType<GameManager>();
        foreach(var player in FindObjectsOfType<Player>())
        {
            if (player.isLocalPlayer)
            {
                _localPlayer = player;
            }
        }
    }

    protected override void SetCardData()
    {
        cardName.SetText(thisCard.cardName);
        cardName.fontSize = thisCard.nameFontSize;

        tier.SetText(thisCard.tier.ToString());
        tierLevel.sprite = thisCard.tierLevel;

        backgroundIcon.sprite = thisCard.cardBackgroundIcon;
        cardType.SetText(thisCard.buildingType);

        cardDescription.SetText(thisCard.description);
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
