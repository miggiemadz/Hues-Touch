using UnityEngine;
using UnityEngine.AI;
public class StayAway : MonoBehaviour, IMoveBehavior {
    public float retreatDistance = 7f;

    public void Move(UnityEngine.AI.NavMeshAgent agent, Transform player) {
        if (player == null) return;

        float distance = Vector3.Distance(agent.transform.position, player.position);

        if (distance < retreatDistance) {
            Vector3 dir = (agent.transform.position - player.position).normalized;
            agent.SetDestination(agent.transform.position + dir * 5f);
        }
    }
}
