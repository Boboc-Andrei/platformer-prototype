using SuperTiled2Unity.Editor.LibTessDotNet;
using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    #region Singleton Boilerplate
    private static GameManager _instance;
    public static GameManager Instance { get; private set; }

    private void SingletonInitialization() {
        if (_instance == null) {
            _instance = this;
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    [SerializeField] private PlayerMovement playerPrefab;
    [SerializeField] private CinemachineCamera cameraPrefab;

    public PlayerMovement Player;
    public CinemachineCamera currentCamera;

    public bool spawnPlayerOnStart = true;
    private Vector3 playerSpawnPosition;

    public Dictionary<string, LevelTransition> levelTransitions;

    private string spawnGateId = "";
    private Vector2 spawnPositionOffset;
    private Vector2 cameraPositionOffset;

    private void Awake() {
        SingletonInitialization();

        if (spawnGateId != "") {

            playerSpawnPosition = (Vector2)levelTransitions[spawnGateId].transform.position + spawnPositionOffset;
        }

        if (spawnPlayerOnStart) {
            SpawnPlayer();
        }

        if (Player != null) {
            CameraSetup();
        }
    }

    public void CameraSetup() {

        currentCamera = FindFirstObjectByType<CinemachineCamera>();
        currentCamera.Target.TrackingTarget = Player.transform;
        currentCamera.transform.position = (Vector2)Player.transform.position + cameraPositionOffset;
    }


    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void Start() {

    }

    private void OnSceneLoaded(Scene nextScene, LoadSceneMode sceneMode) {
        LevelTransitionsSetup();

        if (spawnGateId == "") {
            return;
        }

        print($"on scene loaded player is at {Player.transform.position}");

        Player.transform.position = (Vector2)levelTransitions[spawnGateId].transform.position + spawnPositionOffset;

        print($"on scene loaded player teleported at {Player.transform.position}");

        levelTransitions[spawnGateId].isActive = false;

        CameraSetup();
    }

    private void OnSceneUnloaded(Scene nextScene) {

    }

    private void LevelTransitionsSetup() {
        levelTransitions = new Dictionary<string, LevelTransition>();

        foreach (var levelTransition in FindObjectsByType<LevelTransition>(FindObjectsSortMode.None)) {
            levelTransitions[levelTransition.FromGateID] = levelTransition;
        }
    }


    private void SpawnPlayer() {
        playerSpawnPosition = FindFirstObjectByType<PlayerSpawnPoint>().transform.position;
        playerSpawnPosition = playerSpawnPosition != null ? playerSpawnPosition : Vector3.zero;
        Player = FindFirstObjectByType<PlayerMovement>();

        if (Player == null) {
            Player = Instantiate(playerPrefab, playerSpawnPosition, Quaternion.identity);
        }
    }


    public void LevelTransition(string fromGateID) {

        spawnGateId = levelTransitions[fromGateID].ToGateID;
        LevelTransition levelTransition = levelTransitions[fromGateID];
        spawnPositionOffset = Player.transform.position - levelTransition.transform.position;
        cameraPositionOffset = currentCamera.transform.position - Player.transform.position;
        print($"loading scene {levelTransition.ToScene}");
        SceneManager.LoadScene(levelTransition.ToScene);
    }


}
