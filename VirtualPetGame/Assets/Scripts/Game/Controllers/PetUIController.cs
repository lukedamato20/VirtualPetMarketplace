using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PetUIController : MonoBehaviour
{
    public static PetUIController instance;
    
    public SpriteRenderer petRenderer;

    public Image foodMeter, happinessMeter, energyMeter;
    public Text ageMeter, timeMeter;
    public Text hungerState, happinessState, energyState, ageState;

    public Text maxFood, maxHappiness, maxEnergy, maxBreeding;
    public Text currentHunger, currentHappiness, currentEnergy, currentBreeding;
    public Text foodRate, happinessRate, energyRate;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Debug.LogWarning("More than one PetUIController in the Scene");
    }

    public void UpdateMeters(int food, int happiness, int energy, int breedingCounter, int age, float timePassed)
    {
        //Debug.Log("started needs");
        //foodMeter.fillAmount = (float)food / 100;
        //happinessMeter.fillAmount = (float)happiness / 100;
        //energyMeter.fillAmount = (float)energy / 100;

        float foodFillAmount = (float)food / NeedsController.instance.foodMax;
        float happinessFillAmount = (float)happiness / NeedsController.instance.happinessMax;
        float energyFillAmount = (float)energy / NeedsController.instance.energyMax;

        foodMeter.fillAmount = foodFillAmount;
        happinessMeter.fillAmount = happinessFillAmount;
        energyMeter.fillAmount = energyFillAmount;

        currentHunger.text = food.ToString();
        currentHappiness.text = happiness.ToString();
        currentEnergy.text = energy.ToString();
        currentBreeding.text = breedingCounter.ToString();

        // Check if each value is below 30 and change the color to red
        if (foodFillAmount < 0.3f)
        {
            foodMeter.color = Color.red;
        }
        else
        {
            foodMeter.color = Color.white;
        }

        if (happinessFillAmount < 0.3f)
        {
            happinessMeter.color = Color.red;
        }
        else
        {
            happinessMeter.color = Color.white;
        }

        if (energyFillAmount < 0.3f)
        {
            energyMeter.color = Color.red;
        }
        else
        {
            energyMeter.color = Color.white;
        }


        //Debug.Log("finished needs");
        ageMeter.text = age.ToString();
        timeMeter.text = timePassed.ToString();

        //Debug.Log("finished UpdateMeters");
    }

    public void changePetColour(float[] hsvValue)
    {
        if (hsvValue.Length != 3)
        {
            Debug.LogError("Invalid hsvValue array length.");
            return;
        }

        Color currentColour = petRenderer.color;

        // Convert the current color to HSV
        Color.RGBToHSV(currentColour, out float h, out float s, out float v);

        // Set the new hue and saturation values
        float photoshopHue = hsvValue[0];
        float photoshopSaturation = hsvValue[1];
        float photoshopBrightness = hsvValue[2];

        float unityHue = photoshopHue;
        float unitySaturation = photoshopSaturation;
        float unityBrightness = photoshopBrightness;

        Debug.Log("HUE: " + unityHue);
        Debug.Log("SATU: " + unitySaturation);
        Debug.Log("BRIGHTNESS: " + unityBrightness);

        // Convert the new HSV values back to RGB
        Color newColor = Color.HSVToRGB(unityHue, unitySaturation, unityBrightness);

        // Update the color of the sprite
        petRenderer.color = newColor;
    }

    public void UpdateMaxCounters(int foodMax, int happinessMax, int energyMax, int breedingCounterMax, int foodTickRate, int happinessTickRate, int energyTickRate)
    {
        maxFood.text = foodMax.ToString();
        maxHappiness.text = happinessMax.ToString();
        maxEnergy.text = energyMax.ToString();
        maxBreeding.text = breedingCounterMax.ToString();

        foodRate.text = "-" + foodTickRate.ToString();
        happinessRate.text = "-" + happinessTickRate.ToString();
        energyRate.text = "-" + energyTickRate.ToString();

    }

    public void RevertHungerState()
    {
        hungerState.text = "I am not hungry!";
    }

    public void UpdateAge(int age)
    {
        Debug.Log("Starting to check age: " + age);

        if (age == 0) { ageState.text = "Baby"; }
        else if (age > 0 && age < 12) { ageState.text = "Child"; }
        else if (age >= 13 && age < 20) { ageState.text = "Teen"; }
        else if (age >= 20 && age < 30) { ageState.text = "Young Adult"; }
        else if (age >= 30 && age < 65) { ageState.text = "Adult"; }
        else if (age >= 65 && age < 80) { ageState.text = "Elderly"; }
        else if (age >= 80) { ageState.text = "Dying"; }
        else 
        {
            Debug.Log("Invalid age...");
        }
    }

    public void RevertHappinessState()
    {
        happinessState.text = "I am happy!";
    }

    public void RevertEnergyState()
    {
        energyState.text = "I am not tired!";
    }

    public void UpdateHungerState()
    {
        hungerState.text = "I am hungry!";
    }

    public void UpdateHappinessState()
    {
        happinessState.text = "I am sad!";
    }

    public void UpdateEnergyState()
    {
        energyState.text = "I am tired!";
    }
}
