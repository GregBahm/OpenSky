using System.Collections.Generic;

public interface ISpaceshipWeapon : IAnimationRecorder
{
    IEnumerable<Projectile> Projectiles { get; }
    bool CanInitiateNewAttack { get; }

    void TryInitiateNewAttack(IEnumerable<SpaceShip> enemies);
    void UpdateState();
}
