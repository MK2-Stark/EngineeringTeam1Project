using Meta.XR.ImmersiveDebugger.UserInterface.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MovementManager : MonoBehaviour
{
    [SerializeField] private MovementController controller;

    //joint selection bools
    private bool isBaseJoint = true;//base joint will be default selected joint
    public bool BaseBoolField{
        get{ return isBaseJoint;}
    }

    private bool isShoulderJoint = false;
    public bool ShoulderBoolField
    {
        get { return isShoulderJoint; }
    }

    private bool isElbowJoint = false;
    public bool ElbowBoolField
    {
        get { return isElbowJoint; }
    }


    private bool isWristVertJoint = false;
    public bool WristBoolField
    {
        get { return isWristVertJoint; }
    }

    //Buttons GameObjects
    [SerializeField] private GameObject baseButton;
    [SerializeField] private GameObject shoulderButton;
    [SerializeField] private GameObject elbowButton;
    [SerializeField] private GameObject wristVertButton;

    [SerializeField] private GameObject minYButton;
    [SerializeField] private GameObject posYButton;

    [SerializeField] private GameObject minZButton;
    [SerializeField] private GameObject posZButton;

    [SerializeField] private GameObject sendButton;

    //GameObjects
    [SerializeField] private GameObject baseJoint;
    [SerializeField] private MeshRenderer baseRend;

    [SerializeField] private GameObject shoulderJoint;
    [SerializeField] private MeshRenderer ShoulderRend;
    private Vector3 shoulderStartingV3 = new Vector3(0f, 0f, 0f);

    [SerializeField] private GameObject elbowJoint;
    [SerializeField] private MeshRenderer elbowRend;
    private Vector3 elbowStartingV3 = new Vector3(0.606f, 0.12f, -95f);

    [SerializeField] private GameObject wristVertJoint;
    [SerializeField] private MeshRenderer wristVertRend;
    private Vector3 wristVertStartingV3 = new Vector3(0f, 0f, 90f);

    //Materials
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material selectedMaterial;

    private void Awake()
    {
        //base joint is selected by default on Awake
        baseJointSelected();
        ActiveStartingPosition();
    }
    public void ActiveStartingPosition()
    {
        elbowJoint.transform.localEulerAngles = elbowStartingV3;
        wristVertJoint.transform.localEulerAngles = wristVertStartingV3;
        shoulderJoint.transform.localEulerAngles = shoulderStartingV3;
    }

    public void DeactiveStartingPosition()
    {

    }

    //Base Joint does local pivot rotate on Y axis.
    public void minusYPressed()
    {
        if (isBaseJoint == true)
        {
            controller.negativeYRotation(baseJoint);
        }
    }

    //Base Joint does local pivot rotate on Y axis only.
    public void positveYPressed()
    {
        if (isBaseJoint == true)
        {
            controller.positiveYRotation(baseJoint);
        }
    }

    //shoulder, elbow, and wristvert do local pivot rotate on Z axis only
    public void minusZPressed()
    {

        if (isShoulderJoint == true)
        {
            controller.negativeZRotation(shoulderJoint);
        }
        else if (isElbowJoint == true)
        {
            controller.negativeZRotation(elbowJoint);
        }
        else if (isWristVertJoint == true)
        {
            controller.negativeZRotation(wristVertJoint);
        }
    }

    public void positveZPressed()
    {
        if (isShoulderJoint == true)
        {
            controller.positiveZRotation(shoulderJoint);
        }
        else if (isElbowJoint == true)
        {
            controller.positiveZRotation(elbowJoint);
        }
        else if (isWristVertJoint == true)
        {
            controller.positiveZRotation(wristVertJoint);
        }
    }

    //JOINT SELECTION FUNCTIONS
    public void baseJointSelected()
    {
        isBaseJoint = true;
        isShoulderJoint = false;
        isElbowJoint = false;
        isWristVertJoint = false;

        //highlight the seelcted joint on the robot arm. swapping materials to green and keep the unselected with default orange
        baseRend.material = selectedMaterial;
        ShoulderRend.material = defaultMaterial;
        elbowRend.material = defaultMaterial;
        wristVertRend.material = defaultMaterial;

        //Disable Unusable movement keys and enable usable keys
        //enable - + Y keys
        minYButton.SetActive(true);
        posYButton.SetActive(true);
        //disable - + Z keys
        minZButton.SetActive(false);
        posZButton.SetActive(false);
    }

    public void shoulderJointSelected()
    {
        isBaseJoint = false;
        isShoulderJoint = true;
        isElbowJoint = false;
        isWristVertJoint = false;

        //highlight the seelcted joint on the robot arm. swapping materials to green and keep the unselected with default orange
        baseRend.material = defaultMaterial;
        ShoulderRend.material = selectedMaterial;
        elbowRend.material = defaultMaterial;
        wristVertRend.material = defaultMaterial;

        //Disable Unusable movement keys and enable usable keys
        //disable - + Y keys
        minYButton.SetActive(false);
        posYButton.SetActive(false);
        //enable - + Z keys
        minZButton.SetActive(true);
        posZButton.SetActive(true);

    }

    public void elbowJointSelected()
    {
        isBaseJoint = false;
        isShoulderJoint = false;
        isElbowJoint = true;
        isWristVertJoint = false;

        //highlight the seelcted joint on the robot arm. swapping materials to green and keep the unselected with default orange
        baseRend.material = defaultMaterial;
        ShoulderRend.material = defaultMaterial;
        elbowRend.material = selectedMaterial;
        wristVertRend.material = defaultMaterial;

        //Disable Unusable movement keys and enable usable keys
        //disable - + Y keys
        minYButton.SetActive(false);
        posYButton.SetActive(false);
        //enable - + Z keys
        minZButton.SetActive(true);
        posZButton.SetActive(true);
    }

    public void wristVertJointSelected()
    {
        isBaseJoint = false;
        isShoulderJoint = false;
        isElbowJoint = false;
        isWristVertJoint = true;


        //highlight the seelcted joint on the robot arm. swapping materials to green and keep the unselected with default orange
        baseRend.material = defaultMaterial;
        ShoulderRend.material = defaultMaterial;
        elbowRend.material = defaultMaterial;
        wristVertRend.material = selectedMaterial;

        //Disable Unusable movement keys and enable usable keys
        //disable - + Y keys
        minYButton.SetActive(false);
        posYButton.SetActive(false);
        //enable - + Z keys
        minZButton.SetActive(true);
        posZButton.SetActive(true);
    }
}
