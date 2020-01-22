using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        // Could the ship make a new attack?
        // Get the list of opponent ships
        throw new NotImplementedException();
    }

    public void RegisterDamage(IEnumerable<SpaceShip> activeShips, 
        IEnumerable<Projectile> activeProjectiles)
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

public class Explosion : IViewableSpaceObject
{
    public GameObject GameObject { get; }
    public ExplosionTimeline Timeline { get; }

    public void DisplayAtTime(float time)
    {
        ExplosionKey key = Timeline.GetTransformAtTime(time);
        GameObject.transform.position = key.Position;
        GameObject.transform.rotation = key.Rotation;
        GameObject.SetActive(key.Progression < 1);
        //TODO:: Use progression to display animation
    }
}


public class LaserBlast : Projectile
{
    public LaserBlast(GameObject obj) : base(obj)
    { }
}

public class Missile : Projectile
{
    public Missile(GameObject obj) : base(obj)
    { }
}

public class SpaceShipOrders
{
    public SpaceShip Unit { get; }

    public Vector3 TargetPosition { get; } // TODO: Make this more elaborate

    internal void ApplyOrders()
    {
        throw new NotImplementedException();
    }
}