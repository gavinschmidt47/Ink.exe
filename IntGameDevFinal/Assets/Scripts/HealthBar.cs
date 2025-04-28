using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class HealthBar : MonoBehaviour
{
    public Transform target;  // The target that the health bar will follow
    public Vector3 offset;    // Offset position from the target

    private Slider healthFill;  // The UI Image that represents the health fill
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        healthFill = GetComponent<Slider>();
    }

    void Update()
    {
        // Update the position of the health bar to follow the target
        transform.position = target.position + offset;

        // Make the health bar face the camera
        transform.LookAt(mainCamera.transform);
    }

    // Function to update the health bar fill amount
    public void SetHealth(float currentHealth, float maxHealth)
    {
        healthFill.value = currentHealth / maxHealth;
    }


    public void Health()
    {
        
    }
}