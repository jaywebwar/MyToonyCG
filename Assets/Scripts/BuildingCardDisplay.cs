using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingCardDisplay : CardDisplay
{

    public TextMeshProUGUI turnsToCreate;


    protected override void SetCardData()
    {
        base.SetCardData();

        turnsToCreate.SetText(thisCard.turnsToCreate.ToString());
    }
}
