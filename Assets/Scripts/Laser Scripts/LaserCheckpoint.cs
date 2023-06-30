using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LaserCheckpoint : MonoBehaviour
{

    public float acceptedWavelength;

    // Start is called before the first frame update
    void Start()
    {
        Color color = LaserHelperFunctions.RgbFromWavelength(acceptedWavelength);
        color.a = 0.4f;

        Material material = gameObject.transform.GetChild(0).GetComponent<Renderer>().material;
        material.color = color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void HitByLaser()
    {

    }
}
