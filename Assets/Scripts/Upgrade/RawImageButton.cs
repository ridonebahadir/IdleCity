using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RawImageButton : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public delegate void OnClickRawImage();
    public static OnClickRawImage onClickRawImageEnter;
    public static OnClickRawImage onClickRawImageExit;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        onClickRawImageEnter?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onClickRawImageExit?.Invoke();
    }
}
