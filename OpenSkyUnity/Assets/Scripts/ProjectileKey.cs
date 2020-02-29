using UnityEngine;

public struct ProjectileKey
{
    public Vector3 Position { get; }
    public Quaternion Rotation { get; }
    public float Progression { get; }

    public ProjectileKey(Vector3 position, 
        Quaternion rotation, float progression)
    {
        Position = position;
        Rotation = rotation;
        Progression = progression;
    }

    public ProjectileKey LerpWith(ProjectileKey nextItem, float param)
    {
        Vector3 posRet = Vector3.Lerp(Position, nextItem.Position, param);
        Quaternion rotRet = Quaternion.Lerp(Rotation, nextItem.Rotation, param);
        float progression = Mathf.Lerp(Progression, nextItem.Progression, param);
        return new ProjectileKey(posRet, rotRet, progression);
    }
}
