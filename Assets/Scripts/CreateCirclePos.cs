using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class CreateCirclePos : MonoBehaviour
{
    public float radius = 5f;
    public int numberOfPositions = 30;
    

    [SerializeField] private Transform alliesParent;
    [SerializeField] private Transform enemiesParent;

    [SerializeField] private Domination.Domination domination;
    

    [ContextMenu("Spawn")]
    public void Spawn()
    {
        CreateHalfCirclePositions(0,180,alliesParent,domination.soldiersSlot);
        CreateHalfCirclePositions(180,360,enemiesParent,domination.enemiesSlot);
    }
    public void CreateHalfCirclePositions(float startAngle,float endAngle,Transform parent,List<Transform> list)
    {
        var angleStep = (endAngle - startAngle) / (numberOfPositions - 1);

        for (int i = 0; i < numberOfPositions; i++)
        {
           
            var currentAngle = startAngle + i * angleStep;
            var radians = Mathf.Deg2Rad * currentAngle;
            var x = transform.position.x + radius * Mathf.Cos(radians);
            var z = transform.position.z + radius * Mathf.Sin(radians);
            
            Vector3 newPosition = new Vector3(x, transform.position.y, z);
            GameObject obj = new GameObject();
            obj.transform.position = newPosition;
            obj.transform.parent = parent;
            list.Add(obj.transform);
        }
    }
}
