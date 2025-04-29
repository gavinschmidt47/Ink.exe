using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LevelSelect : MonoBehaviour
{
    public PlayerSave playerSave;

    public GameObject Earl;
    public GameObject Gandldore;
    public GameObject Rodger;
    public GameObject NextCharacter;



    public void Companion()
    {
        switch(playerSave.currentCompanion)
        {
            case 0:
                Earl.SetActive(true);
                return;
            case 1:
                Gandldore.SetActive(true);
                return;
            case 2:
                Rodger.SetActive(true);
                NextCharacter.SetActive(false);
                return;
            default:
                Debug.Log("Out of Range");
                return;
        }
        playerSave.currentCompanion++;
    }

    public void HealthUpgrade()
    {
        playerSave.maxHealth += 5;
        playerSave.health = playerSave.maxHealth;
    }
}
 