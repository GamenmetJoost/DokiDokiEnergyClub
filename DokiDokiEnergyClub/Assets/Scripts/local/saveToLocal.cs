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
        try
        {
            // Create an object to store the variables
            var data = new
            {
                Money,
                Electricity,
                Polution,
                X,
                Y
            };

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

        // Sync local data to the database
        var data = new
        {
            Money,
            Electricity,
            Polution,
            X,
            Y
        };

        Sendtodb dbSender = new Sendtodb();
        dbSender.SendData("users", data);
    }
}
