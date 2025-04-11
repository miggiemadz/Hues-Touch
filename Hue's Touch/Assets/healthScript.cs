using UnityEngine;
using UnityEngine.UI; //allows creation of variables that refer to the UI

public class healthScript : MonoBehaviour
{
    public Slider slider; //refrences slider in "HealthBar"

    public void setMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health; //2 make slider start @ max health
    }

    public void setHealth(int health) {
        slider.value = health; //changes value of the slider 
    }
}
