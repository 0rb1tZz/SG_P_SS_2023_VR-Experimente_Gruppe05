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
        beam = new LaserBeam(gameObject.transform.position, gameObject.transform.forward, laserMaterial);
    }

    // Update is called once per frame
    void Update()
    {
        /*Destroy(GameObject.Find("Laser Beam"));
        Destroy(GameObject.Find("Beam"));
        */
        beam.UpdateRays();
    }
}
