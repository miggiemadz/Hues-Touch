using UnityEngine;

//to test enemy health (integrate into enemyai script later)
public class testEnemy : MonoBehaviour
{
    [SerializeField] FloatingHBar HealthBar;
    [SerializeField] float maxHealth = 100f;
    [SerializeField] float currentHealth;

    //to make bar stay over player
    [SerializeField] private new Camera camera;
    //[SerializeField] private Transform target; //to refernce enemy position
    //[SerializeField] private Vector3 offset; //to move above enemy

    private void Awake()
    {
        HealthBar = GetComponentInChildren <FloatingHBar>(); //reference health bar ui
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HealthBar.UpdateHealth(currentHealth, maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.rotation = camera.transform.rotation; //to have Hbar always face cam
        //transform.position = target.position+offset; //set bar position
    }

    //4 whenever damage is taken (for ex: takeDamage(20) is called on ln 65)
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        HealthBar.UpdateHealth(currentHealth, maxHealth);
    }
}
