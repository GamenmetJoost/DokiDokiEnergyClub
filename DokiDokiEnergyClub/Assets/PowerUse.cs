using UnityEngine;
using UnityEngine.SceneManagement;

public class PowerUse : MonoBehaviour
{
    [SerializeField] private int changeAmount = 1; // Added declaration
    [SerializeField] private float addInterval = 1f; // Seconds between additions
    [SerializeField] private emmisionAmount emmisionAmount = new(); // Reference to the emmisionAmount script
    private float timer = 0f;

        void Update()
    {
        // Only add value at intervals (not every frame)
        timer += Time.deltaTime;
        if (timer >= addInterval)
        {
            if (PowerManager.Instance != null) // Null check
            {
                PowerManager.Instance.AddToValue(changeAmount);
            }
            else
            {
                Debug.LogError("ScoreManager instance not found!");
            }
            timer = 0f; // Reset timer
        }
    }

    public void SetPowerOutput(float newOutput)
    {
        changeAmount = (int)newOutput;
    }

    public float GetPowerOutput()
    {
        return changeAmount;
    }

    public void SetPollutionOutput(float newOutput)
    {
        emmisionAmount.SetPollutionOutput(newOutput);
    }

    public float GetPollutionOutput()
    {
        return emmisionAmount.GetPollutionOutput();
    }
}