using UnityEngine;

/// <summary>
/// A script attached to a laser source generating a laser beam
/// Also handles the activation status of the laser source
/// </summary>
public class Laser : MonoBehaviour
{

    public Material laserMaterial; // The material of the laser beam
    public float laserWavelength; // The wavelength of the light emmited by the laser source
    LaserBeam beam; // The laser beam object
    public GameObject[] detectorList; // A list of all detectors that need to be active for the laser source to emmit a laser beam
    private bool isActivated = false; // True, if the laser source is active, False otherwise
    private float time = 0f; // A variable keeping track of the time passed
    private float updateTime = 0.05f; // The time intervals at which the laser is updated (has to be the same as in the laser detector script)

    /// <summary>
    /// At the start, a laser beam is initialized
    /// </summary>
    void Start()
    {
        beam = new LaserBeam(gameObject, laserMaterial, laserWavelength);
    }

    /// <summary>
    /// If the laser already got activated, the beam will be updated every 'updateTime', otherwise it will be checked if the laser can be set to be active every frame
    /// </summary>
    void Update()
    {
        if(isActivated){
            time += Time.deltaTime;
            if (time > 0.05f)
            {
                time = 0f;
                beam.UpdateLaser();
            }
        }else{
            isActivated = true;

            foreach (GameObject detector in detectorList)
            {
                isActivated = isActivated && detector.GetComponent<LaserDetector>().isActivated;
            }
        }
    }
}