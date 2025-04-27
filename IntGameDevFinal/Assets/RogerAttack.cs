using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class RogerAttack : MonoBehaviour
{
    public GameObject player;
    public float speed;
    public float damage;
    private Rigidbody rb;  // Companion's Rigidbody
    private GameObject target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();  // Get Rigidbody component
        target = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null || !target.activeSelf)
        {
            target = FindTarget();
            // Update rogers animation
            Animator animator = GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetFloat("Speed", 0f); // speed
            }
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
            Animator animator = GetComponent<Animator>();
            if (animator != null) {
                // Update the animator parameters based on player input
                animator.SetFloat("MoveX", directionToEnemy.x); // Horizontal movement (X-axis)
                animator.SetFloat("MoveZ", directionToEnemy.z); // Depth movement (Z-axis)
                animator.SetFloat("Speed", directionToEnemy.magnitude); // speed
            }
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
