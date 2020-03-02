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
        CurrentMomentum = Vector3.zero;
        Vector3 targetPos = Waypoint.transform.position;
        for (int i = 1; i < Iterations; i++)
        {

            MoveEntity(shipIterations[i -1].transform, shipIterations[i].transform, targetPos);
        }
    }

    Vector3 CurrentMomentum;
    public float Acceleration;
    public float MaxAngleChange;
    public float MaxThrust;

    public void MoveEntity(Transform priorTransform, Transform nextTransform, Vector3 targetPosition)
    {
        Vector3 toTarget = targetPosition - priorTransform.position;
        Quaternion lookRot = Quaternion.LookRotation(toTarget);
        nextTransform.rotation = Quaternion.RotateTowards(priorTransform.rotation, lookRot, MaxAngleChange);
        CurrentMomentum = CurrentMomentum + nextTransform.forward * Acceleration;
        if (CurrentMomentum.magnitude > MaxThrust)
        {
            CurrentMomentum = CurrentMomentum.normalized * MaxThrust;
        }
        nextTransform.position = priorTransform.position + CurrentMomentum;
    }

    private IEnumerable<GameObject> CreateShipIterations()
    {
        for (int i = 0; i < Iterations; i++)
        {
            yield return Instantiate(Ship);
        }
    }
}
