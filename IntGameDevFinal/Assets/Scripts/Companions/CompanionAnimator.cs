using UnityEngine;

public class CompanionAnimator : MonoBehaviour
{
    public Animator animator;
    public Transform player;//reference player for x axis and z axis movements.
    private Rigidbody rb;

    // Update is called once per frame
    void Update()
    {
        rb = GetComponent<Rigidbody>();//get rigidbody

        // Get the player's movement input
        float moveX = Input.GetAxis("Horizontal"); // A/D
        float moveZ = Input.GetAxis("Vertical"); // W/S

        // Update the animator parameters based on player input
        animator.SetFloat("MoveX", moveX); // Horizontal movement (X-axis)
        animator.SetFloat("MoveZ", moveZ); // Depth movement (Z-axis)
        animator.SetFloat("Speed", rb.linearVelocity.magnitude); // speed
    }
}
