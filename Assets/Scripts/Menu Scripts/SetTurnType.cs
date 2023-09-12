using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// A script to change the turn type
/// </summary>
public class SetTurnType : MonoBehaviour
{
    public ActionBasedContinuousTurnProvider continuousTurn; // The continuous turn provider
    public ActionBasedSnapTurnProvider snapTurn; // The snap turn provider

    /// <summary>
    /// Changes the turn type based on the given index
    /// </summary>
    /// <param name="index">The index the turn type is changed to</param>
    public void setTypeFromIndex(int index){
        if(index == 0){
            snapTurn.enabled = true;
            continuousTurn.enabled = false;
        } else if (index == 1){
            snapTurn.enabled = false;
            continuousTurn.enabled = true;
        }
    }
}
