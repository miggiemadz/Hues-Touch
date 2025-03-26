using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
public class Enemyai2 : MonoBehaviour
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

    public enum EnemyType { Melee, Ranged }
    [Header("Enemy Customization")]
    [SerializeField] private EnemyType enemyType;

    [SerializeField] private int health = 100;
    [SerializeField] private float WalkPointRange = 50;

    [SerializeField] private float SightRange = 25, AttackRange = 10;
    [SerializeField] private float TimeBetweenAttacks = 2;
    [SerializeField] private GameObject projectile;
    [SerializeField] private int MeleeDamage = 20;

    private void Awake()
    {
        player = GameObject.Find("TestPlayer").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void OnValidate()
    {
        if (enemyType == EnemyType.Melee)
        {
            projectile = null;
            AttackRange = 1;
        }
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
            if (enemyType == EnemyType.Ranged)
            {
                RangeAttack();
            }
            else
            { 
                MeleeAttack(); 
            }
            AlreadyAttacked = true;
            Invoke(nameof(ResetAttack), TimeBetweenAttacks);
        }
    }

    private void RangeAttack()
    {
        if (projectile != null)
        {
            GameObject proj = Instantiate(projectile, transform.position + transform.forward, Quaternion.identity);
            Projectile projScript = proj.GetComponent<Projectile>();
            if (projScript != null)
            {
                Vector3 shootDirection = (player.position - transform.position).normalized;
                projScript.SetDirection(shootDirection);
            }
            else
            {
                Debug.LogError("Projectile script is missing on the instantiated projectile!");
            }
        }
    }

    private void MeleeAttack()
    {
         if (Vector3.Distance(transform.position, player.position) < AttackRange + 1)
        {
            Debug.Log("melee attack!");
            player.TakeDamage(MeleeDamage);
        }
        
    }
    private void ResetAttack()
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

    // Death
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