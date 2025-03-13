using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum Weapon
{
    Pencil,
    Pen,
    PaintBrush
}

public class PlayerController : MonoBehaviour
{
    //UI
    public Slider hpBar;
    public Slider xpBar;
    public GameObject levelUpPrompt;
    public GameObject levelUpScreen;


    //Input
    public InputActionAsset inputs;
    
    private InputAction movement;
    private Vector3 moveDir;


    //Player Stats
    public float speed;
    public float attackTime;
    public float maxHealth = 10;  
    public float health;  
    public float xp = 0;
    public float xpToLevel = 10;
    public bool canLevelUp = false;

    private float lastFaceDir;
    private int level = 1;
    private Weapon weapon = Weapon.Pencil;
    

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

        //Set Health and XP
        maxHealth = 10;
        health = maxHealth;

        xp = 0;
        level = 1;
        xpToLevel = 10;

        //Set HP Bar
        hpBar.maxValue = maxHealth;
        hpBar.value = health;

        //Set XP Bar
        xpBar.maxValue = xpToLevel;
        xpBar.value = xp;


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

    private void OnCollisionEnter(Collision collision)
    {
        //If hit by enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //Take damage
            health -= 1;
            hpBar.value = health;

            //If dead
            if (health <= 0)
            {
                //Destroy player
                Destroy(gameObject);

                //Show Game Over
                Debug.Log("Game Over");
            }
        }
    }

    //Called from PlayerInput
    public void Attack(InputAction.CallbackContext context)
    {
        //Only attack once per press
        if (!context.performed) return;

        switch (weapon)
        {
            case Weapon.Pencil:
                AttackPencil();
                break;
            case Weapon.Pen:
                AttackPen();
                break;
            case Weapon.PaintBrush:
                AttackPaintBrush();
                break;
        }
    }

    private void AttackPencil()
    {
        //Attack with pencil
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

    private void AttackPen()
    {
        //Attack with pen
        Debug.Log("Pen Attack");
    }

    private void AttackPaintBrush()
    {
        //Attack with paint brush
        Debug.Log("Paint Brush Attack");
    }

    //Called from PlayerInput
    public void LevelUp(InputAction.CallbackContext context)
    {
        //Only level up once per press
        if (!context.started) return;

        if (canLevelUp)
        {
            //Level up
            xp -= xpToLevel;
            xpToLevel += 10 * level;
            level++;
            xpBar.maxValue = xpToLevel;
            xpBar.value = xp;
            xpBar.fillRect.GetComponent<Image>().color = Color.blue;

            //Pause Game
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            //UI
            levelUpPrompt.SetActive(false);
            levelUpScreen.SetActive(true);

            canLevelUp = false;
        }
        else return;
    }

    private IEnumerator AttackTimer(GameObject att)
    {
        yield return new WaitForSeconds(attackTime);
        att.SetActive(false);
    }

    public IEnumerator LevelingUI()
    {
        float time = 0;
        while (canLevelUp)
        {
            time += Time.deltaTime;
            float lerp = Mathf.PingPong(time, 1);
            xpBar.fillRect.GetComponent<Image>().color = Color.Lerp(Color.white, Color.blue, lerp);
            yield return null;
        }
    }
}
