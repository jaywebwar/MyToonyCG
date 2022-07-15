using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public string cardName;
    public int nameFontSize;
    public int tier;
    public Sprite tierLevel;
    public int turnsToCreate;
    public string buildingType;
    public Sprite cardBackgroundIcon;
    public string description;

}
