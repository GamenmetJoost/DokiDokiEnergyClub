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
    private string Money;
    private int Electricity;
    private float Polution;
    private bool X;
    private string Y;

    void Start()
    {
        // Set the file path to save the JSON locally
        _filePath = Path.Combine(Application.persistentDataPath, "localData.json");
        Debug.Log("Local JSON file path: " + _filePath);
    }

    public void SaveDataToJson<T>(T data)
    {
        try
        {
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

    public void SaveAndSyncData<T>(T data)
    {
        SaveDataToJson(data);

        // Example logic to sync local data to the database
        Sendtodb dbSender = new Sendtodb();
        dbSender.SendData("users", data);
    }
}
