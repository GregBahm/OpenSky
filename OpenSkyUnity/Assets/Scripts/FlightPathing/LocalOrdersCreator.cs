using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LocalOrdersCreator : MonoBehaviour, IShipOrdersSource
{
    [SerializeField]
    private Waypoint StartPoint;

    [SerializeField]
    private Waypoint EndPoint;

    [SerializeField]
    private GameObject waypointStepIndicatorPrefab;

    public SpaceShip Ship { get; private set; }

    private GameObject[] displayPoints;

    private FlightPath currentFlightPath;

    public void Initialize(SpaceShip ship)
    {
        this.Ship = ship;
        SetupForNextTurn();
        displayPoints = CreateDisplayPoints().ToArray();
    }

    private IEnumerable<GameObject> CreateDisplayPoints()
    {
        for (int i = 0; i < Game.KeyframesPerTurn; i++)
        {
            GameObject retItem = Instantiate(waypointStepIndicatorPrefab);
            retItem.transform.parent = this.transform;
            yield return retItem;
        }
    }

    public void HideVisuals()
    {
        StartPoint.gameObject.SetActive(false);
        EndPoint.gameObject.SetActive(false);
        for (int i = 0; i < displayPoints.Length; i++)
        {
            displayPoints[i].SetActive(false);
        }
    }

    public void ShowVisuals()
    {
        StartPoint.gameObject.SetActive(true);
        EndPoint.gameObject.SetActive(true);
        FlightPath path = GetCurrentFlightpath();
        for (int i = 0; i < displayPoints.Length; i++)
        {
            displayPoints[i].SetActive(true);
            Transform trans = displayPoints[i].transform;
            Pose pose = path.Poses[i];
            trans.position = pose.position;
            trans.rotation = pose.rotation;
        }
    }

    public void Apply()
    {
        Ship.CurrentPath = GetCurrentFlightpath();
    }

    private FlightPath GetCurrentFlightpath()
    {
        return new FlightPath(GetCurrentPath(),
            Ship.Manuverability.RotationUpWeight,
            Ship.Manuverability.RotationStrength,
            Game.KeyframesPerTurn,
            Ship.GameObject.transform.position,
            Ship.GameObject.transform.up);
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
        StartPoint.gameObject.SetActive(Ship.IsActive);
        EndPoint.gameObject.SetActive(Ship.IsActive);
        if (Ship.IsActive)
        {
            float total = Ship.CurrentSpeed * Game.KeyframesPerTurn;
            float handleWeight = total / 3;
            Vector3 startPoint = Ship.GameObject.transform.position;
            Vector3 endPoint = startPoint + Ship.GameObject.transform.forward * total;
            StartPoint.Set(startPoint, Ship.GameObject.transform.rotation, handleWeight);
            EndPoint.Set(endPoint, Ship.GameObject.transform.rotation, handleWeight);
        }
    }
}
