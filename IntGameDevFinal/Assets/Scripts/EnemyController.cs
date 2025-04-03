using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    //Player Components
    private GameObject player;
    private PlayerController playerController;

    //Enemy Sprite
    [Header("Enemy Sprites")]
    public GameObject power;
    public GameObject cone;
    public GameObject PC;

    //Enemy Components
    [Header("Enemy Components")]
    public GameObject healthBarObject;

    private HealthBar healthBar;
    private NavMeshAgent agent;
    private SpriteRenderer spriteRenderer;


    //Enemy Stats
    private float maxHealth { get; set; }
    private float health { get; set; }
    private float damage { get; set; }


    void Start()
    {
        //Gets the HealthBar component
        healthBar = healthBarObject.GetComponent<HealthBar>();

        //Gets the Navmesh Agent component
        agent = GetComponent<NavMeshAgent>();

        //Gets SpriteRenderer
        //spriteRenderer = GetComponent<spriteRenderer>();

        //Sets the player to the player object
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        //Sets the destination of the Navmesh Agent to the player's position
        agent.SetDestination(player.transform.position);
    }

    void OnTriggerEnter(Collider collider)
    {
        //If the enemy collides with the player, destroy the player
        if (collider.gameObject.CompareTag("Player"))
        {
            //Take damage from enemy
            playerController.playerSave.health -= damage;
            playerController.hpBar.value = playerController.playerSave.health;

            //Temporary, destroys enemy
            Destroy(gameObject);
        }
    }

    public void setEnemy(int type)
    {
        //Sets values for one of three types
        switch (type)
        {
            //Clippy
            case 0:
                maxHealth = 2; 
                damage = 1;
                power.SetActive(true);
                break;

            //Cone
            case 1:
                maxHealth = 10;
                damage = 5;
                //spriteRenderer.sprite = cone;
                break;

            //PC
            case 2:
                maxHealth = 50;
                damage = 3;
                //spriteRenderer.sprite = PC;
                break;

            //default (Bug)
            default:
                Debug.Log("Random value not in range");
                break;
        }
        //Sets health to maxHealth
        health = maxHealth;

        //Sets the health bar to the max health
        healthBar.SetHealth(health, maxHealth);
    }

    public void Damage(float damage)
    {
        //Until Sprites are added, helps track enemy spawning
        Debug.Log("Dealing " + damage + " damage to " + health);

        //Deal amount of damage to the enemy
        health -= damage;

        //Update the health bar
        healthBar.SetHealth(health, maxHealth);

        if (health <= 0)
        {
            //If the enemy's health is less than or equal to 0, destroy the enemy and give xp
            playerController.playerSave.xp += 1;
            playerController.xpBar.value = playerController.playerSave.xp;

            //If enough xp to level up
            if (playerController.playerSave.xp >= playerController.playerSave.xpToLevel) 
            {
                playerController.playerSave.canLevelUp = true;

                playerController.levelUpPrompt.SetActive(true);
                
                playerController.StartCoroutine(playerController.LevelingUI());
            }

            Destroy(gameObject);
        }
    }
}
