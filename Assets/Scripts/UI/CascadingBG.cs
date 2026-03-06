using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach to a GameObject that has a SpriteRenderer. Creates child tiles that
/// continuously move in a direction and wrap to create an infinite cascading background.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class CascadingBG : MonoBehaviour
{
    public enum Axis
    {
        Vertical,
        Horizontal
    }

    private Vector2 direction = new();

    [Tooltip("Movement speed in world units per second.")]
    public float speed = 1f;

    [Tooltip("Which axis the tiling runs along.")]
    public Axis axis = Axis.Vertical;

    [Tooltip("Extra tiles to create above the minimum to ensure seamless coverage.")]
    [Range(1, 6)]
    public int extraTiles = 2;

    // Internal
    SpriteRenderer _sourceRenderer;
    readonly List<Transform> _tiles = new List<Transform>();
    Vector2 _tileSizeWorld = Vector2.one;
    Camera _cam;
    int _tileCount = 0;

    void Awake()
    {
        _sourceRenderer = GetComponent<SpriteRenderer>();
        _cam = Camera.main;
        if (_sourceRenderer == null)
        {
            Debug.LogError("CascadingBG requires a SpriteRenderer on the same GameObject.");
            enabled = false;
            return;
        }
    }

    void Start()
    {
        // Keep direction consistent with axis
        if (axis == Axis.Vertical)
        {
            direction.x = 0f;
            if (direction.y == 0f) direction.y = -1f;
        }
        else
        {
            direction.y = 0f;
            if (direction.x == 0f) direction.x = -1f;
        }

        RebuildTiles();
    }

    void Update()
    {
        if (_tiles.Count == 0)
            return;

        Vector3 move = (Vector3)direction.normalized * speed * Time.deltaTime;

        // Move all tiles
        foreach (var t in _tiles)
            t.position += move;

        // Wrapping logic
        if (axis == Axis.Vertical)
            WrapVertical();
        else
            WrapHorizontal();
    }

    void RebuildTiles()
    {
        // Clear previously created tiles
        for (int i = _tiles.Count - 1; i >= 0; i--)
        {
            if (_tiles[i] != null)
                DestroyImmediate(_tiles[i].gameObject);
        }
        _tiles.Clear();

        if (_sourceRenderer.sprite == null)
            return;

        // Compute sprite size in world units (taking lossyScale into account)
        Vector2 spriteSize = _sourceRenderer.sprite.bounds.size;
        Vector3 lossyScale = transform.lossyScale;
        _tileSizeWorld = new Vector2(spriteSize.x * Mathf.Abs(lossyScale.x), spriteSize.y * Mathf.Abs(lossyScale.y));

        // Determine how many tiles are needed to cover the camera view on the chosen axis
        if (_cam == null)
            _cam = Camera.main;

        float visibleAlongAxis = 0f;
        if (_cam != null && _cam.orthographic)
        {
            if (axis == Axis.Vertical)
                visibleAlongAxis = _cam.orthographicSize * 2f;
            else
                visibleAlongAxis = _cam.orthographicSize * 2f * _cam.aspect;
        }
        else
        {
            // Fallback: use one tile plus extras
            visibleAlongAxis = (axis == Axis.Vertical) ? _tileSizeWorld.y : _tileSizeWorld.x;
        }

        float tileAxisSize = (axis == Axis.Vertical) ? _tileSizeWorld.y : _tileSizeWorld.x;
        _tileCount = Mathf.Max(2, Mathf.CeilToInt(visibleAlongAxis / tileAxisSize) + extraTiles);

        // Create tiles centered around this GameObject's position
        Vector3 origin = transform.position;
        float half = (_tileCount - 1) * 0.5f * tileAxisSize;

        for (int i = 0; i < _tileCount; i++)
        {
            GameObject go = new GameObject($"BGTile_{i}");
            go.transform.parent = transform;
            var sr = go.AddComponent<SpriteRenderer>();

            // Copy rendering properties from source renderer
            sr.sprite = _sourceRenderer.sprite;
            sr.sharedMaterial = _sourceRenderer.sharedMaterial;
            sr.color = _sourceRenderer.color;
            sr.flipX = _sourceRenderer.flipX;
            sr.flipY = _sourceRenderer.flipY;
            sr.sortingLayerID = _sourceRenderer.sortingLayerID;
            sr.sortingOrder = _sourceRenderer.sortingOrder;

            // Place tile
            Vector3 pos = origin;
            if (axis == Axis.Vertical)
                pos.y += (i * tileAxisSize) - half;
            else
                pos.x += (i * tileAxisSize) - half;

            go.transform.position = pos;
            go.transform.rotation = transform.rotation;
            // Keep local scale so sprite renders at intended world size
            go.transform.localScale = Vector3.one;
            _tiles.Add(go.transform);
        }
    }

    void WrapVertical()
    {
        float tileH = _tileSizeWorld.y;
        if (_tiles.Count == 0) return;

        // Calculate camera bounds in world space
        float camCenterY = (_cam != null) ? _cam.transform.position.y : 0f;
        float halfView = (_cam != null && _cam.orthographic) ? _cam.orthographicSize : tileH;
        float topLimit = camCenterY + halfView + tileH * 0.5f;
        float bottomLimit = camCenterY - halfView - tileH * 0.5f;

        // If moving down, when a tile falls below bottomLimit, move it to top
        if (direction.y < 0f)
        {
            for (int i = 0; i < _tiles.Count; i++)
            {
                var t = _tiles[i];
                if (t.position.y < bottomLimit)
                {
                    // Find current max Y among tiles
                    float maxY = float.MinValue;
                    foreach (var other in _tiles) if (other.position.y > maxY) maxY = other.position.y;
                    t.position = new Vector3(t.position.x, maxY + tileH, t.position.z);
                }
            }
        }
        else if (direction.y > 0f)
        {
            for (int i = 0; i < _tiles.Count; i++)
            {
                var t = _tiles[i];
                if (t.position.y > topLimit)
                {
                    float minY = float.MaxValue;
                    foreach (var other in _tiles) if (other.position.y < minY) minY = other.position.y;
                    t.position = new Vector3(t.position.x, minY - tileH, t.position.z);
                }
            }
        }
    }

    void WrapHorizontal()
    {
        float tileW = _tileSizeWorld.x;
        if (_tiles.Count == 0) return;

        float camCenterX = (_cam != null) ? _cam.transform.position.x : 0f;
        float halfView = (_cam != null && _cam.orthographic) ? _cam.orthographicSize * _cam.aspect : tileW;
        float rightLimit = camCenterX + halfView + tileW * 0.5f;
        float leftLimit = camCenterX - halfView - tileW * 0.5f;

        if (direction.x < 0f)
        {
            for (int i = 0; i < _tiles.Count; i++)
            {
                var t = _tiles[i];
                if (t.position.x < leftLimit)
                {
                    float maxX = float.MinValue;
                    foreach (var other in _tiles) if (other.position.x > maxX) maxX = other.position.x;
                    t.position = new Vector3(maxX + tileW, t.position.y, t.position.z);
                }
            }
        }
        else if (direction.x > 0f)
        {
            for (int i = 0; i < _tiles.Count; i++)
            {
                var t = _tiles[i];
                if (t.position.x > rightLimit)
                {
                    float minX = float.MaxValue;
                    foreach (var other in _tiles) if (other.position.x < minX) minX = other.position.x;
                    t.position = new Vector3(minX - tileW, t.position.y, t.position.z);
                }
            }
        }
    }
}
