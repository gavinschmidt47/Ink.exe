using UnityEngine;

public class DestroyMusic : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(GameObject.FindGameObjectWithTag("Music"));
    }

}
