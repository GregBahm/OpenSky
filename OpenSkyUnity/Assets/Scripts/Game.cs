using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class Game
{
    private const int keyframesPerTurn = 50;
    private int maxGameTime = 0;

    private readonly ReadOnlyCollection<IViewableSpaceObject> viewableObjects;

    private readonly CurrentGameState currentGameState;

    public Game(IEnumerable<SpaceShip> spaceShips)
    {
        viewableObjects = spaceShips.SelectMany(item => item.ViewableObjects).ToList().AsReadOnly();
        IEnumerable<Projectile> projectiles = spaceShips.SelectMany(item => item.Weapons).SelectMany(item => item.Projectiles);
        currentGameState = new CurrentGameState(spaceShips, projectiles);
    }

    public void Update()
    {

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
