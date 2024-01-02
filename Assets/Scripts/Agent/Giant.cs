using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Giant : MonoBehaviour
{
    [SerializeField] private SOAgentUpgrade soAgentUpgrade;
    [SerializeField] private Transform levelParent;
    
    private void Start()
    {
        foreach (Transform item in levelParent)  item.gameObject.SetActive(false);
        Debug.Log("Level = "+soAgentUpgrade.level);
        levelParent.GetChild(soAgentUpgrade.level-1).gameObject.SetActive(true);
    }
}
