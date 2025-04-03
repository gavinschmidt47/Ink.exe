using System.Collections;
using System.Reflection;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    //Player Components
    private GameObject player;
    private PlayerController playerController;

    //Enemy Sprite
    [Header("Enemy Objects")]
    public GameObject power;
    public GameObject cone;
    public GameObject PC;
    [Tooltip("Projectile for cone")]
    public GameObject mousePointer;

    //Enemy Components
    [Header("Enemy Components")]
    public GameObject healthBarObject;

    private HealthBar healthBar;
    private NavMeshAgent agent;


    //Enemy Stats
    [Header("Enemy Stats")]
    [Header("Archer")]
    [Range(0, 100)]
    public float moveDistance = 25f;
    [Range(0, 10)]
    public float attackTime = 1f;
    [Range(0, 10)]
    public float rotationTime = 1f;

    private float maxHealth { get; set; }
    private float health { get; set; }
    private float damage { get; set; }
    private bool isCone { get; set; }
    private bool firing;


    void Start()
    {
        //Gets the HealthBar component
        healthBar = healthBarObject.GetComponent<HealthBar>();

        //Gets the Navmesh Agent component
        agent = GetComponent<NavMeshAgent>();

        //Sets the player to the player object
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        //Sets the destination of the Navmesh Agent to the player's position
        //If the enemy is a cone, set the destination to the player's position
        if (isCone && agent.remainingDistance > moveDistance && !firing)
        {
            agent.SetDestination(player.transform.position);
        }
        else if (isCone && agent.remainingDistance < moveDistance)
        {
            agent.ResetPath();
            if (!firing)
            {
                //If the enemy is a cone, set the destination to the player's position
                StartCoroutine(Fire());
            }
        }
        else
        {
            agent.SetDestination(player.transform.position);
        }
        
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
                isCone = false;
                power.SetActive(true);
                break;

            //Cone
            case 1:
                maxHealth = 10;
                damage = 5;
                isCone = true;
                //spriteRenderer.sprite = cone;
                break;

            //PC
            case 2:
                maxHealth = 50;
                damage = 3;
                isCone = false;
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

    private IEnumerator Fire()
    {
        //Set firing to true
        firing = true;

        //Rotate the enemy to face the player
        Quaternion targetRotation = Quaternion.Euler(90, 0, 0);
        float elapsedTime = 0f;
        while (elapsedTime < rotationTime)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, elapsedTime / rotationTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.rotation = targetRotation;

        Instantiate(mousePointer, transform.position, Quaternion.identity);

        //Wait for the attack time
        yield return new WaitForSeconds(attackTime);
        Quaternion targetRotation2 = Quaternion.Euler(0, 0, 0);
        elapsedTime = 0f;
        while (elapsedTime < rotationTime)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation2, elapsedTime / rotationTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.rotation = targetRotation2;

        //Set firing to false
        firing = false;
    }
}
