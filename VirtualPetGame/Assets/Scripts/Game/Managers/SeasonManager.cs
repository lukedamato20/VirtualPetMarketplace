using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonManager : MonoBehaviour
{
    public float FullSeason;
    public GameObject summerBackground;
    public GameObject autumnBackground;
    public GameObject winterBackground;
    public float fadeInTime = 1f;  // duration of the fade in effect
    public float fadeOutTime = 1f; // duration of the fade out effect

    private int currentBackground = 0;

    void Start()
    {
        InvokeRepeating("changeBackground", FullSeason, FullSeason);
    }

    public void changeBackground()
    {
        // SUMMER = 0
        // AUTUMN = 1
        // WINTER = 2

        switch (currentBackground)
        {
            case 0:
                StartCoroutine(CrossFade(summerBackground, autumnBackground, fadeInTime));
                StartCoroutine(FadeOutObject(winterBackground, fadeOutTime));
                Debug.Log("Starting Autumn");
                
                break;
            case 1:
                StartCoroutine(CrossFade(autumnBackground, winterBackground, fadeInTime));
                StartCoroutine(FadeOutObject(summerBackground, fadeOutTime));
                Debug.Log("Starting Winter");
                break;
            case 2:
                StartCoroutine(CrossFade(winterBackground, summerBackground, fadeInTime));
                StartCoroutine(FadeOutObject(autumnBackground, fadeOutTime));
                NeedsController.instance.AddAge(); 
                Debug.Log("Starting Summer");
                break;
        }

        currentBackground = (currentBackground + 1) % 3;
    }

    private IEnumerator FadeOutObject(GameObject obj, float time)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        Color color = renderer.material.color;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / time)
        {
            color.a = Mathf.Lerp(1f, 0f, t);
            renderer.material.color = color;
            yield return null;
        }
        color.a = 0f;
        renderer.material.color = color;
        obj.SetActive(false);
    }

    private IEnumerator CrossFade(GameObject oldObject, GameObject newObject, float fadeInTime)
    {
        // Set the new object to be transparent and active
        newObject.SetActive(true);
        SetObjectAlpha(newObject, 0f);

        // Fade in the new object and its children
        FadeObjectIn(newObject, fadeInTime);

        // Fade out the old object and its children
        FadeObjectOut(oldObject, fadeOutTime);

        // Wait for the fade-out to complete
        yield return new WaitForSeconds(fadeOutTime);

        // Activate the new object's children
        ActivateObjectChildren(newObject);

        // Deactivate the old object
        oldObject.SetActive(false);
    }

    private void FadeObjectIn(GameObject obj, float fadeInTime)
    {
        // Fade in the object and its children
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Color color = renderer.material.color;
            color.a = 0f;
            renderer.material.color = color;
            StartCoroutine(FadeIn(renderer, fadeInTime));
            renderer.gameObject.SetActive(true); // activate the child objects
        }
    }

    private void FadeObjectOut(GameObject obj, float fadeOutTime)
    {
        // Fade out the object and its children
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            StartCoroutine(FadeOut(renderer, fadeOutTime));
        }
    }

    private IEnumerator FadeIn(Renderer renderer, float fadeInTime)
    {
        // Fade in the renderer
        Color color = renderer.material.color;
        for (float t = 0f; t < 1f; t += Time.deltaTime / fadeInTime)
        {
            color.a = Mathf.Lerp(0f, 1f, t);
            renderer.material.color = color;
            yield return null;
        }
        color.a = 1f;
        renderer.material.color = color;
    }

    private IEnumerator FadeOut(Renderer renderer, float fadeOutTime)
    {
        // Fade out the renderer
        Color color = renderer.material.color;
        for (float t = 0f; t < 1f; t += Time.deltaTime / fadeOutTime)
        {
            color.a = Mathf.Lerp(1f, 0f, t);
            renderer.material.color = color;
            yield return null;
        }
        color.a = 0f;
        renderer.material.color = color;
    }

    private void SetObjectAlpha(GameObject obj, float alpha)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Color color = renderer.material.color;
            color.a = alpha;
            renderer.material.color = color;
        }
    }

    private void ActivateObjectChildren(GameObject obj)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.gameObject.SetActive(true);
        }
    }
}
