using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BuildManager : MonoBehaviour
{
     public List<Transform> civilBuilding;
     public List<Transform> soldierBuilding;

     public WorkBase GetRandomCivilianBuild()
     {
          var a = Random.Range(0, civilBuilding.Count);
          var b = Random.Range(0,civilBuilding[a].childCount);
          return civilBuilding[a].GetChild(b).GetComponent<WorkBase>();
     }
     public WorkBase GetRandomSoldierBuild()
     {
          var a = Random.Range(0, soldierBuilding[0].childCount);
          return soldierBuilding[0].GetChild(a).GetComponent<WorkBase>();
     }
     
}
