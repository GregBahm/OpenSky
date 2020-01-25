using System;
using UnityEngine;

[Serializable]
public class LaserBlastDefinition : IProjectileDefinition<LaserBlast>
{
    public GameObject GameObject;

    public LaserBlast ToProjectile()
    {
        throw new NotImplementedException();
    }
}
