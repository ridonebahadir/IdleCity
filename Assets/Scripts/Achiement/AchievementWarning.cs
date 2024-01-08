using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementWarning : MonoBehaviour
{
    [SerializeField] private List<SOAchievement> soAchievements;
    [SerializeField] private GameObject warningImage;
    
    private void Start()
    {
        warningImage.SetActive(false);
        ControlWarning();
    }

    private void ControlWarning()
    {
        foreach (var t in soAchievements)
        {
            if (t.ControlMission())
            {
                warningImage.SetActive(true);
                break;
            }
            else
            {
                warningImage.SetActive(false);
            }
        }
    }

    private void OnEnable()
    {
        AchievementSlot.SlotButtonClick += ControlWarning;
    }

    private void OnDisable()
    {
        AchievementSlot.SlotButtonClick -= ControlWarning;
    }
}
