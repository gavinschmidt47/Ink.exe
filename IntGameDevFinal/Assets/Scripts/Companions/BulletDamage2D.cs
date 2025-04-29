using UnityEngine;

public class BulletDamage2D : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 1f;
    private Transform target;

    void Start()
    {
        FindNearestEnemy();
    }

    void Update()
    {
        if (target != null && target.gameObject.activeInHierarchy)
        {
            Vector3 direction = (target.position - transform.position);
            direction.y = 0f;
            direction = direction.normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
        else
        {
            Destroy(gameObject); // destroy if target is lost
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision Enter");
        EnemyController enemy = other.gameObject.GetComponent<EnemyController>();
        if (enemy != null)
        {
            enemy.Damage(damage);
            Destroy(gameObject);
        }
    }

    void FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float closestDistance = Mathf.Infinity;
        Transform nearest = null;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                nearest = enemy.transform;
            }
        }

        target = nearest;
    }
}