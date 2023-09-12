using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// A script to handle the change and display of the tutorials in the tutorial overview
/// </summary>
public class ChangeTutorialElement : MonoBehaviour
{
    public GameObject menu; // The menu from which the tutorial tab is opened
    private List<GameObject> tutorialList = new List<GameObject>(); // A list containing all tutorial panels
    private int currentIndex = 0; // The index of the currently shown tutorial panel
    private TMP_Dropdown dropDown; // A dropdown list to select a tutorial to look at

    /// <summary>
    /// At the start:
    /// The 'tutorialList' is filled with all the tutorial objects
    /// For each tutorial, a corresponding entry is added to the dropdown list
    /// The currently shown tutorial is set to the first tutorial in the list
    /// </summary>
    void Start(){
        Transform allTutorials = this.transform.GetChild(2);
        for(int i = 0; i < allTutorials.childCount; i++){
            tutorialList.Add(allTutorials.GetChild(i).gameObject);
        }

        List<string> optionList = new List<string>();
        foreach(GameObject tut in tutorialList){
            optionList.Add(tut.name);
        }
        dropDown = this.transform.GetChild(3).GetComponent<TMP_Dropdown>();
        dropDown.AddOptions(optionList);
        
        if(tutorialList.Count != 0){
            tutorialList[0].SetActive(true);
        }
    }

    /// <summary>
    /// Changes the shown tutorial panel to the one with the specified index
    /// </summary>
    /// <param name="index">The index of the new tutorial</param>
    public void ChangeTutorialToIndex(int index){
        tutorialList[currentIndex].SetActive(false);

        currentIndex = index;

        tutorialList[currentIndex].SetActive(true);
    }
    
    /// <summary>
    /// Changes the shown tutorial to be the one of the current index shifted by a given value
    /// </summary>
    /// <param name="changeValue">The value by wich the current index is shifted</param>
    public void ChangeTutorialIndexBy(int changeValue){
        tutorialList[currentIndex].SetActive(false);

        currentIndex = (currentIndex + tutorialList.Count + changeValue) % tutorialList.Count;
        dropDown.value = currentIndex;

        tutorialList[currentIndex].SetActive(true);
    }

    /// <summary>
    /// A function to return to the menu
    /// </summary>
    public void GoBackToMenu(){
        this.gameObject.SetActive(false);
        menu.SetActive(true);
    }
}
