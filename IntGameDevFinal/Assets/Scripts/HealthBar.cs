using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Transform target;  // The target that the health bar will follow
    public Vector3 offset;    // Offset position from the target
    public Image healthFill;  // The UI Image that represents the health fill

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Update the position of the health bar to follow the target
        transform.position = target.position + offset;

        // Make the health bar face the camera
        transform.LookAt(mainCamera.transform);
        transform.Rotate(0, 180, 0);  // Rotate 180 degrees to face the camera correctly
    }

    // Function to update the health bar fill amount
    public void SetHealth(float currentHealth, float maxHealth)
    {
        healthFill.fillAmount = currentHealth / maxHealth;
    }
}