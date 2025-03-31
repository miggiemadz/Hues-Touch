using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

    [Header("Aerial Movement Variables")]
    [SerializeField] private float playerJumpSpeed;
    private float playerJumpVelocity;
    [SerializeField] private float playerJumpAcceleration;
    [SerializeField] private float playerJumpDecceleration;
    [SerializeField] private float MAX_JUMP_HEIGHT;

    [Header("Aerial Movement Components")]
    [SerializeField] private InputActionReference playerJump;

    [Header("Other Variables")]
    [SerializeField] private float playerMass; // the mass of the player
    private float gravity; // the gravity force value
    [SerializeField] private float rotationSpeed;

    [Header("Other Components")]
    [SerializeField] private Transform feetPosition; // the position where the floor checks raycasts  
    [SerializeField] private Rigidbody rb; // the kinematic rigidbody that holds all the physics components
    [SerializeField] private InputActionReference playerMovement; // the user input references from unity's new input system
    private RaycastHit groundCheck; // the raycast hit reference that checks for floor collisions
    private Vector3 moveDirection; // the movement vector that gets it's values based on user input
    [SerializeField] private Transform camera;

    [Header("Combat Settings")] // --steven
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform shootPoint; // where the projectile spawns from

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        gravity = -9.8f * playerMass;
        feetPosition = transform.GetChild(0).GetComponent<Transform>();
        camera = transform.GetChild(2).gameObject.transform;
    }

    void Start()
    {
        
    }

    void Update()
    {
        Vector3 inputDirection = new Vector3(playerMovement.action.ReadValue<Vector2>().x, 0, playerMovement.action.ReadValue<Vector2>().y).normalized;

        Vector3 cameraFoward = camera.transform.forward;
        cameraFoward.y = 0;
        cameraFoward.Normalize();

        Vector3 cameraRight = camera.transform.right;
        cameraRight.y = 0;
        cameraRight.Normalize();
        moveDirection = cameraFoward * inputDirection.z + cameraRight * inputDirection.x; // moveDirections vector2 values are read from the playerMovement input map

        if (IsGrounded())
        {
            gravity = 0;
        }
        else
        {
            gravity = -9.8f;
        }

        moveDirection = playerMovement.action.ReadValue<Vector2>(); // moveDirections vector2 values are read from the playerMovement input map
        transform.rotation = Quaternion.Slerp(transform.rotation, playerDirection, playerRotateSpeed * Time.deltaTime);
        // ^ the characters rotations is a Quaternion slerp that starts at its current rotation and interpolates to a new rotation whenever the playerDirection is updated
        if (Keyboard.current.eKey.wasPressedThisFrame) { // sorry to put this here but i might as well -- steven
        ShootProjectile();}
    }

    private void FixedUpdate()
    {
        Debug.Log(gravity + ", " + groundCheck.distance);

        if (playerJump.action.ReadValue<float>() > 0) 
        {
            playerJumpVelocity = Mathf.Clamp(playerJumpSpeed * playerJumpAcceleration, 0, MAX_JUMP_HEIGHT);
            Debug.Log(playerJumpSpeed);
        }
        if (playerJump.action.ReadValue<float>() == 0 && !IsGrounded())
        {
            playerJumpVelocity = Mathf.Clamp(playerJumpSpeed * playerJumpDecceleration, 0, MAX_JUMP_HEIGHT);
        }

        if (moveDirection.z != 0 || moveDirection.x != 0) // if either of the move inputs are pressed (vertical or horizontal)
        {
            Quaternion playerRotation = Quaternion.LookRotation(moveDirection);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, playerRotation, rotationSpeed * Time.deltaTime));

            playerGroundMoveVelocity.x += playerGroundMoveAcceleration * Time.deltaTime * moveDirection.x;
            playerGroundMoveVelocity.y += playerGroundMoveAcceleration * Time.deltaTime * moveDirection.z;
            // ^ updates the velocity in respects to acceleration, time and direction for the y and x axis

            if (moveDirection.z != 0 && moveDirection.x != 0) // if both move inputs are pressed (vertical and horizontal)
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
        if (moveDirection.z == 0)
        {
            playerGroundMoveVelocity.y = Mathf.MoveTowards(playerGroundMoveVelocity.y, 0, playerGroundMoveDecceleration * Time.deltaTime);
        }

        rb.MovePosition(new Vector3(transform.position.x + playerGroundMoveVelocity.x,
                                    transform.position.y + playerJumpVelocity + gravity * Time.deltaTime,
                                    transform.position.z + playerGroundMoveVelocity.y));

        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward), Color.yellow);
    }

    private void OnDrawGizmos()
    {
        Vector3 start = feetPosition.position + Vector3.up * .5f;
        Vector3 end = feetPosition.position + Vector3.up * 1.5f;
        float radius = .5f;
        Vector3 direction = transform.TransformDirection(Vector3.down);
        float maxDistance = 10f;

        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(start, radius);
        Gizmos.DrawWireSphere(end, radius);

        Gizmos.DrawLine(start, start + direction * maxDistance);
        Gizmos.DrawLine(end, end + direction * maxDistance);

        Vector3 finalStart = start + direction * maxDistance;
        Vector3 finalEnd = end + direction * maxDistance;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(finalStart, radius);
        Gizmos.DrawWireSphere(finalEnd, radius);
    }

    private bool IsGrounded()
    {
        Vector3 start = feetPosition.position + Vector3.up * .5f;
        Vector3 end = feetPosition.position + Vector3.up * 1.5f;
        float radius = .5f;
        Vector3 direction = transform.TransformDirection(Vector3.down);
        float maxDistance = .2f;
        if (Physics.CapsuleCast(start, end, radius, direction, out groundCheck, maxDistance))
        {
            return groundCheck.distance < maxDistance;
        }
        return false;
    }
 
    private void ShootProjectile() { // -- steven
    if (projectilePrefab == null) {
        Debug.LogError("No projectile prefab assigned!");
        return;
    }

    // Determine spawn position and direction
    Vector3 spawnPos = transform.position + transform.forward; // or use shootPoint.position if you have one
    Vector3 shootDirection = transform.forward;

    // Instantiate and launch
    GameObject proj = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
    Projectile projScript = proj.GetComponent<Projectile>();

    if (projScript != null) {
        projScript.SetDirection(shootDirection);
    } else {
        Debug.LogError("Projectile script is missing on the instantiated projectile!");}
    }    
}


