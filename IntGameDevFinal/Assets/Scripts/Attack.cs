using UnityEngine;

public class Attack : MonoBehaviour
{
    private PlayerController playerController;
    private EnemyController enemyController;
    public float damage;
    void Start()
    {
        //Get Controller to change xp
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Enemy")) 
        {
            enemyController = other.GetComponent<EnemyController>();

            enemyController.Damage(playerController.playerSave.damage * damage);

            //this.gameObject.SetActive(false);
        }
    }
}
