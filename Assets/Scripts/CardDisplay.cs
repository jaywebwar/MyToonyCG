using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CardDisplay : MonoBehaviour
{

    public Card thisCard;

    public TextMeshProUGUI cardName;
    public TextMeshProUGUI tier;
    public Image tierLevel;
    public Image backgroundIcon;
    public TextMeshProUGUI cardType;
    public TextMeshProUGUI cardDescription;

    protected Card currentCard;

    public void SetThisCard(Card card)
    {
        thisCard = card;
    }

    void Start()
    {
        currentCard = thisCard;
        SetCardData();
    }

    void Update()
    {
        if (thisCard != currentCard)
        {
            currentCard = thisCard;
            SetCardData();
        }
    }

    protected virtual void SetCardData()
    {
        
        cardName.SetText(thisCard.cardName);
        cardName.fontSize = thisCard.nameFontSize;

        tier.SetText(thisCard.tier.ToString());
        tierLevel.sprite = thisCard.tierLevel;

        backgroundIcon.sprite = thisCard.cardBackgroundIcon;
        cardType.SetText(thisCard.buildingType);

        cardDescription.SetText(thisCard.description);
    }
}
