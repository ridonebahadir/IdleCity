using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public enum HomeType
{
   Melee,
   Archer,
   Digger,
   Giant,
   VillageTown,
}
public class Select : MonoBehaviour,ISelectable
{ 
   [SerializeField] private HomeType homeType;
   [SerializeField] private Transform levelParent;
   [SerializeField] private SOAgentUpgrade soTownUpgrade;
   [SerializeField] private ParticleSystem cloud;
   [SerializeField] private SOGameSettings soGameSettings;
   [SerializeField] private CharacterUpgradePanel characterUpgradePanel;
   [SerializeField] private GameObject warning;
   
   private SelectCharacterUpgrade selectCharacterUpgrade;
   public Collider col;
   [SerializeField] private List<Select> otherSelects;
   private int _cost;
   private void Start()
   {
      if (homeType != HomeType.VillageTown) ControlWarning();
      else ControlTownWarning();

     
      selectCharacterUpgrade = transform.GetComponentInParent<SelectCharacterUpgrade>();
      LevelParentHome();
     
   }
   

   private void ControlWarning()
   {
      if (soGameSettings.totalXp>=SetCost())
      {
         warning.SetActive(true);
      }
      else
      {
         warning.SetActive(false);
      }
   }

   private void ControlTownWarning()
   {
      if (soGameSettings.totalXp>=SetCost()||(soGameSettings.totalXp>=SetRateUpgradeCost()))
      {
         warning.SetActive(true);
      }
      else
      {
         warning.SetActive(false);
      }
   }

   private int SetCost()
   {
      var cost = characterUpgradePanel.FormulaCost(characterUpgradePanel.soAgentUpgrade.multipherCost.a,characterUpgradePanel.soAgentUpgrade.multipherCost.b,characterUpgradePanel.soAgentUpgrade.multipherCost.c,1,characterUpgradePanel.soAgentUpgrade.totalLevel);
      return cost;
   }

   private int SetRateUpgradeCost()
   {
      var cost = characterUpgradePanel.FormulaCost(characterUpgradePanel.soAgentUpgrade.multipherRateCost.a,characterUpgradePanel.soAgentUpgrade.multipherRateCost.b,characterUpgradePanel.soAgentUpgrade.multipherRateCost.c,1,characterUpgradePanel.soAgentUpgrade.totalLevel);
      return cost;
   }
   public void Selected()
   {
      CloseCollider();
      switch (homeType)
      {
         case HomeType.Melee:
            selectCharacterUpgrade.Melee();
            break;
         case HomeType.Archer:
            selectCharacterUpgrade.Archer();
            break;
         case HomeType.Digger:
            selectCharacterUpgrade.Digger();
            break;
         case HomeType.Giant:
            selectCharacterUpgrade.Giant();
            break;
         case HomeType.VillageTown:
            selectCharacterUpgrade.VillageTown();
            break;
      }
   }
   private void CloseCollider()
   {
      col.enabled = false;
      foreach (var t in otherSelects)
      {
         if (t != this)
         {
            t.col.enabled = true;
         };
        
      }
      // otherSelect.col.enabled = true;
      // otherSelect2.col.enabled = true;
   }
   private void OpenCollider()
   {
      StartCoroutine(OpenColliderIE());
      IEnumerator OpenColliderIE()
      {
         yield return new WaitForSeconds(0.5f);
         foreach (var t in otherSelects)
         {
            t.col.enabled = true;
         }
         //col.enabled = true;
         // otherSelect.col.enabled = true;
         // otherSelect2.col.enabled = true;
      }
      
   }

 
   
   private void LevelParentHome()
   {
      StartCoroutine(ChangeMesh());

      IEnumerator ChangeMesh()
      {
        
         var startScale = levelParent.localScale;
         yield return new WaitForSeconds(0.25f);
         levelParent.localScale = Vector3.zero;
         levelParent.DOScale(startScale, 1f).SetEase(Ease.OutBounce);
         cloud.Play();
         foreach (Transform item in levelParent)  item.gameObject.SetActive(false);
         levelParent.GetChild(soTownUpgrade.level-1).gameObject.SetActive(true);
      }
     
   }

  
   private void OnEnable()
   {
      CharacterUpgradePanel.onClickClose += OpenCollider;
      CharacterUpgradePanel.onClickUpgradeTown += LevelParentHome;
      UIBottomButton.OnBottomButtonClose += OpenCollider;
      if (homeType==HomeType.VillageTown)
      {
         CharacterUpgradePanel.OnClickTownRequirement += Selected;
         GameManager.Instance.OnXpChange += ControlTownWarning;
      }
      else
      {
         GameManager.Instance.OnXpChange += ControlWarning;
      }
     
   }

   private void OnDisable()
   {
      CharacterUpgradePanel.onClickClose -= OpenCollider;
      CharacterUpgradePanel.onClickUpgradeTown -= LevelParentHome;
      UIBottomButton.OnBottomButtonClose -= OpenCollider;
      if (homeType==HomeType.VillageTown)
      {
         CharacterUpgradePanel.OnClickTownRequirement -= Selected;
         GameManager.Instance.OnXpChange -= ControlTownWarning;
      }
      else
      {
         GameManager.Instance.OnXpChange -= ControlWarning;
      }
      
      
   }
   
}

public interface ISelectable
{
   public void Selected();
} 
