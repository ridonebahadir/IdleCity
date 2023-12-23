using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum HomeType
{
   Melee,
   Archer,
   Digger
}
public class Select : MonoBehaviour
{ 
   [SerializeField] private HomeType homeType;
   private SelectCharacterUpgrade selectCharacterUpgrade;
   public Collider col;
   
   [SerializeField] private Select otherSelect;
   [SerializeField] private Select otherSelect2;
   
   private void Start()
   {
      selectCharacterUpgrade = transform.GetComponentInParent<SelectCharacterUpgrade>();
   }

   private void OnMouseDown()
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
      }
   }

   private void CloseCollider()
   {
      col.enabled = false;
      otherSelect.col.enabled = true;
      otherSelect2.col.enabled = true;
   }
}
