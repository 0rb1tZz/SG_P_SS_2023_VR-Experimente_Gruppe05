using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HitCounter : MonoBehaviour
{
    
    public Transform head;

    // Start is called before the first frame update
    void Start()
    {
        head = GameObject.FindWithTag("MainCamera").transform;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.LookAt(new Vector3(head.position.x, gameObject.transform.position.y, head.position.z));
        gameObject.transform.forward *= -1;
    }

    public void SetText(string textToSet){
        gameObject.GetComponent<TextMeshPro>().text = textToSet;
    }
}
