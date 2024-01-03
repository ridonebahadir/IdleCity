
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;

public class CheckPoints : MonoBehaviour
{
    private List<CheckPoint> checkPoints = new List<CheckPoint>();
    [SerializeField] private int homeTurn;
    [SerializeField] private SplineFollower splineFollower;
    public bool startSetup;
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
            if (homeTurn==1) GameManager.Instance.FailPanelOpen();
            homeTurn--;
            checkPoints[homeTurn].SetActiveHome();
           
        }
        else
        {
            if (homeTurn==checkPoints.Count-1) GameManager.Instance.WinPanelOpen();
            checkPoints[homeTurn].SetActiveHome();
            homeTurn++;
        }
       
    }

   
}
