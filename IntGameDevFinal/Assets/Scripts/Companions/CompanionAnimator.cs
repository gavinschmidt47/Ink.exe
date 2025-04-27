using UnityEngine;

public class CompanionAnimator : MonoBehaviour
{
    public Transform player;//reference player for x axis and z axis movements.
    private Animator animator;
    private Rigidbody rb;

    // Update is called once per frame
    void Update()
    {
        animator = GetComponent<Animator>();// animator
        rb = GetComponent<Rigidbody>();// get rigidbody

        // Get the player's movement input
        float moveX = Input.GetAxis("Horizontal"); // A/D
        float moveZ = Input.GetAxis("Vertical"); // W/S

        // Update the animator parameters based on player input
        animator.SetFloat("MoveX", moveX); // Horizontal movement (X-axis)
        animator.SetFloat("MoveZ", moveZ); // Depth movement (Z-axis)
        animator.SetFloat("Speed", rb.linearVelocity.magnitude); // speed
    }
}
