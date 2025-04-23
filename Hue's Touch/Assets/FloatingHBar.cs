using UnityEngine;
using UnityEngine.UI;

public class FloatingHBar : MonoBehaviour
{
    [SerializeField] private Slider slider; //refrences DamageDummy slider/healthbar

    public void UpdateHealth(float maxHealth, float currentHealth)
    {
        slider.value = currentHealth / maxHealth;
    }
}
