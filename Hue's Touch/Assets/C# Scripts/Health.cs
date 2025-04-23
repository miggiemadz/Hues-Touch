using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour // I HATE UI IT SUCKSSSSSSS ;-;
{
    public float maxHealth = 100f;
    public float currentHealth;
    private Image healthFill;
    [SerializeField] private bool ShowHealthBar = true;
    public EnemySpawner spawner;
    public void Start()
    {
        currentHealth = maxHealth;
        if (ShowHealthBar)
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
        }
    }
    private void Update()
    {
        if (ShowHealthBar)
        {
            UpdateHealthBar();
        }
    }

    private void UpdateHealthBar()
    {
        if (ShowHealthBar)
        {
            float normalized = currentHealth / maxHealth;
            normalized = Mathf.Clamp01(normalized);
            
            // Scale the X of the fill
            healthFill.rectTransform.localScale = new Vector3(normalized, 1, 1);
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        GetComponent<EnemyAI>()?.Provoke();
        if (currentHealth <= 0)
        {
            Debug.Log("Dead");
            Destroy(gameObject);
                spawner?.EnemyDied();
        }
        
    }
}
