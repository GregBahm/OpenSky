using System.Collections.Generic;
using UnityEngine;

public class ItemTimeline<T>
    where T : IItemKey<T>
{
    private readonly List<T> keyframes;

    public ItemTimeline(T initialKey)
    {
        keyframes = new List<T>() { initialKey };
    }

    public void AddKeyframe(T keyframe)
    {
        keyframes.Add(keyframe);
    }

    public T GetTransformAtTime(float time)
    {
        float lerp = time % 1;
        T previousKey = GetFrame(Mathf.FloorToInt(time));
        T nextKey = GetFrame(Mathf.CeilToInt(time));
        return previousKey.LerpWith(nextKey, lerp);
    }

    protected T GetFrame(int frame)
    {
        frame = Mathf.Clamp(frame, 0, keyframes.Count - 1);
        return keyframes[frame];
    }
}
