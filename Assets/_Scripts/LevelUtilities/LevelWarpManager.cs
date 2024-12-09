using System;
using Unity.Cinemachine;
using UnityEngine;
using System.Collections.Generic;

public class LevelWarpManager : MonoBehaviour {
    [SerializeField] WarpBoundary warpGateLeft;
    [SerializeField] WarpBoundary warpGateRight;
    [SerializeField] private Transform player;
    [SerializeField] private Transform dummyPlayer;
    [SerializeField] private CinemachineCamera playerCamera;
    [SerializeField] private CinemachineCamera dummyCamera;
    public float Displacement => Mathf.Abs(warpGateLeft.transform.position.x - warpGateRight.transform.position.x);

    private Queue<Action> teleportQueue = new Queue<Action>();

    private void Start() {
        warpGateLeft.Init(this);
        warpGateRight.Init(this);
    }

    private void LateUpdate() {
        while (teleportQueue.Count > 0) {
            teleportQueue.Dequeue()?.Invoke();
        }
    }

    public void QueueLateUpdateAction(Action callback) {
        teleportQueue.Enqueue(callback);
    }

    private void SwapCameras() {

        QueueLateUpdateAction(() => {
            (playerCamera, dummyCamera) = (dummyCamera, playerCamera);
            playerCamera.Target.TrackingTarget = player;
            dummyCamera.Target.TrackingTarget = dummyPlayer;
            playerCamera.Priority = 1;
            dummyCamera.Priority = 0;
        });
    }

    public void TeleportPlayer(Transform from, Transform to) {
        (player.position, dummyPlayer.position) = (dummyPlayer.position, player.position);
        SwapCameras();
    }
}
