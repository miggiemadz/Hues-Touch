using UnityEngine;
using UnityEngine.InputSystem;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private float playerMass;

    [SerializeField] private Transform feetPosition;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private PlayerInput playerControls;

    private Vector3 gravityVector;
    private RaycastHit groundCheck;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        gravityVector = new Vector3(0, -9.8f * playerMass, 0);
        feetPosition = transform.GetChild(0).GetComponent<Transform>();
        playerControls = gameObject.GetComponent<PlayerInput>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (!isGrounded())
        {
            rb.MovePosition(transform.position + gravityVector * Time.fixedDeltaTime);
        }

        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward), Color.yellow);
    }

    private bool isGrounded()
    {
        Physics.Raycast(feetPosition.position, transform.TransformDirection(Vector3.down), out groundCheck, Mathf.Infinity);
        Debug.DrawRay(feetPosition.position, transform.TransformDirection(Vector3.down * groundCheck.distance), Color.green);
        return groundCheck.distance < .2f;
    }
}
