using UnityEngine;

public class WaypointCreator : MonoBehaviour
{
    [SerializeField]
    private Transform weightHandle;

    public float Weight { get { return weightHandle.localPosition.z; } }
}
