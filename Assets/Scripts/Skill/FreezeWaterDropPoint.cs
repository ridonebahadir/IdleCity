
using Dreamteck.Splines;
using UnityEngine;


public class FreezeWaterDropPoint : MonoBehaviour
{
    public static FreezeWaterDropPoint Create(GameObject obj,SplineComputer splineComputer,Domination.Domination domination)
    {
        var cloneObj = Instantiate(obj).transform.GetComponent<FreezeWaterDropPoint>();
        cloneObj.Init(splineComputer,domination);
        return cloneObj;
    }
    
    [SerializeField] private SplineFollower splineFollower;
    private Domination.Domination _domination;
    
    private void Init(SplineComputer splineComputer,Domination.Domination domination)
    {
        splineFollower.spline = splineComputer;
        _domination = domination;
        
    }
    private void Update()
    {
        var dist = Vector3.Distance(_domination.transform.position,transform.position);
        if (dist<=1)  Destroy(gameObject);
    }
}
