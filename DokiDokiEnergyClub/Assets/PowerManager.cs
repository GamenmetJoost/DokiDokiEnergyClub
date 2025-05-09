using UnityEngine;
using UnityEngine.UI;

public class PowerManager : MonoBehaviour
{
    public static PowerManager Instance; // Singleton pattern

    [SerializeField] public int power = 0;
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
            displayText.text = $"Power: {power}";
        }
    }

    public void AddToValue(int amount)
    {
        power += amount;
    }

    public void SubtractFromValue(int amount)
    {
        power -= amount;
    }

    // For external access to the current value
    public int GetCurrentValue()
    {
        return power;
    }
}