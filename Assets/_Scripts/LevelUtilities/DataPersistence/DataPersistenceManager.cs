using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;
using UnityEngine.Windows;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour {
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    public bool persistData;

    private GameData gameData;
    private GameData defaultGameData;
    private List<IPersistentData> persistentDataObjects;
    private GameDataFileHandler dataFileHandler;
    public static DataPersistenceManager Instance { get; private set; }


    private void Awake() {
        DontDestroyOnLoad(gameObject);

        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        dataFileHandler = new GameDataFileHandler(Application.persistentDataPath, fileName);
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        this.persistentDataObjects = FindAllPersistentDataObjects();
        SetDefaultData();
        LoadGame();        
    }

    private void Start() {
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }


    public void OnSceneUnloaded(Scene scene) {
        SaveGame();
    }

    private List<IPersistentData> FindAllPersistentDataObjects() {
        IEnumerable<IPersistentData> persistentDataObjects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IPersistentData>();
        return new List<IPersistentData>(persistentDataObjects);
    }

    public void NewGame() {
        gameData = defaultGameData;
    }

    public void LoadGame() {
        this.gameData = dataFileHandler.Load();

        if (gameData == null || !persistData) {
            NewGame();
        }

        foreach (IPersistentData persistentDataObject in persistentDataObjects) {
            persistentDataObject.LoadPersistentData(gameData);
        }
    }

    public void SaveGame() {
        foreach (IPersistentData persistentDataObject in persistentDataObjects) {
            persistentDataObject.SavePersistentData(ref gameData);
        }

        dataFileHandler.Save(gameData);
    }

    public void ClearSave() {
            dataFileHandler.Clear();
    }

    public void SetDefaultData() {
        defaultGameData = new GameData();
        foreach (IPersistentData persistentDataObject in persistentDataObjects) {
            persistentDataObject.SetDefaultPersistentData(ref defaultGameData);
        }
    }

    private void OnApplicationQuit() {
        if (persistData) {
            SaveGame();
        }
        else {
            ClearSave();
        }
    }
}
