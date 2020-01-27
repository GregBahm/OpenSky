using System;
using UnityEngine;

public class Missile : Projectile
{
    public Missile(float damage,
        float radius,
        GameObject gameObject)
        : base(damage, radius, gameObject)
    { }

    public SpaceShip AttackTarget { get; set; }

    public override void MoveEntity()
    {
        throw new NotImplementedException();
    }

    public override void UpdateState()
    {
        throw new NotImplementedException();
    }

    protected override ProjectileKey MakeKeyFromGameobject()
    {
        throw new NotImplementedException();
    }
}
