using UnityEngine;

public struct LaserWeaponKey : ISpaceObjectKey<LaserWeaponKey>
{
    public float Recoil { get; }

    public LaserWeaponKey(float recoil)
    {
        Recoil = recoil;
    }

    public LaserWeaponKey LerpWith(LaserWeaponKey nextKey, float param)
    {
        float recoil = Mathf.Lerp(Recoil, nextKey.Recoil, param);
        return new LaserWeaponKey(recoil);
    }
}