using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RandomEnvironment : MonoBehaviour
{
    public int numObjectsToSpawn;

    public float worldXDimension = 200f;
    public float worldZDimension = 200f;

    public float minObjDistance = 4f;
    public float minObjScale = 2f;
    public float maxObjScale = 5f;

    public GameObject[] randomObjsToSpawn;

    private List<GameObject> objects = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DistributeRandomObjects();
    }

    private void DistributeRandomObjects()
    {
        for (int i = 0; i < numObjectsToSpawn; i++)
        {
            bool validPositionFound = false;
            Vector3 objPos = Vector3.zero;

            while (!validPositionFound)
            {
                objPos.x = Random.Range(5, (worldXDimension / 2) - 5) * Mathf.Sign(Random.value * 2 - 1);
                objPos.z = Random.Range(5, (worldXDimension / 2) - 5) * Mathf.Sign(Random.value * 2 - 1);

                validPositionFound = CheckObjectOverlap(objPos);
            }

            PrimitiveType primitiveType;
            int randType = Random.Range(1, 3);
            if (randType == 1)
            {
                primitiveType = PrimitiveType.Cube;
            }
            else if (randType == 2)
            {
                primitiveType = PrimitiveType.Sphere;
            }
            else
            {
                primitiveType = PrimitiveType.Cylinder;
            }

            GameObject newObj = Instantiate(randomObjsToSpawn[Random.Range(0, randomObjsToSpawn.Length)]);
            newObj.transform.parent = transform;
            newObj.transform.position = objPos;
            Vector3 randomScale = Vector3.one;
            randomScale.x = Random.Range(minObjScale, maxObjScale);
            randomScale.z = Random.Range(minObjScale, maxObjScale);
            newObj.transform.localScale = randomScale;
            objects.Add(newObj);
        }
    }

    private bool CheckObjectOverlap(Vector3 newPos)
    {
        for (int i = 0; i < objects.Count; i++)
        {
            if ((newPos - objects[i].transform.position).sqrMagnitude < minObjDistance * minObjDistance)
            {
                return false;
            }
        }

        return true;
    }
}
