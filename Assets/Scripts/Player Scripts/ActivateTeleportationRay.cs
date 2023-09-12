using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

/// <summary>
/// A script to handle the movement via teleportation
/// </summary>
public class ActivateTeleportationRay : MonoBehaviour
{
    
    public GameObject leftTeleportation; // The left teleportation ray object
    public GameObject rightTeleportation; // The right teleportation ray object

    public InputActionProperty leftActivate; // The input to teleport with the left controller
    public InputActionProperty rightActivate; // The input to teleport with the right controller

    public InputActionProperty leftCancel; // The input to cancel the teleportation with the left controller
    public InputActionProperty rightCancel; // The input to cancel the teleportation with the right controller

    public XRRayInteractor leftRay; // The left interaction ray object
    public XRRayInteractor rightRay; // The right interaction ray object

    /// <summary>
    /// Handles the teleportation for both controllers
    /// </summary>
    void Update()
    {
        bool isLeftRayHovering = leftRay.TryGetHitInfo(out Vector3 leftPos, out Vector3 leftNormal, out int leftNumber, out bool leftValid);
        bool isRightRayHovering = rightRay.TryGetHitInfo(out Vector3 rightPos, out Vector3 rightNormal, out int rightNumber, out bool rightValid);

        leftTeleportation.SetActive(!isLeftRayHovering && leftCancel.action.ReadValue<float>() == 0 && leftActivate.action.ReadValue<float>() > 0.1f);
        rightTeleportation.SetActive(!isRightRayHovering && rightCancel.action.ReadValue<float>() == 0 && rightActivate.action.ReadValue<float>() > 0.1f);
    }
}
