using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public class PlayerController : MonoBehaviour
{
    //Stores weapons as ints, set to their levels needed
    public enum Weapon
    {
        Paintbrush = 1,
        Pencil = 2,
        Pen = 3,
        PaintBucket = 4
    }

    public LevelSelect LevelSelect;
    public static PlayerController Instance;

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

    public float lastFaceDir;

    public bool correctWeapon = true; 
    

    //Player Children
    [Header("Player Children")]
    public GameObject leftPaintBAtt;
    public GameObject topPaintBAtt;
    public GameObject rightPaintBAtt;
    public GameObject bottomPaintBAtt;
    public GameObject leftPencilAtt;
    public GameObject rightPencilAtt;
    public GameObject leftPenAtt;
    public GameObject rightPenAtt;
    public GameObject PaintBucketAtt;



    //Player Components
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
        //Singleton pattern for static reference for player
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        Instance = this;

        // load data from save
        LoadSaveData();

        //Get Components
        rb = GetComponent<Rigidbody>();

        //Freeze Rotation
        rb.constraints = RigidbodyConstraints.FreezePositionY;

        //Set Health and XP
        //playerSave.Reset((int) Weapon.Paintbrush);

        //Set HP Bar
        hpBar.maxValue = playerSave.maxHealth;
        hpBar.value = playerSave.health;
        Debug.Log("start: player health is " + hpBar.value);

        //Set XP Bar
        xpBar.maxValue = playerSave.xpToLevel;
        xpBar.value = playerSave.xp;

        StartCoroutine(WeaponAttackLoop());
        //StartCoroutine(WeaponAttackLoop(leftPenAtt, rightPenAtt));
        //StartCoroutine(WeaponAttackLoop(leftPaintBAtt, topPaintBAtt, rightPaintBAtt, bottomPaintBAtt));
    }

    /*public void WeaponSwitch()
    {
        if()
    }*/

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
            playerSave.health -= 1;
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

        //Checks for which active weapon
        switch ((int) playerSave.weapon)
        {
            case (int) Weapon.Pencil:
                AttackPencil();
                break;
            case (int) Weapon.Pen:
                AttackPen();
                break;
            case (int) Weapon.Paintbrush:
                AttackPaintBrush();
                break;
            case (int) Weapon.PaintBucket:
                AttackPaintBucket();
                break;
        }
    }

    public void WeaponChoice()
    {
        ++playerSave.weapon;
        if ((int) playerSave.weapon > 4)
        {
            playerSave.weapon = (int) Weapon.Paintbrush;
        }
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

    IEnumerator WeaponAttackLoop()
    {
        while (true)
        {
            switch ((int)playerSave.weapon)
            {
                case (int)Weapon.Pencil:
                    rightPencilAtt.SetActive(false);
                    leftPencilAtt.SetActive(true);
                    StartCoroutine(AttackTimer(leftPencilAtt));
                    yield return new WaitForSeconds(attackTime);
                    leftPencilAtt.SetActive(false);
                    rightPencilAtt.SetActive(true);
                    StartCoroutine(AttackTimer(rightPencilAtt));
                    yield return new WaitForSeconds(attackTime);
                    break;
                case (int)Weapon.Pen:
                    rightPenAtt.SetActive(false);
                    leftPenAtt.SetActive(true);
                    StartCoroutine(AttackTimer(leftPenAtt));
                    yield return new WaitForSeconds(attackTime);
                    leftPenAtt.SetActive(false);
                    rightPenAtt.SetActive(true);
                    StartCoroutine(AttackTimer(rightPenAtt));
                    yield return new WaitForSeconds(attackTime);
                    break;
                case (int)Weapon.Paintbrush:
                    leftPaintBAtt.SetActive(true);
                    StartCoroutine(AttackTimer(leftPaintBAtt));
                    yield return new WaitForSeconds(attackTime / 2);
                    topPaintBAtt.SetActive(true);
                    StartCoroutine(AttackTimer(topPaintBAtt));
                    yield return new WaitForSeconds(attackTime / 2);
                    rightPaintBAtt.SetActive(true);
                    StartCoroutine(AttackTimer(rightPaintBAtt));
                    yield return new WaitForSeconds(attackTime / 2);
                    bottomPaintBAtt.SetActive(true);
                    StartCoroutine(AttackTimer(bottomPaintBAtt));
                    yield return new WaitForSeconds(attackTime / 2);
                    break;
                default:
                    Debug.Log("No weapon selected");
                    break;
            
            }
            yield return new WaitForSeconds(attackTime);  // Delay between attacks
        }
    }

    public IEnumerator WeaponAttackLoop(GameObject weaponHitbox1, GameObject weaponHitbox2, GameObject weaponHitbox3, GameObject weaponHitbox4)
    {
        if(!correctWeapon)
        {
            correctWeapon = true;
            yield break;
        }
        weaponHitbox1.SetActive(true);
        StartCoroutine(AttackTimer(weaponHitbox1));
        yield return new WaitForSeconds(attackTime / 2);
        weaponHitbox2.SetActive(true);
        StartCoroutine(AttackTimer(weaponHitbox2));
        yield return new WaitForSeconds(attackTime / 2);
        weaponHitbox3.SetActive(true);
        StartCoroutine(AttackTimer(weaponHitbox3));
        yield return new WaitForSeconds(attackTime / 2);
        weaponHitbox4.SetActive(true);
        StartCoroutine(AttackTimer(weaponHitbox4));
        yield return new WaitForSeconds(attackTime / 2);

        StartCoroutine(WeaponAttackLoop(weaponHitbox1, weaponHitbox2, weaponHitbox3, weaponHitbox4));
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
            //levelUpPrompt.SetActive(false);
            //levelUpScreen.SetActive(true);

            playerSave.canLevelUp = false;
            SavePlayerData();
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

    // Loads in values from save data and applies them to player
    private void LoadSaveData()
    {
        Debug.Log("tried loading save");
        if (XMLGameSaveManager.Instance)
        {
            GameSaveData saveData = XMLGameSaveManager.Instance.saveData;
            playerSave = saveData.playerSave;
            hpBar.maxValue = playerSave.maxHealth;
            hpBar.value = playerSave.health;
            Debug.Log("saved health: " + hpBar.value);
            xpBar.maxValue = playerSave.xpToLevel;
            xpBar.value = playerSave.xp;
        }
}

    // Saves player data in XML save scripts
    public void SavePlayerData()
    {
        XMLGameSaveManager.Instance.SavePlayerData(playerSave);
    }
}
