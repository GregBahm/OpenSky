using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShipDefinition : MonoBehaviour
{
    public float MaxThrust;
    public float MaxAngleChange;
    public float Acceleration;
    public GameObject Prefab;
    public WeaponDefinition[] Weapons;

    public SpaceShip ToSpaceship(int teamId, Transform startingLocation)
    {
        SpaceManuverability manuverability = new SpaceManuverability(MaxThrust, MaxAngleChange, Acceleration);
        IEnumerable<ISpaceshipWeapon> weapons = Weapons.Select(item => item.ToWeapon()).ToArray();
        GameObject gameObject = Instantiate(Prefab);
        gameObject.transform.SetParent(startingLocation, false);
        gameObject.transform.SetParent(null, true);
        return new SpaceShip(teamId,
            manuverability,
            weapons,
            gameObject
            );
    }
}
