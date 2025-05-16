using NUnit.Framework.Constraints;
using UnityEngine;

public class UpgradesScript : MonoBehaviour
{
    void Start()
    {
    }

    void Update()
    {
    }

    public void UpgradePowerOutput25Percent()
    {
        int money = MoneyManager.Instance.GetCurrentValue();

        int cost = 0; // Cost of the upgrade !! set to 100 after testing
        if (money >= cost)
        {
            MoneyManager.Instance.SubtractFromValue(cost);

            // Find all PowerUse components in the scene
            PowerUse[] allPowerUses = FindObjectsOfType<PowerUse>();
            if (allPowerUses.Length > 0)
            {
                foreach (var powerUse in allPowerUses)
                {
                    float currentOutput = powerUse.GetPowerOutput();
                    float upgradedOutput = currentOutput * 1.25f;
                    powerUse.SetPowerOutput(upgradedOutput);
                }
                Debug.Log("All building power outputs upgraded by 25%.");
            }
            else
            {
                Debug.LogError("No PowerUse instances found.");
            }
        }
        else
        {
            Debug.Log("Not enough power to upgrade all building outputs.");
        }
    }

    public void ReduceEmissionOutput25Percent()
    {
        int money = MoneyManager.Instance.GetCurrentValue();

        int cost = 0; // Cost of the upgrade !! set to 100 after testing
        if (money >= cost)
        {
            MoneyManager.Instance.SubtractFromValue(cost);

            // Find all PowerUse components in the scene
            PowerUse[] allPowerUses = FindObjectsOfType<PowerUse>();
            if (allPowerUses.Length > 0)
            {
                foreach (var powerUse in allPowerUses)
                {
                    float currentPollution = powerUse.GetPollutionOutput();
                    float reducedPollution = currentPollution * 0.75f;
                    powerUse.SetPollutionOutput(reducedPollution);
                }
                Debug.Log("All building pollution outputs reduced by 25%.");
            }
            else
            {
                Debug.LogError("No PowerUse instances found.");
            }
        }
        else
        {
            Debug.Log("Not enough power to reduce all building pollution outputs.");
        }
    }

    // Reset all upgrades
    public void ResetUpgrades()
    {
        Debug.Log("Upgrades reset to default values.");
    }
} 
