using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Civilian : Human
{
    private bool _isWork = true;
    private void Start()
    {
        SleepCoroutine = MoveToSleep();
        WorkCoroutine = MoveToWork(true);
        HumanPoints( GameManager.Instance.buildManager.civilBuilding);
        StartCoroutine(WorkCoroutine);
        shiftControl.onClick.AddListener(ShiftControl);
    }
    void ShiftControl()
    {
        if (_isWork)
        {
                
            StopCoroutine(WorkCoroutine);
            StartCoroutine(SleepCoroutine);
            _isWork = false;
        }
        else
        {
            StartCoroutine(WorkCoroutine);
            StopCoroutine(SleepCoroutine);
            _isWork = true;
        }
    }

   
}
