using System;
using System.Collections;
using UnityEngine;

public class SpaceShipOrders
{
    public SpaceShip Unit { get; }

    public Vector3 TargetPosition { get; } // TODO: Make this more elaborate

    internal void ApplyOrders()
    {
        Unit.TargetPosition = TargetPosition;
    }
}