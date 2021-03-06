using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSBuilding : MonoBehaviour
{
    public Building thisBuilding;
    public TeamColor thisTeam;


    public void SetThisBuilding(Building building)
    {
        thisBuilding = building;
    }

    public void SetThisTeamColor(TeamColor color)
    {
        thisTeam = color;
    }

    void OnEnable()
    {
        //set material from Scriptable Object
        GetComponent<MeshRenderer>().material = thisTeam.buildingColor;

        GetComponent<MeshFilter>().mesh = thisBuilding.buildingPrefab.GetComponent<MeshFilter>().sharedMesh;
        GetComponent<BoxCollider>().center = thisBuilding.buildingPrefab.GetComponent<BoxCollider>().center;
        GetComponent<BoxCollider>().size = thisBuilding.buildingPrefab.GetComponent<BoxCollider>().size;

        gameObject.transform.localScale = thisBuilding.buildingPrefab.transform.localScale;
    }
}
