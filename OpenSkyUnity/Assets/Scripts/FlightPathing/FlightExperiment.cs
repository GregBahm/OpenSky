using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FlightExperiment : MonoBehaviour
{
    public WaypointCreator[] Waypoints;
    public GameObject Ship;
    public int Iterations;

    public float RotationUpWeight;
    public float RotationStrength;

    [Range(0, 1)]
    public float ShipScale = 1;

    private FlightPathDisplay[] pathSegments;

    private void Start()
    {
        pathSegments = CreatePathSegments().ToArray();
    }

    private IEnumerable<FlightPathDisplay> CreatePathSegments()
    {
        for (int i = 1; i < Waypoints.Length; i++)
        {
            WaypointCreator start = Waypoints[i - 1];
            WaypointCreator end = Waypoints[i];
            Transform[] shipIterations = CreateShipIterations().ToArray();
            yield return new FlightPathDisplay(start, end, shipIterations, this);
        }
    }

    private void Update()
    {
        Vector3 scale = new Vector3(ShipScale, ShipScale, ShipScale);
        Vector3 lastPos = Waypoints[0].transform.position - Waypoints[0].transform.forward;
        Vector3 lastUp = Vector3.up;
        foreach (FlightPathDisplay segment in pathSegments)
        {
            segment.SetIterations(lastPos, lastUp);
            lastUp = segment.LastIteration.up;
            lastPos = segment.LastIteration.position;
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

public class FlightPathDisplay
{
    public WaypointCreator StartPoint { get; }
    public WaypointCreator EndPoint { get; }

    private readonly FlightExperiment mothership;

    private readonly Transform[] shipIterations;

    public Transform LastIteration { get { return shipIterations[shipIterations.Length - 1]; } }

    public FlightPathDisplay(WaypointCreator startPoint, WaypointCreator endPoint, Transform[] shipIterations, FlightExperiment mothership)
    {
        StartPoint = startPoint;
        EndPoint = endPoint;
        this.shipIterations = shipIterations;
        this.mothership = mothership;
    }

    public void SetIterations(Vector3 fromPosition, Vector3 fromUp)
    {
        FlightPath flightPath = new FlightPath(GetCurrentPath(),
            mothership.RotationUpWeight,
            mothership.RotationStrength,
            mothership.Iterations,
            fromPosition,
            fromUp);

        for (int i = 0; i < shipIterations.Length; i++)
        {
            shipIterations[i].localScale = new Vector3(mothership.ShipScale, mothership.ShipScale, mothership.ShipScale);
            shipIterations[i].position = flightPath.Poses[i].position;
            shipIterations[i].rotation = flightPath.Poses[i].rotation;
        }
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
}