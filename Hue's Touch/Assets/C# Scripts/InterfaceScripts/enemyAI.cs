using UnityEngine;
using UnityEngine.AI;

// This script controls the behavior of an enemy AI, including patrolling, chasing, attacking, and retreating.
public class EnemyAI : MonoBehaviour
{
    // Interfaces for different behaviors
    private IMoveBehavior moveBehavior; // Single behavior for both patrol and chase
    private IRetreatBehavior retreatBehavior; // Behavior for retreating
    private IAttackBehavior attackBehavior; // Behavior for attacking

    [Header("Detection Settings")]
    [SerializeField] private float sightRange = 10f; // Range within which the enemy can see the player
    [SerializeField] private float attackRange = 5f; // Range within which the enemy can attack the player
    [SerializeField] private float retreatDistance = 7f; // Distance to trigger retreat behavior
    [SerializeField] private LayerMask playerLayer; // Layer mask to identify the player

    [Header("Components")]
    [SerializeField] private NavMeshAgent agent; // NavMeshAgent for pathfinding
    [SerializeField] private Transform player; // Reference to the player's transform

    [Header("Attack Settings")]
    [SerializeField] private float timeBetweenAttacks = 2f; // Cooldown time between attacks

    private bool alreadyAttacked; // Tracks if the enemy has already attacked
    private bool provoked; // Indicates if the enemy has been provoked
    private bool isRetreating = false; // Tracks if the enemy is currently retreating

    void Awake()
    {
        // Initialize components
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        if (player == null)
        {
            player = GameObject.FindWithTag("Player").transform;
        }

        moveBehavior = GetComponent<IMoveBehavior>();
        retreatBehavior = GetComponent<IRetreatBehavior>();
        attackBehavior = GetComponent<IAttackBehavior>();

        // Ensure the required components are present
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent is missing! EnemyAI will not function correctly.");
        }

        if (player == null)
        {
            Debug.LogError("Player Transform is missing! EnemyAI will not function correctly.");
        }
    }

    void Update()
    {
        bool inSight = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        bool inAttack = Physics.CheckSphere(transform.position, attackRange, playerLayer);
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (provoked || inSight)
        {
            provoked = true;

            // If the player is too close, trigger retreat behavior
            if (ShouldRetreat(distanceToPlayer))
            {
                isRetreating = true; // Start retreating
                retreatBehavior.StayAwayFromPlayer(agent, player);
            }
            // If the enemy is retreating but has reached a safe distance, stop retreating
            else if (isRetreating && distanceToPlayer > retreatDistance + 5f) // Add a buffer zone (e.g., 5 units)
            {
                isRetreating = false; // Stop retreating
            }
            // If not retreating, handle other behaviors
            else if (!isRetreating)
            {
                if (inSight && inAttack)
                {
                    Attack();
                }
                else if (inSight) 
                {
                    moveBehavior?.Chase(agent, player);
                }
                
                else
                {
                Patrol();
                }
            }
            
        }
        else
        {
            Patrol();
        }
    }

    // Determines if the enemy should retreat
    bool ShouldRetreat(float distance)
    {
        return distance < retreatDistance && retreatBehavior != null;
    }

    // Handles patrolling behavior
    void Patrol()
    {
        moveBehavior?.Patrol(agent);
    }

    // Handles the attack behavior of the enemy
    void Attack()
    {
        // Stop moving and face the player
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            // Perform the attack
            attackBehavior?.Attack(player);
            alreadyAttacked = true;

            // Reset the attack after a cooldown
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    // Resets the attack cooldown
    void ResetAttack()
    {
        alreadyAttacked = false;
    }

    // External method to provoke the enemy
    public void Provoke()
    {
        provoked = true;
    }

    // Draws debug gizmos in the editor to visualize sight, attack, and retreat ranges
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange); // Attack range

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange); // Sight range

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, retreatDistance); // Retreat distance
    }
}
