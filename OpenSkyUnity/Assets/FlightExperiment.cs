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
        for (int i = 0; i < Iterations; i++)
        {
            float param = (float)(i + 1) / Iterations;
            PlaceShip(shipIterations[i].transform, param);
        }
    }

    private void PlaceShip(Transform transform, float param)
    {
        throw new NotImplementedException();
    }

    private IEnumerable<GameObject> CreateShipIterations()
    {
        for (int i = 0; i < Iterations; i++)
        {
            yield return Instantiate(Ship);
        }
    }
}
