using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Transform target;  // The target that the health bar will follow
    public Vector3 offset;    // Offset position from the target


    private Slider healthFill;  // The UI Slider that represents the health fill
    private Camera mainCamera;

    void Awake()
    {
        mainCamera = Camera.main;
        healthFill = GetComponent<Slider>();
        if (healthFill == null)
        {
            Debug.LogError("Slider component not found on the GameObject.");
        }
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
}