using System.Collections.Generic;
using System.Collections.ObjectModel;

public class ProjectilesPool<T>
    where T : Projectile
{
    public ReadOnlyCollection<T> Pool { get; }
    private int nextProjectileIndex = 0;

    public ProjectilesPool(IProjectileDefinition<T> baseProjectile, int maxRounds)
    {
        Pool = CreatePool(baseProjectile, maxRounds).AsReadOnly();
    }

    private List<T> CreatePool(IProjectileDefinition<T> baseProjectile, int maxRounds)
    {
        List<T> ret = new List<T>();
        for (int i = 0; i < maxRounds; i++)
        {
            ret.Add(baseProjectile.ToProjectile());
        }
        return ret;
    }

    public T GetNextProjectile()
    {
        T ret = Pool[nextProjectileIndex];
        nextProjectileIndex = (nextProjectileIndex + 1) % Pool.Count;
        return ret;
    }
}
