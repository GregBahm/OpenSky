using System;
using System.Collections.Generic;
using UnityEngine;

public class LaserWeaponDefinition : WeaponDefinition
{
    public int MaxRounds;
    public float DelayBetweenVolleys;
    public float DelayBetweenBlasts;
    public GameObject BlastPrefab;
    public float ProjectileSpeed;
    public float ProjectileDuration;
    public float Damage;
    public float BlastRadius;
    public float FiringConeAngle;
    public float FiringConeDistance;

    public override ISpaceshipWeapon ToWeapon()
    {
        IEnumerable<LaserBlast> blasts = CreateBlasts();
        TargettingCone targetting = new TargettingCone(FiringConeAngle, FiringConeDistance);
        return new LaserWeapon(blasts,
            DelayBetweenVolleys,
            DelayBetweenBlasts,
            targetting
            );
    }

    private IEnumerable<LaserBlast> CreateBlasts()
    {
        LaserBlast[] ret = new LaserBlast[MaxRounds];
        for (int i = 0; i < MaxRounds; i++)
        {
            ret[i] = CreateNewBlast();
        }
        return ret;
    }

    private LaserBlast CreateNewBlast()
    {
        GameObject blastPrefab = Instantiate(BlastPrefab);
        return new LaserBlast(ProjectileSpeed, ProjectileDuration, Damage, BlastRadius, blastPrefab);
    }
}