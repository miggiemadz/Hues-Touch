using UnityEngine;
using UnityEngine.AI;
public class Trap : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int enemies = 3;
    [SerializeField] private float spawnRadius = 10f;
    private Transform player;
    [SerializeField] private LayerMask WhatIsPlayer;
    [SerializeField] private bool Armed = true;

    private void Update()
    {
        if (Physics.CheckSphere(transform.position, spawnRadius, WhatIsPlayer) && Armed)
        {
            SpawnEnemies();
            Armed = false;
        }
    }
    private void SpawnEnemies()
    {
        for (int i = 1; i<=enemies; i++)
        {
            Vector3 randomPoint = transform.position + Random.insideUnitSphere * spawnRadius;
            randomPoint.y = transform.position.y;
            // Try to find a position on the NavMesh near that point
            if (UnityEngine.AI.NavMesh.SamplePosition(randomPoint, out UnityEngine.AI.NavMeshHit hit, spawnRadius, UnityEngine.AI.NavMesh.AllAreas))
            {
                GameObject enemy = Instantiate(enemyPrefab, hit.position, Quaternion.identity);
            }
        }
    }
    private void OnDrawGizmosSelected(){
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}