using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// A script attached to the game menu
/// </summary>
public class GameMenuManager : MonoBehaviour
{
    public GameObject menu; // The game menu itself
    public InputActionProperty showMenu; // The input action required to display the menu in game
    public Transform head; // The main camera of the player
    public float spawnDistance = 2; // The distance, the menu spawns from the 'head'
    public GameObject tutorial; // The tutorial display panel
    public GameObject mainMenu; // The main menu panel
    
    /// <summary>
    /// At the start, the menu gets angled toward 'head' and the game audio is set to 0.5
    /// </summary>
    void Start()
    {
        menu.transform.position = head.position + new Vector3(head.forward.x, 0, head.forward.z).normalized * spawnDistance;
        AudioListener.volume = 0.5f; // The audio listener is set to a default volume of 0.5, matching the volume slider in the game menu (can't be performed in the AudioManager script since it is not active at game start)
    }

    /// <summary>
    /// Every frame, if the menu-input was pressed, the menu will either show or 'unshow' and the menu will be angled toward 'head'
    /// </summary>
    void Update()
    {
        if(showMenu.action.WasPressedThisFrame()){
            menu.SetActive(!menu.activeSelf);

            menu.transform.position = head.position + new Vector3(head.forward.x, 0, head.forward.z).normalized * spawnDistance;
        }

        menu.transform.LookAt(new Vector3(head.position.x, menu.transform.position.y, head.position.z));
        menu.transform.forward *= -1;
    }

    /// <summary>
    /// Switches to the tutorial screen, disabling the main menu screen
    /// </summary>
    public void OpenTutorial(){
        mainMenu.SetActive(false);
        tutorial.SetActive(true);
    }
}
