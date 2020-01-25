using System;
using System.Linq;
using UnityEngine;

public class GameBehaviour : MonoBehaviour
{
    [SerializeField]
    private InitialShip[] ships;
    private Game game;

    private void Start()
    {
        SpaceShip[] spaceships = ships.Select(ship => ship.ToShip()).ToArray();
        game = new Game(spaceships);
    }

    private void Update()
    {
        game.Update();
    }

    [Serializable]
    public class InitialShip
    {
        public int TeamId;

        public ShipDefinition Definition;

        public Transform InitialLocation;

        public SpaceShip ToShip()
        {
            throw new NotImplementedException();
        }
    }
}
