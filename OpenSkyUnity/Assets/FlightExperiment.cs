using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathSegment
{
    public WaypointCreator StartPoint { get; }
    public WaypointCreator EndPoint { get; }
    private readonly FlightExperiment mothership;

    private readonly Transform[] shipIterations;

    public PathSegment(WaypointCreator startPoint, WaypointCreator endPoint, Transform[] shipIterations, FlightExperiment mothership)
    {
        StartPoint = startPoint;
        EndPoint = endPoint;
        this.shipIterations = shipIterations;
        this.mothership = mothership;
    }

    public Transform LastIteration { get { return shipIterations[shipIterations.Length - 1]; } }

    public void SetShipScale(Vector3 scale)
    {
        foreach (Transform ship in shipIterations)
        {
            ship.localScale = scale;
        }
    }

    public void SetShipRotations(Vector3 lastPos, Vector3 lastUp)
    {
        SetStartingRotation(lastPos, lastUp);
        for (int i = 1; i < shipIterations.Length - 1; i++)
        {
            Transform previous = shipIterations[i - 1];
            Transform current = shipIterations[i];
            Transform next = shipIterations[i + 1];
            current.rotation = GetShipRotation(previous.position, current.position, next.position, previous.up);
        }
        SetEndingRotation();
    }

    private void SetStartingRotation(Vector3 lastPos, Vector3 lastUp)
    {
        Transform current = shipIterations[0];
        Vector3 next = shipIterations[1].position;
        current.rotation = GetShipRotation(lastPos, current.position, next, lastUp);
    }

    private void SetEndingRotation()
    {
        Transform current = shipIterations[shipIterations.Length - 1];
        Transform previous = shipIterations[shipIterations.Length - 2];
        current.rotation = GetShipRotation(previous.position, current.position, EndPoint.transform.position, previous.up);
    }

    private Quaternion GetShipRotation(
        Vector3 previous,
        Vector3 current,
        Vector3 next,
        Vector3 previousUp)
    {
        Vector3 toCurrent = current - previous;
        Vector3 toNext = next - current;
        Vector3 average = (toCurrent + toNext) / 2;

        Vector3 maxTilt = (toNext.normalized - toCurrent.normalized) / 2;

        float mag = Mathf.Pow(maxTilt.magnitude, this.mothership.RotationUpWeight);
        Vector3 up = Vector3.Lerp(Vector3.up, maxTilt, mag);

        Vector3 progressive = Vector3.Lerp(previousUp, up, this.mothership.RotationStrength);
        return Quaternion.LookRotation(average, progressive);
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

    public float RotationUpWeight;
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
            yield return new PathSegment(start, end, shipIterations, this);
        }
    }

    private void Update()
    {
        Vector3 scale = new Vector3(ShipScale, ShipScale, ShipScale);
        Vector3 lastPos = Waypoints[0].transform.position - Waypoints[0].transform.forward;
        Vector3 lastUp = Vector3.up;
        foreach (PathSegment segment in pathSegments)
        {
            segment.SetShipPositions();
            segment.SetShipRotations(lastPos, lastUp);
            segment.SetShipScale(scale);
            lastUp = segment.LastIteration.up;
            lastPos = segment.LastIteration.position;

            BezierCurve currentPath = segment.GetCurrentPath();
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

    public float MeasureCurve(int precision)
    {
        float ret = 0;
        Vector3 lastPoint = Start;
        for (int i = 0; i < precision; i++)
        {
            float param = (float)i / precision;
            Vector3 nextPoint = PlotPosition(param);
            ret += (lastPoint = nextPoint).magnitude;
            lastPoint = nextPoint;
        }
        return ret;
    }
}