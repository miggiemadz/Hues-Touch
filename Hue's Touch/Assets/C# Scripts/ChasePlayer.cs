using UnityEngine;
public class ChasePlayer : MonoBehaviour, IMoveBehavior {
    public void Move(UnityEngine.AI.NavMeshAgent agent, Transform player) {
        if (player != null)
            agent.SetDestination(player.position);
    }
}
