using System;

public abstract class AnimationRecorder<T> : IAnimationRecorder
    where T : ISpaceObjectKey<T>
{
    private T lastKey;
    private bool lastKeyRecorded;

    protected abstract T MakeKeyFromCurrentState();

    public abstract bool IsActive { get; }

    protected abstract void Display(T key);

    public bool HasAnimationToRecord { get { return !lastKeyRecorded; } }

    public ISpaceObjectAnimator GetNextAnimator()
    {
        lastKeyRecorded = IsActive;
        T startKey = lastKey;
        lastKey = MakeKeyFromCurrentState();
        return new SpaceObjectAnimator(startKey, lastKey, Display);
    }

    private class SpaceObjectAnimator : ISpaceObjectAnimator
    {
        private readonly T startKey;
        private readonly T endKey;
        private readonly Action<T> displayFunction;

        public SpaceObjectAnimator(T startKey, T endKey, Action<T> displayFunction)
        {
            this.startKey = startKey;
            this.endKey = endKey;
            this.displayFunction = displayFunction;
        }

        public void Display(float timeWithinFrame)
        {
            T key = startKey.LerpWith(endKey, timeWithinFrame);
            displayFunction(key);
        }
    }
}
