using UnityEngine;

// This script handles melee attack behavior for an enemy.
public class MeleeAttack : MonoBehaviour, IAttackBehavior
{
    // Damage dealt to the player during an attack
    public int damage = 10;

    // Executes the melee attack, dealing damage to the player
    public void Attack(Transform player)
    {
        // Get the Health component from the player
        Health playerHealth = player.GetComponent<Health>();

        // If the player has a Health component, deal damage
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
    }
}
