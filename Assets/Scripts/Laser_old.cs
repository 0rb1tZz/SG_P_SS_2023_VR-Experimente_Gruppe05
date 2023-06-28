using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser_old : MonoBehaviour
{
    public Material material;
    public float beamSizeFactor = 3;
    private MeshRenderer renderer;
    public LaserBeam_old laserBeam;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<MeshRenderer>();
        float width = renderer.bounds.size.x / beamSizeFactor;
        //laserBeam = new LaserBeam(gameObject, width, width, Color.red, Color.red, material);
        laserBeam = new LaserBeam_old(gameObject, new List<float>(), width, gameObject.transform.position, gameObject.transform.forward);
    }

    // Update is called once per frame
    void Update()
    {
        //laserBeam.UpdateLaser();
    }
}
