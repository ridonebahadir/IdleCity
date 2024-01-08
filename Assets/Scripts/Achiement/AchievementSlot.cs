using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementSlot : MonoBehaviour
{
    [SerializeField] private SOAchievement soAchievement;
    private GameManager _gameManager;
    [SerializeField] private Image fill;
    [SerializeField] private TextMeshProUGUI fillText;
    [SerializeField] private Button _button;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI missionInfo;

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _button.onClick.AddListener(GetReward);
        icon.sprite = soAchievement.icon;
        missionInfo.SetText(soAchievement.missionInfo);
    }

    private void OnEnable()
    {
        Inıt();
    }

    public static Action SlotButtonClick;
    private void Inıt()
    {
        if (soAchievement.did)
        {
            fill.fillAmount = 1;
            fillText.SetText(soAchievement.maxValue.ToString()+" / "+soAchievement.maxValue);
            return;
        }
        SetSlider();
        _button.interactable = soAchievement.ControlMission();
    }

    private void GetReward()
    {
        _gameManager.SetTotalXp(soAchievement.reward);
        soAchievement.did = true;
        _button.interactable = false;
        SlotButtonClick?.Invoke();
    }

    private void SetSlider()
    {
        fillText.SetText(soAchievement.value.ToString()+" / "+soAchievement.maxValue);
        var a=(float)soAchievement.value/soAchievement.maxValue;
        fill.fillAmount= a;
    }
}
