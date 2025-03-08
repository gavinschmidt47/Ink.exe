using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    //Input
    private bool paused;


    //UI
    public GameObject pausePanel; 


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    //Called by InputAction "Pause"
    public void Pause(InputAction.CallbackContext context)
    {
        //If correct state pressed
        if (!context.started) return;

        //Freeze time scale, show UI, and set paused state
        if (paused)
        {
            Time.timeScale = 1;
            pausePanel.SetActive(false);
            paused = false;
        }
        else
        {
            Time.timeScale = 0;
            pausePanel.SetActive(true);
            paused = true;
        }
    }

    //Called by resume button in pause menu
    public void Resume()
    {
        //Freeze time scale, show UI, and set paused state
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        paused = false;
    }
}
