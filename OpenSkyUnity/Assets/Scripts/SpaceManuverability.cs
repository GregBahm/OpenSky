public class SpaceManuverability
{
    public float MaxThrust { get; }
    public float Acceleration { get; }
    public float RotationUpWeight { get; }
    public float RotationStrength { get; }

    public SpaceManuverability(float maxThrust,
        float rotationStrength,
        float rotationUpWeight,
        float acceleration)
    {
        MaxThrust = maxThrust;
        RotationUpWeight = rotationUpWeight;
        RotationStrength = rotationStrength;
        Acceleration = acceleration;
    }
}
