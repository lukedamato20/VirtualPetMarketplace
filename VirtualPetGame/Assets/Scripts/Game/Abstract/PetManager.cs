using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PetManager : MonoBehaviour
{
    public NeedsController needsController;
    private TimeManager timeManager;

    private GameObject playerPrefab;
    private GameObject playerInstance;
    public GameObject interactUI;

    Vector3 startingPosition = new Vector3(276, 742, 10);

    int currentHungerLevel = 0, currentHappinessLevel = 0, currentEnergyLevel = 0;
    int currentAge = 0;
    int currentBreedingCounter = 0;

    bool ableToInteract;

    public static PetManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Debug.LogWarning("More than one PetManager in the Scene");
    }

    void Start()
    {
        NeedsController needsController = GetComponent<NeedsController>();
        ableToInteract = false;

        GameObject interactUI = GetComponent<GameObject>();

        timeManager = FindObjectOfType<TimeManager>();
    }

    public void UpdateTraits(int newMaxFood, int newMaxHappiness, int newMaxEnergy, int newBreedingCounter)
    {
        needsController.foodMax = newMaxFood;
        needsController.happinessMax = newMaxHappiness;
        needsController.energyMax = newMaxEnergy;
        needsController.breedingCounter = newBreedingCounter;
    }

    public void UpdateNeeds(int newFoodTickRate, int newHappinessTickRate, int newEnergyTickRate)
    {
        needsController.foodTickRate = newFoodTickRate;
        needsController.happinessTickRate = newHappinessTickRate;
        needsController.energyTickRate = newEnergyTickRate;
    }

    public bool readyToInteract()
    {
        currentHungerLevel = needsController.food;
        currentHappinessLevel = needsController.happiness;
        currentEnergyLevel = needsController.energy;
        currentAge = needsController.age;
        currentBreedingCounter = needsController.breedingCounter;
        
        ableToInteract = true;

        updatePlayer();
        startInteract();
        return ableToInteract;
    }

    private void updatePlayer()
    {
        GameObject playerPrefab = GameObject.Find("Player");
        Vector3 scale = new Vector3(83.4f, 83.4f, 83.4f);

        if (playerPrefab != null)
        {
            Destroy(playerPrefab);
        }

        playerPrefab = Instantiate(playerPrefab, startingPosition, Quaternion.identity);
        playerPrefab.name = "Player";

        foreach (Behaviour component in playerPrefab.GetComponentsInChildren<Behaviour>())
        {
            component.enabled = true;
        }

        GameObject interactUI = GameObject.Find("StartInteractUI");

        // Set the new object as a child of the parent object
        GameObject parentObject = GameObject.Find("BreedingContainer");
        playerPrefab.transform.SetParent(parentObject.transform);
        playerPrefab.transform.localScale = scale;

        SmoothCameraFollow cameraMovement = Camera.main.GetComponent<SmoothCameraFollow>();
        cameraMovement.target = playerPrefab.transform;
    }

    private void startInteract()
    {
        //timeManager.Pause();

        interactUI.SetActive(true);
    }

}
