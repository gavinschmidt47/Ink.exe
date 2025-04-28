using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu]
public class LevelSelect : MonoBehaviour
{
    /*private WeaponUnlockData weaponUnlockData;
    public string Name;
    public Sprite icon;*/

    public PlayerController playerController;
    public PlayerSave playerSave;

    public void Weapon()
    {
        switch ((int) playerSave.weapon)
        {
            case (int) PlayerController.Weapon.Pencil:
                StartCoroutine(playerController.WeaponAttackLoop(playerController.leftPencilAtt, playerController.rightPencilAtt));
                break;
            case (int) PlayerController.Weapon.Pen:
                StartCoroutine(playerController.WeaponAttackLoop(playerController.leftPenAtt, playerController.rightPenAtt));
                break;
            case (int) PlayerController.Weapon.Paintbrush:
                StartCoroutine(playerController.WeaponAttackLoop(playerController.leftPaintBAtt, playerController.topPaintBAtt, playerController.rightPaintBAtt, playerController.bottomPaintBAtt));
                break;
        }
    }
}
 