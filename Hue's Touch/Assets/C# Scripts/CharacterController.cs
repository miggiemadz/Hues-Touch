using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [Header("Grounded Movement Variables")]
    [SerializeField] private float playerRotateSpeed; // how fast the character rotates as it turns
    [SerializeField] private float playerGroundMoveAcceleration; // how fast the player accelerates from the start of input
    [SerializeField] private float playerGroundMoveDecceleration; // how fast the player deccelerates after inputs end
    [SerializeField] private float PLAYER_MAX_SPEED; // the max speed the player can reach
    private bool floorCloseThisFrame;
    private bool floorCollidingThisFrame;

    [Header("Grounded Movement Components")]
    private Vector2 playerGroundMoveVelocity; // the directional velocity that the player travels in

    [Header("Aerial Movement Variables")]
    [SerializeField] private float playerJumpSpeed;
    [SerializeField] private float jumpVelocityCutOff;
    [SerializeField] private float MAX_JUMP_HEIGHT;
    [SerializeField] private int jumpCount;
    [SerializeField] private int maxJumpCount;
    private bool canJump;
    private float playerJumpVelocity;
    private bool jumpPressed;
    private float initialJumpPosition;
    private float gravity; // the gravity force value
    private float lastYPosition = 0;

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
    [SerializeField] private Transform cameraFollowPosition;
    private float cameraYPosition;

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
        // Grounded Movement Variables
        playerRotateSpeed = 20f;
        playerGroundMoveAcceleration = 1f;
        playerGroundMoveDecceleration = 1.2f;
        PLAYER_MAX_SPEED = 0.4f;

        // Aerial Movement Variables
        playerJumpSpeed = 900f;
        jumpVelocityCutOff = 150f;
        MAX_JUMP_HEIGHT = 7f;
        jumpCount = 0;
        maxJumpCount = 2;

        // Camera Components
        cameraFollowPosition = GameObject.Find("CameraFollow").transform;

        rb = gameObject.GetComponent<Rigidbody>();
        feetPosition = transform.GetChild(0).GetComponent<Transform>();
        cameraReferenceRoot = transform.GetChild(3).gameObject.transform;
    }
    void Start()
    {
        
    }

    void Update()
    {
        jumpPressed = playerJump.action.ReadValue<float>() > 0;
        gravity = 9.8f * playerMass;

        inputDirection = new Vector3(playerMovement.action.ReadValue<Vector2>().x, 0, playerMovement.action.ReadValue<Vector2>().y).normalized;
        
        Vector3 cameraDiff = playerCamera.Follow.position - cameraTransform.position;
        cameraDiff.y = 0;
        cameraDiff.Normalize();
        cameraReferenceRoot.forward = cameraDiff;

        Vector3 flatForward = cameraReferenceRoot.forward;
        Vector3 flatRight = cameraReferenceRoot.right;

        moveDirection = flatForward * inputDirection.z + flatRight * inputDirection.x; // moveDirections vector2 values are read from the playerMovement input map

        cameraFollowPosition.transform.position = Vector3.Lerp(cameraFollowPosition.transform.position, new Vector3(transform.position.x, initialJumpPosition, transform.position.z), Time.deltaTime * 5f);


        if (Keyboard.current.eKey.wasPressedThisFrame) { // sorry to put this here but i might as well -- steven
        ShootProjectile();}
    }

    private void FixedUpdate()
    {
        floorCloseThisFrame = IsFloorClose();
        floorCollidingThisFrame = IsFloorColliding();

        if (!floorCollidingThisFrame)
        {
            playerJumpVelocity -= gravity * Time.fixedDeltaTime;
        }
        if (floorCloseThisFrame)
        {
            playerJumpVelocity = 0;
            initialJumpPosition = gameObject.transform.position.y;
            jumpCount = 0;
        }

        if ((gameObject.transform.position.y - initialJumpPosition > MAX_JUMP_HEIGHT && jumpCount > 0) || (!jumpPressed && gameObject.transform.position.y > initialJumpPosition))
        {
            if (playerJumpVelocity > 0)
            {
                playerJumpVelocity -= jumpVelocityCutOff * Time.fixedDeltaTime;
            }
        }

        if (jumpCount < maxJumpCount && jumpPressed)
        {
            jumpCount++;
            playerJumpVelocity += playerJumpSpeed * Time.fixedDeltaTime;
        }

        if (moveDirection.z != 0 || moveDirection.x != 0) // if either of the move inputs are pressed (vertical or horizontal)
        {
            Quaternion playerRotation = Quaternion.LookRotation(moveDirection);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, playerRotation, rotationSpeed * Time.fixedDeltaTime));

            playerGroundMoveVelocity.x += playerGroundMoveAcceleration * Time.fixedDeltaTime * moveDirection.x;
            playerGroundMoveVelocity.y += playerGroundMoveAcceleration * Time.fixedDeltaTime * moveDirection.z;
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
            playerGroundMoveVelocity.x = Mathf.MoveTowards(playerGroundMoveVelocity.x, 0, playerGroundMoveDecceleration * Time.fixedDeltaTime);
        }
        if (moveDirection.z < .1f && moveDirection.z > -.1f)
        {
            playerGroundMoveVelocity.y = Mathf.MoveTowards(playerGroundMoveVelocity.y, 0, playerGroundMoveDecceleration * Time.fixedDeltaTime);
        }

        if (!IsWallColliding())
        {
            rb.MovePosition(new Vector3(transform.position.x + playerGroundMoveVelocity.x,
                            transform.position.y + playerJumpVelocity * Time.fixedDeltaTime,
                            transform.position.z + playerGroundMoveVelocity.y));
        }

        Debug.Log(IsWallColliding());
    }

    private void LateUpdate()
    {
        lastYPosition = gameObject.transform.position.y;
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

    private bool IsFalling()
    {
        if (IsFloorClose() || lastYPosition < transform.position.y)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private bool IsJumping()
    {
        if (lastYPosition > transform.position.y || IsFloorClose())
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private bool IsFloorClose()
    {
        Vector3 start = feetPosition.position + Vector3.up * .5f;
        Vector3 end = feetPosition.position + Vector3.up * 1.5f;
        float radius = .5f;
        Vector3 direction = transform.TransformDirection(Vector3.down);
        float maxDistance = 5f;

        int excludedLayers = LayerMask.GetMask("Player", "Walls");
        int groundMask = ~excludedLayers;

        if (Physics.CapsuleCast(start, end, radius, direction, out groundCheck, maxDistance, groundMask))
        {
            return Mathf.Clamp(groundCheck.distance, 0, 1) < .3f;
        }

        return false;
    }

    private bool IsFloorColliding()
    {
        Vector3 start = feetPosition.position + Vector3.up * .5f;
        Vector3 end = feetPosition.position + Vector3.up * 1.5f;
        float radius = .5f;

        int excludedLayers = LayerMask.GetMask("Player", "Walls");
        int groundMask = ~excludedLayers;

        if (Physics.CheckCapsule(start, end, radius, groundMask))
        {
            Collider[] floorHit = Physics.OverlapCapsule(start, end, radius, groundMask);

            Vector3 playerPosition = transform.position;
            Collider floorCollider;
            Vector3 closestPoint;
            float distanceToPoint;

            foreach (Collider c in floorHit)
            {
                floorCollider = c;
                closestPoint = floorCollider.ClosestPoint(playerPosition);
                distanceToPoint = Vector3.Distance(closestPoint,playerPosition);

                transform.position = new Vector3(transform.position.x, transform.position.y + distanceToPoint, transform.position.z);

            }
            return true;
        }
        else
        {
            return false;
        }
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

    private bool IsWallColliding()
    {
        Vector3 start = feetPosition.position + Vector3.up * .5f;
        Vector3 end = feetPosition.position + Vector3.up * 1.5f;
        float radius = .3f;
        Vector3 direction = moveDirection;

        int excludedLayers = LayerMask.GetMask("Player", "Floor");
        int wallMask = ~excludedLayers;

        if (Physics.CheckCapsule(start, end, radius, wallMask))
        {
            Collider[] wallHit = Physics.OverlapCapsule(start, end, radius, wallMask);

            Vector3 playerPosition = transform.position;
            Collider floorCollider;
            Vector3 closestPoint;
            float distanceToPoint;

            foreach (Collider c in wallHit)
            {
                floorCollider = c;
                closestPoint = floorCollider.ClosestPoint(playerPosition);
                distanceToPoint = Vector3.Distance(closestPoint, playerPosition);

                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

            }
            return true;
        }
        else
        {
            return false;
        }
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


