using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Color", menuName ="Team Color")]
public class TeamColor : ScriptableObject
{
    public Material buildingColor;
    public Material unitColor;
}
