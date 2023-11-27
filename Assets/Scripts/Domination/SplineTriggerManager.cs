using System;
using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;
using UnityEngine.Events;

public class SplineTriggerManager : MonoBehaviour
{
    [SerializeField] private SplineComputer _splineComputer;
    [SerializeField] private float count;
    [SerializeField] private CheckPoints checkPoints;

    private void Start()
    {
        Create();
    }
#if (UNITY_EDITOR)
    [ContextMenu("Spline Trigger Create")]
    void Create()
    {
        float pos = 0.7f / count;
        TriggerGroup triggerGroup = _splineComputer.triggerGroups[0];
        var spline = new SplineTrigger[(int)count];
        for (int i = 0; i < count; i++)
        {
           
            SplineTrigger trigger = new SplineTrigger(SplineTrigger.Type.Double);
            trigger.name = i.ToString();
            trigger.position = (pos*i)+pos;
            trigger.color=Color.green; 
            trigger.onCross.AddListener(checkPoints.SetHomeState);
            spline[i] = trigger;

        }

        triggerGroup.triggers = spline;
    }
   

  


#endif
}
