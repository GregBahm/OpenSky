using UnityEngine;

public interface IDamageSource
{
    float Damage { get; }
    CapsuleCollider Footprint { get; }
}
