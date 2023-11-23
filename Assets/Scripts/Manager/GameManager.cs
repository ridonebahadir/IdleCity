using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance;
   public BuildManager buildManager;
   public UIManager uIManager;

   public Domination dominationArea;
   private void Awake()
   {
      if(Instance == null)
      {
         Instance = this;
      }
      
     
   }


   
}
