using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public delegate void ClickAction();
    public static event ClickAction OnClickedShiftButton;

   public void OnOnClickedShiftButton()
    {
        OnClickedShiftButton?.Invoke();
    }
}
