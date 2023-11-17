using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : Human
{
    private bool _isWork = true;
    private void Start()
    {
        SleepCoroutine = MoveToSleep();
        WorkCoroutine = MoveToWork(false);
        
        HumanPoints( GameManager.Instance.buildManager.soldierBuilding);
        StartCoroutine(SleepCoroutine);
        shiftControl.onClick.AddListener(ShiftControl);
    }

    void ShiftControl()
    {
        if (_isWork)
        {
                
            StopCoroutine(SleepCoroutine);
            StartCoroutine(WorkCoroutine);
            _isWork = false;
        }
        else
        {
            StartCoroutine(SleepCoroutine);
            StopCoroutine(WorkCoroutine);
            _isWork = true;
        }
    }
   
}
