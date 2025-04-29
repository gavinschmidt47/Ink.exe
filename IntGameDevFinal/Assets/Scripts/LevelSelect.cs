using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LevelSelect : MonoBehaviour
{
    public PlayerSave playerSave;
    public PlayerController player;

    public GameObject Earl;
    public GameObject Gandldore;
    public GameObject Rodger;
    public GameObject NextCharacter;

    public void Companion()
    {
        playerSave.currentCompanion++;
        switch(playerSave.currentCompanion)
        {
            case 1:
                Earl.SetActive(true);
                return;
            case 2:
                Gandldore.SetActive(true);
                return;
            case 3:
                Rodger.SetActive(true);
                NextCharacter.SetActive(false);
                return;
            default:
                Debug.Log("Out of Range");
                return;
        }
    }

    public void HealthUpgrade()
    {
        playerSave.maxHealth += 5;
        playerSave.health = playerSave.maxHealth;
        player.hpBar.maxValue = playerSave.maxHealth;
        player.hpBar.value = playerSave.health;
    }
}
 