using System;
using System.Collections;
using System.Security.Cryptography;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class WarpBoundary : MonoBehaviour {
    public enum Direction { Left = -1, Right = 1 }

    [SerializeField] private WarpBoundary otherTrigger;
    [SerializeField] public Direction directionToLoop;

    private bool teleporting = false;
    private Rigidbody2D player;
    private Vector3 oldPosition;

    private LevelWarpManager warpManager;

    private void Start() {

    }

    public void FixedUpdate() {
        if (teleporting) {
            teleporting = false;
            warpManager.TeleportPlayer(transform, otherTrigger.transform);

        }
    }

    public void Init(LevelWarpManager warpManager) {
        this.warpManager = warpManager;
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player") && Mathf.Sign(collision.attachedRigidbody.linearVelocityX) == (int)directionToLoop) {
            player = collision.transform.parent.GetComponent<Rigidbody2D>();
            oldPosition = player.position;
            teleporting = true;
        }
    }
}
