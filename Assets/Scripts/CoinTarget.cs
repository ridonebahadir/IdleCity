using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinTarget : MonoBehaviour
{
    public Camera overlayCamera; // Overlay kamera
    public RectTransform uiObject;


    private void OnEnable()
    {
        UIManager.OnClickedBattle += Move;
    }

    private void OnDisable()
    {
        UIManager.OnClickedBattle -= Move;
    }

    void Move()
    {
        // UI objesinin dünya koordinatlarını al
        Vector3 worldPosition = overlayCamera.ScreenToWorldPoint(new Vector3(uiObject.position.x, uiObject.position.y, Mathf.Abs(overlayCamera.transform.position.z - uiObject.position.z)));

        // Küpü sadece X ve Z eksenlerinde konumlandır
        gameObject.transform.position = new Vector3(worldPosition.x+1, gameObject.transform.position.y, worldPosition.z+10);
    }
}

