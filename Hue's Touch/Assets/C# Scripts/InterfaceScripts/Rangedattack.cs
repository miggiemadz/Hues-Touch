using UnityEngine;

// This script handles ranged attack behavior for an enemy, shooting projectiles at the player.
public class RangedAttack : MonoBehaviour, IAttackBehavior
{
    // The projectile prefab to instantiate when attacking
    public GameObject projectile;

    // The point from which the projectile is fired
    public Transform shootPoint;

    // Executes the ranged attack, shooting a projectile toward the player
    public void Attack(Transform player)
    {
        // Ensure all required components are assigned
        if (projectile == null || shootPoint == null || player == null)
        {
            return;
        }

        // Instantiate the projectile at the shoot point
        GameObject proj = Instantiate(projectile, shootPoint.position, Quaternion.identity);

        // Get the Projectile script attached to the instantiated projectile
        Projectile projScript = proj.GetComponent<Projectile>();

        // If the projectile has a script, set its direction toward the player
        if (projScript != null)
        {
            Vector3 shootDir = (player.position - shootPoint.position).normalized;
            projScript.SetDirection(shootDir);
        }
    }
}
