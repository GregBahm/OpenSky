using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class GameBehaviour : MonoBehaviour
{
    [SerializeField]
    private InitialShip[] ships;
    private Game game;

    [Range(0, 1)]
    public float TimeToDisplay;
    public bool AdvanceToNextTurn;

    private void Start()
    {
        ShipStuff[] shipStuff = ships.Select(ship => CreateShip(ship)).ToArray();
        game = new Game(shipStuff.Select(item => item.SpaceShip), 
            shipStuff.Select(item => item.OrdersSource));
    }

    private ShipStuff CreateShip(InitialShip initialShip)
    {
        SpaceShip ship = initialShip.Definition.ToSpaceship(initialShip.TeamId, initialShip.InitialLocation);
        IShipOrdersSource ordersSource = initialShip.Definition.CreateOrdersSource(ship);
        return new ShipStuff(ship, ordersSource);
    }

    private void Update()
    {
        if(AdvanceToNextTurn)
        {
            AdvanceToNextTurn = false;
            game.AdvanceToNextTurn();
            TimeToDisplay = 1;
        }
        else
        {
            game.DisplayNormalizedGametime(TimeToDisplay);
        }
    }

    [Serializable]
    public class InitialShip
    {
        public int TeamId;

        public ShipDefinition Definition;

        public Transform InitialLocation;
    }

    private class ShipStuff
    {
        public SpaceShip SpaceShip { get; }
        public IShipOrdersSource OrdersSource { get; }

        public ShipStuff(SpaceShip spaceShip, IShipOrdersSource ordersSource)
        {
            SpaceShip = spaceShip;
            OrdersSource = ordersSource;
        }
    }
}
