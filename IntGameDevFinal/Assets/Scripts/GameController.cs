using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    //Input
    private bool paused;


    //UI
    public GameObject pausePanel; 


    //Game Stats
    public float spawnRate;
    public float spawnShield;


    //Game Objects
    public GameObject enemyPrefab;
    public GameObject player;


    public void Start()
    {
        //Set initial values
        Time.timeScale = 1;
        paused = false;
        pausePanel.SetActive(false);

        //Disable cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //Start spawning
        InvokeRepeating("Spawn", 0, spawnRate);
    }

    //Called by InvokeRepeating
    public void Spawn()
    {
        //Spawn enemy
        Vector3 targetSpawn;
        float distanceToPlayer;

        //Ensure the enemy is not spawned too close to the player
        do
        {
            //Gets a random position around the player
            targetSpawn = new Vector3(player.transform.position.x + Random.Range(-10, 10), 0, player.transform.position.z + Random.Range(-10, 10));

            //Gets the distance to the player
            distanceToPlayer = Vector3.Distance(targetSpawn, player.transform.position);
        } while (distanceToPlayer < spawnShield);

        Instantiate(enemyPrefab, targetSpawn, Quaternion.identity);
    }

    //Called by InputAction "Pause"
    public void Pause(InputAction.CallbackContext context)
    {
        //If correct state pressed
        if (!context.started) return;

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

    //Called by resume button in pause menu
    public void Resume()
    {
        //Freeze time scale, show UI, and set paused state
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        paused = false;
    }
}
