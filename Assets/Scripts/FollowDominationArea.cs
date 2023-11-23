using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowDominationArea : MonoBehaviour
{
    [SerializeField] private Transform dominationArea;
    
    void Update()
    {
        transform.position = dominationArea.position;
    }
}
