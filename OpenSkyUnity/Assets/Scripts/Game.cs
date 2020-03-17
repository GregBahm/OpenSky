using System.Collections.Generic;
using System.Linq;

public class Game
{
    public const int KeyframesPerTurn = 50;
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
    
    public void AdvanceToNextTurn(IEnumerable<SpaceShipOrders> unitOrders)
    {
        SetTimeToCurrent();
        currentGameState.SetUnitOrders(unitOrders);
        for (int i = 0; i < KeyframesPerTurn; i++)
        {
            float turnProgress = (float)i / KeyframesPerTurn;
            DoNextKeyframe(turnProgress);
        }
        maxGameTime += KeyframesPerTurn;
    }

    public void SetTimeToCurrent()
    {
        DisplayNormalizedGametime(1);
    }

    public void DisplayNormalizedGametime(float normalizedTime)
    {
        timeline.DisplayAt(normalizedTime * maxGameTime);
    }

    private void DoNextKeyframe(float turnProgress)
    {
        currentGameState.AdvanceGameOneStep(turnProgress);
        timeline.CaptureKeyframe();
        maxGameTime++;
    }
}
