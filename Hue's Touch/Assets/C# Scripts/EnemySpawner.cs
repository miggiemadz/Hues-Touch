using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour {
    [Header("Spawner Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnRadius = 10f;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private int maxEnemies = 5;

    [Header("NavMesh Settings")]
    [SerializeField] private LayerMask navMeshLayer;

    private int currentEnemyCount = 0;

    private void Start() {
        InvokeRepeating(nameof(SpawnEnemy), 2f, spawnInterval);
    }

    private void SpawnEnemy() {
        if (currentEnemyCount >= maxEnemies) return;

        Vector3 randomPoint = transform.position + Random.insideUnitSphere * spawnRadius;
        randomPoint.y = transform.position.y; // Keep it level with spawner

        // Try to find a position on the NavMesh near that point
        if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, spawnRadius, NavMesh.AllAreas)) {
            GameObject enemy = Instantiate(enemyPrefab, hit.position, Quaternion.identity);
            currentEnemyCount++;

            // talk to enemy created
            Health ai = enemy.GetComponent<Health>();
            if (ai != null) {
                ai.spawner = this;
            }
        } else {
            Debug.Log("Could not find NavMesh position near: " + randomPoint);
        }
    }
    
    public void EnemyDied() {
        currentEnemyCount--;
    }
    private void OnDrawGizmosSelected(){
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
