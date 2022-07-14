using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardGUIBehavior : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseOver()
    {
        Debug.Log("Hovering over image");
    }

    private void OnMouseExit()
    {
        Debug.Log("No Longer hovering");
    }
}
