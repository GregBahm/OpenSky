using System;
using System.Collections.Generic;

public class LaserWeapon : ISpaceshipWeapon
{
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
    private readonly float delayBetweenBlasts;

    public LaserWeapon(IEnumerable<LaserBlast> projectiles,
        float delayBetweenVolleys,
        float delayBetweenBlasts,
        TargettingCone targetting)
    {
        ProjectilesPool = new ProjectilesPool<LaserBlast>(projectiles);
        this.delayBetweenVolleys = delayBetweenVolleys;
        this.delayBetweenBlasts = delayBetweenBlasts;
    }
    
    private SpaceShip TryGetTarget(IEnumerable<SpaceShip> targets)
    {
        throw new NotImplementedException();
    }

    public void TryInitiateNewAttack(IEnumerable<SpaceShip> enemies)
    {
        SpaceShip target = TryGetTarget(enemies);
        if(target != null)
        {
            LaserBlast blast = ProjectilesPool.GetNextProjectile();
            FireBlast(blast, target);
        }
    }

    private void FireBlast(LaserBlast blast, SpaceShip target)
    {
        throw new NotImplementedException();
    }
}
