using UnityEngine;
using UnityEngine.UI;

public class EmissionManager : MonoBehaviour
{
    public static EmissionManager Instance;

    [SerializeField] public int emission = 0;
    [SerializeField] private Text displayText;

    private Camera mainCamera;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (displayText != null)
        {
            displayText.text = $"Pollution: {emission}";
        }

        UpdateBackgroundColor();
    }

    private void UpdateBackgroundColor()
    {
        if (mainCamera == null) return;

        // Map pollution from 0 (dark green) to 2000 (brown)
        float t = Mathf.Clamp01(emission / 2000f);

        Color darkGreen = new Color(0f, 0.3f, 0f);   // Dark green
        Color brown = new Color(0.4f, 0.26f, 0.13f); // Brown

        Color backgroundColor = Color.Lerp(darkGreen, brown, t);
        mainCamera.backgroundColor = backgroundColor;
    }


    public void AddToValue(int amount)
    {
        emission += amount;
    }

    public void SubtractFromValue(int amount)
    {
        emission -= amount;
    }

    public int GetCurrentValue()
    {
        return emission;
    }
}
