using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private MovementManager movementManager;
    //joints
    private Vector3 selectedJoint;

    //Defined angular limitations of each joint
    //private Vector3 minBaseLimit;
    //private Vector3 maxBaseLimit;

    //private Vector3 minShoulderLimit;
    //private Vector3 maxShoulderLimit;

    //private Vector3 minElbowLimit;
    //private Vector3 maxElbowLimit;

    //private Vector3 minWristLimit;
    //private Vector3 maxWristLimit;

    //Min Max values for rotation limits of joints
    private bool isMinBase = false;
    private bool isMaxBase = false;

    private bool isMinShoulder = false;
    private bool isMaxShoulder = false;

    private bool isMinElbow = false;
    private bool isMaxElbow = false;

    private bool isMinWristVert = false;
    private bool isMaxWristVert = false;


    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
       // GetBeginningAngularStateOfEachJoint();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //private void GetBeginningAngularStateOfEachJoint()
    //{

    //}

    public void positiveYRotation(GameObject jointToMove)
    {
        //Joint angular local pivot rotation on +Y axis.
        
        selectedJoint = jointToMove.transform.localEulerAngles;
        selectedJoint.y += 1;
        
        if (movementManager.BaseBoolField == true)
        {
            Debug.Log("--Positive Y Rotation Occured");
            if (selectedJoint.y <= 180)
                jointToMove.transform.localEulerAngles = selectedJoint;
        }
    }


    public void negativeYRotation(GameObject jointToMove)
    {
        //Joint angular local pivot rotation on -Y axis.
        selectedJoint = jointToMove.transform.localEulerAngles;
        selectedJoint.y -= 1;

        if (movementManager.BaseBoolField == true)
        {
            Debug.Log("--Negative Y Rotation Occured");
            if (selectedJoint.y >= 0)
                jointToMove.transform.localEulerAngles = selectedJoint;
        }
    }

    public void positiveZRotation(GameObject jointToMove)
    {
        //Joint angular local pivot rotation on +Z axis.
        selectedJoint = jointToMove.transform.localEulerAngles;
        selectedJoint.z += 1;
        if (movementManager.ShoulderBoolField == true)
        {
            if (selectedJoint.z <= 75)
                jointToMove.transform.localEulerAngles = selectedJoint;
        }
        if (movementManager.ElbowBoolField == true)
        {
            if (selectedJoint.z >= 47)
                jointToMove.transform.localEulerAngles = selectedJoint;
        }

        if (movementManager.WristBoolField == true)
        {
            if (selectedJoint.z >= 90)
                jointToMove.transform.localEulerAngles = selectedJoint;
        }

    }

    public void negativeZRotation(GameObject jointToMove)
    {
        //Joint angular local pivot rotation on -Z axis.
        selectedJoint = jointToMove.transform.localEulerAngles;
        selectedJoint.z -= 1;
        if (movementManager.ShoulderBoolField == true)
        {
            if (selectedJoint.z >= -75)
                jointToMove.transform.localEulerAngles = selectedJoint;
        }
        if (movementManager.ElbowBoolField == true)
        {
            if (selectedJoint.z >= -133)
                jointToMove.transform.localEulerAngles = selectedJoint;
        }

        if (movementManager.WristBoolField == true)
        {
            if (selectedJoint.z >= -90)
                jointToMove.transform.localEulerAngles = selectedJoint;
        }
    }

    //private bool minMaxBaseJointLimits()
    //{
    //    return false;
    //}

    //private bool minMaxShoulderJointLimits()
    //{
    //    return false;
    //}

    //private bool minMaxElbowJointLimits()
    //{
    //    return false;
    //}

    //private bool minMaxWristVertJointLimits()
    //{
    //    return false;
    //}
}
