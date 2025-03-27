using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject camera;
    [SerializeField] private CinemachineOrbitalFollow cof;
    private Vector3 currentMousePosition;
    private Vector3 oldMousePosition;
    private Vector3 mouseDirection;
    [SerializeField] private float cameraSensitivityY;
    [SerializeField] private float cameraSensitivityX;

    void Awake()
    {
        camera = GameObject.Find("PlayerFollowCamera");
        cof = camera.GetComponent<CinemachineOrbitalFollow>();
    }

    // Update is called once per frame
    void Update()
    {
        currentMousePosition = Input.mousePosition;

        if (Input.GetMouseButton(1)) 
        {
            cof.VerticalAxis.Value += 
        }

        if (Input.GetMouseButtonUp(1))
        {
            currentMousePosition = Vector3.zero;
        }

    }
}
