using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDetector : MonoBehaviour
{

    public float acceptedWavelength;
    public int level;

    // Start is called before the first frame update
    void Start()
    {
        Color color = LaserHelperFunctions.RgbFromWavelength(acceptedWavelength);

        Material material = gameObject.GetComponent<Renderer>().material;
        material.color = color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void HitByLaser()
    {
        Debug.Log("HIT BY LASER!");
        LevelManager.FinishLevel(level);
    }
}
