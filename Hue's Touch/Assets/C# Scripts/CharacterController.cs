using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Cinemachine;
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
    private float initialJumpPosition;
    private float gravity; // the gravity force value
    private bool canJump;
    private bool isFalling;


    [Header("Aerial Movement Components")]
    [SerializeField] private InputActionReference playerJump;

    [Header("Other Variables")]
    [SerializeField] private float playerMass; // the mass of the player
    [SerializeField] private float rotationSpeed;
    private Vector3 inputDirection;
    private Vector3 moveDirection; // the movement vector that gets it's values based on user input

    [Header("Other Components")]
    [SerializeField] private Transform feetPosition; // the position where the floor checks raycasts  
    [SerializeField] private Rigidbody rb; // the kinematic rigidbody that holds all the physics components
    [SerializeField] private InputActionReference playerMovement; // the user input references from unity's new input system

    [Header("Camera Components")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform cameraReferenceRoot;
    [SerializeField] private CinemachineCamera playerCamera;

    [Header("Collision Components")]
    [SerializeField] private CapsuleCollider characterCollider;
    private float distanceToCollider;
    private RaycastHit groundCheck; // the raycast hit reference that checks for floor collisions
    private RaycastHit collisionCheck;
    private RaycastHit wallCheck;

    [Header("Combat Settings")] // --steven
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform shootPoint; // where the projectile spawns from

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        gravity = -9.8f * playerMass;
        feetPosition = transform.GetChild(0).GetComponent<Transform>();
        cameraReferenceRoot = transform.GetChild(3).gameObject.transform;
    }
    void Start()
    {
        
    }

    void Update()
    {
        inputDirection = new Vector3(playerMovement.action.ReadValue<Vector2>().x, 0, playerMovement.action.ReadValue<Vector2>().y).normalized;
        
        distanceToCollider = Mathf.Clamp(CollisionHandler(), 0, 2);

        Vector3 cameraDiff = playerCamera.Follow.position - cameraTransform.position;
        cameraDiff.y = 0;
        cameraDiff.Normalize();
        cameraReferenceRoot.forward = cameraDiff;

        Vector3 flatForward = cameraReferenceRoot.forward;
        Vector3 flatRight = cameraReferenceRoot.right;

        moveDirection = flatForward * inputDirection.z + flatRight * inputDirection.x; // moveDirections vector2 values are read from the playerMovement input map

        if (!IsFloorClose() && !isGrounded())
        {
            
            gravity = -9.8f;
        }
        if (!isGrounded() && IsFloorClose())
        {
            initialJumpPosition = gameObject.transform.position.y;
            gravity = 0f;
            playerJumpVelocity = 0f;
            canJump = true;
            isFalling = false;
        }
        if (!IsFloorClose() && isGrounded()) 
        {
            gravity = 9.8f;
            playerJumpVelocity = 0f;
        }

        if (Keyboard.current.eKey.wasPressedThisFrame) { // sorry to put this here but i might as well -- steven
        ShootProjectile();}
    }

    private void FixedUpdate()
    {
        if (gameObject.transform.position.y - initialJumpPosition > MAX_JUMP_HEIGHT)
        {
            canJump = false;
            isFalling = true;
        }
        if (playerJump.action.ReadValue<float>() == 0 && playerJumpVelocity > 0)
        {
            isFalling = true;
        }
        if (playerJump.action.ReadValue<float>() > 0 && canJump)
        {
            playerJumpVelocity += playerJumpSpeed * playerJumpAcceleration;
        }
        if (!IsFloorClose() && playerJumpVelocity > 0 && isFalling)
        {
            playerJumpVelocity -= playerJumpSpeed * playerJumpDecceleration;
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
        if (moveDirection.x < .1f && moveDirection.x > -.1f)
        {
            playerGroundMoveVelocity.x = Mathf.MoveTowards(playerGroundMoveVelocity.x, 0, playerGroundMoveDecceleration * Time.deltaTime);
        }
        if (moveDirection.z < .1f && moveDirection.z > -.1f)
        {
            playerGroundMoveVelocity.y = Mathf.MoveTowards(playerGroundMoveVelocity.y, 0, playerGroundMoveDecceleration * Time.deltaTime);
        }

        if (distanceToCollider < .2f)
        {
            playerGroundMoveVelocity = Vector2.zero;
        }

        rb.MovePosition(new Vector3(transform.position.x + playerGroundMoveVelocity.x,
                                    transform.position.y + playerJumpVelocity + gravity * Time.deltaTime,
                                    transform.position.z + playerGroundMoveVelocity.y));

        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward), Color.yellow);

    }

    private void OnDrawGizmos()
    {
        Vector3 collisionStart = feetPosition.position + Vector3.up * .5f;
        Vector3 collisionEnd = feetPosition.position + Vector3.up * 1.5f;
        float collisionRadius = .5f;
        Vector3 collisionDirection = inputDirection;
        float collisionMaxDistance = .3f;

        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(collisionStart, collisionRadius);
        Gizmos.DrawWireSphere(collisionEnd, collisionRadius);

        Gizmos.DrawLine(collisionStart, collisionStart + collisionDirection * collisionMaxDistance);
        Gizmos.DrawLine(collisionEnd, collisionEnd + collisionDirection * collisionMaxDistance);

        Vector3 collisionFinalStart = collisionStart + collisionDirection * collisionMaxDistance;
        Vector3 collisionFinalEnd = collisionEnd + collisionDirection * collisionMaxDistance;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(collisionFinalStart, collisionRadius);
        Gizmos.DrawWireSphere(collisionFinalEnd, collisionRadius);

        Vector3 start = feetPosition.position + Vector3.up * .5f;
        Vector3 end = feetPosition.position + Vector3.up * 1.5f;
        float radius = .5f;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(start, radius);
        Gizmos.DrawWireSphere(end, radius);
        Gizmos.DrawLine(start + Vector3.forward * radius, end + Vector3.forward * radius);
        Gizmos.DrawLine(start - Vector3.forward * radius, end - Vector3.forward * radius);
        Gizmos.DrawLine(start + Vector3.right * radius, end + Vector3.right * radius);
        Gizmos.DrawLine(start - Vector3.right * radius, end - Vector3.right * radius);
    }

    private bool IsFloorClose()
    {
        Vector3 start = feetPosition.position + Vector3.up * .5f;
        Vector3 end = feetPosition.position + Vector3.up * 1.5f;
        float radius = .5f;
        Vector3 direction = transform.TransformDirection(Vector3.down);
        float maxDistance = 2f;

        int excludedLayers = LayerMask.GetMask("Player", "Walls");
        int groundMask = ~excludedLayers;

        if (Physics.CapsuleCast(start, end, radius, direction, out groundCheck, maxDistance, groundMask))
        {
            return Mathf.Clamp(groundCheck.distance,0,1) < .2f;
        }

        return false;
    }

    private bool isGrounded()
    {
        Vector3 start = feetPosition.position + Vector3.up * .5f;
        Vector3 end = feetPosition.position + Vector3.up * 1.5f;
        float radius = .5f;

        int excludedLayers = LayerMask.GetMask("Player", "Walls");
        int groundMask = ~excludedLayers;

        return Physics.CheckCapsule(start, end, radius, groundMask);
    }

    private bool WallChecker()
    {
        Vector3 start = feetPosition.position + Vector3.up * .5f;
        Vector3 end = feetPosition.position + Vector3.up * 1.5f;
        float radius = .5f;
        Vector3 direction = moveDirection;
        float maxDistance = .5f;

        if (Physics.CapsuleCast(start, end, radius, direction + Vector3.right, out wallCheck, maxDistance, 3))
        {
            return Mathf.Clamp(wallCheck.distance, 0, 1) < .1f;
        }
        if (Physics.CapsuleCast(start, end, radius, direction - Vector3.right, out wallCheck,maxDistance, 3))
        {
            return Mathf.Clamp(wallCheck.distance, 0, 1) < .1f;
        }

        return false;
    }

    private float CollisionHandler()
    {
        Vector3 start = feetPosition.position + Vector3.up * .5f;
        Vector3 end = feetPosition.position + Vector3.up * 1.5f;
        float radius = .5f;
        Vector3 direction = moveDirection;
        float maxDistance = .5f;

        if (Physics.CapsuleCast(start, end, radius, direction, out collisionCheck, maxDistance, 3))
        {
            return collisionCheck.distance;
        }
        else
        {
            Vector3 vectorRight = new Vector3(moveDirection.x * Mathf.Cos(20) + moveDirection.z * Mathf.Sin(20), 0, -moveDirection.x * Mathf.Sin(20) + moveDirection.z * Mathf.Cos(20));
            Vector3 vectorLeft = new Vector3(moveDirection.x * Mathf.Cos(20) - moveDirection.z * Mathf.Sin(20), 0, moveDirection.x * Mathf.Sin(20) + moveDirection.z * Mathf.Cos(20));

            if (!WallChecker())
            if (Physics.CapsuleCast(start, end, radius, vectorLeft, out collisionCheck, maxDistance, 3))
            {
                return collisionCheck.distance;
            }

            if (Physics.CapsuleCast(start, end, radius, vectorRight, out collisionCheck, maxDistance, 3))
            {
                return collisionCheck.distance;
            }
        }
        return 2;
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


