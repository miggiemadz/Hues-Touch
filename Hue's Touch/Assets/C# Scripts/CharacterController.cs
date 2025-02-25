using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private float playerMass;
    [SerializeField] private float playerRotateSpeed;
    [SerializeField] private float playerGroundMoveAcceleration;
    [SerializeField] private float playerGroundMoveDecceleration;
    [SerializeField] private float PLAYER_MAX_SPEED;
    private Vector2 playerGroundMoveSpeed;
    private Vector2 playerGroundMoveVelocity;
    private Quaternion playerDirection;

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
        transform.rotation = Quaternion.Slerp(transform.rotation, playerDirection, playerRotateSpeed * Time.deltaTime);

        PlayerMovementRotation(moveDirection);

        Debug.Log(playerDirection);

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

        if (!IsGrounded())
        {
            rb.MovePosition(new Vector3(transform.position.x, transform.position.y + gravity * Time.deltaTime, transform.position.z));
        }

        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward), Color.yellow);
    }

    private bool IsGrounded()
    {
        if (Physics.Raycast(feetPosition.position, transform.TransformDirection(Vector3.down), out groundCheck, Mathf.Infinity))
        {
            Debug.DrawRay(feetPosition.position, transform.TransformDirection(Vector3.down * groundCheck.distance), Color.green);
            return groundCheck.distance < .2f;
        }
        return false;
    }

    private void PlayerMovementRotation(Vector2 movementInput)
    {
        var directionMap = new Dictionary<(float, float), float>
        {
            { (0, 1), 0 },   // North
            { (.707107f, .707107f), 45 },   // North East
            { (1, 0), 90 },   // East
            { (.707107f, -.707107f), 135 },  // South East
            { (0, -1), 180 },  // South
            { (-.707107f, -.707107f), 225 }, // South West
            { (-1, 0), 270 },  // West
            { (-.707107f, .707107f), 315 }   // North West
        };
        float direction;

        if (directionMap.TryGetValue((movementInput.x, movementInput.y),out direction))
        {
            playerDirection = Quaternion.Euler(0, direction, 0);
        }
        
    }
}
