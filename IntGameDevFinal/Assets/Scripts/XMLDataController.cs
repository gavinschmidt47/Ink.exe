using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;

[System.Serializable]
public class CharacterData
{
    [XmlAttribute("name")]  // Save as XML attribute
    public string name;

    public bool unlocked;
    public int levelUnlocked;
    public int level;
    public int health;
    public int attack;
}

[System.Serializable]
public class WeaponData
{
    [XmlAttribute("name")]
    public string name;

    public bool unlocked;
    public int levelUnlocked;
    public int damage;
    public float fireRate;
    public int ammoCapacity;
}

[System.Serializable]
[XmlRoot("GameSaveData")]
public class GameSaveData
{
    [XmlArray("Characters"), XmlArrayItem("Character")]
    public List<CharacterData> characters;// = new List<CharacterData>();

    [XmlArray("Weapons"), XmlArrayItem("Weapon")]
    public List<WeaponData> weapons;// = new List<WeaponData>();

    [XmlElement("Player")]
    public PlayerSave playerSave;// = new PlayerSave();
}