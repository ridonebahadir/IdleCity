using System;
using System.Collections;
using System.Collections.Generic;
using Agent;
using UnityEngine;

public class SmallTrigger : MonoBehaviour
{
    public AgentType _agentType;
    public AgentBase agentBase;
    public bool reverseRotate;
    public bool isEnemy;
    
    private readonly Vector3 _worldForwardDirection = Vector3.forward;
    private readonly Vector3 _worldBackwardDirection = Vector3.back;
    private void LateUpdate()
    {
       
        
        if (reverseRotate)
        {
            transform.rotation = Quaternion.LookRotation(isEnemy ? _worldBackwardDirection : _worldForwardDirection, Vector3.up);
            
        }
        else
        {
            var target = agentBase.target.position;
            var targetPosition = new Vector3(target.x, transform.position.y, target.z);
            transform.LookAt(targetPosition);
        }
       
        
        
        
    }
}
