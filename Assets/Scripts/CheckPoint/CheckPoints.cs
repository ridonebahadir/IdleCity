
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;

public class CheckPoints : MonoBehaviour
{
    private List<CheckPoint> checkPoints = new List<CheckPoint>();
    [SerializeField] private int homeTurn;
    [SerializeField] private SplineFollower splineFollower;
    

    private void Start()
    {
        foreach (Transform item in transform)
        {
            checkPoints.Add(item.transform.GetComponent<CheckPoint>());
        }
    }

    public void SetHomeState(SplineUser arg0)
    {
        if (splineFollower.direction == Spline.Direction.Backward)
        {
            homeTurn--;
            checkPoints[homeTurn].SetActiveHome();
           
        }
        else
        {
            checkPoints[homeTurn].SetActiveHome();
            homeTurn++;
        }
       
    }

   
}
