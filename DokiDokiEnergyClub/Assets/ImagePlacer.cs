using UnityEngine;

public class ImagePlacer : MonoBehaviour
{
    public GameObject imagePrefab;
    public float tileWidth = 3.2f;
    public float tileHeight = 1.6f;

    private bool isPlacing = false;

    public void StartPlacing() => isPlacing = true;

    void Update()
    {
        if (isPlacing && Input.GetMouseButtonDown(0))
        {
            Vector3 worldPos = GetMouseWorldPositionOnPlane();
            Vector2Int gridPos = WorldToIsoGrid(worldPos);
            Vector3 snappedWorldPos = IsoGridToWorld(gridPos);

            // Instantiate and configure rendering order
            GameObject newTile = Instantiate(imagePrefab, snappedWorldPos, Quaternion.identity);
            SetTileSortingOrder(newTile, gridPos);

            isPlacing = false;
        }
    }

    void SetTileSortingOrder(GameObject tile, Vector2Int gridPos)
    {
        SpriteRenderer renderer = tile.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            // Higher (gridX + gridY) = further back = lower sorting order
            renderer.sortingOrder = -(gridPos.x + gridPos.y);
        }
        else
        {
            Debug.LogWarning("Prefab has no SpriteRenderer component!");
        }
    }

    Vector3 GetMouseWorldPositionOnPlane()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.forward, Vector3.zero);
        return plane.Raycast(ray, out float distance) ? ray.GetPoint(distance) : Vector3.zero;
    }

    Vector2Int WorldToIsoGrid(Vector3 worldPos)
    {
        float gridX = (worldPos.x / (tileWidth * 0.5f) + worldPos.y / (tileHeight * 0.5f)) / 2;
        float gridY = (worldPos.y / (tileHeight * 0.5f) - worldPos.x / (tileWidth * 0.5f)) / 2;
        return new Vector2Int(Mathf.RoundToInt(gridX), Mathf.RoundToInt(gridY));
    }

    Vector3 IsoGridToWorld(Vector2Int gridPos)
    {
        float x = (gridPos.x - gridPos.y) * (tileWidth * 0.5f);
        float y = (gridPos.x + gridPos.y) * (tileHeight * 0.5f);
        return new Vector3(x, y, 0);
    }
}