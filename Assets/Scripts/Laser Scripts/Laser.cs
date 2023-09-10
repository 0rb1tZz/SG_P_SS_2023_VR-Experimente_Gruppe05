using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    public Material laserMaterial;
    public float laserWavelength;
    public int checkpoints;
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