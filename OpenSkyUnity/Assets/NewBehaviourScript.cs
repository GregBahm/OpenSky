using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Game
{
    private const int keyframesPerTurn = 50;
    private int maxGameTime = 0;

    private readonly List<SpaceObject> objects = new List<SpaceObject>();

    private readonly CurrentGameState currentGameState;

    public Game()
    {
        currentGameState = new CurrentGameState();
    }

    public void DisplayTime(float time)
    {
        foreach (SpaceObject obj in objects)
        {
            obj.DisplayTime(time);
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
    private List<SpaceObject> activeEntities = new List<SpaceObject>();

    private void MoveEntities()
    {
        foreach (SpaceObject obj in activeEntities)
        {
            obj.MoveToNextKeyframe();
            SpaceObjectTransform frame = new SpaceObjectTransform(obj.GameObject.transform.position, obj.GameObject.transform.rotation);
            obj.Timeline.AddKeyframe(frame);
        }
    }

    public void SetUnitOrders(IEnumerable<SpaceShipOrders> unitOrders)
    {
        foreach (SpaceShipOrders order in unitOrders)
        {
            order.Unit.TargetPosition = order.TargetPosition;
        }
    }

    private List<SpaceObject> GetNewActiveEntities()
    {
        List<SpaceObject> ret = new List<SpaceObject>();
        foreach (SpaceObject item in activeEntities)
        {
            if(item.IsAlive)
            {
                ret.Add(item);
                ret.AddRange(item.GetSpawnedObjects());
            }
        }
        return ret;
    }

    private void UpdateEntityStates()
    {
        foreach (SpaceObject obj in activeEntities)
        {
            obj.UpdateEntityState(activeEntities);
        }
    }

    private void SpawnNewEntities()
    {
        foreach (SpaceObject obj in activeEntities)
        {
            obj.UpdateEntityState(activeEntities);
        }
    }

    internal void DoNextKeyframe()
    {
        MoveEntities();
        UpdateEntityStates();
        activeEntities = GetNewActiveEntities();
    }
}

public class SpaceShipOrders
{
    public SpaceShip Unit { get; }
    
    public Vector3 TargetPosition { get; } // TODO: Make this more elaborate
}

public class SpaceShip : SpaceObject
{
    public Vector3 TargetPosition { get; set; }

    public override bool IsAlive => throw new NotImplementedException();

    public SpaceShip(GameObject obj)
        :base(obj)
    { }

    public override void MoveToNextKeyframe()
    {
        throw new NotImplementedException();
    }

    public override void UpdateEntityState(IEnumerable<SpaceObject> aciveObjects)
    {
        throw new NotImplementedException();
    }

    public override IEnumerable<SpaceObject> GetSpawnedObjects()
    {
        throw new NotImplementedException();
    }
}

public class Projectile : SpaceObject
{
    public Projectile(GameObject gameObject) 
        : base(gameObject)
    { }

    public override bool IsAlive => throw new NotImplementedException();

    public override IEnumerable<SpaceObject> GetSpawnedObjects()
    {
        throw new NotImplementedException();
    }

    public override void MoveToNextKeyframe()
    {
        throw new NotImplementedException();
    }

    public override void UpdateEntityState(IEnumerable<SpaceObject> aciveObjects)
    {
        throw new NotImplementedException();
    }
}

public abstract class SpaceObject
{
    public GameObject GameObject { get; }
    public SpaceObjectTimeline Timeline { get; }
    public abstract bool IsAlive { get; }
    
    public SpaceObject(GameObject gameObject)
    {
        GameObject = gameObject;
        Timeline = new SpaceObjectTimeline(gameObject.transform.position, gameObject.transform.rotation);
    }

    public abstract void MoveToNextKeyframe();
    public abstract void UpdateEntityState(IEnumerable<SpaceObject> aciveObjects);

    public abstract IEnumerable<SpaceObject> GetSpawnedObjects();

    public void DisplayTime(float time)
    {
        bool doesExist = Timeline.DoesExistAtTime(time);
        GameObject.SetActive(doesExist);
        if(doesExist)
        {
            SpaceObjectTransform trans = Timeline.GetTransformAtTime(time);
            GameObject.transform.position = trans.Position;
            GameObject.transform.rotation = trans.Rotation;
        }
    }
}

public class SpaceObjectTimeline
{
    private int timelineStart;
    private readonly List<SpaceObjectTransform> keyframes;

    public SpaceObjectTimeline(Vector3 startingPosition, Quaternion startingRotation)
    {
        keyframes = new List<SpaceObjectTransform> { new SpaceObjectTransform(startingPosition, startingRotation) };
    }

    public void AddKeyframe(SpaceObjectTransform keyframe)
    {
        keyframes.Add(keyframe);
    }

    public bool DoesExistAtTime(float time)
    {
        if(time > timelineStart)
        {
            return time < timelineStart + keyframes.Count;
        }
        return false;
    }

    public SpaceObjectTransform GetTransformAtTime(float time)
    {
        float lerp = time % 1;
        SpaceObjectTransform previousKey = GetFrame(Mathf.FloorToInt(time));
        SpaceObjectTransform nextKey = GetFrame(Mathf.CeilToInt(time));
        Vector3 posRet = Vector3.Lerp(previousKey.Position, nextKey.Position, lerp);
        Quaternion rotRet = Quaternion.Lerp(previousKey.Rotation, nextKey.Rotation, lerp);
        return new SpaceObjectTransform(posRet, rotRet);
    }

    private SpaceObjectTransform GetFrame(int frame)
    {
        frame = frame - timelineStart;
        frame = Mathf.Clamp(frame, 0, keyframes.Count - 1);
        return keyframes[frame];
    }
}

public struct SpaceObjectTransform
{
    public Vector3 Position { get; }
    public Quaternion Rotation { get; }

    public SpaceObjectTransform(Vector3 position, Quaternion rotation)
    {
        Position = position;
        Rotation = rotation;
    }
}