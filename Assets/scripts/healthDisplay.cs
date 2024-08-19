using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthDisplay : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;   //this is used to change the color of the fill image according to health
    public Image fill;

    public void setMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        fill.color = gradient.Evaluate(1f);     // the 1f refers to the color at the extreme left of the gradient, it unlocks only after you define the gradient parameter
    }

    public void setHealth(int health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);     //the slider.mormalizedValue will make sure to take value between 0 and 1 and then it will change the gradient accordingly
    }
}
