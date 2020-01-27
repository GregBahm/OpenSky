using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

public class ProjectilesPool<T>
    where T : Projectile
{
    public ReadOnlyCollection<T> Pool { get; }
    private int nextProjectileIndex = 0;

    public ProjectilesPool(IEnumerable<T> projectiles)
    {
        Pool = projectiles.ToList().AsReadOnly();
    }
    

    public T GetNextProjectile()
    {
        T ret = Pool[nextProjectileIndex];
        nextProjectileIndex = (nextProjectileIndex + 1) % Pool.Count;
        return ret;
    }
}
