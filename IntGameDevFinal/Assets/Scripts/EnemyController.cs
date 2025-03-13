using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    //Player Components
    private GameObject player;
    private PlayerController playerController;

    //Enemy Components
    private NavMeshAgent agent;


    void Start()
    {
        //Gets the Navmesh Agent component
        agent = GetComponent<NavMeshAgent>();

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
            playerController.health -= 1;
            playerController.hpBar.value = playerController.health;
            Destroy(gameObject);
        }
    }
}
