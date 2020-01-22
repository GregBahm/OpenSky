using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SpaceShip : IViewableSpaceObject
{
    public int TeamId { get; }
    public GameObject GameObject { get; }
    public ItemTimeline<SpaceshipKey> Timeline { get; }

    public bool IsActive { get; private set; }
    public Vector3 TargetPosition { get; set; }

    public SpaceShip(int teamId, GameObject gameObject)
    {
        TeamId = teamId;
        GameObject = gameObject;
        Timeline = new ItemTimeline<SpaceshipKey>(MakeKeyFromGameobject());
    }

    private SpaceshipKey MakeKeyFromGameobject()
    {
        throw new NotImplementedException();
    }

    public void RegisterDamage(IEnumerable<SpaceShip> activeShips, 
        IEnumerable<Projectile> activeProjectiles)
    {
        throw new NotImplementedException();
    }

    public void MoveEntity()
    {
        throw new NotImplementedException();
    }
    
    public void UpdateState()
    {
        throw new NotImplementedException();
    }

    internal void InitiateNewAttacks(IEnumerable<SpaceShip> friends, 
        IEnumerable<SpaceShip> enemies)
    {
        throw new NotImplementedException();
    }

    public void DisplayAtTime(float time)
    {
        throw new NotImplementedException();
    }

    public void RegisterKeyframe()
    {
        throw new NotImplementedException();
    }
}
