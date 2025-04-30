using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Camera Components")]
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private CinemachineOrbitalFollow cof;
    [SerializeField] private InputActionReference cameraInput;

    [Header("Camera Values")]
    [SerializeField] private float cameraSensitivityY;
    [SerializeField] private float cameraSensitivityX;
    private Vector2 cameraMovement;
    [SerializeField] private float cameraDeadZoneX;
    [SerializeField] private float cameraDeadZoneY;

    void Awake()
    {
        playerCamera = GameObject.Find("PlayerFollowCamera");
        cof = playerCamera.GetComponent<CinemachineOrbitalFollow>();

        playerCamera.GetComponent<CinemachineCamera>().Target.TrackingTarget = GameObject.Find("CameraFollow").transform;
    }

    // Update is called once per frame
    void Update()
    {
        cameraMovement = cameraInput.action.ReadValue<Vector2>();

        if (Mathf.Abs(cameraMovement.x) > cameraDeadZoneX)
        {
            cof.HorizontalAxis.Value = cof.HorizontalAxis.Value + Mathf.Sign(cameraMovement.x) * cameraSensitivityX;
        }

        if (Mathf.Abs(cameraMovement.y) > cameraDeadZoneY)
        {
            cof.VerticalAxis.Value = Mathf.Clamp(cof.VerticalAxis.Value + Mathf.Sign(cameraMovement.y) * cameraSensitivityY, -10, 45);
        }

    }

}
