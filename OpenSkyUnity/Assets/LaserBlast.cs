using UnityEngine;

public class LaserBlast : Projectile
{
    public float Speed { get; }

    public float Duration { get; }

    private float currentAge;

    public LaserBlast(float speed,
        float duration,
        GameObject gameObject)
        :base(gameObject)
    {
        Speed = speed;
        Duration = duration;
    }

    public void Fire(Vector3 position, Quaternion rotation)
    {
        currentAge = 0;
        IsActive = true;
        GameObject.transform.position = position;
        GameObject.transform.rotation = rotation;
    }

    public override void MoveEntity()
    {
        Vector3 movement = this.GameObject.transform.forward * Speed;
        this.GameObject.transform.position += movement;
    }

    protected override ProjectileKey MakeKeyFromGameobject()
    {
        float progression = currentAge / Duration;
        return new ProjectileKey(GameObject.transform.position,
            GameObject.transform.rotation,
            progression);
    }

    public override void UpdateState()
    {
        currentAge += 1;
        IsActive = currentAge > Duration;
    }
}