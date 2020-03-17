using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField]
    private Transform weightHandle;

    public float Weight { get { return weightHandle.localPosition.z; } }
}
