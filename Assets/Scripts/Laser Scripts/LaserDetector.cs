using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDetector : MonoBehaviour
{

    public float acceptedWavelength;
    public int activationCount;
    public GameObject[] checkpointList;
    public bool isActivated = false;
    private int hitCount = 0;
    private float time = 0f;
    private float resetTime = 0.05f;

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
        time += Time.deltaTime;

        if (time > resetTime) // might want to set a global variable (internal update rate) since same time is also used in Laser.cs
        {

            bool allCheckpointsActive = true;

            foreach (GameObject checkpoint in checkpointList)
            {
                allCheckpointsActive = allCheckpointsActive && checkpoint.GetComponent<LaserCheckpoint>().isActivated;
            }
            
            if (hitCount >= activationCount && allCheckpointsActive)
            {
                isActivated = true;
            }
            else
            {
                isActivated = false;
            }

            time = 0f;
            hitCount = 0;
        }
    }

    void HitByLaser()
    {
        hitCount++;
    }
}