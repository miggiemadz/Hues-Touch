using UnityEngine;
using UnityEngine.AI;
public class EnemyAI : MonoBehaviour {
    private IMoveBehavior moveBehavior;
    private UnityEngine.AI.NavMeshAgent agent;
    private Transform player;

    void Awake() {
        moveBehavior = GetComponent<IMoveBehavior>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update() {
        if (moveBehavior != null) {
            moveBehavior.Move(agent, player);
        }
    }
}
