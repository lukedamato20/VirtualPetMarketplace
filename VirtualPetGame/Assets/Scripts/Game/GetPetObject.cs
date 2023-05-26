using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static GetPetObject;

public class GetPetObject : MonoBehaviour
{
    public string currentId = "";
    public string BodyColourOG = "";
    public int age = 0;
    public int CurrentFood = 0;
    public int CurrentHappiness = 0;
    public int CurrentEnergy = 0;
    public int MaxFood = 0;
    public int MaxHappiness = 0;
    public int MaxEnergy = 0;
    public int BreedingCounter = 0;
    public int FoodTickRate = 0;
    public int HappinessTickRate = 0;
    public int EnergyTickRate = 0;
    public float[] BodyColour;

    public static GetPetObject instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Debug.LogWarning("More than one GetPetObject in the Scene");
    }

    public class Child
    {
        public string id;
        public int age;
        public int current_food;
        public int current_happiness;
        public int current_energy;
        public string maxfood_level;
        public string maxhappiness_level;
        public string maxenergy_level;
        public int fertility_counter;
        public string hunger_level;
        public string sadness_level;
        public string laziness_level;
        public string body_colour;
    }

    public void getPet(string childJson)
    {
        Child child = JsonUtility.FromJson<Child>(childJson);

        Debug.Log("Received Child: " + child);

        Debug.Log("getting data from child");

        currentId = child.id;
        age = child.age;
        BodyColourOG = child.body_colour;

        if (child.current_food == 0 && child.current_happiness == 0 && child.current_energy == 0)
        {
            CurrentFood = getTraitsIntegerValue(child.maxfood_level);
            CurrentHappiness = getTraitsIntegerValue(child.maxhappiness_level);
            CurrentEnergy = getTraitsIntegerValue(child.maxenergy_level);
        }
        else
        {
            CurrentFood = child.current_food;
            CurrentHappiness = child.current_happiness;
            CurrentEnergy = child.current_energy;
        }

        // Pets' Traits init
        MaxFood = getTraitsIntegerValue(child.maxfood_level);
        MaxHappiness = getTraitsIntegerValue(child.maxhappiness_level);
        MaxEnergy = getTraitsIntegerValue(child.maxenergy_level);

        // Pet's needs init
        FoodTickRate = getNeedsIntegerValue(child.hunger_level);
        HappinessTickRate = getNeedsIntegerValue(child.sadness_level);
        EnergyTickRate = getNeedsIntegerValue(child.laziness_level);

        BodyColour = getBodyColourHue(child.body_colour);
        BreedingCounter = child.fertility_counter;

        Debug.Log("currentId = " + currentId);
        Debug.Log("age = " + age);
        Debug.Log("Food = " + CurrentFood);
        Debug.Log("Happiness = " + CurrentHappiness);
        Debug.Log("Energy = " + CurrentEnergy);
        Debug.Log("MaxFood = " + MaxFood);
        Debug.Log("MaxHappiness = " + MaxHappiness);
        Debug.Log("MaxEnergy = " + MaxEnergy);
        Debug.Log("BreedingCounter = " + BreedingCounter);
        Debug.Log("FoodTickRate = " + FoodTickRate);
        Debug.Log("HappinessTickRate = " + HappinessTickRate);
        Debug.Log("EnergyTickRate = " + EnergyTickRate);
        Debug.Log("BodyColour = " + BodyColour);
    }

    public float[] getBodyColourHue(string value)
    {
        float[] hsvValues = new float[3];
        switch (value)
        {
            // HUE VALUES 
            case "red":
                hsvValues[0] = 0.0f;
                hsvValues[1] = 1.0f;
                hsvValues[2] = 1.0f;
                break;
            case "orange":
                hsvValues[0] = 39.0f / 360.0f;
                hsvValues[1] = 1.0f;
                hsvValues[2] = 1.0f;
                break;
            case "yellow":
                hsvValues[0] = 59.0f / 360.0f;
                hsvValues[1] = 1.0f;
                hsvValues[2] = 1.0f;
                break;
            case "green":
                hsvValues[0] = 100.0f / 360.0f;
                hsvValues[1] = 1.0f;
                hsvValues[2] = 1.0f;
                break;
            case "blue":
                hsvValues[0] = 197.0f / 360.0f;
                hsvValues[1] = 1.0f;
                hsvValues[2] = 1.0f;
                break;
            case "purple":
                hsvValues[0] = 271.0f / 360.0f;
                hsvValues[1] = 1.0f;
                hsvValues[2] = 1.0f;
                break;
            case "pink":
                hsvValues[0] = 301.0f / 360.0f;
                hsvValues[1] = 1.0f;
                hsvValues[2] = 1.0f;
                break;
            default:
                hsvValues[0] = 0.0f;
                hsvValues[1] = 0.0f; // gray
                hsvValues[2] = 1.0f;
                break;
        }
        return hsvValues;
    }

    public int getTraitsIntegerValue(string value)
    {
        switch (value)
        {
            case "very_low":
                return 120;
            case "low":
                return 140;
            case "medium":
                return 160;
            case "high":
                return 180;
            case "very_high":
                return 200;
            default:
                return 120;
        }
    }
    public int getNeedsIntegerValue(string value)
    {
        switch (value)
        {
            case "very_low":
                return 1;
            case "low":
                return 2;
            case "medium":
                return 3;
            case "high":
                return 4;
            case "very_high":
                return 5;
            default:
                return 1;
        }
    }

    public void fillPetAttributes()
    {
        UpdateTraits(currentId, age, CurrentFood, CurrentHappiness, CurrentEnergy, MaxFood, MaxHappiness, MaxEnergy, BreedingCounter, BodyColour);
        UpdateNeeds(FoodTickRate, HappinessTickRate, EnergyTickRate);
    }

    public void UpdateTraits(string currentId, int newAge, int currentFood, int currentHappiness, int currentEnergy, int newMaxFood, int newMaxHappiness, int newMaxEnergy, int newBreedingCounter, float[] BodyColour)
    {
        Debug.Log("started updatetraits");

        NeedsController.instance.petId = currentId;
        Debug.Log("petId: " + NeedsController.instance.petId);

        NeedsController.instance.age = newAge;
        Debug.Log("age: " + NeedsController.instance.age);

        NeedsController.instance.food = currentFood;
        Debug.Log("foodMax: " + NeedsController.instance.food);

        NeedsController.instance.happiness = currentHappiness;
        Debug.Log("happinessMax: " + NeedsController.instance.happiness);

        NeedsController.instance.energy = currentEnergy;
        Debug.Log("energyMax: " + NeedsController.instance.energy);

        NeedsController.instance.foodMax = newMaxFood;
        Debug.Log("foodMax: " + NeedsController.instance.foodMax);

        NeedsController.instance.happinessMax = newMaxHappiness;
        Debug.Log("happinessMax: " + NeedsController.instance.happinessMax);

        NeedsController.instance.energyMax = newMaxEnergy;
        Debug.Log("energyMax: " + NeedsController.instance.energyMax);

        NeedsController.instance.breedingCounter = newBreedingCounter;
        Debug.Log("breedingCounter: " + NeedsController.instance.breedingCounter);

        NeedsController.instance.petColourValues = BodyColour;
        Debug.Log("petColourValues: " + NeedsController.instance.petColourValues);

        Debug.Log("finished updatetraits");

    }

    public void UpdateNeeds(int newFoodTickRate, int newHappinessTickRate, int newEnergyTickRate)
    {
        Debug.Log("started updateneeds");

        NeedsController.instance.foodTickRate = newFoodTickRate;
        Debug.Log("foodTickRate: " + NeedsController.instance.foodTickRate);

        NeedsController.instance.happinessTickRate = newHappinessTickRate;
        Debug.Log("happinessTickRate: " + NeedsController.instance.happinessTickRate);

        NeedsController.instance.energyTickRate = newEnergyTickRate;
        Debug.Log("energyTickRate: " + NeedsController.instance.energyTickRate);

        Debug.Log("finished updatetraits");

    }
}



