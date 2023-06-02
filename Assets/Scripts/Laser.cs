using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public Material material;
    public float beamSizeFactor;
    private MeshRenderer renderer;
    private LaserBeam laserBeam;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<MeshRenderer>();
        float width = renderer.bounds.size.x / beamSizeFactor;
        laserBeam = new LaserBeam(gameObject, transform.position, transform.forward, width, width, Color.red, Color.red, material);
    }

    // Update is called once per frame
    void Update()
    {
        laserBeam.UpdateRays();
    }
}
