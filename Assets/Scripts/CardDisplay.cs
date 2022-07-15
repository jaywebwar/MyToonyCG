using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CardDisplay : MonoBehaviour
{

    public Card thisCard;

    public TextMeshProUGUI cardName;
    public TextMeshProUGUI tier;
    public TextMeshProUGUI turnsToCreate;
    public Image hourglassImage;
    public Image tierLevel;
    public Image backgroundIcon;
    public TextMeshProUGUI buildingType;
    public TextMeshProUGUI cardDescription;

    Card currentCard;


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

    void SetCardData()
    {
        
        cardName.SetText(thisCard.cardName);
        cardName.fontSize = thisCard.nameFontSize;

        tier.SetText(thisCard.tier.ToString());
        tierLevel.sprite = thisCard.tierLevel;

        backgroundIcon.sprite = thisCard.cardBackgroundIcon;
        buildingType.SetText(thisCard.buildingType);

        if (thisCard.buildingType == "Event" || thisCard.buildingType == "Condition")
        {
            //turn off unnecessary UI components for Event & Condition cards
            turnsToCreate.gameObject.SetActive(false);
            hourglassImage.gameObject.SetActive(false);
        }
        else
        {
            turnsToCreate.SetText(thisCard.turnsToCreate.ToString());
        }
        cardDescription.SetText(thisCard.description);
    }
}
