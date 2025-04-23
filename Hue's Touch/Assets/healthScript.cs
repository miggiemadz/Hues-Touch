using UnityEngine;
using UnityEngine.UI; //allows creation of variables that refer to the UI

public class healthScript : MonoBehaviour
{
    public Slider slider; 
    public Gradient gradient;//to show different colors @ different health values (red,yellow,green)
    public Image fill;

    public void setMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health; //2 make slider start @ max health
        fill.color = gradient.Evaluate(100f); //should be green
    }

    public void setHealth(int health) //update slider value
    {
        slider.value = health; //changes value of the slider 
        fill.color = gradient.Evaluate(slider.normalizedValue); //change color value
    }
}