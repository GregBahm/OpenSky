using UnityEngine;

public abstract class WeaponDefinition : MonoBehaviour
{
    public abstract ISpaceshipWeapon ToWeapon(SpaceShip ship);
}
