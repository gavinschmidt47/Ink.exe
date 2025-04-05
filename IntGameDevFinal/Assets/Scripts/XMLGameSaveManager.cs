using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class XMLGameSaveManager : MonoBehaviour
{
    public GameSaveData saveData = new GameSaveData();
    private string path;
        void Awake()
        {
            path = Application.persistentDataPath + "/gamesave.xml";
            LoadGame();
        }
        public void UnlockCharacter()
        {
            foreach (var character in saveData.characters)
            {
                if (!character.unlocked)
                {
                    character.unlocked = true;
                    character.level = 1;
                    SaveGame();
                    Debug.Log($"Unlocked {character.name}!");
                    return;
                }
            }
            Debug.Log("All characters are already unlocked!");
        }
        public void UnlockWeapon()
        {
            foreach (var weapon in saveData.weapons)
            {
                if (!weapon.isUnlocked)
                {
                    weapon.isUnlocked = true;
                    SaveGame();
                    Debug.Log($"Unlocked {weapon.name}!");
                    return;
                }
            }
            Debug.Log("All weapons are already unlocked!");
        }
        public void SaveGame()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(GameSaveData));
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                serializer.Serialize(stream, saveData);
            }
            Debug.Log("Game Saved!");
        }
        
        public void LoadGame()
        {
            if (File.Exists(path))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(GameSaveData));
                using (FileStream stream = new FileStream(path, FileMode.Open))
                {
                    saveData = (GameSaveData)serializer.Deserialize(stream);
                }
                Debug.Log("Game Loaded!");
            }
            else
            {
                InitializeGameData();
                Debug.Log("No save file found, starting new game.");
            }
        }
        void InitializeGameData()
        {
            saveData.characters = new List<CharacterData>
                {
                    new CharacterData { name = "Warrior", unlocked = true, level = 1, health = 100, attack = 10 },
                    new CharacterData { name = "Mage", unlocked = false, level = 0, health = 70, attack = 20 },
                    new CharacterData { name = "Rogue", unlocked = false, level = 0, health = 80, attack = 15 },
                    new CharacterData { name = "Archer", unlocked = false, level = 0, health = 90, attack = 12 }
                };
            saveData.weapons = new List<WeaponData>
                {
                    new WeaponData { name = "Plasma Gun", damage = 25, fireRate = 1.2f, ammoCapacity = 30, isUnlocked = true },
                    new WeaponData { name = "Laser Rifle", damage = 40, fireRate = 0.8f, ammoCapacity = 20, isUnlocked = false },
                    new WeaponData { name = "Shotgun", damage = 60, fireRate = 1.5f, ammoCapacity = 8, isUnlocked = false },
                    new WeaponData { name = "Rocket Launcher", damage = 100, fireRate = 2.0f, ammoCapacity = 5, isUnlocked = false }
                };

            SaveGame();
        }

    }
