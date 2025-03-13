using UnityEngine;

public class PencilAttack : MonoBehaviour
{
    private PlayerController playerController;
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Enemy")) 
        {
            playerController.xp += 1;
            playerController.xpBar.value = playerController.xp;
            if (playerController.xp >= playerController.xpToLevel) 
            {
                playerController.canLevelUp = true;

                playerController.levelUpPrompt.SetActive(true);
                
                playerController.StartCoroutine(playerController.LevelingUI());
            }

            this.gameObject.SetActive(false);

            Destroy(other.gameObject);
        }
    }
}
