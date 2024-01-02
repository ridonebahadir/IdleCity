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
    public CharactersType CharactersType;
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
        SetHealth();
        SetDamage();
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
        soAgentUpgrade.cost = Formula(soAgentUpgrade.multipherCost.a,soAgentUpgrade.multipherCost.b,soAgentUpgrade.multipherCost.c);
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

    private void SetHealth()
    {
        soAgent.health = Formula(soAgentUpgrade.multipherHealth.a, soAgentUpgrade.multipherHealth.b,
            soAgentUpgrade.multipherHealth.c);
        //_ratioHealthText.SetText("+" + value);
        healthText.SetText("Health = "+soAgent.health.ToString());
    }

    private void SetDamage()
    {
        soAgent.damage = Formula(soAgentUpgrade.multipherDamage.a,soAgentUpgrade.multipherDamage.b,soAgentUpgrade.multipherDamage.c);
        //_ratioDamageText.SetText("+" + value);
        damageText.SetText("Damage = "+soAgent.damage.ToString());
    }

    private void SetLevel()
    {
        //soAgent.level++;
        soAgentUpgrade.stage++;
        if (soAgentUpgrade.stage == _stageCount+1)
        {
            soAgentUpgrade.level++;
            soAgentUpgrade.stage = 1;
            nextIcon.sprite = soAgentUpgrade.nextIcon;
            UpgradeType();
            
        }
        levelText.SetText("Level = "+soAgentUpgrade.level);
    }
    private void SetTotalLevel()
    {
        soAgentUpgrade.totalLevel++;
    }

    private void UpgradeType()
    {
        onClickUpgrade?.Invoke();
        switch (CharactersType)
        {
            case CharactersType.Melee:
                onClickUpgradeMelee?.Invoke();
                break;
            case CharactersType.Archer:
                onClickUpgradeArcher?.Invoke();
                break;
            case CharactersType.Digger:
                onClickUpgradeDigger?.Invoke();
                break;
            case CharactersType.Giant:
                onClickUpgradeGiant?.Invoke();
                break;
        }
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

    

    private int Formula(float a, float b, float c)
    {
        // math.ceil((a*(i**2)+b*i+c)*(1.2**((i-1)//5)))
        var formula = Math.Ceiling(
            (a * Mathf.Pow(soAgentUpgrade.totalLevel, 2) + (b * soAgentUpgrade.totalLevel) + c)
            * Math.Pow(1.2,(soAgentUpgrade.totalLevel-1)/5));
        return (int)formula;
    }
    private void SetValue()
    {
        // if (soAgentUpgrade.level == _stageCount)
        // {
        //     sliderFilled.fillAmount = 1;
        //     return;
        // }
        SetLevel();
        SetTotalLevel();
        SetSlider();
        SetDigSpeed(0.5f);
        SetHealth();
        SetCost();
        SetDamage();

    }

   

    public delegate void OnClickClose();
    public static OnClickClose onClickClose;
    
    public static OnClickClose onClickUpgrade;
    public static OnClickClose onClickUpgradeMelee;
    public static OnClickClose onClickUpgradeArcher;
    public static OnClickClose onClickUpgradeDigger;
    public static OnClickClose onClickUpgradeGiant;
    
    private void CloseButton()
    {
        onClickClose?.Invoke();
    }
   
}

