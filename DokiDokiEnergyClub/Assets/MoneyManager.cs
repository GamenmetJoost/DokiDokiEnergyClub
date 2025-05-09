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
            displayText.text = $"money: {money}";
        }

        UpdateBackgroundColor();
    }

    private void UpdateBackgroundColor()
    {
        if (mainCamera == null) return;

        // Map pollution from 0 (green) to 100 (red)
        float t = Mathf.Clamp01(money / 2000f);
        Color backgroundColor = Color.Lerp(Color.green, Color.red, t);
        mainCamera.backgroundColor = backgroundColor;
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
