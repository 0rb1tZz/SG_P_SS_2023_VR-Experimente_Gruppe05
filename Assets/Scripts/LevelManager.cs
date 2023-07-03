using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void FinishLevel(int level)
    {
        if (level == 1)
        {
            GameObject door = GameObject.Find("Door_Lvl_2");
            if (door != null)
            {
                door.SetActive(false);
            }
        }
        else if (level == 2)
        {
            GameObject door = GameObject.Find("Door_Lvl_3");
            if (door != null)
            {
                door.SetActive(false);
            }
        }
    }
}
