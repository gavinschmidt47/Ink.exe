using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    //GameInfo
    //public Health health;?

    //Enemy Components
    public Transform player;

    private NavMeshAgent agent;


    void Start()
    {
        //Gets the Navmesh Agent component
        agent = GetComponent<NavMeshAgent>();

        //Sets the player to the player object
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        //Sets the destination of the Navmesh Agent to the player's position
        agent.SetDestination(player.position);
    }

    void OnTriggerEnter(Collider collider)
    {
        //If the enemy collides with the player, destroy the player
        if (collider.gameObject.CompareTag("Player"))
        {
            // health.Damage(1);?
            Debug.Log("Player Hit");
        }
    }
}
