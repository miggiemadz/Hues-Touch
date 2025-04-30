using UnityEngine;
using UnityEngine.AI;

public interface IMoveBehavior
{
    void Patrol(NavMeshAgent agent);
    void Chase(NavMeshAgent agent, Transform player);
}

public interface IRetreatBehavior
{
    void StayAwayFromPlayer(NavMeshAgent agent, Transform player);
}

public interface IAttackBehavior
{
    void Attack(Transform player);
}