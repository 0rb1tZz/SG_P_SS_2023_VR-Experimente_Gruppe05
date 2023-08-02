using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangeTutorialElement : MonoBehaviour
{
    public GameObject menu;
    private List<GameObject> tutorialList = new List<GameObject>();
    private int currentIndex = 0;
    private GameObject currentTutorial;
    private float time = 0f;
    private TMP_Dropdown dropDown;

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

    public void ChangeTutorialToIndex(int index){
        tutorialList[currentIndex].SetActive(false);

        currentIndex = index;

        tutorialList[currentIndex].SetActive(true);
    }

    public void ChangeTutorialIndexBy(int changeValue){
        tutorialList[currentIndex].SetActive(false);

        currentIndex = (currentIndex + tutorialList.Count + changeValue) % tutorialList.Count;
        dropDown.value = currentIndex;

        tutorialList[currentIndex].SetActive(true);
    }

    public void GoBackToMenu(){
        this.gameObject.SetActive(false);
        menu.SetActive(true);
    }
}
