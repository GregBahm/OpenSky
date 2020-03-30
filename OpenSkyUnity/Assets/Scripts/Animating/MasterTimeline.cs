using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class MasterTimeline
{
    private readonly ReadOnlyCollection<IAnimationRecorder> recorders;
    private readonly List<GameFrame> frames = new List<GameFrame>();

    public float CurrentTime { get; private set; }

    public MasterTimeline(IEnumerable<IAnimationRecorder> viewables)
    {
        recorders = viewables.ToList().AsReadOnly();
    }

    public void DisplayAt(float time)
    {
        if(frames.Count == 0)
        {
            return;
        }
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
        CurrentTime = time;
    }

    internal void FinishKeyframeCapture()
    {
        IEnumerable<ISpaceObjectAnimator> animators = GatherAnimators();
        GameFrame newFrame = new GameFrame(animators);
        frames.Add(newFrame);
    }

    private IEnumerable<ISpaceObjectAnimator> GatherAnimators()
    {
        foreach (IAnimationRecorder item in recorders)
        {
            if(item.IsActive)
            {
                yield return item.FinishCapture();
            }
        }
    }

    internal void BeginKeyframeCapture()
    {
        foreach (IAnimationRecorder item in recorders)
        {
            item.StartCapture();
        }
    }
}
