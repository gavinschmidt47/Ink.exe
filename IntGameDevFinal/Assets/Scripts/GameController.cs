using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    //Input
    private bool paused;


    //UI
    [Header("UI")]
    public GameObject pausePanel; 


    //Game Stats
    [Header("Game Stats")]
    [Tooltip("How fast enemies spawn")]
    public float spawnRate;
    [Tooltip("Distance away from player spawns must be")]
    public float spawnShield;


    //Game Objects
    [Header("Game Objects")]
    public GameObject enemyPrefab;
    public GameObject player;

    private PlayerController playerController;
    private EnemyController currEnemy;


    public void Start()
    {
        //Set initial values
        Time.timeScale = 1;
        paused = false;
        pausePanel.SetActive(false);

        //Disable cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //Get PlayerController
        playerController = player.GetComponent<PlayerController>();

        //Start spawning
        InvokeRepeating("Spawn", 0, spawnRate);
    }

    //Called by InvokeRepeating
    public void Spawn()
    {
        //Spawn enemy
        Vector3 targetSpawn;
        float distanceToPlayer;
        int enemyType;

        //Ensure the enemy is not spawned too close to the player
        do
        {
            //Gets a random position around the player
            targetSpawn = new Vector3(player.transform.position.x + Random.Range(-10, 10), 0, player.transform.position.z + Random.Range(-10, 10));

            enemyType = GetRandomValue();

            //Gets the distance to the player
            distanceToPlayer = Vector3.Distance(targetSpawn, player.transform.position);
        } while (distanceToPlayer < spawnShield);

        currEnemy = Instantiate(enemyPrefab, targetSpawn, Quaternion.identity).GetComponent<EnemyController>();
        currEnemy.setEnemy(enemyType);
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
    
    private int GetRandomValue()
    {
        float random = Random.Range(0, 100.1f);
        if (random < 96.8f - (playerController.playerSave.level) * 0.99f)
        {
            return 0;
        }
        else if (random < 3.2f + (playerController.playerSave.level))
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }
}
