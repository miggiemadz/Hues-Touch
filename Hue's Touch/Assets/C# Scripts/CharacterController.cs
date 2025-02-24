using UnityEngine;
using UnityEngine.InputSystem;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private float playerMass;
    [SerializeField] private float playerGroundMoveSpeed;

    [SerializeField] private Transform feetPosition;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private PlayerInput playerControls;
    [SerializeField] private InputActionReference playerMovement;

    private Vector3 gravityVector;
    private RaycastHit groundCheck;
    private Vector2 moveDirection;

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
        moveDirection = playerMovement.action.ReadValue<Vector2>();
        Debug.Log(moveDirection);
    }

    private void FixedUpdate()
    {

        rb.MovePosition(new Vector3(transform.position.x + (moveDirection.x * playerGroundMoveSpeed) * Time.fixedDeltaTime,
                                    transform.position.y,
                                    transform.position.z + (moveDirection.y * playerGroundMoveSpeed) * Time.fixedDeltaTime));

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
