using UnityEngine;

public class LocalOrdersCreator : MonoBehaviour, IShipOrdersSource
{
    [SerializeField]
    private Waypoint StartPoint;

    [SerializeField]
    private Waypoint EndPoint;

    SpaceShip ship;

    public void Initialize(SpaceShip ship)
    {
        this.ship = ship;
        SetupForNextTurn();
    }

    public void Apply()
    {
        if(ship.IsActive)
        {
            ship.CurrentPath = new FlightPath(GetCurrentPath(),
                ship.Manuverability.RotationUpWeight,
                ship.Manuverability.RotationStrength,
                Game.KeyframesPerTurn,
                ship.GameObject.transform.position,
                ship.GameObject.transform.up);
        }
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

    public void SetupForNextTurn()
    {
        if(ship.IsActive)
        {
            // TODO: place the start and end point where the ship at the end maintains it's current ending and speed
        }
    }
}
