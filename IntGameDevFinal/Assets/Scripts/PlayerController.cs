using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //GameInfo

    //UI

    //Input
    public InputActionAsset inputs;
    private InputAction movement;
    private Vector3 moveDir;

    //Player Components
    private Rigidbody rb;

    void OnEnable()
    {
        movement = inputs.FindAction("Move");
        movement.Enable();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePositionY;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        moveDir = movement.ReadValue<Vector2>();
        rb.linearVelocity = new Vector3 (moveDir.x, 0, moveDir.y) * 100f * Time.deltaTime;
    }
}
