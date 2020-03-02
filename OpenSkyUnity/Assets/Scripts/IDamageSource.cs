using UnityEngine;

public interface IDamageSource
{
    float Damage { get; }
    Vector3 Position { get; }
    float Radius { get; }
}
