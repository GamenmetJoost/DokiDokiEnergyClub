using UnityEngine;
using UnityEngine.SceneManagement;

public class emmisionAmount : MonoBehaviour
{
    [SerializeField] private int changeAmount = 1; // Added declaration
    [SerializeField] private float addInterval = 1f; // Seconds between additions
    private float timer = 0f;

    void Update()
    {
        // Only add value at intervals (not every frame)
        timer += Time.deltaTime;
        if (timer >= addInterval)
        {
            if (EmissionManager.Instance != null) // Null check
            {
                EmissionManager.Instance.AddToValue(changeAmount);
            }
            else
            {
                Debug.LogError("ScoreManager instance not found!");
            }
            timer = 0f; // Reset timer
        }
    }

    public void SetPollutionOutput(float newOutput)
    {
        changeAmount = (int)newOutput;
    }

    public float GetPollutionOutput()
    {
        return changeAmount;
    }
}
