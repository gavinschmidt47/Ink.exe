using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LevelUp : MonoBehaviour
{
    public GameObject levelUp;
    public GameObject xpBar;
    public GameObject levelPrompt;
    public GameObject pausePanel;
    public Slider hpBar;
    
    public bool xpFull;

    private bool paused;
    private bool level;
    private bool open = false;
    public static bool IsLevelOpen { get; private set; }
    public static bool IsPauseOpen { get; private set; }
    public static int IsXPFull { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelUp.SetActive(false); // Hide the panel at start
        levelPrompt.SetActive(false); // Hide the panel at start

        //Set initial values
        Time.timeScale = 1;
        paused = false;
        pausePanel.SetActive(false);

        StartCoroutine(HealthTracker());
    }

    public void ActivateLevelPrompt()
    {
        level = true;
        levelPrompt.SetActive(true);
    }

    public void ToggleLevelUI()
    {
        open = !open;
        IsLevelOpen = open;
        levelUp.SetActive(open);
        levelPrompt.SetActive(false);
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

    public void LevelOpen(InputAction.CallbackContext context)
    {
        //If correct state pressed
        //if (!context.started) return;

        //Freeze time scale, show UI, and set level state
        if (level)
        {
            //Set unpaused values
            Time.timeScale = 1;
            levelUp.SetActive(false);
            level = false;

            //Disable cursor
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Debug.Log("Level Up Opened");
            //Set level values
            Time.timeScale = 0;
            levelUp.SetActive(true);
            levelPrompt.SetActive(false);
            level = true;

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

        if(xpFull)
        {
            ActivateLevelPrompt();
        }

        if(Input.GetKeyDown(KeyCode.Space) && !level)
        {
            LevelOpen(new InputAction.CallbackContext()); // Call the LevelOpen method with a new context
        }
    }

    IEnumerator HealthTracker()
    {
        while(hpBar.value > 0)
        {
            yield return new WaitUntil(() => hpBar.value <= 0);
        }

        DeathScreen();
    }

    public void DeathScreen()
    {
        SceneManager.LoadScene(2);
    }
}
