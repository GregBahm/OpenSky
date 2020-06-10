using System;

public abstract class AnimationRecorder<T> : IAnimationRecorder
    where T : ISpaceObjectKey<T>
{
    private T startKey;
    private bool wasActiveLastKey;

    protected abstract T MakeKeyFromCurrentState();

    public abstract bool IsActive { get; }

    protected abstract void Display(T key);

    public abstract void ClearVisuals();

    public void StartCapture()
    {
        startKey = MakeKeyFromCurrentState();
    }

    public ISpaceObjectAnimator FinishCapture()
    {
        wasActiveLastKey = IsActive;
        T endKey = MakeKeyFromCurrentState();
        return new SpaceObjectAnimator(this.startKey, endKey, this.Display);
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
