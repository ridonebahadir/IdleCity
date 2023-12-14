using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class CreateCirclePos : MonoBehaviour
{
    public float radius = 1;
    public int numberOfPositions = 30;
    public GameObject obj;

    [SerializeField] private Transform alliesParent;
    [SerializeField] private Transform enemiesParent;

    [SerializeField] private Domination.Domination domination;
    

    [ContextMenu("Spawn")]
    public void Spawn()
    {
        CreateHalfCirclePositions(90,180,alliesParent,domination.alliesSlot);
        CreateHalfCirclePositions(270,360,enemiesParent,domination.enemiesSlot);
    }

   
    public void CreateHalfCirclePositions(float startAngle,float endAngle,Transform parent,List<Transform> list)
    {
        var angleStep = (endAngle - startAngle) / (numberOfPositions - 1);

        for (var i = 1; i <= numberOfPositions; i++)
        {
            float currentAngle;
            if (i%2==0)
            {
                 currentAngle = startAngle - (i * angleStep);
                
            }
            else
            {
                currentAngle = startAngle + (i *angleStep);
            }
            var radians = Mathf.Deg2Rad * currentAngle;
            var x = transform.position.x + radius * Mathf.Cos(radians);
            var z = transform.position.z + radius * Mathf.Sin(radians);
            
            Vector3 newPosition = new Vector3(x, transform.position.y, z);
            GameObject cloneObj = Instantiate(obj);
            cloneObj.transform.position = newPosition;
            cloneObj.transform.parent = parent;
            list.Add(cloneObj.transform);
        }
    }
}
