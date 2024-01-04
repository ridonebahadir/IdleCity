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
     [SerializeField] private EventSystem _eventSystem;
     
     
    private void Start()
    {
        _stageCount = soAgentUpgrade.stageCount;
        SetCost();
        SetHealth();
        SetDamage();
        SetSlider();
        SetDigSpeed();
        levelText.SetText("Level "+soAgentUpgrade.stage);
        characterName.SetText(soAgentUpgrade.name);
        currentIcon.sprite = soAgentUpgrade.icon;
        nextIcon.sprite = soAgentUpgrade.nextIcon;
        button.onClick.AddListener(Clicked);
        closeButton.onClick.AddListener(CloseButton);
    }

    private void SetCost()
    {
        soAgentUpgrade.cost = FormulaCost(soAgentUpgrade.multipherCost.a,soAgentUpgrade.multipherCost.b,soAgentUpgrade.multipherCost.c,0);
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
        soAgent.health =(int)Formula(soAgentUpgrade.multipherHealth.a, soAgentUpgrade.multipherHealth.b,
            soAgentUpgrade.multipherHealth.c,0);
        var ratio = Formula(soAgentUpgrade.multipherHealth.a, soAgentUpgrade.multipherHealth.b,
            soAgentUpgrade.multipherHealth.c, 1)-soAgent.health;
        _ratioHealthText.SetText("+" + ratio);
        healthText.SetText("Health = "+soAgent.health.ToString());
    }

    private void SetDamage()
    {
        soAgent.damage = (int)Formula(soAgentUpgrade.multipherDamage.a,soAgentUpgrade.multipherDamage.b,soAgentUpgrade.multipherDamage.c,0);
        var ratio = Formula(soAgentUpgrade.multipherDamage.a, soAgentUpgrade.multipherDamage.b,
            soAgentUpgrade.multipherDamage.c, 1)-soAgent.damage;
        _ratioDamageText.SetText("+" + ratio);
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
        levelText.SetText("Level "+soAgentUpgrade.level);
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

    private void SetDigSpeed()
    {
        soAgent.digSpeed = (soAgentUpgrade.totalLevel*0.02f)+0.1f; //0.25f first value
        var dig = soAgent.digSpeed * 20;
        var ratio=(((soAgentUpgrade.totalLevel+1)*0.02f+0.1f)*20)-dig;
        _ratioDiggSpeedText.SetText("+" + Math.Round(ratio));
        digSpeedText.SetText("Digg Speed = "+(int)dig);
    }

    private void Clicked()
    {
        if (soAgentUpgrade.level>=5 && soAgentUpgrade.stage>=4) return;
        
        SetValue();
      
    }

    

    private int Formula(float a, float b, float c,int levelPlus)
    {
        // math.ceil((a*(i**2)+b*i+c)*(1.2**((i-1)//5)))
        var i = soAgentUpgrade.totalLevel+levelPlus;
        var formula=Math.Ceiling((a * Math.Pow(i, 2) + b * i + c) * Math.Pow(1.2, (i - 1) / 5));
        return (int)formula;
    }
    private int FormulaCost(float a, float b, float c,int levelPlus)
    {
        //math.ceil((a**i+b*i+c)*(1.2**((i)//5)))
        var i = soAgentUpgrade.totalLevel+levelPlus;
        var formula=Math.Ceiling((Math.Pow(a, i) + b * i + c) * Math.Pow(1.2, i / 5));
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
        SetDigSpeed();
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

