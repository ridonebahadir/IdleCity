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
   
   [Header("Game List")]
   public List<Transform> enemies;
   public List<Transform> soldiers; 
   public List<Transform> destroyRiverPoints;
   public List<Transform> patrolPoints;
  

   [Header("Game List Parent")] 
   public Transform enemiesParent;
   public Transform soldiersParent;
   public Transform patrolPointsParent; 
   public Transform destroyRiverParent;
   
   private void Awake()
   {
      if(Instance == null)
      {
         Instance = this;
      }
      /*GameListHealthAddItem(enemies,enemiesParent);
      GameListHealthAddItem(soldiers,soldiersParent);
      GameListHealthAddItem(riverPoints,riverPointsParent);*/
      GameListTransformAddItem(enemies,enemiesParent);
      GameListTransformAddItem(soldiers,soldiersParent);
      GameListTransformAddItem(patrolPoints,patrolPointsParent);
      GameListTransformAddItem(destroyRiverPoints,destroyRiverParent);
     
   }
   public Transform GetRandomHealths(List<Transform> list)
   {
      var a = Random.Range(0, list.Count);
      return list[a];
   }
   /*public Health GetRandomSoldiers()
   {
      var a = Random.Range(0, soldiers.Count);
      return soldiers[a].GetComponent<Health>();
   }*/

   public Transform GetRandomTransformPoints(List<Transform> list)
   {
      if (list.Count == 0) return null;
      var a = Random.Range(0, list.Count);
      return list[a];
   }

   public void RemoveList(HealthType healthType,Transform health)
   {
      switch (healthType)
      {
         case HealthType.Enemy:
            enemies.Remove(health);
            break;
         case HealthType.Soldier:
            soldiers.Remove(health);
            break;
         case HealthType.DestroyRiverPoint:
            destroyRiverPoints.Remove(health);
            break;
      }
     
   }

   private void GameListTransformAddItem(List<Transform> list, Transform parent)
   {
      foreach (Transform item in parent)
      {
         list.Add(item);
      }
   }
   /*private void GameListHealthAddItem(List<Health> list, Transform parent)
   {
      foreach (Transform item in parent)
      {
         list.Add(item.GetComponent<Health>());
      }
   }*/
}
