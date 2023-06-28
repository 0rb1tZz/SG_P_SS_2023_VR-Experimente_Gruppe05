using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrismMaterialScript : MonoBehaviour
{

    public Material material;

    void Start()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            gameObject.transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().material = material;
        }
    }

}
