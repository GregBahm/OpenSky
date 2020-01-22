using UnityEngine;

public abstract class Projectile : IViewableSpaceObject
{
    public bool IsActive { get; protected set; }

    public GameObject GameObject { get; }
    public ItemTimeline<ProjectileKey> Timeline { get; }

    public Projectile(GameObject gameObject)
    {
        GameObject = gameObject;
        Timeline = new ItemTimeline<ProjectileKey>(MakeKeyFromGameobject());
    }

    public void DisplayAtTime(float time)
    {
        ProjectileKey key = Timeline.GetTransformAtTime(time);
        GameObject.transform.position = key.Position;
        GameObject.transform.rotation = key.Rotation;
        GameObject.SetActive(key.Progression < 1);
        //TODO:: Use progression to display animation
    }

    protected abstract ProjectileKey MakeKeyFromGameobject();

    public void RegisterKeyframe()
    {
        Timeline.AddKeyframe(MakeKeyFromGameobject());
    }

    public abstract void MoveEntity();

    public abstract void UpdateState();
}
