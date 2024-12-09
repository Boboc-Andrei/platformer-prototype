using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelBoundsGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    private BoundsInt bounds;
    public BoxCollider2D boundsCollider;

    void Awake()
    {
        bounds = tilemap.cellBounds;

        Vector2 min = (Vector2Int) bounds.min + Vector2.right;
        Vector2 max = (Vector2Int) bounds.max + Vector2.up * 100 + Vector2.left;


        boundsCollider.size = max - min;
        boundsCollider.offset = transform.InverseTransformPoint((min + max) / 2f);
    }

}
