using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

public class CurrentGameState
{
    private ReadOnlyCollection<SpaceShip> allPossibleSpaceships;
    private ReadOnlyCollection<Projectile> allPossibleProjectiles;

    public CurrentGameState(IEnumerable<SpaceShip> spaceships,
        IEnumerable<Projectile> projectiles)
    {
        this.allPossibleProjectiles = projectiles.ToList().AsReadOnly();
        this.allPossibleSpaceships = spaceships.ToList().AsReadOnly();
    }
    
    internal void AdvanceGameOneStep(int turnStep)
    {
        IEnumerable<SpaceShip> activeShips = allPossibleSpaceships.Where(ship => ship.IsActive).ToArray();
        IEnumerable<Projectile> activeProjectiles = allPossibleProjectiles.Where(projectile => projectile.IsActive).ToArray();

        MoveEntities(activeShips, activeProjectiles, turnStep);
        RegisterDamage(activeShips, activeProjectiles);
        UpdateState();
        InitiateNewAttacks(activeShips);
    }

    private void MoveEntities(IEnumerable<SpaceShip> activeShips, 
        IEnumerable<Projectile> activeProjectiles,
        int turnStep)
    {
        foreach (SpaceShip ship in activeShips)
        {
            ship.MoveEntity(turnStep);
        }
        foreach (Projectile projectile in activeProjectiles)
        {
            projectile.MoveEntity();
        }
    }

    private void RegisterDamage(IEnumerable<SpaceShip> activeShips,
        IEnumerable<Projectile> activeProjectiles)
    {
        foreach (SpaceShip ship in activeShips)
        {
            ship.RegisterDamage(activeShips, activeProjectiles);
        }
    }

    private void UpdateState()
    {
        foreach (SpaceShip ship in allPossibleSpaceships.Where(item => item.IsActive))
        {
            ship.UpdateState();
        }
        foreach (Projectile projectile in allPossibleProjectiles.Where(item => item.IsActive))
        {
            projectile.UpdateState();
        }
    }

    private void InitiateNewAttacks(IEnumerable<SpaceShip> activeShips)
    {
        foreach (SpaceShip attackingShip in activeShips)
        {
            IEnumerable<SpaceShip> enemies = activeShips.Where(ship => ship.TeamId != attackingShip.TeamId && ship != attackingShip);
            attackingShip.InitiateNewAttacks(enemies);
        }
    }
}