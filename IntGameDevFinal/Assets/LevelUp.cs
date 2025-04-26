using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class LevelUp : MonoBehaviour
{
    public GameObject levelUp;
    public GameObject pausePanel; 
    
    private bool paused;
    private bool open = false;
    public static bool IsLevelOpen { get; private set; }
    public static bool IsPauseOpen { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelUp.SetActive(false); // Hide the panel at start
        //pausemenu.SetActive(false); // Hide the panel at start

        //Set initial values
        Time.timeScale = 1;
        paused = false;
        pausePanel.SetActive(false);
    }

    public void ToggleLevelUI()
    {
        open = !open;
        IsLevelOpen = open;
        levelUp.SetActive(open);
    }

    public void CloseLevelUI()
    {
        open = false;
        IsLevelOpen = false;
        levelUp.SetActive(false);
    }

    public void Pause(InputAction.CallbackContext context)
    {
        //If correct state pressed
        //if (!context.started) return;

        //Freeze time scale, show UI, and set paused state
        if (paused)
        {
            //Set unpaused values
            Time.timeScale = 1;
            pausePanel.SetActive(false);
            paused = false;

            //Disable cursor
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            //Set paused values
            Time.timeScale = 0;
            pausePanel.SetActive(true);
            paused = true;

            //Enable cursor
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Pause(new InputAction.CallbackContext()); // Call the Pause method with a new context
        }
    }
}
