using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joint : MonoBehaviour
{
    [Header("Common")]
    public Transform pointToHang;
    public float force;
    public ForceMode2D forceType;
    public Vector3 direction = Vector3.right;
    public float forceAngle = 60;

    public enum JointType
    {
        HINGE_JOINT,
        SPRING_JOINT,
        WHEEL_JOINT,
        FIXED_JOINT
    }

    public JointType jointType = JointType.HINGE_JOINT;
    public Vector3 playerStartPositionOffset;

    [Header("SpringJoint")]
    public float springDistance;
    public float springFrequency;
    public float springDampingRatio;

}
