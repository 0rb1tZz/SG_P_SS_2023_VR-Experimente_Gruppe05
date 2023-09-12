using UnityEngine;

/// <summary>
/// A script attached to a LaserCheckpoint
/// </summary>
public class LaserCheckpoint : MonoBehaviour
{
    public float acceptedWavelength = 0; // The wavelength needed to activate the checkpoint
    public bool isActivated = false; // True, if the checkpoint is currently activated
    private float time; // A variable keeping track of the time passed since the last hit by a laser
    private float resetTime = 0.1f; // The time until it is checked again if the CP is still active
    private Color glassColor; // The color of the glass of the detector

    /// <summary>
    /// At the start:
    /// the material color is set according to the wavelength
    /// time is set to resetTime, so that the CP is not activated at the beginning
    /// </summary>
    void Start()
    {
        Color color = LaserHelperFunctions.RgbFromWavelength(acceptedWavelength);

        Material material = gameObject.transform.GetChild(1).GetComponent<Renderer>().material;
        material.color = color;

        glassColor = gameObject.transform.GetChild(0).GetComponent<Renderer>().material.color;

        time = resetTime;
    }

    /// <summary>
    /// While 'time' (the time since the last hit by a laser) is less than resetTime, the CP's color will be the one of the laser, else it will be set to the default color
    /// </summary>
    void Update()
    {
        time += Time.deltaTime;

        Renderer glassRenderer = gameObject.transform.GetChild(0).GetComponent<Renderer>();
        if (time <= resetTime)
        {
            Color color = LaserHelperFunctions.RgbFromWavelength(acceptedWavelength);
            color.a = 0.5f;
            glassRenderer.material.color = color;

            isActivated = true;
        }
        else
        {
            glassRenderer.material.color = glassColor;

            isActivated = false;
        }
    }

    /// <summary>
    /// If the CP got hit by a laser of the corresponding color, this function is called, resetting the 'time' variable
    /// </summary>
    void HitByLaser()
    {
        time = 0;
    }
}
