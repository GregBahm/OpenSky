using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

public class Game
{
    private const int keyframesPerTurn = 50;
    private int maxGameTime = 0;

    private readonly ReadOnlyCollection<IViewableSpaceObject> viewableObjects;

    private readonly CurrentGameState currentGameState;

    public Game(IEnumerable<SpaceshipBlueprint> shipBlueprints)
    {
        viewableObjects = shipBlueprints.SelectMany(item => item.ViewableObjects).ToList().AsReadOnly();
        IEnumerable<SpaceShip> spaceships = shipBlueprints.SelectMany(item => item.Spaceships);
        IEnumerable<Projectile> projectiles = shipBlueprints.SelectMany(item => item.Projectiles);
        currentGameState = new CurrentGameState(spaceships, projectiles);
    }

    public void DisplayTime(float time)
    {
        foreach (IViewableSpaceObject obj in viewableObjects)
        {
            obj.DisplayAtTime(time);
        }
    }

    public void AdvanceToNextTurn(IEnumerable<SpaceShipOrders> unitOrders)
    {
        currentGameState.SetUnitOrders(unitOrders);
        for (int i = 0; i < keyframesPerTurn; i++)
        {
            DoNextKeyframe();
        }
    }

    private void DoNextKeyframe()
    {
        DisplayTime(maxGameTime);
        currentGameState.DoNextKeyframe();
        maxGameTime++;
    }
}
