using UnityEngine;

public abstract class Projectile : AnimationRecorder<ProjectileKey>, IDamageSource
{
    protected bool isActive;
    public override bool IsActive => isActive;

    public GameObject GameObject { get; }

    public float Damage { get; }

    public Vector3 Position { get { return GameObject.transform.position; } }

    public float Radius { get; }

    public Projectile(float damage,
        float radius,
        GameObject gameObject)
    {
        Damage = damage;
        Radius = radius;
        GameObject = gameObject;
    }

    protected override void Display(ProjectileKey key)
    { 
        GameObject.transform.position = key.Position;
        GameObject.transform.rotation = key.Rotation;
        GameObject.SetActive(key.Progression < 1);
        //TODO: Use progression to display animation
    }

    public abstract void MoveEntity();

    public abstract void UpdateState();

    public virtual void OnSpaceshipHit(SpaceShip spaceship)
    { }
}
