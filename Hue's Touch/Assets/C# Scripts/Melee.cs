using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Melee : MonoBehaviour
{
    [Header("Core Enemy Variables")]
    [SerializeField]  private NavMeshAgent agent;
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask WhatIsGround, WhatIsPlayer;
    private Vector3 WalkPoint;
    private bool PlayerInSightRange, PlayerInAttackRange;
    private bool WalkPointSet;
    private bool AlreadyAttacked;
    public EnemySpawner spawner; 

    [Header("Enemy Customization")]

    [SerializeField] private int health = 100;
    [SerializeField] private float WalkPointRange = 20;
    [SerializeField] private float SightRange = 10, AttackRange = 1;
    [SerializeField] private float TimeBetweenAttacks = 1;
    [SerializeField] private int Damage = 20;

    private void Awake()
    {
        player = GameObject.Find("TestPlayer").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        PlayerInSightRange = Physics.CheckSphere(transform.position, SightRange, WhatIsPlayer);
        PlayerInAttackRange = Physics.CheckSphere(transform.position, AttackRange, WhatIsPlayer);

        if (!PlayerInSightRange && !PlayerInAttackRange) Patrolling();
        if (PlayerInSightRange && !PlayerInAttackRange) ChasePlayer();
        if (PlayerInSightRange && PlayerInAttackRange) AttackPlayer();
    }

    private void Patrolling()
    {
        if (!WalkPointSet)
        {
            SearchWalkPoint();
        }
        if (WalkPointSet)
        {
            agent.SetDestination(WalkPoint);
        }
        Vector3 DistanceToWalkPoint = transform.position - WalkPoint;
        if (DistanceToWalkPoint.magnitude < 1f)
        {
            WalkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        float RandomX = Random.Range(-WalkPointRange, WalkPointRange);
        float RandomZ = Random.Range(-WalkPointRange, WalkPointRange);

        WalkPoint = new Vector3(transform.position.x + RandomX, transform.position.y, transform.position.z + RandomZ);

        // Ensure the point is on the ground
        if (Physics.Raycast(WalkPoint, -transform.up, 2f, WhatIsGround))
        {
            WalkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position); // Stop moving
        transform.LookAt(player);
        if (!AlreadyAttacked)
        {
            MeleeAttack(); 
        }
            AlreadyAttacked = true;
            Invoke(nameof(ResetAttack), TimeBetweenAttacks);
    }
    
    private void MeleeAttack() {
    if (Vector3.Distance(transform.position, player.position) < AttackRange + 1f) {
        Debug.Log("Enemy performed melee attack!");

        // Get the player's health script
        NewMonoBehaviourScript playerScript = player.GetComponent<NewMonoBehaviourScript>();
        if (playerScript != null) {
            playerScript.TakeDamage(Damage);
        } else {
            Debug.LogWarning("Could not find player script to apply damage.");
        }
    }
}    private void ResetAttack()
    {
        AlreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("I got shot!!");
        if (health <= 0){
            Invoke(nameof(Die), 0.5f);
        }
    }

    private void Die()
    {
        if (spawner != null)
        {
            spawner.EnemyDied();
        }
        Destroy(gameObject);
    }

    // Just to showcase the enemy when turning on gizmos
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, SightRange);
    }
}