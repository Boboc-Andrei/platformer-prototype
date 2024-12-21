using System.Collections.Generic;
using SuperTiled2Unity.Editor;
using UnityEngine;

public class WorldMapper : MonoBehaviour
{
    public SuperAssetWorld world;

    private void Start() {
        world.InitializeScenesList();

        LevelTransition[] transitions = FindObjectsByType<LevelTransition>(FindObjectsSortMode.None);

        if(transitions?.Length > 0) {

        }
    }
}
