using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance;

    [SerializeField] public int money = 0;
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
            displayText.text = $"Money: {money}";
        }

    }

    public void AddToValue(int amount)
    {
        money += amount;
    }

    public void SubtractFromValue(int amount)
    {
        money -= amount;
    }

    public int GetCurrentValue()
    {
        return money;
    }
}
