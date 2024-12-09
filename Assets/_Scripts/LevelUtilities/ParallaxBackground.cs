using System;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour {
    public Camera cam;
    public Transform subject;
    public const  float parallaxFarClipPlane = 50f;
    public const float parallaxNearClipPlane = 0.1f;

    private Vector2 startPosition;
    private float startZ;
    private Vector2 travel => (Vector2)cam.transform.position - startPosition;
    private float distanceFromSubject => transform.position.z - subject.position.z;
    private float clippingPlane => (cam.transform.position.z + distanceFromSubject > 0) ? parallaxFarClipPlane : parallaxNearClipPlane;
    private float parallaxFactor => MathF.Abs(distanceFromSubject) / clippingPlane;
    //public float parallaxFactor = 1;

    void Start() {
        startPosition = transform.position;
        startZ = transform.position.z;

    }

    void Update() {
        Vector2 newPos = startPosition + travel * parallaxFactor;
        transform.position = new Vector3(newPos.x, newPos.y, startZ);
    }
}
