using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SetTurnType : MonoBehaviour
{
    public ActionBasedContinuousTurnProvider continuousTurn;
    public ActionBasedSnapTurnProvider snapTurn;
    public GameObject tutorial;

    /*
    Handles the selection of the preferred turn type from a drop-down menu.
    */
    public void setTypeFromIndex(int index){
        if(index == 0){
            snapTurn.enabled = true;
            continuousTurn.enabled = false;
        } else if (index == 1){
            snapTurn.enabled = false;
            continuousTurn.enabled = true;
        }
    }

    /*
    Switches to the tutorial screen, disabling the main menu screen.
    */
    public void OpenTutorial(){
        this.gameObject.SetActive(false);
        tutorial.SetActive(true);
    }
}
