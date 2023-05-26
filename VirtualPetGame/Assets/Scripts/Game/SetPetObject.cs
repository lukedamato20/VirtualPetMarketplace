using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPetObject : MonoBehaviour
{
    public string currentId = "";
    public string BodyColour = "";

    public int age = 0;
    public int food = 0;
    public int happiness = 0;
    public int energy = 0;
    public int MaxFood = 0;
    public string MaxFoodStr = "";
    public int MaxHappiness = 0;
    public string MaxHappinessStr = "";
    public int MaxEnergy = 0;
    public string MaxEnergyStr = "";
    public int BreedingCounter = 0;
    public int FoodTickRate = 0;
    public string FoodTickRateStr = "";
    public int HappinessTickRate = 0;
    public string HappinessTickRateStr = "";
    public int EnergyTickRate = 0;
    public string EnergyTickRateStr = "";
    public bool isDead = false;

    public static SetPetObject instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Debug.LogWarning("More than one SetPetObject in the Scene");
    }

    public void setPet()
    {
        Debug.Log("starting to save pet");

        // updated
        age = NeedsController.instance.age;
        food = NeedsController.instance.food;
        happiness = NeedsController.instance.happiness;
        energy = NeedsController.instance.energy;
        BreedingCounter = NeedsController.instance.breedingCounter;

        MaxFood = NeedsController.instance.foodMax;
        MaxFoodStr = getStringTraits(MaxFood);
        MaxHappiness = NeedsController.instance.happinessMax;
        MaxHappinessStr = getStringTraits(MaxHappiness);
        MaxEnergy = NeedsController.instance.energyMax;
        MaxEnergyStr = getStringTraits(MaxEnergy);
        FoodTickRate = NeedsController.instance.foodTickRate;
        FoodTickRateStr = getStringNeeds(FoodTickRate);
        HappinessTickRate = NeedsController.instance.happinessTickRate;
        HappinessTickRateStr = getStringNeeds(HappinessTickRate);
        EnergyTickRate = NeedsController.instance.energyTickRate;
        EnergyTickRateStr = getStringNeeds(EnergyTickRate);

        isDead = NeedsController.instance.isDead;

        // unchanged
        currentId = GetPetObject.instance.currentId;
        BodyColour = GetPetObject.instance.BodyColourOG;

        Debug.Log("saved stats");

        SendPetData();
    }

    public string getStringTraits(int value)
    {
        switch (value)
        {
            case 120:
                return "very_low";
            case 140:
                return "low";
            case 160:
                return "medium";
            case 180:
                return "high";
            case 200:
                return "very_high";
            default:
                return "error";
        }
    }

    public string getStringNeeds(int value)
    {
        switch (value)
        {
            case 1:
                return "very_low";
            case 2:
                return "low";
            case 3:
                return "medium";
            case 4:
                return "high";
            case 5:
                return "very_high";
            default:
                return "error";
        }
    }

    public void SendPetData()
    {
        Debug.Log("starting to load array");

        
        List<object> petData = new List<object>();
        petData.Add(currentId);
        petData.Add(BodyColour);
        petData.Add(age);
        petData.Add(food);
        petData.Add(happiness);
        petData.Add(energy); 

        petData.Add(MaxFoodStr);
        petData.Add(MaxHappinessStr);
        petData.Add(MaxEnergyStr);
        petData.Add(BreedingCounter);
        petData.Add(FoodTickRateStr);
        petData.Add(HappinessTickRateStr);
        petData.Add(EnergyTickRateStr);
        petData.Add(isDead);

        Debug.Log("currentId = " + currentId);
        Debug.Log("age = " + age);
        Debug.Log("food = " + food);
        Debug.Log("happiness = " + happiness);
        Debug.Log("energy = " + energy);
        Debug.Log("MaxFood = " + MaxFoodStr);
        Debug.Log("MaxHappiness = " + MaxHappinessStr);
        Debug.Log("MaxEnergy = " + MaxEnergyStr);
        Debug.Log("BreedingCounter = " + BreedingCounter);
        Debug.Log("FoodTickRate = " + FoodTickRateStr);
        Debug.Log("HappinessTickRate = " + HappinessTickRateStr);
        Debug.Log("EnergyTickRate = " + EnergyTickRateStr);
        Debug.Log("BodyColour = " + BodyColour);
        Debug.Log("isDead = " + isDead);

        Debug.Log("array loaded and created");

        Application.ExternalCall("setUpdatedPetData", petData);
        Debug.Log("sent message");

    }
}
