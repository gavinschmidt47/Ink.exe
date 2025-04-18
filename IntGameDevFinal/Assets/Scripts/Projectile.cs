using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Components")]
    public GameObject player;

    private ProjectilePool projectilePool;
    private PlayerController playerController;

    [Header("Projectile Stats")]
    [Tooltip("Speed of the projectile")]
    [Range(0, 100)]
    public float speed = 10f;
    [Tooltip("Time before the projectile is destroyed")]
    [Range(0, 10)]
    public float life = 5f;
    [Tooltip("Damage of the projectile")]
    [Range(0, 100)]
    public float damage = 10f;

    void Start()
    {
        //Get the projectile pool component
        if (transform.parent == null)
        {
            Debug.LogError($"Parent object is null for {gameObject.name}. Ensure the projectile has a parent object.");
            return;
        }
        GameObject parent = transform.parent.gameObject;
        projectilePool = parent.GetComponent<ProjectilePool>();
        if (projectilePool == null)
        {
            Debug.LogError($"ProjectilePool component not found in parent object of {gameObject.name}. Ensure the parent object has a ProjectilePool component.");
            return;
        }
    }
    void OnEnable()
    {
        //Set the player to the player object
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();

        //Set to look and go towards the player
        transform.LookAt(player.transform.position);
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * speed;

        rb.useGravity = false; // Disable gravity for the projectile

        //Set the projectile to be destroyed after a certain amount of time
        StartCoroutine(ReturnAfterTime(life));
    }

    //Gets rid of projectile after a certain amount of time
    IEnumerator ReturnAfterTime(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        if (!this.gameObject.activeSelf) yield break; // Exit if the projectile is already inactive
        projectilePool.ReturnProjectile(this.gameObject);
    }

    //Destroy with collisions
    void OnTriggerEnter(Collider other)
    {
        // Check if the projectile hit the player
        if (other.CompareTag("Player"))
            playerController.TakeDamage(damage); // Replace 10 with the appropriate damage value

        // Return projectile to the pool if it hits something
        if (!this.gameObject.activeSelf) return; //Avoid returning if already inactive
        projectilePool.ReturnProjectile(this.gameObject);
    }

    //Destroy with collisions
    void OnCollisionEnter(Collision collision)
    {
        // Return projectile to the pool if it hits something
        if (!this.gameObject.activeSelf) return; //Avoid returning if already inactive
        projectilePool.ReturnProjectile(this.gameObject);
    }

    void OnDisable()
    {
        StopAllCoroutines(); // Stop any running coroutines when the projectile is disabled
    }
}
