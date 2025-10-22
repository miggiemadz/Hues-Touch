using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour // I HATE UI IT SUCKSSSSSSS ;-;
{
    [Header("Grounded Movement Variables")]
    public float maxHealth = 100f;
    private float currentHealth;
    public healthScript healthBar;

    [Header("Respawn Variables")]
    public float threshold; // minimum y-value until player respawns(-10)
    public GameObject respawnScreen; //to show respawn screen
    public GameManager gameManagerScript;

    //TO-DO: new healthbar-> use splatter sprite & create dull version for lost heart/damage



    //private Image healthFill;
    //[SerializeField] private bool ShowHealthBar = true;

    public void Start() 
    {
        currentHealth = maxHealth; //set player health to 100 when game starts
        healthBar.setMaxHealth((int)maxHealth); //for healthbar (changing it to int because setMaxHealth method is a dick)
        /*if (ShowHealthBar)
        {
            // Create Canvas
            GameObject canvasGO = new GameObject("HealthBarCanvas");
            canvasGO.transform.SetParent(transform);
            canvasGO.transform.localPosition = new Vector3(0, 2f, 0); // Position above enemy
            Canvas canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main; // ← connects it to the main camera
            canvas.GetComponent<RectTransform>().sizeDelta = new Vector2(1.5f, 0.3f);

            // Add the other usual components
            canvasGO.AddComponent<CanvasScaler>();
            canvasGO.AddComponent<GraphicRaycaster>();

            // Background Image
            GameObject bgGO = new GameObject("Background");
            bgGO.transform.SetParent(canvasGO.transform);
            Image bgImage = bgGO.AddComponent<Image>();
            bgImage.color = Color.black;
            RectTransform bgRect = bgGO.GetComponent<RectTransform>();
            bgRect.anchorMin = Vector2.zero;
            bgRect.anchorMax = Vector2.one;
            bgRect.offsetMin = Vector2.zero;
            bgRect.offsetMax = Vector2.zero;

            // Fill Image
            GameObject fillGO = new GameObject("Fill");
            fillGO.transform.SetParent(bgGO.transform);
            healthFill = fillGO.AddComponent<Image>();
            healthFill.color = Color.red;
            RectTransform fillRect = fillGO.GetComponent<RectTransform>();
            fillRect.anchorMin = Vector2.zero;
            fillRect.anchorMax = Vector2.one;
            fillRect.offsetMin = Vector2.zero;
            fillRect.offsetMax = Vector2.zero;
        }*/
    }

    private void Update()
    {

        //DAMAGE TEST (bc its not working w dummy atm)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(20);
        }

        //fill.fillAmount = currentHealth / maxHealth;
    }

    public void FixedUpdate()
    {
        //if player falls off map/if player health = 0
        if (transform.position.y < threshold || currentHealth == 0) //if playerhealth<0 or if player reaches threshold/falls off map
        {
            Debug.Log("Dead");
            gameManagerScript.gameOver(); //accessing func from gamemanager (display respawn screen)
        }
    }


    //4 whenever damage is taken (for ex: takeDamage(20) is called on ln 65)
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        healthBar.setHealth((int)currentHealth);
    }

    //private void UpdateHealthBar()
    //{
    //    if (ShowHealthBar)
    //    {
    //        float normalized = currentHealth / maxHealth;
    //        normalized = Mathf.Clamp01(normalized);

    //        // Scale the X of the fill
    //        healthFill.rectTransform.localScale = new Vector3(normalized, 1, 1);
    //    }
    //}
}
