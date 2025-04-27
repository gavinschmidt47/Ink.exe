using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class RogerAttack : MonoBehaviour
{
    public GameObject player;
    public float speed;
    public float damage;
    public float followDistance;
    private Rigidbody rb;  // Companion's Rigidbody
    private GameObject target;
    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        target = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null || !target.activeSelf)
        {
            target = FindTarget();
            FollowPlayer();
        }
        else
        {
            // Calculate direction from roger to enemy
            Vector3 directionToEnemy = target.transform.position - transform.position;

            // Remove y axis
            directionToEnemy.y = 0;

            // Apply the new position to the companion
            rb.MovePosition(transform.position + directionToEnemy.normalized * speed * Time.deltaTime);

            // Update rogers animation
            animator.SetFloat("MoveX", directionToEnemy.x); // Horizontal movement (X-axis)
            animator.SetFloat("MoveZ", directionToEnemy.z); // Depth movement (Z-axis)
            animator.SetFloat("Speed", directionToEnemy.magnitude); // speed
        }
    }

    private void FollowPlayer()
    {
        // Calculate direction from companion to player
        Vector3 directionToPlayer = player.transform.position - transform.position;

        // No need to move if we're already within follow distance
        if (directionToPlayer.magnitude > followDistance)
        {
            // Calculate desired position to maintain the follow distance
            Vector3 desiredPosition = player.transform.position - directionToPlayer.normalized * followDistance;

            // Smoothly move the companion to the desired position
            Vector3 newPosition = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * 5f);

            Vector3 movement = newPosition - transform.position;
            animator.SetFloat("MoveX", movement.x); // Horizontal movement (X-axis)
            animator.SetFloat("MoveZ", movement.z); // Depth movement (Z-axis)
            animator.SetFloat("Speed", movement.magnitude); // speed

            // Apply the new position to the companion
            rb.MovePosition(newPosition);
        }
        else
        {
            animator.SetFloat("Speed", 0f); // speed
        }
    }

    // Find the enemy nearest to the Player
    GameObject FindTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float closestDistance = Mathf.Infinity;
        Transform nearest = null;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(player.transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                nearest = enemy.transform;
            }
        }

        if (nearest == null)
        {
            return null;
        } 
        else
        {
            return nearest.gameObject;
        }
    }

    private void OnCollisionStay(Collision other)
    {
        EnemyController enemyController = other.gameObject.GetComponent<EnemyController>();
        if (enemyController != null)
        {
            enemyController.Damage(damage * Time.fixedDeltaTime);
        }
    }
}
