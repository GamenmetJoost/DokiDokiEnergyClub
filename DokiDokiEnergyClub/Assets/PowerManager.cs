using UnityEngine;
using UnityEngine.UI;

public class PowerManager : MonoBehaviour
{
    public static PowerManager Instance; // Singleton pattern

    [SerializeField] private int currentValue = 0;
    [SerializeField] private Text displayText;

    void Awake()
    {
        // Singleton setup
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

    void Update()
    {
        // Update display text every frame
        if (displayText != null)
        {
            displayText.text = $"Power: {currentValue}";
        }
    }

    public void AddToValue(int amount)
    {
        currentValue += amount;
    }

    public void SubtractFromValue(int amount)
    {
        currentValue -= amount;
    }

    // For external access to the current value
    public int GetCurrentValue()
    {
        return currentValue;
    }
}