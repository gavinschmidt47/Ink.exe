using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(AfterDeath());
    }

    IEnumerator AfterDeath()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(0);
    }
}
