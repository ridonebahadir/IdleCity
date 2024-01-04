using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public enum HomeType
{
   Melee,
   Archer,
   Digger,
   Giant,
   VillageTown,
}
public class Select : MonoBehaviour
{ 
   [SerializeField] private HomeType homeType;
   [SerializeField] private Transform levelParent;
   [SerializeField] private SOAgentUpgrade soTownUpgrade;
   
   private SelectCharacterUpgrade selectCharacterUpgrade;
   public Collider col;
   
   [SerializeField] private List<Select> otherSelects;
   
   private void Start()
   {
      selectCharacterUpgrade = transform.GetComponentInParent<SelectCharacterUpgrade>();
   }

   private void OnMouseDown()
   {
      if (EventSystem.current.IsPointerOverGameObject()) return;
      Selected();
   }

   private void Selected()
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
      foreach (Transform item in levelParent)  item.gameObject.SetActive(false);
      levelParent.GetChild(soTownUpgrade.level-1).gameObject.SetActive(true);
   }
   
   private void OnEnable()
   {
      CharacterUpgradePanel.onClickClose += OpenCollider;
      CharacterUpgradePanel.onClickUpgradeTown += LevelParentHome;
      if (homeType==HomeType.VillageTown)
      {
         CharacterUpgradePanel.OnClickTownRequirement += Selected;
      }
   }

   private void OnDisable()
   {
      CharacterUpgradePanel.onClickClose -= OpenCollider;
      CharacterUpgradePanel.onClickUpgradeTown -= LevelParentHome;
      if (homeType==HomeType.VillageTown)
      {
         CharacterUpgradePanel.OnClickTownRequirement -= Selected;
      }
   }
   
}
