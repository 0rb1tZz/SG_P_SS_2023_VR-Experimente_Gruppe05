using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// A script to animate the player's hands based on the controller input
/// </summary>
public class AnimateHandOnInput : MonoBehaviour
{
    public InputActionProperty pinchAnimationAction; // The input to trigger the pinch animation
    public InputActionProperty gripAnimationAction; // The input to trigger the grip animation
    public Animator handAnimator; // The hand animator component

    /// <summary>
    /// Checks if the pinch or grip buttons are pressed, and animates the player's hands according to those inputs
    /// </summary>
    void Update()
    {
        float triggerValue = pinchAnimationAction.action.ReadValue<float>();
        handAnimator.SetFloat("Trigger", triggerValue);
        
        float gripValue = gripAnimationAction.action.ReadValue<float>();
        handAnimator.SetFloat("Grip", gripValue);
    }
}
