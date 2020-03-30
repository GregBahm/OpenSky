using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

public class Game
{
    public const int KeyframesPerTurn = 50;
    private int maxGameTime = 0;
    public bool ShowingLatest
    {
        get
        {
            return timeline.CurrentTime == maxGameTime;
        }
    }

    private readonly MasterTimeline timeline;

    private readonly CurrentGameState currentGameState;
    private readonly ReadOnlyCollection<IShipOrdersSource> orderCreators;
    private readonly ReadOnlyCollection<LocalOrdersCreator> localOrders;

    public Game(IEnumerable<SpaceShip> spaceShips, IEnumerable<IShipOrdersSource> orderCreators)
    {
        this.orderCreators = orderCreators.ToList().AsReadOnly();
        this.localOrders = GetLocalOrderCreators();
        IEnumerable<IAnimationRecorder> viewables = spaceShips.SelectMany(item => item.ViewableObjects);
        timeline = new MasterTimeline(viewables);
        IEnumerable<Projectile> projectiles = spaceShips.SelectMany(item => item.Weapons).SelectMany(item => item.Projectiles);
        currentGameState = new CurrentGameState(spaceShips, projectiles);
        SetTimeToCurrent();
    }

    private ReadOnlyCollection<LocalOrdersCreator> GetLocalOrderCreators()
    {
        return this.orderCreators.Select(item => item as LocalOrdersCreator)
            .Where(item => item != null)
            .ToList().AsReadOnly();
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
        UpdateOrderCreators();
    }

    private void UpdateOrderCreators()
    {
        foreach (LocalOrdersCreator item in localOrders)
        {
            if(ShowingLatest)
            {
                item.ShowVisuals();
            }
            else
            {
                item.HideVisuals();
            }
        }
    }

    private void DoNextKeyframe(int turnStep)
    {
        timeline.BeginKeyframeCapture();
        currentGameState.AdvanceGameOneStep(turnStep);
        timeline.FinishKeyframeCapture();
        maxGameTime++;
    }
}
