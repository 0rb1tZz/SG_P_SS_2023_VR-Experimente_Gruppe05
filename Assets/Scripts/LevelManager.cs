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

    public static void FinishFirstLevel()
    {
        GameObject door = GameObject.Find("Door_Lvl_2");
        if (door != null)
        {
            door.SetActive(false);
        }
    }
}
