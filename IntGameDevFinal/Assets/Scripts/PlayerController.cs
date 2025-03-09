using System.Collections;
using System.Collections.Generic;
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


    //Player Stats
    public float speed;
    public float attackTime;

    private float lastFaceDir;


    //Player Components
    public GameObject leftAtt;
    public GameObject rightAtt;

    private Rigidbody rb;

    void OnEnable()
    {
        //Find and set each InputAction
        movement = inputs.FindAction("Move");
        movement.Enable();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Get Components
        rb = GetComponent<Rigidbody>();

        //Freeze Rotation
        rb.constraints = RigidbodyConstraints.FreezePositionY;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Take in input and apply with speed and time
        moveDir = movement.ReadValue<Vector2>();
        rb.linearVelocity = new Vector3 (moveDir.x, 0, moveDir.y) * speed * Time.deltaTime;

        //Remember last face direction
        if (moveDir.normalized.x != 0)
            lastFaceDir = moveDir.normalized.x;
    }

    //Called from PlayerInput
    public void Attack(InputAction.CallbackContext context)
    {
        //Only attack once per press
        if (!context.performed) return;

        if (lastFaceDir > 0)
        {
            rightAtt.SetActive(true);
            StartCoroutine(AttackTimer(rightAtt));
        }
        else
        {
            leftAtt.SetActive(true);
            StartCoroutine(AttackTimer(leftAtt));
        }
    }
    private IEnumerator AttackTimer(GameObject att)
    {
        yield return new WaitForSeconds(attackTime);
        att.SetActive(false);
    }
}
