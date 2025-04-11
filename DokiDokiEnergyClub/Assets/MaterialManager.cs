using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    public static MaterialManager Instance;

    public int Money = 100; // Starting amount
    public int Electricity = 100; // Starting amount

    void Awake()
    {
        // Make sure there's only one instance
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Keep across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }
}