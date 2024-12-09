using SuperTiled2Unity.Editor.LibTessDotNet;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public Vector3 playerPosition;
    public Vector3 playerVelocity;

    public GameData() {
        playerPosition = Vector3.zero;
        playerVelocity = Vector3.zero;
    }
}
