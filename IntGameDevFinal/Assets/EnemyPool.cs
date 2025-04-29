using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject enemyPrefab;

    [Header("Pool Stats")]
    [Tooltip("The maximum amount of enemies that can be in the pool")]
    [Range(1, 100)]
    public int maxEnemy = 50;
    [Tooltip("Offset of the enemy")]
    public Vector3 offset = new Vector3(0, 0, 5f);

    [Header("Game Objects")]
    public GameObject playerController;

    private Queue<GameObject> enemyPool;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //Create array for the objects
        if (enemyPool == null)
        {
            enemyPool = new Queue<GameObject>();
        }

        for (int i = 0; i < maxEnemy; i++)
        {
            //Create prefab
            GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);

            //Set as child of this object
            enemy.transform.SetParent(transform);
            enemy.GetComponent<EnemyController>().enemyPool = this;
            enemyPool.Enqueue(enemy);

            //Set the enemy to inactive
            enemy.SetActive(false);
        }
    }

    //Gets the enemy from the pool and sets it's position.
    public void GetEnemy(Vector3 targetPosition, int enemyType)
    {
        //If the pool is empty, return
        if (enemyPool.Count > 0)
        {
            GameObject enemy = enemyPool.Dequeue();
            enemy.transform.position = targetPosition + offset;
            enemy.SetActive(true);
            try
            {
                enemy.GetComponent<EnemyController>().setEnemy(enemyType);
            }
            catch (System.ArgumentException exception)
            {
                Debug.LogException(exception);
            }
            return;
        }
        else
        {
            Debug.LogWarning("No available enemies in the pool.");
            return;
        }
    }

    //Gets enemy back to pool
    public void ReturnEnemy(GameObject enemy)
    {
        if (enemyPool.Count < maxEnemy)
        {
            enemy.SetActive(false);
            enemyPool.Enqueue(enemy);
        }
        else
        {
            Debug.LogWarning("Enemy pool is full. Destroying enemy.");
            Destroy(enemy);
        }
    }
}
