using System;
using System.Collections;
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

public class CurrentGameState
{
    private ReadOnlyCollection<SpaceShip> spaceships;
    private ReadOnlyCollection<Projectile> projectiles;

    public CurrentGameState(IEnumerable<SpaceShip> spaceships,
        IEnumerable<Projectile> projectiles)
    {
        this.projectiles = projectiles.ToList().AsReadOnly();
        this.spaceships = spaceships.ToList().AsReadOnly();
    }

    private void MoveEntities()
    {
        foreach (SpaceShip ship in spaceships)
        {
            ship.MoveEntity();
        }
        foreach (Projectile projectile in projectiles)
        {
            projectile.MoveEntity();
        }
    }

    public void SetUnitOrders(IEnumerable<SpaceShipOrders> unitOrders)
    {
        foreach (SpaceShipOrders order in unitOrders)
        {
            order.ApplyOrders();
        }
    }

    internal void DoNextKeyframe()
    {
        MoveEntities();
        RegisterDamage();
        ApplyDamage();
        InitiateNewAttacks();
    }

    private void RegisterDamage()
    {
        IEnumerable<SpaceShip> ships = spaceships.Where(ship => ship.IsActive).ToArray();
        IEnumerable<Projectile> activeProjectiles = projectiles.Where(projectile => projectile.IsActive).ToArray();
        foreach (SpaceShip ship in spaceships)
        {
            ship.RegisterDamage(ships, activeProjectiles);
        }
    }

    private void ApplyDamage()
    {
        foreach (SpaceShip ship in spaceships)
        {
            ship.ApplyDamage();
        }
        foreach (Projectile projectile in projectiles)
        {
            projectile.ApplyDamage();
        }
    }

    private void InitiateNewAttacks()
    {
        foreach (SpaceShip ship in spaceships)
        {
            ship.InitiateNewAttacks();
        }
    }
}

public class SpaceShipOrders
{
    public SpaceShip Unit { get; }
    
    public Vector3 TargetPosition { get; } // TODO: Make this more elaborate

    internal void ApplyOrders()
    {
        throw new NotImplementedException();
    }
}

public interface IViewableSpaceObject
{
    void DisplayAtTime(float time);
}

public abstract class SpaceObject : IViewableSpaceObject
{
    public bool IsActive { get; private set; }
    public GameObject GameObject { get; }
    public SpaceObjectTimeline Timeline { get; }
    
    public SpaceObject(GameObject gameObject)
    {
        GameObject = gameObject;
        Timeline = new SpaceObjectTimeline();
    }

    public void RegisterKeyframe()
    {
        SpaceObjectKey frame = new SpaceObjectKey(GameObject.transform.position,
            GameObject.transform.rotation,
            IsActive);
        Timeline.AddKeyframe(frame);
    }

    public void DisplayAtTime(float time)
    {
        ISpaceObjectKey key = Timeline.GetTransformAtTime(time);
        GameObject.transform.position = key.Position;
        GameObject.transform.rotation = key.Rotation;
        GameObject.SetActive(key.Visible);
    }

    public abstract void MoveEntity();
}

public abstract class SpaceObjectTimeline<T>
    where T : ISpaceObjectKey
{
    private readonly List<T> keyframes = new List<T>();

    public void AddKeyframe(T keyframe)
    {
        keyframes.Add(keyframe);
    }

    public abstract T GetTransformAtTime(float time);

    protected T GetFrame(int frame)
    {
        frame = Mathf.Clamp(frame, 0, keyframes.Count - 1);
        return keyframes[frame];
    }
}

public class SpaceObjectTimeline : SpaceObjectTimeline<SpaceObjectKey>
{
    public override SpaceObjectKey GetTransformAtTime(float time)
    {
        float lerp = time % 1;
        SpaceObjectKey previousKey = GetFrame(Mathf.FloorToInt(time));
        SpaceObjectKey nextKey = GetFrame(Mathf.CeilToInt(time));
        Vector3 posRet = Vector3.Lerp(previousKey.Position, nextKey.Position, lerp);
        Quaternion rotRet = Quaternion.Lerp(previousKey.Rotation, nextKey.Rotation, lerp);
        bool visibleRet = lerp > .5f ? nextKey.Visible : previousKey.Visible;
        return new SpaceObjectKey(posRet, rotRet, visibleRet);
    }
}

public interface ISpaceObjectKey
{
    Vector3 Position { get; }
    Quaternion Rotation { get; }
    bool Visible { get; }
}

public struct SpaceObjectKey : ISpaceObjectKey
{
    public Vector3 Position { get; }
    public Quaternion Rotation { get; }
    public bool Visible { get; }

    public SpaceObjectKey(Vector3 position, Quaternion rotation, bool visible)
    {
        Position = position;
        Rotation = rotation;
        Visible = visible;
    }
}