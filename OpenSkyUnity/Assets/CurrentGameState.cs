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

    public void SetUnitOrders(IEnumerable<SpaceShipOrders> unitOrders)
    {
        foreach (SpaceShipOrders order in unitOrders)
        {
            order.ApplyOrders();
        }
    }

    internal void DoNextKeyframe()
    {
        IEnumerable<SpaceShip> activeShips = allPossibleSpaceships.Where(ship => ship.IsActive).ToArray();
        IEnumerable<Projectile> activeProjectiles = allPossibleProjectiles.Where(projectile => projectile.IsActive).ToArray();

        MoveEntities(activeShips, activeProjectiles);
        RegisterDamage(activeShips, activeProjectiles);
        UpdateState();
        InitiateNewAttacks(activeShips);
    }

    private void MoveEntities(IEnumerable<SpaceShip> activeShips, 
        IEnumerable<Projectile> activeProjectiles)
    {
        foreach (SpaceShip ship in activeShips)
        {
            ship.MoveEntity();
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
        foreach (SpaceShip ship in allPossibleSpaceships)
        {
            ship.UpdateState();
        }
        foreach (Projectile projectile in allPossibleProjectiles)
        {
            projectile.UpdateState();
        }
    }

    private void InitiateNewAttacks(IEnumerable<SpaceShip> activeShips)
    {
        foreach (SpaceShip attackingShip in activeShips)
        {
            IEnumerable<SpaceShip> friends = activeShips.Where(ship => ship.TeamId == attackingShip.TeamId && ship != attackingShip);
            IEnumerable<SpaceShip> enemies = activeShips.Where(ship => ship.TeamId != attackingShip.TeamId && ship != attackingShip);
            attackingShip.InitiateNewAttacks(friends, enemies);
        }
    }
}
