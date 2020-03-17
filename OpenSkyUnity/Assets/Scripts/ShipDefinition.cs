﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShipDefinition : MonoBehaviour
{
    public float MaxThrust;
    public float RotationUpWeight = 0.2f;
    public float RotationStrength = 0.5f;
    public float Acceleration;
    public float Hitpoints;
    public GameObject Prefab;
    public WeaponDefinition[] Weapons;

    public SpaceShip ToSpaceship(int teamId, Transform startingLocation)
    {
        SpaceManuverability manuverability = new SpaceManuverability(MaxThrust, RotationStrength, RotationUpWeight, Acceleration);
        GameObject gameObject = CreateShipGameobject(startingLocation);
        return new SpaceShip(teamId,
            Hitpoints,
            manuverability,
            GetWeaponGetters(),
            gameObject
            );
    }

    public IEnumerable<Func<SpaceShip, ISpaceshipWeapon>> GetWeaponGetters()
    {
        foreach (var weaponDefinition in Weapons)
        {
            yield return weaponDefinition.ToWeapon;
        }
    }

    private GameObject CreateShipGameobject(Transform startingLocation)
    {
        GameObject ret = Instantiate(Prefab);
        ret.transform.SetParent(startingLocation, false);
        ret.transform.SetParent(null, true);
        return ret;
    }
}
