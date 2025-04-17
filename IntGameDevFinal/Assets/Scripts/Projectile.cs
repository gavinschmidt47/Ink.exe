using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Components")]
    public EnemyController enemyController;
    public Transform player;

    private ProjectilePool projectilePool;

    [Header("Projectile Stats")]
    [Tooltip("Speed of the projectile")]
    [Range(0, 100)]
    public float speed = 10f;
    [Tooltip("Time before the projectile is destroyed")]
    [Range(0, 10)]
    public float life = 5f;

    //Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        //Set to look and go towards the player
        transform.LookAt(player.position);
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * speed;

        //Get the projectile pool component
        projectilePool = GetComponentInParent<ProjectilePool>();
        if (projectilePool == null)
        {
            Debug.LogError("ProjectilePool component not found in parent object.");
            return;
        }

        //Set the projectile to be destroyed after a certain amount of time
        StartCoroutine(ReturnAfterTime(life));
    }

    //Gets rid of projectile after a certain amount of time
    IEnumerator ReturnAfterTime(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        projectilePool.ReturnProjectile(this.gameObject);
    }

    //Destroy with collisions
    void OnCollisionEnter(Collision collision)
    {
        //Check if the projectile hit the player
        if (collision.gameObject.CompareTag("Player"))
        {
            //Damage player
            enemyController.Damage(enemyController.damage);
        }
        //Destroy projectile is it hits something
        projectilePool.ReturnProjectile(this.gameObject);
    }
}
