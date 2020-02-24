using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHealth : MonoBehaviour
{
     public PlayerMovement PlayerScript;
     public Slider Healthbar;
     public Slider Stamina;
     public Slider Shield;
      
    private void Start()
    {

        Healthbar.value = 1f;
        Stamina.value = 1f;
        Shield.value = 1f;
    }
    private void Update()
    {
        Stamina.value = CalcualteStamina();
        Healthbar.value = CalculateHealth();
        Shield.value = CalculateShield();
    }
    float CalculateHealth()
    {
        return PlayerScript.CurrentHealth / PlayerScript.MaxHealth;
    }
    float CalcualteStamina()
    {
        return PlayerScript.Stamina / PlayerScript.MaxStamina;
    }
    float CalculateShield()
    {
        return PlayerScript.Shield / PlayerScript.MaxShield;
    }
}
