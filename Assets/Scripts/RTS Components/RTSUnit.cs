using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSUnit : MonoBehaviour
{
    [SerializeField] Unit thisUnit;
    [SerializeField] TeamColor teamColor;

    void OnEnable()
    {
        List<GameObject> childrenToSkin = new List<GameObject>();
        if(thisUnit.isMounted)
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                if(transform.GetChild(i).childCount != 0)
                {
                    Transform mountedSpine = transform.GetChild(i).GetChild(1).GetChild(0).GetChild(4).GetChild(0).GetChild(1).GetChild(2);
                    Transform shieldParent = mountedSpine.GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetChild(1);
                    AddChildrenToList(childrenToSkin, shieldParent);
                    Transform headParent = mountedSpine.GetChild(2).GetChild(0).GetChild(1);
                    AddChildrenToList(childrenToSkin, headParent);
                    Transform weaponParent = mountedSpine.GetChild(3).GetChild(0).GetChild(0).GetChild(0).GetChild(0);
                    AddChildrenToList(childrenToSkin, weaponParent);

                }
                else
                {
                    childrenToSkin.Add(transform.GetChild(i).gameObject);
                }
            }
        }
        else if(thisUnit.isSiegeEquipment)
        {
            for(int i = 0; i< transform.childCount; i++)
            {
                if(transform.GetChild(i).childCount == 0)
                {
                    childrenToSkin.Add(transform.GetChild(i).gameObject);
                }
            }
        }
        else
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                if(transform.GetChild(i).childCount != 0)
                {
                    Transform spine = transform.GetChild(i).GetChild(1).GetChild(2);

                    Transform leftHandParent = spine.GetChild(1).GetChild(0).GetChild(0).GetChild(0);
                    Transform leftWeaponParent = leftHandParent.GetChild(0);
                    AddChildrenToList(childrenToSkin, leftWeaponParent);
                    Transform shieldParent = leftHandParent.GetChild(1);
                    AddChildrenToList(childrenToSkin, shieldParent);

                    Transform headParent = spine.GetChild(2).GetChild(0).GetChild(1);
                    AddChildrenToList(childrenToSkin, headParent);

                    Transform weaponParent = spine.GetChild(3).GetChild(0).GetChild(0).GetChild(0).GetChild(0);
                    AddChildrenToList(childrenToSkin, weaponParent);

                }
                else
                {
                    childrenToSkin.Add(transform.GetChild(i).gameObject);
                }
            }
        }
        for(int i = 0; i < childrenToSkin.Count; i++)
        {
            childrenToSkin[i].GetComponent<MeshRenderer>().material = teamColor.teamColor;
        }
    }

    void AddChildrenToList(List<GameObject> List, Transform parent)
    {
        for (int j = 0; j < parent.childCount; j++)
        {
            List.Add(parent.GetChild(j).gameObject);
        }
    }

    public void SetThisUnit(Unit unit)
    {
        thisUnit = unit;
    }
}
