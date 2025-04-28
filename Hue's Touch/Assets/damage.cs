using UnityEngine;

public class damage : MonoBehaviour
{
    //private float dummyDamage = 20;
    private Health playerHealth;
    //[SerializeField]public healthScript healthBar;
    bool playerDetection = false;

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

    private void Update()
    {
        //if player presses "F" while in range
        if (playerDetection)
        {
            Debug.Log("Player collided");
            playerHealth.TakeDamage(20); //-20hp (accessing takeDamage func from playerhealth)
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.name == "TestPlayer")
        //{
        //    playerHealth.TakeDamage(10); //-20hp (accessing takeDamage func from playerhealth)
        //}
        if (other.CompareTag("Player"))
        {
            playerDetection = true;
        }
    }
}
