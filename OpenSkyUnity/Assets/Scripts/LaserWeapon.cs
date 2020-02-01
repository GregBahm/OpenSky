using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LaserWeapon : ISpaceshipWeapon
{
    public SpaceShip Ship { get; }

    public ProjectilesPool<LaserBlast> ProjectilesPool { get; }

    public IEnumerable<Projectile> Projectiles { get { return ProjectilesPool.Pool; } }

    public TargettingCone Targetting { get; }

    public bool CanInitiateNewAttack
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    private readonly float delayBetweenVolleys;
    private float timeSinceLastVolley;
    private readonly float delayBetweenBlasts;
    private float timeBetweenLastBlast;

    public LaserWeapon(
        SpaceShip ship,
        IEnumerable<LaserBlast> projectiles,
        float delayBetweenVolleys,
        float delayBetweenBlasts,
        TargettingCone targetting)
    {
        Ship = ship;
        ProjectilesPool = new ProjectilesPool<LaserBlast>(projectiles);
        this.delayBetweenVolleys = delayBetweenVolleys;
        this.delayBetweenBlasts = delayBetweenBlasts;
    }
    
    private bool TargetsPresent(IEnumerable<SpaceShip> targets)
    {
        return targets.Any(ship => IsWithinTargettingCone(ship));
    }

    private bool IsWithinTargettingCone(SpaceShip enemyShip)
    {
        Vector3 shipPos = enemyShip.GameObject.transform.position;
        Vector3 weaponPos = Ship.GameObject.transform.position;
        Vector3 weaponForward = Ship.GameObject.transform.forward;
        Vector3 toTarget = (weaponPos - shipPos).normalized;
        float theDot = Vector3.Dot(weaponForward, toTarget);
        if(theDot < Targetting.Angle)
        {
            float dist = (weaponPos - shipPos).magnitude;
            return dist < Targetting.Distance;
        }
        return false;
    }

    public void TryInitiateNewAttack(IEnumerable<SpaceShip> enemies)
    {
        if(TargetsPresent(enemies))
        {
            LaserBlast blast = ProjectilesPool.GetNextProjectile();
            FireBlast(blast);
        }
    }

    private void FireBlast(LaserBlast blast)
    {
        Vector3 pos = Ship.GameObject.transform.position;
        Quaternion rot = Ship.GameObject.transform.rotation;
        blast.Fire(pos, rot);
        this.timeBetweenLastBlast = 0;
    }
}
