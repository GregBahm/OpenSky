using System;
using System.Collections;
using UnityEngine;

public class SpaceShipOrders
{
    public SpaceShip Unit { get; }

    public WaypointCreator StartPoint { get; }
    public WaypointCreator EndPoint { get; }

    internal void ApplyOrders()
    {
        Unit.CurrentPath = new FlightPath(GetCurrentPath(),
            mothership.RotationUpWeight,
            mothership.RotationStrength,
            mothership.Iterations,
            fromPosition,
            fromUp);
    }

    private BezierCurve GetCurrentPath()
    {
        Vector3 startAnchor = StartPoint.transform.position + StartPoint.transform.forward * StartPoint.Weight;
        Vector3 endAnchor = EndPoint.transform.position - EndPoint.transform.forward * EndPoint.Weight;
        return new BezierCurve(StartPoint.transform.position,
            startAnchor,
            EndPoint.transform.position,
            endAnchor);
    }
}