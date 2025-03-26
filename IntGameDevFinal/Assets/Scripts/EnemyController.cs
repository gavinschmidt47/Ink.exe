using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    //Player Components
    private GameObject player;
    private PlayerController playerController;

    //Enemy Sprite
    [Header("Enemy Sprites")]
    public Sprite clippy;
    public Sprite cone;
    public Sprite PC;

    //Enemy Components
    private NavMeshAgent agent;
    private SpriteRenderer spriteRenderer;


    //Enemy Stats
    private float health { get; set; }
    private float damage { get; set; }


    void Start()
    {
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
                health = 2;
                damage = 1;
                //spriteRenderer.sprite = clippy;
                break;

            //Cone
            case 1:
                health = 10;
                damage = 5;
                //spriteRenderer.sprite = cone;
                break;

            //PC
            case 2:
                health = 50;
                damage = 3;
                //spriteRenderer.sprite = PC;
                break;

            //default (Bug)
            default:
                Debug.Log("Random value not in range");
                break;
        }
    }

    public void Damage(float damage)
    {
        //Until Sprites are added, helps track enemy spawning
        Debug.Log("Dealing " + damage + " damage to " + health);

        //Deal amount of damage to the enemy
        health -= damage;
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
