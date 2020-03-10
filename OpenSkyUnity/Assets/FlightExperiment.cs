using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathSegment
{
    public WaypointCreator StartPoint { get; }
    public WaypointCreator EndPoint { get; }

    private readonly Transform[] shipIterations;

    public PathSegment(WaypointCreator startPoint, WaypointCreator endPoint, Transform[] shipIterations)
    {
        StartPoint = startPoint;
        EndPoint = endPoint;
        this.shipIterations = shipIterations;
    }

    public Vector3 LastIterationPosition { get { return shipIterations[shipIterations.Length - 1].position; } }

    public void SetShipScale(Vector3 scale)
    {
        foreach (Transform ship in shipIterations)
        {
            ship.localScale = scale;
        }
    }

    public void SetShipRotations(Vector3 endOfLastSegment, float rotationStrength)
    {
        SetStartingRotation(endOfLastSegment, rotationStrength);
        for (int i = 1; i < shipIterations.Length - 1; i++)
        {
            Transform previous = shipIterations[i - 1];
            Transform current = shipIterations[i];
            Transform next = shipIterations[i + 1];

            current.rotation = GetShipRotation(previous.position, current.position, next.position, rotationStrength);
        }
        SetEndingRotation(rotationStrength);
    }

    private void SetStartingRotation(Vector3 endOfLastSegment, float rotationStrength)
    {
        Transform current = shipIterations[0];
        Vector3 next = shipIterations[1].position;
        current.rotation = GetShipRotation(endOfLastSegment, current.position, next, rotationStrength);
    }

    private void SetEndingRotation(float rotationStrength)
    {
        Transform current = shipIterations[shipIterations.Length - 1];
        Transform previous = shipIterations[shipIterations.Length - 2];
        current.rotation = GetShipRotation(previous.position, current.position, EndPoint.transform.position, rotationStrength);
    }

    private static Quaternion GetShipRotation(Vector3 previous, Vector3 current, Vector3 next, float rotationStrength)
    {
        Vector3 toCurrent = current - previous;
        Vector3 toNext = next - current;
        Vector3 average = (toCurrent + toNext) / 2;

        Vector3 maxTilt = (toNext.normalized - toCurrent.normalized) / 2;
        float mag = Mathf.Pow(maxTilt.magnitude, rotationStrength);
        Vector3 up = Vector3.Lerp(Vector3.up, maxTilt, mag);
        return Quaternion.LookRotation(average, up);
    }


    public BezierCurve GetCurrentPath()
    {
        Vector3 startAnchor = StartPoint.transform.position + StartPoint.transform.forward * StartPoint.Weight;
        Vector3 endAnchor = EndPoint.transform.position - EndPoint.transform.forward * EndPoint.Weight;
        return new BezierCurve(StartPoint.transform.position,
            startAnchor,
            EndPoint.transform.position,
            endAnchor);
    }

    public void SetShipPositions()
    {
        BezierCurve currentPath = GetCurrentPath();
        for (int i = 0; i < shipIterations.Length; i++)
        {
            float param = (float)i / shipIterations.Length;
            shipIterations[i].transform.position = currentPath.PlotPosition(param);
        }
    }
}

public class FlightExperiment : MonoBehaviour
{
    public WaypointCreator[] Waypoints;
    public GameObject Ship;
    public int Iterations;

    public float RotationStrength;

    [Range(0, 1)]
    public float ShipScale = 1;

    private PathSegment[] pathSegments;

    private void Start()
    {
        pathSegments = CreatePathSegments().ToArray();
    }

    private IEnumerable<PathSegment> CreatePathSegments()
    {
        for (int i = 1; i < Waypoints.Length; i++)
        {
            WaypointCreator start = Waypoints[i - 1];
            WaypointCreator end = Waypoints[i];
            Transform[] shipIterations = CreateShipIterations().ToArray();
            yield return new PathSegment(start, end, shipIterations);
        }
    }

    private void Update()
    {
        Vector3 scale = new Vector3(ShipScale, ShipScale, ShipScale);
        Vector3 lastIterationPosition = Vector3.zero;
        foreach (PathSegment segment in pathSegments)
        {
            segment.SetShipPositions();
            segment.SetShipRotations(lastIterationPosition, RotationStrength);
            segment.SetShipScale(scale);
            lastIterationPosition = segment.LastIterationPosition;

            BezierCurve currentPath = segment.GetCurrentPath();

            Debug.DrawLine(currentPath.Start, currentPath.StartAnchor, Color.red);
            Debug.DrawLine(currentPath.End, currentPath.EndAnchor, Color.green);
        }
    }

    private IEnumerable<Transform> CreateShipIterations()
    {
        for (int i = 0; i < Iterations; i++)
        {
            GameObject newShip = Instantiate(Ship);
            yield return newShip.transform;
        }
    }
}

public struct BezierCurve
{
    public Vector3 Start { get; }
    public Vector3 End { get; }

    public Vector3 StartAnchor { get; }
    public Vector3 EndAnchor { get; }

    public BezierCurve(Vector3 start, 
        Vector3 startAnchor, 
        Vector3 end, 
        Vector3 endAnchor) : this()
    {
        Start = start;
        End = end;
        StartAnchor = startAnchor;
        EndAnchor = endAnchor;
    }

    public Vector3 PlotPosition(float param)
    {
        Vector3 ab = Vector3.Lerp(Start, StartAnchor, param);
        Vector3 bc = Vector3.Lerp(StartAnchor, EndAnchor, param);
        Vector3 cd = Vector3.Lerp(EndAnchor, End, param);
        Vector3 abc = Vector3.Lerp(ab, bc, param);
        Vector3 bcd = Vector3.Lerp(bc, cd, param);
        return Vector3.Lerp(abc, bcd, param);
    }
}