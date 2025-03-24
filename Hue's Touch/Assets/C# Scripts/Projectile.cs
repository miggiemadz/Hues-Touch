using UnityEngine;

public class Projectile : MonoBehaviour {
    private Rigidbody rb;
    [Header("Projectile Customization")]
    public float speed = 20f; 
    public float lifetime = 1f;
    [SerializeField] private int Damage = 20;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        if (rb == null) {
            Debug.LogError("Projectile is missing a Rigidbody!");
        }
    }

    public void SetDirection(Vector3 direction) {
        if (rb != null) {
            rb.AddForce(direction * speed, ForceMode.Impulse); 
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            NewMonoBehaviourScript player = other.GetComponent<NewMonoBehaviourScript>();
            if (player != null) {
                player.TakeDamage(Damage);
            }
        
        }
        if (other.CompareTag("Enemy")) {
            Enemyai2 enemy = other.GetComponent<Enemyai2>();
            if (enemy != null) {
                enemy.TakeDamage(Damage);
            }
        }
    Destroy(gameObject);
    }

    private void Start() {
        Destroy(gameObject, lifetime);
    }
}
