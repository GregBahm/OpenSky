﻿using UnityEngine;

public struct SpaceshipKey : IItemKey<SpaceshipKey>
{
    public Vector3 Position { get; }
    public Quaternion Rotation { get; }
    public float Destruction { get; }
    public float AttackProgress { get; }

    public SpaceshipKey(Vector3 position, 
        Quaternion rotation, 
        float destruction,
        float attackProgress)
    {
        Position = position;
        Rotation = rotation;
        Destruction = destruction;
        AttackProgress = attackProgress;
    }

    public SpaceshipKey LerpWith(SpaceshipKey nextItem, float param)
    {
        Vector3 posRet = Vector3.Lerp(Position, nextItem.Position, param);
        Quaternion rotRet = Quaternion.Lerp(Rotation, nextItem.Rotation, param);
        float destruction = Mathf.Lerp(Destruction, nextItem.Destruction, param);
        float attackProgress = Mathf.Lerp(AttackProgress, nextItem.AttackProgress, param);
        return new SpaceshipKey(posRet, rotRet, destruction, attackProgress);
    }
}