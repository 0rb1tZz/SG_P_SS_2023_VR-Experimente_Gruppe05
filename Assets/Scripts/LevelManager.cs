using UnityEngine;

/// <summary>
/// A script attached to each 'level' (object that contains everything of a level), handeling its completion
/// </summary>
public class LevelManager : MonoBehaviour
{

    public GameObject door; // The door that connects the current level to the next level
    public GameObject[] detectorList; // All the detectors that need to be activated to open the door
    private bool completed = false; // A variable, that is true if the level is completed

    /// <summary>
    /// Each frame, while the level is not completed, it is checked if all detectors are activated, if that is the case, the level is set as completed
    /// </summary>
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

    /// <summary>
    /// A function that gets called when a level is finished, opening the door and playing a corresponding sound
    /// </summary>
    public void FinishLevel()
    {
        if (door != null)
        {
            door.transform.GetChild(0).gameObject.SetActive(false);
            door.GetComponent<AudioSource>().Play();
        }
    }
}
