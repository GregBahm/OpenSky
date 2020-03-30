using UnityEngine;

public class SpaceshipKey : ISpaceObjectKey<SpaceshipKey>
{
    public Vector3 Position { get; }
    public Quaternion Rotation { get; }
    public float Destruction { get; }

    public SpaceshipKey(Vector3 position, 
        Quaternion rotation, 
        float destruction)
    {
        Position = position;
        Rotation = rotation;
        Destruction = destruction;
    }

    public SpaceshipKey LerpWith(SpaceshipKey nextItem, float param)
    {
        Vector3 posRet = Vector3.Lerp(Position, nextItem.Position, param);
        Quaternion rotRet = Quaternion.Lerp(Rotation, nextItem.Rotation, param);
        float destruction = Mathf.Lerp(Destruction, nextItem.Destruction, param);
        return new SpaceshipKey(posRet, rotRet, destruction);
    }
}
