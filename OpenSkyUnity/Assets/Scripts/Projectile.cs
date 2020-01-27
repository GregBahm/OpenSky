using UnityEngine;

public abstract class Projectile : IViewableSpaceObject, IDamageSource
{
    public bool IsActive { get; protected set; }

    public GameObject GameObject { get; }
    public ItemTimeline<ProjectileKey> Timeline { get; }

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
        Timeline = new ItemTimeline<ProjectileKey>(MakeKeyFromGameobject());
    }

    public void DisplayAtTime(float time)
    {
        ProjectileKey key = Timeline.GetTransformAtTime(time);
        GameObject.transform.position = key.Position;
        GameObject.transform.rotation = key.Rotation;
        GameObject.SetActive(key.Progression < 1);
        //TODO: Use progression to display animation
    }

    protected abstract ProjectileKey MakeKeyFromGameobject();

    public void RegisterKeyframe()
    {
        Timeline.AddKeyframe(MakeKeyFromGameobject());
    }

    public abstract void MoveEntity();

    public abstract void UpdateState();
}

public interface IDamageSource
{
    float Damage { get; }
    Vector3 Position { get; }
    float Radius { get; }
}

public interface IHitable
{
    bool IsHitBy(IDamageSource source);
}