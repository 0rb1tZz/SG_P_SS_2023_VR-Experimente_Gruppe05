using TMPro;
using UnityEngine;

/// <summary>
/// A script attached to a 'panel' (text field), to change its displayed content
/// </summary>
public class HitCounter : MonoBehaviour
{
    public Transform head; // The position the panel will be angled to

    /// <summary>
    /// At the start, setting the main camera as the head
    /// </summary>
    void Start()
    {
        head = GameObject.FindWithTag("MainCamera").transform;
    }

    /// <summary>
    /// Every frame, the orientation of the panel is updated to face the 'head'
    /// </summary>
    void Update()
    {
        gameObject.transform.LookAt(new Vector3(head.position.x, gameObject.transform.position.y, head.position.z));
        gameObject.transform.forward *= -1;
    }

    /// <summary>
    /// A function to replace the text on the panel with a new one
    /// </summary>
    /// <param name="textToSet">The new text to display on the panel</param>
    public void SetText(string textToSet){
        gameObject.GetComponent<TextMeshPro>().text = textToSet;
    }
}
