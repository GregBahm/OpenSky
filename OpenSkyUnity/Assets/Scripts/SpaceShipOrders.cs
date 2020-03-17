using System;
using System.Collections;
using UnityEngine;

public class SpaceShipOrders
{
    public SpaceShip Unit { get; }

    public Waypoint StartPoint { get; }
    public Waypoint EndPoint { get; }

    internal void ApplyOrders()
    {
        Unit.CurrentPath = new FlightPath(GetCurrentPath(),
            Unit.Manuverability.RotationUpWeight,
            Unit.Manuverability.RotationStrength,
            Game.KeyframesPerTurn,
            Unit.GameObject.transform.position,
            Unit.GameObject.transform.up);
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