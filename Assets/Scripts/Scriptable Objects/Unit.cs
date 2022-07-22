using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit", menuName = "Unit")]
public class Unit : ScriptableObject
{
    public GameObject unitPrefab;
    public string unitName;
    public bool isMounted = false;
    public bool isSiegeEquipment = false;
}
