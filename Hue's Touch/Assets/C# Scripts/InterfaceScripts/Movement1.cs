using UnityEngine;
using UnityEngine.AI;

// This script handles the patrol and chase behaviors for an enemy.
public class Movement1 : MonoBehaviour, IMoveBehavior
{
    // Patrol range for random walk points
    public float range = 25f;

    // Layer mask to ensure walk points are on the ground
    public LayerMask groundLayer;

    // Current walk point for patrolling
    private Vector3 WalkPoint;

    // Whether a walk point has been set
    private bool walkPointSet;

    // Handles the patrol behavior
    public void Patrol(NavMeshAgent agent)
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }
        else
        {
            // Move the agent to the walk point
            agent.SetDestination(WalkPoint);

            // Check if the agent has reached the walk point
            if (Vector3.Distance(agent.transform.position, WalkPoint) < 1f)
            {
                walkPointSet = false;
            }
        }
    }

    // Handles the chase behavior
    public void Chase(NavMeshAgent agent, Transform player)
    {
        if (player != null)
        {
            // Move the agent toward the player's position
            agent.SetDestination(player.position);
        }
    }

    // Searches for a random walk point within the patrol range
    private void SearchWalkPoint()
    {
        // Generate random X and Z coordinates within the range
        float RandomX = Random.Range(-range, range);
        float RandomZ = Random.Range(-range, range);

        // Calculate the walk point position
        WalkPoint = new Vector3(transform.position.x + RandomX, transform.position.y, transform.position.z + RandomZ);

        // Ensure the walk point is on the ground
        if (Physics.Raycast(WalkPoint, -transform.up, 2f, groundLayer))
        {
            walkPointSet = true;
        }
    }
}
