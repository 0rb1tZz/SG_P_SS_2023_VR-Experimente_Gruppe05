using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public GameObject door;
    public GameObject[] detectorList;
    private bool completed = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!completed)
        {
            bool levelCompleted = true;

            foreach (GameObject detector in detectorList)
            {
                levelCompleted = levelCompleted && detector.GetComponent<LaserDetector>().isActivated;
            }

            if (levelCompleted)
            {
                completed = true;
                FinishLevel();
            }
        }
    }

    public void FinishLevel()
    {
        if (door != null)
        {
            door.transform.GetChild(0).gameObject.SetActive(false);
            door.GetComponent<AudioSource>().Play();
        }
    }
}
