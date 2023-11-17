using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance;
   public BuildManager buildManager;
   public UIManager uIManager;
   
   
   private void Awake()
   {
      if(Instance == null)
      {
         Instance = this;
      }
   }
}
