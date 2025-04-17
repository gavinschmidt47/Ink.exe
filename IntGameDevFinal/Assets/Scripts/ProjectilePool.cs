using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject projectilePrefab;

    [Header("Pool Stats")]
    [Tooltip("The maximum amount of projectiles that can be in the pool")]
    [Range(1, 100)]
    public int maxProjectile = 10;
    [Tooltip("Offset of the projectile from the enemy")]
    public Vector3 offset = new Vector3(0, 0, 5f);

    public static ProjectilePool Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }


    private Queue<GameObject> projectilePool;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < maxProjectile; i++)
        {
            //Create prefab
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

            //Set as child of this object
            projectile.transform.SetParent(transform);

            //Create array for the objects
            if (projectilePool == null)
            {
                projectilePool = new Queue<GameObject>();
            }
            projectilePool.Enqueue(projectile);

            //Set the projectile to inactive
            projectile.SetActive(false);
        }
    }

    //Gets the projectile from the pool and sets it to the player position
    public void GetProjectile(Vector3 targetPosition)
    {
        if (projectilePool.Count > 0)
        {
            GameObject projectile = projectilePool.Dequeue();
            projectile.transform.position = targetPosition + offset;
            projectile.SetActive(true);
        }
        else
        {
            Debug.LogWarning("No available projectiles in the pool.");
        }
    }

    //Gets projectile back to pool
    public void ReturnProjectile(GameObject projectile)
    {
        if (projectilePool.Count < maxProjectile)
        {
            projectile.SetActive(false);
            projectilePool.Enqueue(projectile);
        }
        else
        {
            Debug.LogWarning("Projectile pool is full. Destroying projectile.");
            Destroy(projectile);
        }
    }
}
