using UnityEngine;
using UnityEngine.InputSystem;

/*
A script to always spawn the menu in front of the player at a certain distance.
*/
public class GameMenuManager : MonoBehaviour
{
    public GameObject menu;
    public InputActionProperty showMenu;
    public Transform head;
    public float spawnDistance = 2;
    // Start is called before the first frame update
    void Start()
    {
        menu.transform.position = head.position + new Vector3(head.forward.x, 0, head.forward.z).normalized * spawnDistance;
        AudioListener.volume = 0.5f; // default Volume matching the Volume Slider, being on 0.5
    }

    // Update is called once per frame
    void Update()
    {
        if(showMenu.action.WasPressedThisFrame()){
            menu.SetActive(!menu.activeSelf);

            menu.transform.position = head.position + new Vector3(head.forward.x, 0, head.forward.z).normalized * spawnDistance;
        }

        menu.transform.LookAt(new Vector3(head.position.x, menu.transform.position.y, head.position.z));
        menu.transform.forward *= -1;
    }
}
