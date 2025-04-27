using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "PlayerSave", menuName = "Scriptable Objects/PlayerSave")]
public class PlayerSave : ScriptableObject
{
    //Player Stats
    [Header("Player Stats")]
    public float health;
    public float maxHealth;
    public float damage;
    public float speed;
    public float xp;
    public float level;
    

    //BTS Variables
    [Header("Backend")]
    public float xpToLevel;
    public bool canLevelUp;
    public int weapon;

    public void Reset()
    {
        health = 10;
        xp = 0;
        xpToLevel = 10;
        canLevelUp = false;
        level = 1;
        maxHealth = 10;
        damage = 1;
        speed = 200;
        this.weapon = 0;
    }
}
