using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RawImageButton : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    public delegate void OnClickRawImage();
    public static OnClickRawImage onClickRawImageEnter;
    public static OnClickRawImage onClickRawImageExit;
    public static OnClickRawImage OnClickDown;
    public static OnClickRawImage OnClickUp;
    
    
    // public void OnPointerEnter(PointerEventData eventData)
    // {
    //     //onClickRawImageEnter?.Invoke();
    // }
    //
    // public void OnPointerExit(PointerEventData eventData)
    // {
    //     //onClickRawImageExit?.Invoke();
    // }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnClickDown?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnClickUp?.Invoke();
    }
}
