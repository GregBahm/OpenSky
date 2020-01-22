using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SpaceshipBlueprint
{
    public IEnumerable<IViewableSpaceObject> ViewableObjects { get; }

    public IEnumerable<SpaceShip> Spaceships { get; }
    public IEnumerable<Projectile> Projectiles { get; }
}

public class SpaceShip : SpaceObject
{
    public Vector3 TargetPosition { get; set; }

    public SpaceShip(GameObject obj)
        : base(obj)
    { }

    public void InitiateNewAttacks()
    {
        throw new NotImplementedException();
    }

    public void RegisterDamage(IEnumerable<SpaceShip> activeShips, IEnumerable<Projectile> activeProjectiles)
    {
        throw new NotImplementedException();
    }

    public override void MoveEntity()
    {
        throw new NotImplementedException();
    }
    
    public void ApplyDamage()
    {
        throw new NotImplementedException();
    }
}

public class Projectile : SpaceObject
{
    public Projectile(GameObject obj) 
        : base(obj)
    { }

    public override void MoveEntity()
    {
        throw new NotImplementedException();
    }

    public void ApplyDamage()
    {
        throw new NotImplementedException();
    }
}