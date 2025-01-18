using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TerrainSensor : MonoBehaviour
{
    public new Collider2D collider;
    public LayerMask groundMask;

    public bool IsTouching = false;
    public float TimeSinceTouched;

    private void FixedUpdate() {
        bool touchingNow = Physics2D.OverlapAreaAll(collider.bounds.min, collider.bounds.max, groundMask).Length > 0;

        if (touchingNow) {
            Touching();
        }
        else {
            NotTouching();
        }

    }

    private void Touching() {
        TimeSinceTouched = 0;
        IsTouching = true;
    }

    private void NotTouching() {
        TimeSinceTouched += Time.fixedDeltaTime;
        IsTouching = false;
    }

    public bool IsTouchingLayer(string layer) {
        int layerIndex = LayerMask.NameToLayer(layer);

        if (layerIndex < 0) {
            Debug.LogWarning($"Layer {layer} does not exist");
            return false;
        }
        int layerMask = 1 << layerIndex;
        return Physics2D.OverlapAreaAll(collider.bounds.min, collider.bounds.max, layerMask).Length > 0;
    }
}
