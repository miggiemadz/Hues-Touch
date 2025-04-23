using UnityEngine;

public class damage : MonoBehaviour
{
    private float dummyDamage = 20;
    public Health playerHealth;
    [SerializeField]public healthScript healthBar;

    //or use onTriggerStay to update every frame

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player")){
    //        doDamage();
    //    }
    //}

    //void doDamage()
    //{
    //    playerHealth.currentHealth = playerHealth.currentHealth - dummyDamage;
    //    healthBar.setHealth((int)playerHealth.currentHealth);
    //    //this.gameObject.SetActive(false);
    //}


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }


  
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.name == "TestPlayer")
    //    {
    //        playerHealth.TakeDamage(10); //-20hp (accessing takeDamage func from playerhealth)
    //    }
    //    //if (other.gameObject.tag == "Player")
    //    //{
    //    //    playerHealth.TakeDamage(20); //-20hp (accessing takeDamage func from playerhealth)
    //    //}
    //}
}
