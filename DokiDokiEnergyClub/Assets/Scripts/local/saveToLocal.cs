// ============================================================
// Created by: Miro Vaassen
// File: saveToLocal.cs
// Description: Handles saving and loading data locally in JSON format.
// ============================================================

using UnityEngine;
using System.IO;

public class saveToLocal : MonoBehaviour
{
    private string _filePath;

    // New variables
    private int Money;
    private int Electricity;
    private int Polution;
    private bool X;
    private string Y;

    void Start()
    {
        // Set the file path to save the JSON locally
        _filePath = Path.Combine(Application.persistentDataPath, "localData.json");
        Debug.Log("Local JSON file path: " + _filePath);

        // Initialize variables
        Money = MoneyManager.Instance.GetCurrentValue();
        Electricity = PowerManager.Instance.GetCurrentValue();
        Polution = EmissionManager.Instance.GetCurrentValue();
        X = true; // Example value
        Y = "ExampleString"; // Example value
    }

    public void SaveDataToJson()
    {
        Money = MoneyManager.Instance.GetCurrentValue();
        Electricity = PowerManager.Instance.GetCurrentValue();
        Polution = EmissionManager.Instance.GetCurrentValue();
        X = true; // Example value
        Y = "ExampleString"; // Example value
        try
        {
            var data = new Sendtodb.UserDataPayload
            {
                money = Money,
                electricity = Electricity,
                polution = Polution,
                x = X,
                y = Y
            };
            Debug.Log("Saving data to JSON: " + JsonUtility.ToJson(data, true));

            string jsonData = JsonUtility.ToJson(data, true);
            File.WriteAllText(_filePath, jsonData);
            Debug.Log("Data saved locally to JSON: " + jsonData);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error saving data to JSON: " + ex.Message);
        }
    }

    public T LoadDataFromJson<T>()
    {
        try
        {
            if (File.Exists(_filePath))
            {
                string jsonData = File.ReadAllText(_filePath);
                Debug.Log("Data loaded from JSON: " + jsonData);
                return JsonUtility.FromJson<T>(jsonData);
            }
            else
            {
                Debug.LogWarning("No local JSON file found.");
                return default;
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error loading data from JSON: " + ex.Message);
            return default;
        }
    }

    public void SaveAndSyncData()
    {
        SaveDataToJson();

        var data = new Sendtodb.UserDataPayload
        {
            money = Money,
            electricity = Electricity,
            polution = Polution,
            x = X,
            y = Y
        };

        Sendtodb dbSender = new Sendtodb();
        dbSender.SendData("users", data); // "users" is de collectionName
    }
}

// ============================================================
// Gemaakt door: Miro Vaassen
// ============================================================
