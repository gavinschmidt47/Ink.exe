using UnityEngine;

public class CompanionFollow : MonoBehaviour
{
    private Rigidbody rb;  // Companion's Rigidbody
    public Transform player;  // Reference the player
    public float followDistance = 3f;  // Distance companion follows the player

    private void Start()
    {
        rb = GetComponent<Rigidbody>();  // Get Rigidbody component
    }

    private void FixedUpdate()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        // Calculate direction from companion to player
        Vector3 directionToPlayer = player.position - transform.position;

        // No need to move if we're already within follow distance
        if (directionToPlayer.magnitude > followDistance)
        {
            // Calculate desired position to maintain the follow distance
            Vector3 desiredPosition = player.position - directionToPlayer.normalized * followDistance;

            // Smoothly move the companion to the desired position
            Vector3 newPosition = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * 5f);

            // Apply the new position to the companion
            rb.MovePosition(newPosition);
        }
    }
}
