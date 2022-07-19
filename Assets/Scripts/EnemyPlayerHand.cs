using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class EnemyPlayerHand : NetworkBehaviour
{

    [SerializeField] GameManager gameManager;
    [SerializeField] Transform handLayout;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] Vector3 emphasizePosition = new Vector3(0, -50, 0);
    [SerializeField] Vector3 emphasizeScale = new Vector3 (1.2f, 1.2f, 1.2f);

    [Client]
    public void DrawCard()
    {
        GameObject cardObject = Instantiate(cardPrefab);
        cardObject.transform.SetParent(handLayout.transform, false);
    }

    public void EmphasizeCard(GameObject cardUI, bool shouldEmphasize)
    {
        if(cardUI.GetComponent<CardGUIBehavior>() != null)
        {
            RectTransform _rt = cardUI.GetComponent<RectTransform>();
            if(shouldEmphasize)
            {
                _rt.localPosition = _rt.localPosition + emphasizePosition;
                _rt.localScale = emphasizeScale;
            }
            else
            {
                _rt.localPosition = _rt.localPosition - emphasizePosition;
                _rt.localScale = Vector3.one;
            }
        }
    }
}
