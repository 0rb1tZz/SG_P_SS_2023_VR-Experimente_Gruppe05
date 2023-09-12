using UnityEngine;

/*
A script to spawn and update a laser beam from a laser source
Also handles the activation status of the laser source.
*/
public class Laser : MonoBehaviour
{

    public Material laserMaterial;
    public float laserWavelength;
    LaserBeam beam;
    public GameObject[] detectorList;
    private bool isActivated = false;

    private float time = 0f;

    // Start is called before the first frame update
    void Start()
    {
        beam = new LaserBeam(gameObject, laserMaterial, laserWavelength);
    }

    // Update is called once per frame
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