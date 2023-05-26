using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ItemSlotController : MonoBehaviour, IDropHandler
{
    public SpriteRenderer petRenderer;

    public Text foodValue;
    public Text happinessValue;
    public Text energyValue;
    public Text foodTickRate;
    public Text happinessTickRate;
    public Text energyTickRate;
    public Text maxFood;
    public Text maxHappiness;
    public Text maxEnergy;

    public GameObject food1Object;
    public GameObject food2Object;
    public GameObject energy1Object;
    public GameObject energy2Object;
    public GameObject happiness1Object;
    public GameObject happiness2Object;

    public GameObject NotifUI;
    private Color petColor;

    public static ItemSlotController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Debug.LogWarning("More than one ItemSlotController in the Scene");
    }

    public void Start()
    {
        changePetColour(NeedsController.instance.petColourValues);
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
        RectTransform draggedRectTransform = eventData.pointerDrag.GetComponent<RectTransform>();

        if (foodValue.text == "" && happinessValue.text == "" && energyValue.text == "" && foodTickRate.text == "" && happinessTickRate.text == "" && energyTickRate.text == "" && maxFood.text == "" && maxHappiness.text == "" && maxEnergy.text == "")
        {
            NotifUI.SetActive(false);
        }
        else
        {
            NotifUI.SetActive(true);
        }

        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            // Fade out the dragged object
            StartCoroutine(FadeOutAndDeactivate(draggedRectTransform.gameObject));
        }
    }

    private IEnumerator FadeOutAndDeactivate(GameObject draggedObject)
    {
        CanvasGroup canvasGroup = draggedObject.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = draggedObject.AddComponent<CanvasGroup>();
        }

        // Wait for 1 second before starting to fade out
        yield return new WaitForSecondsRealtime(1f);

        // Fade out the dragged object over 1 second
        float duration = 1f;
        float t = 0f;
        while (t < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t / duration);
            t += Time.unscaledDeltaTime;
            yield return null;
        }

        // Deactivate the dragged object
        draggedObject.SetActive(false);
        NotifUI.SetActive(true);

        if (draggedObject.tag == "GoodFood")
        {
            NeedsController.instance.PetGiveGoodFood();
        }
        else if (draggedObject.tag == "BadFood")
        {
            NeedsController.instance.PetGiveBadFood();
        }
        else if (draggedObject.tag == "Play")
        {
            NeedsController.instance.PetPlay();
        }
        else if (draggedObject.tag == "Train")
        {
            NeedsController.instance.PetTrain();
        }
        else if (draggedObject.tag == "Nap")
        {
            NeedsController.instance.PetNap();
        }
        else if (draggedObject.tag == "Sleep")
        {
            NeedsController.instance.PetSleep();
        }

        // Wait for 5 seconds before reactivating the object and resetting its position
        yield return new WaitForSecondsRealtime(2f);

        draggedObject.SetActive(true);
        canvasGroup.alpha = 1f;

        if (draggedObject.tag == "GoodFood" || draggedObject.tag == "Play" || draggedObject.tag == "Nap")
        {
            draggedObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-74.00053f, -305f);
            activateObjects();
        }
        else if (draggedObject.tag == "BadFood" || draggedObject.tag == "Train" || draggedObject.tag == "Sleep")
        {
            draggedObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(85.99943f, -305f);
            activateObjects();
        }
        else
        {
            draggedObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            activateObjects();
        }
    }

    public void cleanText()
    {
        foodValue.text = "";
        happinessValue.text = "";
        energyValue.text = "";
        foodTickRate.text = "";
        happinessTickRate.text = "";
        energyTickRate.text = "";
        maxFood.text = "";
        maxHappiness.text = "";
        maxEnergy.text = "";
    }

    public void activateObjects()
    {
        food1Object.SetActive(true);
        food2Object.SetActive(true);
        energy1Object.SetActive(true);
        energy2Object.SetActive(true); 
        happiness1Object.SetActive(true); 
        happiness2Object.SetActive(true); 
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

}
