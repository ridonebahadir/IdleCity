using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Devil : MonoBehaviour
{
    public static Devil Create(GameObject angelObj,Transform angelSpawnPoint,AgentBase target,float lifeTime)
    {
        var cloneAngel=Instantiate(angelObj,angelSpawnPoint.position,Quaternion.identity,angelSpawnPoint).transform;
        var angel = cloneAngel.GetComponent<Angel>();
        angel.Setup(target,lifeTime);
        return angel;
    }
}
