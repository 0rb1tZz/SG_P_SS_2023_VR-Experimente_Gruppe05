using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    public Material laserMaterial;
    LaserBeam beam;

    // Start is called before the first frame update
    void Start()
    {
        beam = new LaserBeam(gameObject, laserMaterial);
    }

    // Update is called once per frame
    void Update()
    {
        beam.UpdateLaser();
    }
}
