using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LaserCheckpoint : MonoBehaviour
{

    public float acceptedWavelength;
    private float time = 0.1f;
    private float resetTime = 0.1f;

    private Color glassColor;

    // Start is called before the first frame update
    void Start()
    {
        Color color = LaserHelperFunctions.RgbFromWavelength(acceptedWavelength);

        Material material = gameObject.transform.GetChild(1).GetComponent<Renderer>().material;
        material.color = color;

        glassColor = gameObject.transform.GetChild(0).GetComponent<Renderer>().material.color;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        Renderer glassRenderer = gameObject.transform.GetChild(0).GetComponent<Renderer>();
        if (time < resetTime)
        {
            Color color = LaserHelperFunctions.RgbFromWavelength(acceptedWavelength);
            color.a = 0.5f;
            glassRenderer.material.color = color;
        }
        else
        {
            glassRenderer.material.color = glassColor;
        }
    }

    void HitByLaser()
    {
        time = 0;
    }
}
