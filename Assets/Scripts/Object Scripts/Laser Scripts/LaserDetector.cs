using UnityEngine;

/// <summary>
/// The script is attached to a detector and handles its behaviour
/// </summary>
public class LaserDetector : MonoBehaviour
{
    public float acceptedWavelength; // The wavelength of the light needed to activate the detector
    public int activationCount; // The amount of lasers that need to hit the detector to activate it
    public GameObject[] checkpointList; // A list of all checkpoints that need to be collected to be able the activate the laser
    public bool isActivated = false; // True if the detector got activated
    public GameObject hitCounterText; // A text field GameObject to display how many of the needed lasers to activate the detector are hitting it
    private HitCounter hitCounterTextScript; // The script of the text field GameObject
    private int hitCount = 0; // The count of how many lasers are hitting the detector
    private float time = 0f; // A variable to keep track of the time passed
    private float updateTime = 0.05f; // The rate at wich the detector checks if it it got activated (has to be the same as in the laser script)

    /// <summary>
    /// At the start:
    /// the color of the detector gets determined based on the wavelength
    /// the material is set
    /// each checkpoint affiliated to the detector gets set to its color
    /// if a 'hitCounterText' object exists, the corresponding script is determined
    /// </summary>
    void Start()
    {
        Color color = LaserHelperFunctions.RgbFromWavelength(acceptedWavelength);

        Material material = gameObject.GetComponent<Renderer>().material;
        material.color = color;

        foreach (GameObject checkpoint in checkpointList)
        {
            checkpoint.GetComponent<LaserCheckpoint>().acceptedWavelength = this.acceptedWavelength;
        }

        if(hitCounterText != null)
        {
            hitCounterTextScript = hitCounterText.GetComponent<HitCounter>();
        }
        
    }

    /// <summary>
    /// When the time gets bigger than updateTime, it is checked if all CPs are activated and if enough lasers hit the detector, if both is true it is set to be activated
    /// also time and hitcount get reset and the hitCounterText (if existing) will be set to display how many of the needed lasers hit the detector
    /// </summary>
    void Update()
    {
        time += Time.deltaTime;

        if (time > updateTime) // might want to set a global variable (internal update rate) since same time is also used in Laser.cs
        {

            bool allCheckpointsActive = true;

            foreach (GameObject checkpoint in checkpointList)
            {
                allCheckpointsActive = allCheckpointsActive && checkpoint.GetComponent<LaserCheckpoint>().isActivated;
            }
            
            if (hitCount >= activationCount && allCheckpointsActive)
            {
                isActivated = true;
            }
            else
            {
                isActivated = false;
            }

            if(hitCounterText != null){
                hitCounterTextScript.SetText(hitCount + "/" + activationCount);
            }

            time = 0f;
            hitCount = 0;
        }
    }

    /// <summary>
    /// If the detector got hit by a laser of the corresponding color, this function is called, increasing the hit count by one
    /// </summary>
    void HitByLaser()
    {
        hitCount++;
    }
}