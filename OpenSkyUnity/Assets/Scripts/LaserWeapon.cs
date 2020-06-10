using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LaserWeapon : AnimationRecorder<LaserWeaponKey>,  ISpaceshipWeapon
{
    public SpaceShip Ship { get; }

    public IReadOnlyCollection<LaserBlast> ProjectilesPool { get; }

    public IEnumerable<Projectile> Projectiles { get { return ProjectilesPool; } }

    public TargettingCone Targetting { get; }

    public bool CanInitiateNewAttack
    {
        get
        {
            return currentBlastCooldown <= 0
                && ProjectilesPool.Any(item => !item.IsActive);
        }
    }

    public override bool IsActive => Ship.IsActive;
    
    private readonly float blastCooldown;
    private float currentBlastCooldown;

    public LaserWeapon(
        SpaceShip ship,
        IEnumerable<LaserBlast> projectiles,
        float delayBetweenBlasts,
        TargettingCone targetting)
    {
        Ship = ship;
        ProjectilesPool = projectiles.ToList().AsReadOnly();
        this.blastCooldown = delayBetweenBlasts;
        Targetting = targetting;
    }

    public void UpdateState()
    {
        currentBlastCooldown -= 1;
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
            LaserBlast blast = ProjectilesPool.GetNext();
            FireBlast(blast);
        }
    }

    public override void ClearVisuals()
    { }

    private void FireBlast(LaserBlast blast)
    {
        Vector3 pos = Ship.GameObject.transform.position;
        Quaternion rot = Ship.GameObject.transform.rotation;
        blast.Fire(pos, rot);
        this.currentBlastCooldown = blastCooldown;
    }

    protected override LaserWeaponKey MakeKeyFromCurrentState()
    {
        return new LaserWeaponKey(currentBlastCooldown);
    }

    protected override void Display(LaserWeaponKey key)
    {
        // TODO: display laser weapon game object
    }
}
