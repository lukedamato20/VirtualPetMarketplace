using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedsPageSwitcher : MonoBehaviour
{
    public GameObject[] needsCategories;
    private int currentIndex = 0;

    // 0: hunger panel
    // 1: happiness panel
    // 2: energy panel

    // Update is called once per frame
    public void NextButton()
    {
        needsCategories[currentIndex].SetActive(false);

        currentIndex++;

        Debug.Log("Next Button: " + currentIndex);
        if (currentIndex == 3)
        {
            currentIndex = 0;
        }

        needsCategories[currentIndex].SetActive(true);
    }

    public void PrevButton() 
    {
        needsCategories[currentIndex].SetActive(false);

        currentIndex--;
        Debug.Log("Prev Button: " + currentIndex);

        if (currentIndex == -1)
        {
            currentIndex = 2;
        }

        needsCategories[currentIndex].SetActive(true);
    }
}
