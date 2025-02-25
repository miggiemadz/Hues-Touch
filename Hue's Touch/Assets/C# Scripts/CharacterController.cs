using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private float playerMass;
    [SerializeField] private float playerGroundMoveAcceleration;
    [SerializeField] private float playerGroundMoveDecceleration;
    [SerializeField] private float PLAYER_MAX_SPEED;
    private Vector2 playerGroundMoveSpeed;
    private Vector2 playerGroundMoveVelocity;
    private bool isMoving;

    [SerializeField] private Transform feetPosition;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private PlayerInput playerControls;
    [SerializeField] private InputActionReference playerMovement;

    private float gravity;
    private RaycastHit groundCheck;
    private Vector2 moveDirection;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        gravity = -9.8f * playerMass;
        feetPosition = transform.GetChild(0).GetComponent<Transform>();
        playerControls = gameObject.GetComponent<PlayerInput>();
        playerGroundMoveSpeed = playerGroundMoveVelocity;
    }

    void Start()
    {
        
    }

    void Update()
    {
        moveDirection = playerMovement.action.ReadValue<Vector2>();
        isMoving = moveDirection != Vector2.zero;
    }

    private void FixedUpdate()
    {
        if (moveDirection.y != 0 || moveDirection.x != 0)
        {
            playerGroundMoveSpeed.x += playerGroundMoveAcceleration * Time.deltaTime * moveDirection.x;
            playerGroundMoveSpeed.y += playerGroundMoveAcceleration * Time.deltaTime * moveDirection.y;

            if (moveDirection.y != 0 && moveDirection.x != 0)
            {
                playerGroundMoveSpeed.x = Mathf.Clamp(playerGroundMoveSpeed.x, -PLAYER_MAX_SPEED / Mathf.Sqrt(2), PLAYER_MAX_SPEED / Mathf.Sqrt(2));
                playerGroundMoveSpeed.y = Mathf.Clamp(playerGroundMoveSpeed.y, -PLAYER_MAX_SPEED / Mathf.Sqrt(2), PLAYER_MAX_SPEED / Mathf.Sqrt(2));
            }
            else
            {
                playerGroundMoveSpeed.x = Mathf.Clamp(playerGroundMoveSpeed.x, -PLAYER_MAX_SPEED, PLAYER_MAX_SPEED);
                playerGroundMoveSpeed.y = Mathf.Clamp(playerGroundMoveSpeed.y, -PLAYER_MAX_SPEED, PLAYER_MAX_SPEED);
            }
        }
        if (moveDirection.x == 0)
        {
            playerGroundMoveSpeed.x = Mathf.MoveTowards(playerGroundMoveSpeed.x, 0, playerGroundMoveDecceleration * Time.deltaTime);
        }
        if (moveDirection.y == 0)
        {
            playerGroundMoveSpeed.y = Mathf.MoveTowards(playerGroundMoveSpeed.y, 0, playerGroundMoveDecceleration * Time.deltaTime);
        }

        if (playerGroundMoveSpeed != Vector2.zero)
        {
            rb.MovePosition(new Vector3(transform.position.x + playerGroundMoveSpeed.x,
                                        transform.position.y,
                                        transform.position.z + playerGroundMoveSpeed.y));
        }

        if (!isGrounded())
        {
            rb.MovePosition(new Vector3(transform.position.x, transform.position.y + gravity * Time.deltaTime, transform.position.z));
        }

        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward), Color.yellow);
        Debug.Log(groundCheck.distance);
    }

    private bool isGrounded()
    {
        if (Physics.Raycast(feetPosition.position, transform.TransformDirection(Vector3.down), out groundCheck, Mathf.Infinity))
        {
            Debug.DrawRay(feetPosition.position, transform.TransformDirection(Vector3.down * groundCheck.distance), Color.green);
            return groundCheck.distance < .2f;
        }
        return false;
    }
}
