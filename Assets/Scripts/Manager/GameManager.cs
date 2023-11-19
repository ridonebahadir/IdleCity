using System;
using System.Collections;
using System.Collections.Generic;
using Agent.Enemy;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance;
   public BuildManager buildManager;
   public UIManager uIManager;
   
   [Header("Game List")]
   public List<Health> enemies;
   public List<Health> soldiers; 
   public List<Transform> patrolPoints;

   [Header("Game List Parent")] 
   public Transform enemiesParent;
   public Transform soldiersParent;
   public Transform patrolPointsParent;
   
   private void Awake()
   {
      if(Instance == null)
      {
         Instance = this;
      }
      GameListHealthAddItem(enemies,enemiesParent);
      GameListHealthAddItem(soldiers,soldiersParent);
      GameListTransformAddItem(patrolPoints,patrolPointsParent);
   }
   public Health GetRandomEnemies()
   {
      var a = Random.Range(0, enemies.Count);
      return enemies[a].GetComponent<Health>();
   }
   public Health GetRandomSoldiers()
   {
      var a = Random.Range(0, soldiers.Count);
      return soldiers[a].GetComponent<Health>();
   }

   public Transform GetRandomPatrolPoints()
   {
      var a = Random.Range(0, patrolPoints.Count);
      return patrolPoints[a];
   }

   public void RemoveList(HealthType healthType,Health health)
   {
      switch (healthType)
      {
         case HealthType.Enemy:
            enemies.Remove(health);
            break;
         case HealthType.Soldier:
            soldiers.Remove(health);
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
   private void GameListHealthAddItem(List<Health> list, Transform parent)
   {
      foreach (Transform item in parent)
      {
         list.Add(item.GetComponent<Health>());
      }
   }
}
public enum HealthType
{
   Home,
   Soldier, 
   Enemy,
}