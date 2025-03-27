using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [Header("Grounded Movement Variables")]
    [SerializeField] private float playerRotateSpeed; // how fast the character rotates as it turns
    [SerializeField] private float playerGroundMoveAcceleration; // how fast the player accelerates from the start of input
    [SerializeField] private float playerGroundMoveDecceleration; // how fast the player deccelerates after inputs end
    [SerializeField] private float PLAYER_MAX_SPEED; // the max speed the player can reach

    [Header("Grounded Movement Components")]
    private Vector2 playerGroundMoveVelocity; // the directional velocity that the player travels in

    [Header("Other Variables")]
    [SerializeField] private float playerMass; // the mass of the player
    private float gravity; // the gravity force value

    [Header("Other Components")]
    private Quaternion playerDirection; // the rotational value of the character model in respect to its forward facing vector
    [SerializeField] private Transform feetPosition; // the position where the floor checks raycasts  
    [SerializeField] private Rigidbody rb; // the kinematic rigidbody that holds all the physics components
    [SerializeField] private InputActionReference playerMovement; // the user input references from unity's new input system
    private RaycastHit groundCheck; // the raycast hit reference that checks for floor collisions
    private Vector2 moveDirection; // the movement vector that gets it's values based on user input
    [SerializeField] private GameObject camera;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        gravity = -9.8f * playerMass;
        feetPosition = transform.GetChild(0).GetComponent<Transform>();
        camera = transform.GetChild(2).gameObject;
    }

    void Start()
    {
        
    }

    void Update()
    {
        PlayerMovementRotation(moveDirection); // takes the moveDirection and updates the playerDirection

        moveDirection = playerMovement.action.ReadValue<Vector2>(); // moveDirections vector2 values are read from the playerMovement input map
        transform.rotation = Quaternion.Slerp(transform.rotation, playerDirection, playerRotateSpeed * Time.deltaTime);
        // ^ the characters rotations is a Quaternion slerp that starts at its current rotation and interpolates to a new rotation whenever the playerDirection is updated
    }

    private void FixedUpdate()
    {

        if (moveDirection.y != 0 || moveDirection.x != 0) // if either of the move inputs are pressed (vertical or horizontal)
        {
            gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, camera.transform.rotation, playerRotateSpeed * Time.deltaTime);

            playerGroundMoveVelocity.x += playerGroundMoveAcceleration * Time.deltaTime * moveDirection.x; 
            playerGroundMoveVelocity.y += playerGroundMoveAcceleration * Time.deltaTime * moveDirection.y;
            // ^ updates the velocity in respects to acceleration, time and direction for the y and x axis

            if (moveDirection.y != 0 && moveDirection.x != 0) // if both move inputs are pressed (vertical and horizontal)
            {
                playerGroundMoveVelocity.x = Mathf.Clamp(playerGroundMoveVelocity.x, -PLAYER_MAX_SPEED / Mathf.Sqrt(2), PLAYER_MAX_SPEED / Mathf.Sqrt(2));
                playerGroundMoveVelocity.y = Mathf.Clamp(playerGroundMoveVelocity.y, -PLAYER_MAX_SPEED / Mathf.Sqrt(2), PLAYER_MAX_SPEED / Mathf.Sqrt(2));
                // ^ normalizes the x and y velocities when moving to prevent diagnal boosting by dividing the max speed by sqrt(2)
            }
            else
            {
                playerGroundMoveVelocity.x = Mathf.Clamp(playerGroundMoveVelocity.x, -PLAYER_MAX_SPEED, PLAYER_MAX_SPEED);
                playerGroundMoveVelocity.y = Mathf.Clamp(playerGroundMoveVelocity.y, -PLAYER_MAX_SPEED, PLAYER_MAX_SPEED);
                // ^ sets the max speed to 15 or -15 depending on direction and prevents the velocity from exceeding
            }
        }
        if (moveDirection.x == 0)
        {
            playerGroundMoveVelocity.x = Mathf.MoveTowards(playerGroundMoveVelocity.x, 0, playerGroundMoveDecceleration * Time.deltaTime);
        }
        if (moveDirection.y == 0)
        {
            playerGroundMoveVelocity.y = Mathf.MoveTowards(playerGroundMoveVelocity.y, 0, playerGroundMoveDecceleration * Time.deltaTime);
        }


        rb.MovePosition(new Vector3(transform.position.x + playerGroundMoveVelocity.x,
                                    transform.position.y + (gravity * Time.fixedDeltaTime),
                                    transform.position.z + playerGroundMoveVelocity.y));

        if (IsGrounded())
        {
            gravity = 0;
        }
        else
        {
            gravity = -9.8f;
        }

        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward), Color.yellow);
    }

    private bool IsGrounded()
    {
        if (Physics.Raycast(feetPosition.position, transform.TransformDirection(Vector3.down), out groundCheck, Mathf.Infinity))
        {
            Debug.DrawRay(feetPosition.position, transform.TransformDirection(Vector3.down * groundCheck.distance), Color.green);
            return groundCheck.distance < .3f;
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
