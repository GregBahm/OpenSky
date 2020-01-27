public class SpaceManuverability
{
    public float MaxThrust { get; }
    public float MaxAngleChange { get; }
    public float Acceleration { get; }

    public SpaceManuverability(float maxThrust,
        float maxAngleChange,
        float acceleration)
    {
        MaxThrust = maxThrust;
        MaxAngleChange = maxAngleChange;
        Acceleration = acceleration;
    }
}
