using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class NeedsController : MonoBehaviour
{
    //public Animator PetAnimator;
    private TimeManager timeManager;
    public GameObject GameOverUI;
    public GameObject PetDiedUI;

    private bool isHungry = false, isSad = false, isTired = false, isBreeding = false;
    public bool isDead = false;
    public string petId = "";
    public int food, happiness, energy, age, breedingCounter;

    public int foodTickRate, happinessTickRate, energyTickRate;
    private int probIncFoodTickRate = 0, probDecFoodTickRate = 0, probIncHappinessTickRate = 0, probDecHappinessTickRate = 0, probIncEnergyTickRate = 0, probDecEnergyTickRate = 0;

    public int foodMax, happinessMax, energyMax, breedingCounterMax;
    private int probIncFoodMax = 0, probDecFoodMax = 0, probIncHappinessMax = 0, probDecHappinessMax = 0, probIncEnergyMax = 0, probDecEnergyMax = 0;

    private int notTakenCareOf = 0, chanceOfDying = 0;
    public float timePassed, startTime = 0f;
    public DateTime lastTimeFed, lastTimeHappy, lastTimeGainedEnergy;
    public float[] petColourValues;


    public static NeedsController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Debug.LogWarning("More than one NeedsController in the Scene");

        GetPetObject.instance.fillPetAttributes();
        PetUIController.instance.changePetColour(petColourValues);

        if (foodMax == null || happinessMax == null || energyMax == null || age == null || startTime == null)
        {
            Debug.Log("________ STATS WHERE NULL __________");
            foodMax = 100;
            happinessMax = 100;
            energyMax = 100;
            age = 0;
            startTime = 0f;
            foodTickRate = 2;
            happinessTickRate = 2;
            energyTickRate = 2;

        }

        Debug.Log(foodMax);
        Debug.Log(happinessMax);
        Debug.Log(energyMax);
        Debug.Log(age);
        Debug.Log(startTime);

        PetUIController.instance.UpdateMaxCounters(foodMax, happinessMax, energyMax, breedingCounterMax, foodTickRate, happinessTickRate, energyTickRate);
        // (foodMax, happinessMax, energyMax, age, foodTickRate, happinessTickRate, energyTickRate, timePassed)
        Initialize(food, happiness, energy, age, foodTickRate, happinessTickRate, energyTickRate, startTime);

        //Debug.Log("Initialized");
    }

    public void Initialize(int food, int happiness, int energy, int age, int foodTickRate, int happinessTickRate, int energyTickRate, float timePassed)
    {
        lastTimeFed = DateTime.Now;
        lastTimeHappy = DateTime.Now;
        lastTimeGainedEnergy = DateTime.Now;

        this.food = food;
        this.happiness = happiness;
        this.energy = energy;
        this.age = age;
        this.foodTickRate = foodTickRate;
        this.happinessTickRate = happinessTickRate;
        this.energyTickRate = energyTickRate;
        this.timePassed = timePassed;

        PetUIController.instance.UpdateMeters(food, happiness, energy, breedingCounter, age, timePassed);
    }

    public void Initialize(int food, int happiness, int energy, int age,
        int foodTickRate, int happinessTickRate, int energyTickRate,
        DateTime lastTimeFed, DateTime lastTimeHappy, DateTime lastTimeGainedEnergy, float timePassed)
    {
        this.lastTimeFed = lastTimeFed;
        this.lastTimeHappy = lastTimeHappy;
        this.lastTimeGainedEnergy = lastTimeGainedEnergy;
        this.timePassed = timePassed;

        this.food = food;
        this.happiness = happiness;
        this.energy = energy;
        this.age = age;
        this.foodTickRate = foodTickRate;
        this.happinessTickRate = happinessTickRate;
        this.energyTickRate = energyTickRate;

        PetUIController.instance.UpdateMeters(food, happiness, energy, breedingCounter, age, timePassed);
    }

    void Start()
    {
        timeManager = FindObjectOfType<TimeManager>();
    }

    private void Update()
    {
        if (TimeManager.gameHourTimer < 0)
        {
            ChangeFood(-foodTickRate);
            ChangeHappiness(-happinessTickRate);
            ChangeEnergy(-energyTickRate);
            timePassed = Mathf.FloorToInt(Time.timeSinceLevelLoad);

            PetUIController.instance.UpdateMeters(food, happiness, energy, breedingCounter, age, timePassed);
        }
        checkNeeds();
    }

    public void ChangeFood(int amount)
    {
        food += amount;
        if (amount > 0)
        {
            lastTimeFed = DateTime.Now;
        }
        else if (food > foodMax) { food = foodMax; }

        PetUIController.instance.UpdateMeters(food, happiness, energy, breedingCounter, age, timePassed);
    }

    public void PetGiveGoodFood()
    {
        // FOOD +20
        // 2% chance decrease in energyTickRate 
        // 2% chance increase in happinessTickRate 
        Debug.Log("pet ate good food");

        food += 40;
        probDecEnergyTickRate += 2;
        probIncHappinessTickRate += 2;

        ItemSlotController.instance.happinessValue.text = "+0";
        ItemSlotController.instance.energyValue.text = "+0";
        ItemSlotController.instance.foodValue.text = "+40";
        ItemSlotController.instance.energyTickRate.text = "2% decrease in EnergyTickRate";
        ItemSlotController.instance.happinessTickRate.text = "2% increase in HappinessTickRate";

        CheckForNeedChange(energyTickRate, probDecEnergyTickRate, probIncEnergyTickRate);
        CheckForNeedChange(happinessTickRate, probDecHappinessTickRate, probIncHappinessTickRate);

        Debug.Log("Chance of Energy Change");
        PetUIController.instance.UpdateMeters(food, happiness, energy, breedingCounter, age, timePassed);
    }

    public void PetGiveBadFood()
    {
        // FOOD +50
        // 2% chance increase energyTickRate
        // 2% chance decrease in happinessTickRate 
        Debug.Log("Pet ate bad food");

        food += 80;
        probIncEnergyTickRate += 2;
        probDecHappinessTickRate += 2;

        ItemSlotController.instance.happinessValue.text = "+0";
        ItemSlotController.instance.energyValue.text = "+0";
        ItemSlotController.instance.foodValue.text = "+80";
        ItemSlotController.instance.energyTickRate.text = "2% increase in EnergyTickRate";
        ItemSlotController.instance.happinessTickRate.text = "2% decrease in HappinessTickRate";

        CheckForNeedChange(energyTickRate, probDecEnergyTickRate, probIncEnergyTickRate);
        CheckForNeedChange(happinessTickRate, probDecHappinessTickRate, probIncHappinessTickRate);

        PetUIController.instance.UpdateMeters(food, happiness, energy, breedingCounter, age, timePassed);
    }

    public void ChangeHappiness(int amount)
    {
        happiness += amount;
        if (amount > 0)
        {
            lastTimeHappy = DateTime.Now;
        }
        else if (happiness > happinessMax) { happiness = happinessMax; }

        PetUIController.instance.UpdateMeters(food, happiness, energy, breedingCounter, age, timePassed);
    }

    public void PetTrain()
    {
        // HAPPINESS +40
        // ENERGY -10
        // 4% chance increase energyMax
        // 4% chance decrease energyTickRate
        Debug.Log("Pet is training");

        happiness += 40;
        energy -= 10;
        probIncEnergyMax += 4;
        probDecEnergyTickRate += 4;

        ItemSlotController.instance.foodValue.text = "+0";
        ItemSlotController.instance.happinessValue.text = "+40";
        ItemSlotController.instance.energyValue.text = "-10";
        ItemSlotController.instance.maxEnergy.text = "4% increase in Max Energy";
        ItemSlotController.instance.energyTickRate.text = "4% decrease in EnergyTickRate";

        CheckForTraitChange(energyMax, probDecEnergyMax, probIncEnergyMax);
        CheckForNeedChange(energyTickRate, probDecEnergyTickRate, probIncEnergyTickRate);

        PetUIController.instance.UpdateMeters(food, happiness, energy, breedingCounter, age, timePassed);
    }

    public void PetPlay()
    {
        // HAPPINESS +80
        // ENERGY -30
        // 2% chance increase energyMax
        // 2% chance decrease energyTickRate
        // 4% chance increase happinessMax
        Debug.Log("Pet is playing");

        happiness += 80;
        energy -= 30;
        probIncEnergyMax += 2;
        probDecEnergyTickRate += 2;
        probIncHappinessMax += 4;

        ItemSlotController.instance.foodValue.text = "+0";
        ItemSlotController.instance.happinessValue.text = "+80";
        ItemSlotController.instance.energyValue.text = "-30";
        ItemSlotController.instance.maxEnergy.text = "2% increase in Max Energy";
        ItemSlotController.instance.energyTickRate.text = "2% decrease in EnergyTickRate";
        ItemSlotController.instance.maxHappiness.text = "4% increase in Max Happiness";

        CheckForTraitChange(energyMax, probDecEnergyMax, probIncEnergyMax);
        CheckForNeedChange(energyTickRate, probDecEnergyTickRate, probIncEnergyTickRate);
        CheckForTraitChange(happinessMax, probDecHappinessMax, probIncHappinessMax);

        PetUIController.instance.UpdateMeters(food, happiness, energy, breedingCounter, age, timePassed);
    }

    public void ChangeEnergy(int amount)
    {
        energy += amount;
        if (amount > 0)
        {
            lastTimeGainedEnergy = DateTime.Now;
        }
        else if (energy > energyMax) { energy = energyMax; }

        PetUIController.instance.UpdateMeters(food, happiness, energy, breedingCounter, age, timePassed);
    }

    public void PetNap()
    {
        // ENERGY +20
        // 4% chance increase in energyTickRate 
        // 2% chance increase in happinessTickRate 
        // 2% chance increase in foodTickRate 
        Debug.Log("Pet is napping");

        energy += 30;
        probIncEnergyTickRate += 4;
        probIncHappinessTickRate += 2;
        probIncFoodTickRate += 2;

        ItemSlotController.instance.happinessValue.text = "+0";
        ItemSlotController.instance.foodValue.text = "+0";
        ItemSlotController.instance.energyValue.text = "+30";
        ItemSlotController.instance.energyTickRate.text = "4% increase in EnergyTickRate";
        ItemSlotController.instance.happinessTickRate.text = "2% increase in HappinessTickRate";
        ItemSlotController.instance.foodTickRate.text = "2% increase in FoodTickRate";

        CheckForNeedChange(energyTickRate, probDecEnergyTickRate, probIncEnergyTickRate);
        CheckForNeedChange(happinessTickRate, probDecHappinessTickRate, probIncHappinessTickRate);
        CheckForNeedChange(foodTickRate, probDecFoodTickRate, probIncFoodTickRate);

        PetUIController.instance.UpdateMeters(food, happiness, energy, breedingCounter, age, timePassed);
    }

    public void PetSleep()
    {
        // ENERGY MAX
        // HAPPINESS MAX
        // HUNGER LOW

        // pet is sleeping --- not moving and not interacting

        Debug.Log("Pet is sleeping");

        energy = energyMax;
        happiness = happinessMax;
        if(food > 30)
        {
            food = 30;
        }
        
        ItemSlotController.instance.energyValue.text = "MAX";
        ItemSlotController.instance.happinessValue.text = "MAX";
        ItemSlotController.instance.foodValue.text = "30";

        PetUIController.instance.UpdateMeters(food, happiness, energy, breedingCounter, age, timePassed);
    }

    public void CheckForTraitChange(int traitValue, int currentTraitDecChance, int currentTraitIncChance)
    {
        int randomNum = new System.Random().Next(1, 101);

        Debug.Log("RandomNum : " + randomNum);

        Debug.Log("Trait value: " + traitValue);
        Debug.Log("Chance Of Decrease: " + currentTraitDecChance);
        Debug.Log("Chance Of Increase: " + currentTraitIncChance);

        if (randomNum <= currentTraitDecChance)
        {
            if (traitValue != 120)
            {
                traitValue -= 20;
                Debug.Log("Trait " + traitValue + " downgraded...");
            }
            else
            {
                Debug.Log("Trait already MIN");

            }
        }
        else if (randomNum <= currentTraitIncChance)
        {
            if (traitValue != 200)
            {
                traitValue += 20;
                Debug.Log("Trait " + traitValue + " upgraded...");
            }
            else
            {
                Debug.Log("Trait already MAX");
            }
        }
        else
        {
            Debug.Log("Trait " + traitValue + " unchanged");
        }
    }

    public void CheckForNeedChange(int needValue, int currentNeedDecChance, int currentNeedIncChance)
    {
        int randomNum = new System.Random().Next(1, 101);

        Debug.Log("RandomNum : " + randomNum);

        Debug.Log("Need value: " + needValue);
        Debug.Log("Chance Of Decrease: " + currentNeedDecChance);
        Debug.Log("Chance Of Increase: " + currentNeedIncChance);

        if (randomNum <= currentNeedDecChance)
        {
            if(needValue != 1)
            {
                needValue -= 1;
                Debug.Log("Need " + needValue + " downgraded...");
            }
            else
            {
                Debug.Log("Need already MIN");

            }
        }
        else if (randomNum <= currentNeedIncChance)
        {
            if(needValue != 5)
            {
                needValue += 1;
                Debug.Log("Need " + needValue + " upgraded...");
            } 
            else
            {
                Debug.Log("Need already MAX");
            }
        }
        else
        {
            Debug.Log("Need " + needValue + " unchanged");
        }
    }

    public void AddAge()
    {
        Debug.Log("current age: " + age);
        age = age + 1;
        Debug.Log("AGE ADDED: " + age);
        PetUIController.instance.UpdateAge(age);
        checkAge();
    }

    public void checkNeeds()
    {
        if (food < 30)
        {
            if (food < 10)
            {
                notTakenCareOf += 1;
            }
            if (food < 0) { food = 0; }
            isHungry = true;
            PetUIController.instance.UpdateHungerState();
        }
        if (food > 30)
        {
            PetUIController.instance.RevertHungerState();
            isHungry = false;
        }

        if (happiness < 30)
        {
            if (happiness < 10)
            {
                notTakenCareOf += 1;
            }
            if (happiness < 0) { happiness = 0; }

            isSad = true;
            PetUIController.instance.UpdateHappinessState();
        }
        if (happiness > 30)
        {
            PetUIController.instance.RevertHappinessState();
            isSad = false;
        }

        if (energy < 30)
        {
            if (energy < 10)
            {
                notTakenCareOf += 1;
            }
            if (energy < 0) { energy = 0; }
            isTired = true;
            PetUIController.instance.UpdateEnergyState();
        }
        if (energy > 30)
        {
            PetUIController.instance.RevertEnergyState();
            isTired = false;
        }

        if (food == 0 && energy == 0 && happiness == 0)
        {
            Die();
        }
    }

    public void Die()
    {
        isDead = true;
        transform.rotation = Quaternion.Euler(0, 0, -90);
        timeManager.Pause();
        StartCoroutine(GameOverCoroutine());
    }

    IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSecondsRealtime(2);
        GameOverUI.SetActive(true);
    }

    public void checkAge()
    {
        if (age >= 0 && age < 5)
        {
            // child
            chanceOfDying = 1;
        }
        else if (age >= 5 && age < 15)
        {
            // teen
            chanceOfDying = 2;
        }
        else if (age >= 15 && age < 30)
        {
            // young adult
            chanceOfDying = 3;
        }
        else if (age >= 30 && age < 60)
        {
            // adult
            chanceOfDying = 5;
            if(breedingCounter > 2)
            {
                if(age > 40 && age < 50)
                {
                    breedingCounter = 2;
                }
                else if (age > 50 && age < 60)
                {
                    breedingCounter = 1;
                }
            }
            
        }
        else if (age >= 60 && age < 80)
        {
            // old
            chanceOfDying = 8;
            breedingCounter = 0;
        }
        else if (age >= 80)
        {
            // dying
            chanceOfDying = chanceOfDying + 2;
        } 
        else
        {
            Debug.Log("Invalid age...");
        }

        int randomNum = new System.Random().Next(0, 101);
        Debug.Log("randomNum: " + randomNum);
        Debug.Log("chanceOfDying: " + chanceOfDying);

        if (randomNum <= chanceOfDying)
        {
            DiedOfAge(age, chanceOfDying, notTakenCareOf);
        }
    }
     
    public void DiedOfAge(int currentAge, int chanceOfDying, int notTakenCareOf)
    {
        Debug.Log("Died at age: " + currentAge);
        Debug.Log("with a " + chanceOfDying + "% chance of dying");
        Debug.Log("and " + notTakenCareOf + " bad-care points");

        isDead = true;
        transform.rotation = Quaternion.Euler(0, 0, -90);
        timeManager.Pause();
        StartCoroutine(GameFinishedCoroutine());
    }

    IEnumerator GameFinishedCoroutine()
    {
        yield return new WaitForSecondsRealtime(2);
        PetDiedUI.SetActive(true);
    }
}
