public interface IAnimationRecorder
{
    void StartCapture();
    ISpaceObjectAnimator FinishCapture();
    bool IsActive { get; }
}
