using UnityEngine;
using UnityEngine.UI;

public class ImageSpawner : MonoBehaviour
{
    [Header("Assign your Image prefab and a UI container (like a Panel)")]
    public GameObject imagePrefab; // Drag your Image prefab here
    public RectTransform parentContainer; // Drag a UI object (like a Panel or the Canvas itself)

    [Header("How many images to spawn?")]
    public int requestedAmount = 23;

    [Header("Image layout settings")]
    public Vector2 imageSize = new Vector2(50, 50); // Width/Height of images
    public float spacing = 10f; // Space between images

    void Start()
    {
        int roundedAmount = Mathf.RoundToInt(requestedAmount / 10f) * 10;
        Debug.Log("Rounded to: " + roundedAmount);

        int imagesPerRow = 10;

        for (int i = 0; i < roundedAmount; i++)
        {
            GameObject newImage = Instantiate(imagePrefab, parentContainer);

            // Positioning in grid (row and column)
            int row = i / imagesPerRow;
            int col = i % imagesPerRow;

            RectTransform rt = newImage.GetComponent<RectTransform>();
            rt.sizeDelta = imageSize;
            rt.anchoredPosition = new Vector2(
                col * (imageSize.x + spacing),
                -row * (imageSize.y + spacing)
            );
        }
    }
}
