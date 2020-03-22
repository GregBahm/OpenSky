using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

public class Game
{
    public const int KeyframesPerTurn = 50;
    private int maxGameTime = 0;

    private readonly MasterTimeline timeline;

    private readonly CurrentGameState currentGameState;
    private readonly ReadOnlyCollection<IShipOrdersSource> orderCreators;

    public Game(IEnumerable<SpaceShip> spaceShips, IEnumerable<IShipOrdersSource> orderCreators)
    {
        this.orderCreators = orderCreators.ToList().AsReadOnly();
        IEnumerable<IAnimationRecorder> viewables = spaceShips.SelectMany(item => item.ViewableObjects);
        timeline = new MasterTimeline(viewables);
        IEnumerable<Projectile> projectiles = spaceShips.SelectMany(item => item.Weapons).SelectMany(item => item.Projectiles);
        currentGameState = new CurrentGameState(spaceShips, projectiles);
    }
    
    public void AdvanceToNextTurn()
    {
        SetTimeToCurrent();
        foreach (IShipOrdersSource order in orderCreators)
        {
            order.Apply();
        }
        for (int i = 0; i < KeyframesPerTurn; i++)
        {
            DoNextKeyframe(i);
        }
        maxGameTime += KeyframesPerTurn;
        foreach (IShipOrdersSource order in orderCreators)
        {
            order.SetupForNextTurn();
        }
    }

    public void SetTimeToCurrent()
    {
        DisplayNormalizedGametime(1);
    }

    public void DisplayNormalizedGametime(float normalizedTime)
    {
        timeline.DisplayAt(normalizedTime * maxGameTime);
    }

    private void DoNextKeyframe(int turnStep)
    {
        currentGameState.AdvanceGameOneStep(turnStep);
        timeline.CaptureKeyframe();
        maxGameTime++;
    }
}
