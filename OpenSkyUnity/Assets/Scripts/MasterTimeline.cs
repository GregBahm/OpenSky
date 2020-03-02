using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class MasterTimeline
{
    private readonly ReadOnlyCollection<IAnimationRecorder> recorders;
    private readonly List<GameFrame> frames = new List<GameFrame>();

    public MasterTimeline(IEnumerable<IAnimationRecorder> viewables)
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
        IEnumerable<ISpaceObjectAnimator> animators = GatherAnimators();
        GameFrame newFrame = new GameFrame(animators);
    }

    private IEnumerable<ISpaceObjectAnimator> GatherAnimators()
    {
        return recorders.Where(item => item.HasAnimationToRecord).Select(item => item.GetNextAnimator());
    }
}
