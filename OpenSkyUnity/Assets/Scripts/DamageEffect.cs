using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class DamageEffect : AnimationRecorder<SpaceshipDamageKey>
{
    public GameObject GameObject { get; }

    public override bool IsActive => GameObject.activeInHierarchy;

    public DamageEffect(GameObject gameObject)
    {
        GameObject = gameObject;
    }

    protected override SpaceshipDamageKey MakeKeyFromCurrentState()
    {
        throw new System.NotImplementedException();
    }

    protected override void Display(SpaceshipDamageKey key)
    {
        throw new System.NotImplementedException();
    }

    public override void ClearVisuals()
    {
        throw new System.NotImplementedException();
    }
}

public class SpaceshipDamageKey : ISpaceObjectKey<SpaceshipDamageKey>
{
    // TODO: spaceship damage keying
    public Vector3 Position { get; }
    public Quaternion Rotation { get; }
    public float Progression { get; }

    public SpaceshipDamageKey(Vector3 position,
        Quaternion rotation, float progression)
    {
        Position = position;
        Rotation = rotation;
        Progression = progression;
    }

    public SpaceshipDamageKey LerpWith(SpaceshipDamageKey nextItem, float param)
    {
        Vector3 posRet = Vector3.Lerp(Position, nextItem.Position, param);
        Quaternion rotRet = Quaternion.Lerp(Rotation, nextItem.Rotation, param);
        float progression = Mathf.Lerp(Progression, nextItem.Progression, param);
        return new SpaceshipDamageKey(posRet, rotRet, progression);
    }
}