using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEditor.Overlays;
using UnityEngine;

public class XMLGameSaveManager : MonoBehaviour
{
    public static XMLGameSaveManager Instance;

    public GameSaveData saveData = new GameSaveData();
    private string path;

    private CharacterUnlockData characterUnlockData;
    private WeaponUnlockData weaponUnlockData;

    void Awake()
    {
        //singleton for static reference
        if (Instance != null)
        {
           Destroy(Instance);
        }
        Instance = this;
        DontDestroyOnLoad(this);

        characterUnlockData = (CharacterUnlockData)Resources.Load("CharacterUnlockData");
        weaponUnlockData = (WeaponUnlockData)Resources.Load("WeaponUnlockData");

        path = Application.persistentDataPath + "/gamesave.xml";
        Debug.Log(path);
        LoadGame();
    }

    private void OnEnable()
    {
        LoadGame();
    }
    // writes current player data to save file
    public void SavePlayerData(PlayerSave playerSave)
    {
        saveData.playerSave = playerSave;

        // check if character or weapon should be unlocked at this level
        foreach(var unlockableCharacter in saveData.characters)
        {
            if (!unlockableCharacter.unlocked && unlockableCharacter.levelUnlocked <= playerSave.level)
            {
                UnlockCharacter(unlockableCharacter);
            }
        }
        foreach (var unlockableWeapon in saveData.weapons)
        {
            if (!unlockableWeapon.unlocked && unlockableWeapon.levelUnlocked <= playerSave.level)
            {
                UnlockWeapon(unlockableWeapon);
            }
        }

        // save to xml
        SaveGame();
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
    // overload to unlock specific character
    public void UnlockCharacter(CharacterData characterToUnlock)
    {
        foreach (var character in saveData.characters)
        {
            if (character == characterToUnlock)
            {
                character.unlocked = true;
                character.level = 1;
                SaveGame();
                Debug.Log($"Unlocked {character.name}!");
                return;
            }
        }
        Debug.Log("Character to unlock not found in save data!");
    }
    public void UnlockWeapon()
    {
        foreach (var weapon in saveData.weapons)
        {
            if (!weapon.unlocked)
            {
                weapon.unlocked = true;
                SaveGame();
                Debug.Log($"Unlocked {weapon.name}!");
                return;
            }
        }
        Debug.Log("All weapons are already unlocked!");
    }
    // overload to unlock specific weapon
    public void UnlockWeapon(WeaponData weaponToUnlock)
    {
        foreach (var weapon in saveData.weapons)
        {
            if (weapon == weaponToUnlock)
            {
                weapon.unlocked = true;
                SaveGame();
                Debug.Log($"Unlocked {weapon.name}!");
                return;
            }
        }
        Debug.Log("Weapon to unlock not found in save data!");
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
    public void InitializeGameData()
    {
        // write list of unlockable characters/weapons to save data
        if (weaponUnlockData != null && characterUnlockData != null)
        {
            //saveData.characters = characterUnlockData.characters;
            saveData.characters = new List<CharacterData>();
            foreach (var character in characterUnlockData.characters)
            {
                bool isUnlocked = character.levelUnlocked == 1;
                saveData.characters.Add(new CharacterData
                {
                    name = character.name,
                    unlocked = character.levelUnlocked == 1, // Automatically unlock level 1 characters
                    levelUnlocked = character.levelUnlocked,
                    level = character.levelUnlocked == 1 ? 1 : 0,
                    health = character.health,
                    attack = character.attack
                });
                if (isUnlocked)
                {
                    Debug.Log($"[Init] Unlocked character: {character.name} at level {character.levelUnlocked}");
                }
                else
                {
                    Debug.Log($"[Init] Locked character: {character.name}, unlocks at level {character.levelUnlocked}");
                }

            }
            saveData.weapons = weaponUnlockData.weapons;
        }
        // default/blank if no weapon/character data objects exists
        else
        {
            Debug.LogWarning("No weapon or character unlock data found.");
            saveData.characters = new List<CharacterData>
            {
                new CharacterData { name = "Earl", unlocked = true, levelUnlocked = 1, level = 1, health = 50, attack = 10 },
                new CharacterData { name = "Merlin", unlocked = false, levelUnlocked = 2, level = 0, health = 60, attack = 20 },
                new CharacterData { name = "Rodger", unlocked = false, levelUnlocked = 3, level = 0, health = 100, attack = 40 },
                //new CharacterData { name = "Archer", unlocked = false, level = 0, health = 90, attack = 12 }
            };
            saveData.weapons = new List<WeaponData>
            {
                new WeaponData { name = "Plasma Gun", damage = 25, fireRate = 1.2f, ammoCapacity = 30, unlocked = true },
                new WeaponData { name = "Laser Rifle", damage = 40, fireRate = 0.8f, ammoCapacity = 20, unlocked = false },
                new WeaponData { name = "Shotgun", damage = 60, fireRate = 1.5f, ammoCapacity = 8, unlocked = false },
                new WeaponData { name = "Rocket Launcher", damage = 100, fireRate = 2.0f, ammoCapacity = 5, unlocked = false }
            };
        }

        saveData.playerSave = new PlayerSave();
        saveData.playerSave.Reset();
        SaveGame();
    }

}
