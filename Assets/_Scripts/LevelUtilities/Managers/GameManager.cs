using SuperTiled2Unity.Editor.LibTessDotNet;
using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    private static GameManager _instance;
    public static GameManager Instance { get; private set; }

    [SerializeField] private PlayerMovement playerPrefab;
    [SerializeField] private CinemachineCamera cameraPrefab;


    public PlayerMovement Player;
    public CinemachineCamera currentCamera;
    public bool spawnPlayerOnStart = true;
    private Vector3 playerSpawnPosition;
    public Dictionary<int, LevelTransition> levelTransitions;

    private int spawnGateId = -1;
    private Vector2 spawnPositionOffset;
    private Vector2 cameraPositionOffset;

    private void Awake() {
        if (_instance == null) {
            _instance = this;
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        if (spawnGateId > -1) {

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

        if (spawnGateId == -1) {
            return;
        }

        Player.rigidBody.position = (Vector2)levelTransitions[spawnGateId].transform.position + spawnPositionOffset;
        levelTransitions[spawnGateId].isActive = false;

        CameraSetup();
    }

    private void OnSceneUnloaded(Scene nextScene) {

    }

    private void LevelTransitionsSetup() {
        levelTransitions = new Dictionary<int, LevelTransition>();

        foreach (var levelTransition in FindObjectsByType<LevelTransition>(FindObjectsSortMode.None)) {
            levelTransitions[levelTransition.id] = levelTransition;
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


    public void LevelTransition(int transitionId) {
        spawnGateId = transitionId;
        LevelTransition levelTransition = levelTransitions[transitionId];
        spawnPositionOffset = Player.transform.position - levelTransition.transform.position;
        cameraPositionOffset = currentCamera.transform.position - Player.transform.position;
        SceneManager.LoadScene(levelTransition.nextScene);
    }


}
