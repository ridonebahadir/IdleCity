using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BuildManager : MonoBehaviour
{
     public List<Transform> civilBuilding;
     public List<Transform> soldierBuilding;

     public Health RandomTransform()
     {
          var a = Random.Range(0, civilBuilding.Count);
          var b = Random.Range(0,civilBuilding[a].childCount);
          return civilBuilding[a].GetChild(b).GetComponent<Health>();
     }
}
