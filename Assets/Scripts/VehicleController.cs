




using UnityEngine;
using UnityEngine.InputSystem;

public class VehicleController : MonoBehaviour, IInteractable
{
    [Header("Vehicle Settings")]
    [SerializeField] private float motorForce, brakeForce, maxSteerAngle;

    // Wheel Colliders
    [SerializeField] private WheelCollider FLWheelCollider, FRWheelCollider;
    [SerializeField] private WheelCollider RLWheelCollider, RRWheelCollider;

    // Wheel Transforms
    [SerializeField] private Transform FLWheelTransform, FRWheelTransform;
    [SerializeField] private Transform RLWheelTransform, RRWheelTransform;

    // Input System
    private PlayerInput playerInput;
    private InputAction steerAction, driveAction, brakeAction;

    private GameObject driver;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        steerAction = playerInput.actions["Steer"];
        driveAction = playerInput.actions["Drive"];
        brakeAction = playerInput.actions["Brake"];
    }

    private void FixedUpdate()
    {
        if (driver != null)
        {
            HandleMotor();
            HandleSteering();
            UpdateWheels();
        }
    }

    private void HandleMotor()
    {
        float driveInput = driveAction.ReadValue<float>();
        float brakeInput = brakeAction.ReadValue<float>();

        FLWheelCollider.motorTorque = driveInput * motorForce;
        FRWheelCollider.motorTorque = driveInput * motorForce;

        float currentBrakeForce = brakeInput * brakeForce;
        ApplyBraking(currentBrakeForce);
    }

    private void ApplyBraking(float brakeForce)
    {
        FLWheelCollider.brakeTorque = brakeForce;
        FRWheelCollider.brakeTorque = brakeForce;
        RLWheelCollider.brakeTorque = brakeForce;
        RRWheelCollider.brakeTorque = brakeForce;
    }

    private void HandleSteering()
    {
        float steerInput = steerAction.ReadValue<float>();
        float steerAngle = steerInput * maxSteerAngle;

        FLWheelCollider.steerAngle = steerAngle;
        FRWheelCollider.steerAngle = steerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(FLWheelCollider, FLWheelTransform);
        UpdateSingleWheel(FRWheelCollider, FRWheelTransform);
        UpdateSingleWheel(RLWheelCollider, RLWheelTransform);
        UpdateSingleWheel(RRWheelCollider, RRWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 position;
        Quaternion rotation;
        wheelCollider.GetWorldPose(out position, out rotation);
        wheelTransform.position = position;
        wheelTransform.rotation = rotation;
    }

    public void Interact()
    {
        if (driver == null)
        {
            // Enter the vehicle
            EnterVehicle();
        }
        else
        {
            // Exit the vehicle
            ExitVehicle();
        }
    }

    private void EnterVehicle()
    {
        driver = FindObjectOfType<PlayerMovement>().gameObject;
        driver.SetActive(false);
        playerInput.enabled = true;
    }

    private void ExitVehicle()
    {
        driver.SetActive(true);
        driver.transform.position = transform.position + transform.right * 2f;
        driver.GetComponent<PlayerMovement>().EnableMovement(true);
        driver = null;
        playerInput.enabled = false;
    }
}
