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
        currentMousePosition.x = Input.mousePosition.x;
        currentMousePosition.y = Input.mousePosition.y;

        if (Input.GetMouseButton(1)) 
        {
            if (currentMousePosition.x > oldMousePosition.x)
            {
                cof.HorizontalAxis.Value += cameraSensitivityX;
            }

            if (currentMousePosition.x < oldMousePosition.x)
            {
                cof.HorizontalAxis.Value -= cameraSensitivityX;
            }

            if (currentMousePosition.y > oldMousePosition.y)
            {
                cof.VerticalAxis.Value -= cameraSensitivityY;
            }

            if (currentMousePosition.y < oldMousePosition.y)
            {
                cof.VerticalAxis.Value += cameraSensitivityY;
            }
        }

        oldMousePosition = currentMousePosition;
    }


}
