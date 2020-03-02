using System;
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
        SpaceShip[] spaceships = ships.Select(ship => ship.CreateShip()).ToArray();
        game = new Game(spaceships);
    }

    private void Update()
    {
        if(AdvanceToNextTurn)
        {
            AdvanceToNextTurn = false;
            //game.AdvanceToNextTurn(spaceshipOrders);
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

        public SpaceShip CreateShip()
        {
            return Definition.ToSpaceship(TeamId, InitialLocation);
        }
    }
}
