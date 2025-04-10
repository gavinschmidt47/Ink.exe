using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

//Stores weapons as ints, set to their levels needed
public enum Weapon
{
    Paintbrush = 1,
    Pencil = 5,
    Pen = 20,
    PaintBucket = 50
}

public class PlayerController : MonoBehaviour
{
    //UI
    [Header("UI")]
    public Slider hpBar;
    public Slider xpBar;
    public GameObject levelUpPrompt;
    public GameObject levelUpScreen;


    //Input
    [Header("Input")]
    public InputActionAsset inputs;
    
    private InputAction movement;
    private Vector3 moveDir;
    

    //Player Stats
    [Header("Player Save")]
    [Tooltip("Where stats are saved for player")]
    public PlayerSave playerSave;
    [Tooltip("Amount of time collider stays searching")]
    public float attackTime;

    private float lastFaceDir;
    

    //Player Children
    [Header("Player Children")]
    public GameObject leftPaintBAtt;
    public GameObject rightPaintBAtt;
    public GameObject leftPencilAtt;
    public GameObject rightPencilAtt;
    public GameObject leftPenAtt;
    public GameObject rightPenAtt;
    public GameObject PaintBucketAtt;


    //Player Components
    private Rigidbody rb;
    

    //Events
    private delegate void AttackDelegate();
    private AttackDelegate attackDelegate;
    public event Action playerCanLevelUp;


    void OnEnable()
    {
        //Find and set each InputAction
        movement = inputs.FindAction("Move");
        movement.Enable();

        //Set the level up listener
        playerCanLevelUp += PromptLevelUp;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Get Components
        rb = GetComponent<Rigidbody>();

        //Freeze Rotation
        rb.constraints = RigidbodyConstraints.FreezePositionY;

        //Set Health and XP
        playerSave.Reset((int) Weapon.Paintbrush);

        //Set HP Bar
        hpBar.maxValue = playerSave.maxHealth;
        hpBar.value = playerSave.health;

        //Set XP Bar
        xpBar.maxValue = playerSave.xpToLevel;
        xpBar.value = playerSave.xp;

        attackDelegate = AttackPaintBrush;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Take in input and apply with speed and time
        moveDir = movement.ReadValue<Vector2>();
        rb.linearVelocity = new Vector3 (moveDir.x, 0, moveDir.y) * playerSave.speed * Time.deltaTime;

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
            playerSave.health -= collision.gameObject.GetComponent<EnemyController>().damage;
            hpBar.value = playerSave.health;

            //If dead
            if (playerSave.health <= 0)
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

        attackDelegate();
    }

    private void AttackPaintBrush()
    {
        //Attack with paintbrush
        if (lastFaceDir > 0)
        {
            rightPaintBAtt.SetActive(true);
            StartCoroutine(AttackTimer(rightPaintBAtt));
        }
        else
        {
            leftPaintBAtt.SetActive(true);
            StartCoroutine(AttackTimer(leftPaintBAtt));
        }
    }

    private void AttackPencil()
    {
        //Attack with pencil
        if (lastFaceDir > 0)
        {
            rightPencilAtt.SetActive(true);
            StartCoroutine(AttackTimer(rightPencilAtt));
        }
        else
        {
            leftPencilAtt.SetActive(true);
            StartCoroutine(AttackTimer(leftPencilAtt));
        }
    }

    private void AttackPen()
    {
        //Attack with pen
        if (lastFaceDir > 0)
        {
            rightPenAtt.SetActive(true);
            StartCoroutine(AttackTimer(rightPenAtt));
        }
        else
        {
            leftPenAtt.SetActive(true);
            StartCoroutine(AttackTimer(leftPenAtt));
        }
    }

    private void AttackPaintBucket()
    {
        PaintBucketAtt.SetActive(true);
        StartCoroutine(AttackTimer(PaintBucketAtt));
    }

    public void GainXP()
    {
        //Gain XP from enemy death
        Debug.Log("Gaining XP");
        ++playerSave.xp; 
        xpBar.value = playerSave.xp;

        if (playerSave.xp >= playerSave.xpToLevel)
        {
            playerCanLevelUp?.Invoke();
        }
    }

    public void PromptLevelUp()
    {
        //Called from gain XP
        playerSave.canLevelUp = true;
        levelUpPrompt.SetActive(true);
        StartCoroutine(LevelingUI());
    }

    //Called from PlayerInput
    public void LevelUp(InputAction.CallbackContext context)
    {
        //Only level up once per press
        if (!context.started) return;

        if (playerSave.canLevelUp)
        {
            //Level up
            playerSave.xp -= playerSave.xpToLevel;
            playerSave.xpToLevel += 10 * playerSave.level;
            playerSave.level++;
            xpBar.maxValue = playerSave.xpToLevel;
            xpBar.value = playerSave.xp;
            xpBar.fillRect.GetComponent<Image>().color = Color.blue;

            //Pause Game
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            //UI
            levelUpPrompt.SetActive(false);
            levelUpScreen.SetActive(true);

            //clear level up variables
            playerSave.canLevelUp = false;

            //Set new weapon
            if (playerSave.level == 2)
            {
                attackDelegate = AttackPencil;
            }
            else if (playerSave.level == 3)
            {
                attackDelegate = AttackPen;
            }
            else if (playerSave.level == 4)
            {
                attackDelegate = AttackPaintBucket;
            }
        }
        else return;
    }

    //Disables attack collider after attackTime
    private IEnumerator AttackTimer(GameObject att)
    {
        yield return new WaitForSeconds(attackTime);
        att.SetActive(false);
    }

    //Cool flashing lights
    public IEnumerator LevelingUI()
    {
        float time = 0;
        while (playerSave.canLevelUp)
        {
            time += Time.deltaTime;
            float lerp = Mathf.PingPong(time, 1);
            xpBar.fillRect.GetComponent<Image>().color = Color.Lerp(Color.white, Color.blue, lerp);
            yield return null;
        }
    }
}
