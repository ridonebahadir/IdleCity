using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CharacterUpgradePanel : MonoBehaviour
{
   
    public SOAgent soAgent;
    public SOAgentUpgrade soAgentUpgrade;
    [SerializeField] private TextMeshProUGUI characterName;
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private Image sliderFilled;
    [SerializeField] private Image nextIcon;
    [SerializeField] private Image currentIcon;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI levelText; 
    [SerializeField] private TextMeshProUGUI digSpeedText;
    [SerializeField] private TextMeshProUGUI _ratioHealthText;
    [SerializeField] private TextMeshProUGUI _ratioDamageText;
    [SerializeField] private TextMeshProUGUI _ratioDiggSpeedText;
    
    [SerializeField] private Button button;
    [SerializeField] private Button closeButton;
     private int _stageCount;
    
    private void Start()
    {
        _stageCount = soAgentUpgrade.stageCount;
        SetCost();
        SetHealth(0);
        SetDamage(0);
        SetSlider();
        SetDigSpeed(0);
        digSpeedText.SetText("Digg Speed = "+soAgent.digSpeed);
        levelText.SetText("Level = "+soAgentUpgrade.stage);
        characterName.SetText(soAgentUpgrade.name);
        //currentIcon.sprite = soAgentUpgrade.icon;
        nextIcon.sprite = soAgentUpgrade.nextIcon;
        button.onClick.AddListener(Clicked);
        closeButton.onClick.AddListener(CloseButton);
    }

    private void SetCost()
    {
        soAgentUpgrade.cost += ((soAgentUpgrade.stage+1)*(soAgentUpgrade.level+1));
        costText.SetText(soAgentUpgrade.cost.ToString());
    }

    private void SetSlider()
    {
        progressText.SetText(soAgentUpgrade.stage.ToString()+" / "+_stageCount);
       
        if (soAgentUpgrade.stage==0)
        {
            sliderFilled.fillAmount= 0;
        }
        else
        {
            var a= (float)(soAgentUpgrade.stage)/_stageCount;
            sliderFilled.fillAmount= a;
           
        }
       
    }

    private void SetHealth(int value)
    {
        soAgent.health += value;
        _ratioHealthText.SetText("+" + value);
        healthText.SetText("Health = "+soAgent.health.ToString());
    }

    private void SetDamage(int value)
    {
        soAgent.damage += value;
        _ratioDamageText.SetText("+" + value);
        damageText.SetText("Damage = "+soAgent.damage.ToString());
    }

    private void SetLevel()
    {
        //soAgent.level++;
        soAgentUpgrade.stage++;
        if (soAgentUpgrade.stage == _stageCount)
        {
            soAgentUpgrade.level++;
            soAgentUpgrade.stage = 1;
        }
        levelText.SetText("Level = "+soAgentUpgrade.level);
    }

    private void SetDigSpeed(float value)
    {
        soAgent.digSpeed += value;
        _ratioDiggSpeedText.SetText("+" + value);
        digSpeedText.SetText("Digg Speed = "+soAgent.digSpeed);
    }
    private void Clicked()
    {
        if (soAgentUpgrade.level>=5 && soAgentUpgrade.stage>=4) return;
        SetValue();
      
    }

    private void SetValue()
    {
        // if (soAgentUpgrade.level == _stageCount)
        // {
        //     sliderFilled.fillAmount = 1;
        //     return;
        // }
        SetLevel();
        SetCost();
        SetHealth(1);
        SetDamage(1);
        SetSlider();
        SetDigSpeed(0.5f);
        nextIcon.sprite = soAgentUpgrade.nextIcon;
        onClickUpgrade?.Invoke();
    }
    public delegate void OnClickClose();
    public static OnClickClose onClickClose;
    public static OnClickClose onClickUpgrade;
    private void CloseButton()
    {
        onClickClose?.Invoke();
    }
   
}
