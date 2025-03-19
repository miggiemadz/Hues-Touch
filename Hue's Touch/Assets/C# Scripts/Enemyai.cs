using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour {
    // Core variables for enemy
    [Header("Core Enemy Variables")]
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask WhatIsGround, WhatIsPlayer;
    public Vector3 WalkPoint;
    public bool PlayerInSightRange, PlayerInAttackRange;
    bool WalkPointSet;
    bool AlreadyAttacked;

    [Header("Enemy Customization")]

    public int health = 100;
    public float WalkPointRange;
    public float SightRange, AttackRange;
    public float TimeBetweenAttacks;

    public GameObject projectile;

    private void Awake() {
        player = GameObject.Find("TestPlayer").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update() {
        PlayerInSightRange = Physics.CheckSphere(transform.position, SightRange, WhatIsPlayer);
        PlayerInAttackRange = Physics.CheckSphere(transform.position, AttackRange, WhatIsPlayer);

        if (!PlayerInSightRange && !PlayerInAttackRange) Patrolling();
        if (PlayerInSightRange && !PlayerInAttackRange) ChasePlayer();
        if (PlayerInSightRange && PlayerInAttackRange) AttackPlayer(); 
    }

    private void Patrolling() {
        if (!WalkPointSet) SearchWalkPoint();

        if (WalkPointSet)
            agent.SetDestination(WalkPoint);

        Vector3 DistanceToWalkPoint = transform.position - WalkPoint;
        if (DistanceToWalkPoint.magnitude < 1f)
            WalkPointSet = false;
    }

    private void SearchWalkPoint() {
        float RandomX = Random.Range(-WalkPointRange, WalkPointRange);
        float RandomZ = Random.Range(-WalkPointRange, WalkPointRange);

        WalkPoint = new Vector3(transform.position.x + RandomX, transform.position.y, transform.position.z + RandomZ);

        // Ensure the point is on the ground
        if (Physics.Raycast(WalkPoint, -transform.up, 2f, WhatIsGround)) 
            WalkPointSet = true;
    }

    private void ChasePlayer() {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer() {
    agent.SetDestination(transform.position); // Stop moving
    transform.LookAt(player);

    if (!AlreadyAttacked) {
        if (projectile != null) {
            GameObject proj = Instantiate(projectile, transform.position + transform.forward, Quaternion.identity);
            Projectile projScript = proj.GetComponent<Projectile>();

            if (projScript != null) {
                Vector3 shootDirection = (player.position - transform.position).normalized;
                projScript.SetDirection(shootDirection);
            } else {
                Debug.LogError("Projectile script is missing on the instantiated projectile!");
            }
        }

        AlreadyAttacked = true;
        Invoke(nameof(ResetAttack), TimeBetweenAttacks);
    }
}


    private void ResetAttack() {
        AlreadyAttacked = false;
    }

    public void TakeDamage(int damage) {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }

    // Death
    private void DestroyEnemy() {
        Destroy(gameObject);
    }

    // Just to showcase the enemy when turning on gizmos
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, SightRange);
    }
}
