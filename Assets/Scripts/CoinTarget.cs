using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinTarget : MonoBehaviour
{
    public Camera mainCamera;
    public RectTransform uiObject;
    void Update()
    {
        // UI objesinin ekran koordinatlarını al
        Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(mainCamera, uiObject.position);

        // Ekran koordinatlarını dünya koordinatlarına dönüştür
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, Mathf.Abs(mainCamera.transform.position.z - uiObject.position.z)));

        // Küpü sadece X ve Z eksenlerinde konumlandır
        gameObject.transform.position = new Vector3(worldPosition.x, gameObject.transform.position.y, worldPosition.z);
    }
}
