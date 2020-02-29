using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class MasterTimeline
{
    private readonly ReadOnlyCollection<IKeyframeRecorder> recorders;
    private readonly List<GameFrame> frames = new List<GameFrame>();

    public MasterTimeline(IEnumerable<IKeyframeRecorder> viewables)
    {
        recorders = viewables.ToList().AsReadOnly();
    }

    public void DisplayAt(float time)
    {
        if(time > frames.Count)
        {
            throw new InvalidOperationException("Can't DisplayAt() " + time + " when there are only " + frames.Count + " frames.");
        }
        float timeWithinFrame = time % 1;
        int frame = Mathf.FloorToInt(time);
        if(frame == frames.Count)
        {
            frame = frame - 1;
            timeWithinFrame = 1;
        }
        frames[frame].Display(timeWithinFrame);
    }

    internal void CaptureKeyframe()
    {
        IEnumerable<ISpaceObjectKey> keys = GatherKeys();
        GameFrame newFrame = new GameFrame(keys);
    }

    private IEnumerable<ISpaceObjectKey> GatherKeys()
    {
        return recorders.Select(item => item.RecordNextKey()).Where(item => item != null);
    }
}
