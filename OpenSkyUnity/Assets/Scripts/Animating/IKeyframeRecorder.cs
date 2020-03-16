public interface IAnimationRecorder
{
    bool HasAnimationToRecord { get; }
    ISpaceObjectAnimator GetNextAnimator();
}
