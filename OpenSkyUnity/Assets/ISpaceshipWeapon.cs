using System.Collections.Generic;

public interface ISpaceshipWeapon
{
    IEnumerable<Projectile> Projectiles { get; }
    bool CanInitiateNewAttack { get; }

    void TryInitiateNewAttack(IEnumerable<SpaceShip> enemies);
}
