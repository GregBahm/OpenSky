using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FlightExperiment : MonoBehaviour
{
    public WaypointCreator Waypoint;
    public GameObject Ship;
    public int Iterations;
    private GameObject[] shipIterations;
    
    private void Start()
    {
        shipIterations = CreateShipIterations().ToArray(); ;
    }

    private void Update()
    {
        AnchoredPathSegment currentPath = GetCurrentPath();
        for (int i = 1; i < Iterations; i++)
        {
        }
    }
    
    private IEnumerable<GameObject> CreateShipIterations()
    {
        for (int i = 0; i < Iterations; i++)
        {
            yield return Instantiate(Ship);
        }
    }
}

public struct AnchoredPathSegment
{
    public Vector3 Start { get; }
    public Vector3 StartForward { get; }
    public Vector3 End { get; }
    public Vector3 EndForward { get; }

    private readonly Vector3 startAnchor;
    private readonly Vector3 endAnchor;

    public AnchoredPathSegment(Vector3 start, 
        Vector3 startForward, 
        Vector3 end, 
        Vector3 endForward) : this()
    {
        Start = start;
        StartForward = startForward;
        End = end;
        EndForward = endForward;

        float length = (Start - End).magnitude;
        startAnchor = StartForward * length * (1f / 3);
        endAnchor = -EndForward * length * (1f / 3);
    }

    public Vector3 PlotPosition(float param)
    {
        Vector3 ab = Vector3.Lerp(Start, startAnchor, param);
        Vector3 bc = Vector3.Lerp(startAnchor, endAnchor, param);
        Vector3 cd = Vector3.Lerp(endAnchor, End, param);
        Vector3 abc = Vector3.Lerp(ab, bc, param);
        Vector3 bcd = Vector3.Lerp(bc, cd, param);
        return Vector3.Lerp(abc, bcd, param);
    }
}