using UnityEngine;
using UnityEngine.AI;
public interface IMoveBehavior {
    void Move(NavMeshAgent agent, Transform player);
}
