using UnityEngine;
using UnityEngine.AI;

// This script makes the enemy stay away from the player when within a certain range.
public class StayAway : MonoBehaviour, IRetreatBehavior
{
    // Distance at which the enemy starts retreating
    public float retreatDistance = 20f;

    // Stay away behavior: Makes the enemy retreat when the player is too close
    public void StayAwayFromPlayer(NavMeshAgent agent, Transform player)
    {
        // Ensure the player reference is valid
        if (player == null)
        {
            return;
        }

        // Calculate the direction away from the player
        Vector3 dir = (agent.transform.position - player.position).normalized;

        // Calculate the target position retreatDistance units away from the player
        Vector3 targetPosition = player.position + dir * retreatDistance;

        // Set the destination to move away from the player
        agent.SetDestination(targetPosition);
    }
}