using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSUnit : NetworkBehaviour
{
    [SerializeField] Unit thisUnit;
    [SerializeField] TeamColor teamColor;

    RTSManager _rtsm;
    Rigidbody _rb;
    Animator _animator;
    bool shouldStopRunning;
    [SyncVar][SerializeField] int owningPlayerNumber = 0;
    bool owningBuildingSet;
    private Vector3 destination;
    private bool unitIsInit = false;

    bool isLeaderUnit = false;
    [SyncVar] int owningBuildingIndex;
    GameObject owningBuilding;
    

    //void Update()
    //{
    //    SimpleNetworkedMovementTest();
    //}

    //private void SimpleNetworkedMovementTest()
    //{
    //    if(hasAuthority)
    //    {
    //        float moveHorizontal = Input.GetAxis("Horizontal");
    //        float moveVeritcal = Input.GetAxis("Vertical");

    //        if (!(moveHorizontal == 0f && moveVeritcal == 0f))
    //        {
    //            Debug.Log("Movement Detected..");
    //        }
    //        else
    //        {
    //            Debug.Log("Should be stationary..");
    //        }

    //        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVeritcal);

    //       transform.position = transform.position + movement;
    //    }
    //}

    //void OnEnable()
    //{
    //    StartCoroutine(WaitForOwningPlayerDetailsToBeSet());
    //}

    //IEnumerator WaitForOwningPlayerDetailsToBeSet()
    //{
    //    //while(owningPlayerNumber == 0)
    //    //{
    //    //    yield return null;
    //    //}

    //    while(!owningBuildingSet)
    //    {
    //        yield return null;
    //    }
    //    ContinueOnEnable();
    //}

    void OnEnable()
    {
        string thisUnitName = thisUnit.unitName;

        if (thisUnitName == "Arch Mage"
        || thisUnitName == "King"
        || thisUnitName == "Noble"
        || thisUnitName == "Ranger")
        {
            isLeaderUnit = true;
        }
        else
        {
            isLeaderUnit = false;
        }

        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();

        Player[] players = FindObjectsOfType<Player>();
        foreach (Player player in players)
        {
            if (player.GetPlayerNumber() == owningPlayerNumber)
            {
                _rtsm = player.GetComponent<RTSManager>();
            }
        }

        SetOwningBuilding();

        if (_rtsm.GetComponent<Player>().GetPlayerNumber() == owningPlayerNumber)
        {
            //We are the owning player
            teamColor = _rtsm.GetSelfColor();
        }
        else
        {
            //enemy owned unit
            teamColor = _rtsm.GetEnemyColor();
        }

        Debug.Log("I am " + thisUnit.unitName + ". I belong to Player " + owningPlayerNumber + ". My color on this client is " + teamColor.unitColor.name);

        TextureUnit();

        
        if(isServer)
        {
            Transform rallyPoint = transform;

            RTSBuilding myBuilding = owningBuilding.GetComponent<RTSBuilding>();
            if(myBuilding.thisBuilding.buildingName == "Keep 1"
                || myBuilding.thisBuilding.buildingName == "Keep 2"
                || myBuilding.thisBuilding.buildingName == "Keep 3")
            {
                for (int i = 0; i < myBuilding.transform.parent.childCount; i++)
                {
                    if(myBuilding.transform.parent.GetChild(i).childCount == 0)
                    {
                        //Must be the Rally point set for leaders
                        rallyPoint = myBuilding.transform.parent.GetChild(i);
                    }
                }
            }
            else
            {
                //Use Building Rally Point
                rallyPoint = myBuilding.transform.GetChild(1);
            }
            SetMovementDestination(rallyPoint);
            Debug.Log("Should move the unit toward the rally point.");
        }
        

        unitIsInit = true;
    }

    [Server]
    public void SetOwningPlayerNumber(int playerNumber)
    {
        owningPlayerNumber = playerNumber;
    }

    void SetMovementDestination(Transform rallyPoint)
    {
        destination = rallyPoint.position;
        shouldStopRunning = false;
    }

    void FixedUpdate()
    {
        if(unitIsInit)
        {
            if (!shouldStopRunning)
            {
                MoveTo(destination);
            }
        }
    }

    void MoveTo(Vector3 position)
    {
        transform.LookAt(position);
        //Vector3 moveDirection = position - transform.position;


        //_rb.velocity = transform.forward;
        //if (transform.position.x == position.x && transform.position.z == position.z)
        //{
        //    shouldStopRunning = true;
        //}
    }

    void Update()
    {
        // Animations
        // Run
        if(unitIsInit)
        {
            AnimatorSetRunning(!shouldStopRunning);
        }
    }

    void AnimatorSetRunning(bool isRunning)
    {
        _animator.SetBool("isRunning", isRunning);
    }

    void TextureUnit()
    {
        List<GameObject> childrenToSkin = new List<GameObject>();
        if (thisUnit.isMounted)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).childCount != 0)
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
                    if (transform.GetChild(i).gameObject.activeSelf)
                    {
                        transform.GetChild(i).gameObject.GetComponent<SkinnedMeshRenderer>().material = teamColor.unitColor;
                    }
                }
            }
        }
        else if (thisUnit.isSiegeEquipment)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).childCount == 0)
                {
                    childrenToSkin.Add(transform.GetChild(i).gameObject);
                }
            }
        }
        else
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).childCount != 0)
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
                    if (transform.GetChild(i).gameObject.activeSelf)
                    {
                        transform.GetChild(i).gameObject.GetComponent<SkinnedMeshRenderer>().material = teamColor.unitColor;
                    }
                }
            }
        }
        for (int i = 0; i < childrenToSkin.Count; i++)
        {
            childrenToSkin[i].GetComponent<MeshRenderer>().material = teamColor.unitColor;
        }
    }

    void AddChildrenToList(List<GameObject> List, Transform parent)
    {
        for (int j = 0; j < parent.childCount; j++)
        {
            if(parent.GetChild(j).gameObject.activeSelf)
            {
                List.Add(parent.GetChild(j).gameObject);
            }
        }
    }

    public void SetThisUnit(Unit unit)
    {
        thisUnit = unit;
    }

    [Server]
    public void SetOwningBuildingIndex(int buildingIndex)
    {
        owningBuildingIndex = buildingIndex;
    }

    void SetOwningBuilding()
    {
        if(owningBuildingIndex == -1)
        {
            owningBuilding = _rtsm.GetKeep();
        }
        else
        {
            owningBuilding = _rtsm.GetPrimaryBuilding(owningBuildingIndex);
        }
    }
}
