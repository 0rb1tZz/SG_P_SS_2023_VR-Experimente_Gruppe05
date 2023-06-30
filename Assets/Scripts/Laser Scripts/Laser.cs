using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    public Material laserMaterial;
    public float laserWavelength;
    public int checkpoints;
    LaserBeam beam;

    private float time = 0f;

    // Start is called before the first frame update
    void Start()
    {
        beam = new LaserBeam(gameObject, laserMaterial, laserWavelength, checkpoints);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > 0.05f)
        {
            time = 0f;
            beam.UpdateLaser();
        }
    }
}
