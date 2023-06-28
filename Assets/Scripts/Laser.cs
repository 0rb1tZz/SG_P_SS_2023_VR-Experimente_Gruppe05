using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    public Material laserMaterial;
    public float laserWavelength;
    LaserBeam beam;

    // Start is called before the first frame update
    void Start()
    {
        beam = new LaserBeam(gameObject, laserMaterial, laserWavelength);
    }

    // Update is called once per frame
    void Update()
    {
        beam.UpdateLaser();
    }
}
