using System.Collections.Generic;
using System.Linq;

public class Game
{
    private const int keyframesPerTurn = 50;
    private int maxGameTime = 0;

    private readonly MasterTimeline timeline;

    private readonly CurrentGameState currentGameState;

    public Game(IEnumerable<SpaceShip> spaceShips)
    {
        IEnumerable<IAnimationRecorder> viewables = spaceShips.SelectMany(item => item.ViewableObjects);
        timeline = new MasterTimeline(viewables);
        IEnumerable<Projectile> projectiles = spaceShips.SelectMany(item => item.Weapons).SelectMany(item => item.Projectiles);
        currentGameState = new CurrentGameState(spaceShips, projectiles);
    }

    public void Update()
    {

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
        currentGameState.AdvanceGameOneStep();
        timeline.CaptureKeyframe();
        maxGameTime++;
    }
}
