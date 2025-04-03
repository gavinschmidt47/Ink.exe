using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;
public class CharacterData
{
    [XmlAttribute("name")]  // Save as XML attribute
    public string name;

    public bool unlocked;
    public int level;
    public int health;
    public int attack;
}

[System.Serializable]
[XmlRoot("CharacterSaveData")]  // Root element in XML
public class CharacterSaveData
{
    [XmlArray("Characters"), XmlArrayItem("Character")]
    public List<CharacterData> characters = new List<CharacterData>();
}
