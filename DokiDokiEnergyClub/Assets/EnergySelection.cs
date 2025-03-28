using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnergySelection : MonoBehaviour
{
    public TextMeshProUGUI selectionText;

    // Energiecentrales lijst
    private PowerPlant[] powerPlants = {
        new PowerPlant("Zonnecentrale", "Laag", "Duurzaam"),
        new PowerPlant("Windcentrale", "Gemiddeld", "Duurzaam"),
        new PowerPlant("Waterkrachtcentrale", "Hoog", "Duurzaam"),
        new PowerPlant("Kolencentrale", "Hoog", "Niet duurzaam")
    };

    // Wordt gekoppeld aan knoppen in de Unity Editor
    public void SelectPowerPlant(int index)
    {
        if (index >= 0 && index < powerPlants.Length)
        {
            PowerPlant selectedPlant = powerPlants[index];
            selectionText.text = $"Gekozen: {selectedPlant.Name}\nEfficiëntie: {selectedPlant.Efficiency}\nType: {selectedPlant.Type}";
        }
    }
}

// Klasse voor energiecentrales
public class PowerPlant
{
    public string Name { get; }
    public string Efficiency { get; }
    public string Type { get; }

    public PowerPlant(string name, string efficiency, string type)
    {
        Name = name;
        Efficiency = efficiency;
        Type = type;
    }
}
