
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.XR.WSA;

#if WINDOWS_UWP
using Windows.Perception;
using Windows.Perception.People;
using Windows.UI.Input.Spatial;
#endif

public class Hands : MonoBehaviour
{
    private SpatialInteractionManager spatialInteractionManager;
    private static readonly HandJointKind[] jointIndices = new HandJointKind[]
    {
        HandJointKind.Palm,
        HandJointKind.Wrist,
        HandJointKind.ThumbMetacarpal,
        HandJointKind.ThumbProximal,
        HandJointKind.ThumbDistal,
        HandJointKind.ThumbTip,
        HandJointKind.IndexMetacarpal,
        HandJointKind.IndexProximal,
        HandJointKind.IndexIntermediate,
        HandJointKind.IndexDistal,
        HandJointKind.IndexTip,
        HandJointKind.MiddleMetacarpal,
        HandJointKind.MiddleProximal,
        HandJointKind.MiddleIntermediate,
        HandJointKind.MiddleDistal,
        HandJointKind.MiddleTip,
        HandJointKind.RingMetacarpal,
        HandJointKind.RingProximal,
        HandJointKind.RingIntermediate,
        HandJointKind.RingDistal,
        HandJointKind.RingTip,
        HandJointKind.LittleMetacarpal,
        HandJointKind.LittleProximal,
        HandJointKind.LittleIntermediate,
        HandJointKind.LittleDistal,
        HandJointKind.LittleTip
    };
    private readonly JointPose[] jointPoses = new JointPose[jointIndices.Length];

    private Transform[] leftHandProxies;
    private Transform[] rightHandProxies;

#if UNITY_WSA
#if NETFX_CORE
        [DllImport("DotNetNativeWorkaround.dll", EntryPoint = "MarshalIInspectable")]
        private static extern void GetSpatialCoordinateSystem(IntPtr nativePtr, out SpatialCoordinateSystem coordinateSystem);

        public static SpatialCoordinateSystem SpatialCoordinateSystem => spatialCoordinateSystem ?? (spatialCoordinateSystem = GetSpatialCoordinateSystem(WorldManager.GetNativeISpatialCoordinateSystemPtr()));

        /// <summary>
        /// Helps marshal WinRT IInspectable objects that have been passed to managed code as an IntPtr.
        /// </summary>
        /// <remarks>
        /// On .NET Native, IInspectable pointers cannot be marshaled from native to managed code using Marshal.GetObjectForIUnknown.
        /// This class calls into a native method that specifically marshals the type as a specific WinRT interface, which
        /// is supported by the marshaller on both .NET Core and .NET Native.
        /// Please see https://microsoft.github.io/MixedRealityToolkit-Unity/Documentation/InputSystem/HandTracking.html#net-native for more info.
        /// </remarks>
        private static SpatialCoordinateSystem GetSpatialCoordinateSystem(IntPtr nativePtr)
        {
            try
            {
                GetSpatialCoordinateSystem(nativePtr, out SpatialCoordinateSystem coordinateSystem);
                return coordinateSystem;
            }
            catch
            {
                UnityEngine.Debug.LogError("Call to the DotNetNativeWorkaround plug-in failed. The plug-in is required for correct behavior when using .NET Native compilation");
                return Marshal.GetObjectForIUnknown(nativePtr) as SpatialCoordinateSystem;
            }
        }
#elif WINDOWS_UWP
    public static SpatialCoordinateSystem SpatialCoordinateSystem => spatialCoordinateSystem ?? (spatialCoordinateSystem = Marshal.GetObjectForIUnknown(WorldManager.GetNativeISpatialCoordinateSystemPtr()) as SpatialCoordinateSystem);
#else
        public static SpatialCoordinateSystem SpatialCoordinateSystem
        {
            get
            {
                var spatialCoordinateSystemPtr = WorldManager.GetNativeISpatialCoordinateSystemPtr();
                if (spatialCoordinateSystem == null && spatialCoordinateSystemPtr != System.IntPtr.Zero)
                {
                    spatialCoordinateSystem = SpatialCoordinateSystem.FromNativePtr(WorldManager.GetNativeISpatialCoordinateSystemPtr());
                }
                return spatialCoordinateSystem;
            }
        }
#endif
    private static SpatialCoordinateSystem spatialCoordinateSystem = null;
#endif // UNITY_WSA

    void Start()
    {
        UnityEngine.WSA.Application.InvokeOnUIThread(() =>
        {
            spatialInteractionManager = SpatialInteractionManager.GetForCurrentView();
        }, true);
    }

    void Update()
    {
        PerceptionTimestamp perceptionTimestamp = PerceptionTimestampHelper.FromHistoricalTargetTime(DateTimeOffset.Now);
        IReadOnlyList<SpatialInteractionSourceState> sources = spatialInteractionManager?.GetDetectedSourcesAtTimestamp(perceptionTimestamp);
        
        foreach (SpatialInteractionSourceState sourceState in sources)
        {
            HandPose handPose = sourceState.TryGetHandPose();
            if (handPose != null && handPose.TryGetJoints(SpatialCoordinateSystem, jointIndices, jointPoses))
            {
                SpatialInteractionSourceHandedness handIndex = sourceState.Source.Handedness;
                if(handIndex == SpatialInteractionSourceHandedness.Left)
                {
                    ApplyTransforms(leftHandProxies, jointPoses);
                }
                else
                {
                    ApplyTransforms(rightHandProxies, jointPoses);
                }
            }
        }
    }

    private void ApplyTransforms(Transform[] handProxies, JointPose[] jointPoses)
    {
        for (int i = 0; i < jointPoses.Length; i++)
        {
            ApplyPose(handProxies[i], jointPoses[i]);
        }
    }

    private void ApplyPose(Transform transform, JointPose jointPose)
    {
        transform.position = SystemVector3ToUnity(jointPose.Position);
        transform.rotation = SystemQuaternionToUnity(jointPose.Orientation);
    }

    public static UnityEngine.Vector3 SystemVector3ToUnity(System.Numerics.Vector3 vector)
    {
        return new UnityEngine.Vector3(vector.X, vector.Y, -vector.Z);
    }

    public static UnityEngine.Quaternion SystemQuaternionToUnity(System.Numerics.Quaternion quaternion)
    {
        return new UnityEngine.Quaternion(-quaternion.X, -quaternion.Y, quaternion.Z, quaternion.W);
    }
    
    IEnumerable<Transform> CreateHand()
    {
        Transform hand = new GameObject("Hand").transform;
        foreach (HandJointKind item in jointIndices)
        {
            Transform joint = new GameObject(item.ToString()).transform;
            joint.parent = hand;
            yield return joint;
        }
    }
}
