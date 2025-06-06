// ============================================================
// Created by: Miro Vaassen
// File: saveToLocal.cs
// Description: Handles saving and loading data locally in JSON format.
// ============================================================

using UnityEngine;
using System.IO;
using System;

public class saveToLocal : MonoBehaviour
{
    private string _filePath;
    private string _userId;

    // Add static accessor for userId
    public static string CurrentUserId { get; private set; }

    // New variables
    private int Money;
    private int Electricity;
    private int Polution;

    // Add prefab data list
    private PrefabDataList prefabDataList = new PrefabDataList();

    [System.Serializable]
    private class SaveFileData
    {
        public string userId;
        public int money;
        public int electricity;
        public int polution;
        public PrefabDataList prefabData; // Add this line
    }

    void Start()
    {
        // Set the file path to save the JSON locally
        _filePath = Path.Combine(Application.persistentDataPath, "localData.json");
        Debug.Log("Local JSON file path: " + _filePath);

        // Try to load userId from file, else generate and save
        LoadOrCreateUserId();

        // Load saved values and set them in the managers
        LoadStatsFromSave();

        // Initialize variables (optional, will be overwritten by LoadStatsFromSave if file exists)
        Money = MoneyManager.Instance.GetCurrentValue();
        Electricity = PowerManager.Instance.GetCurrentValue();
        Polution = EmissionManager.Instance.GetCurrentValue();
    }

    private void LoadOrCreateUserId()
    {
        if (File.Exists(_filePath))
        {
            string jsonData = File.ReadAllText(_filePath);
            SaveFileData loaded = JsonUtility.FromJson<SaveFileData>(jsonData);
            if (!string.IsNullOrEmpty(loaded.userId))
            {
                _userId = loaded.userId;
                CurrentUserId = _userId; // Set static property
                Debug.Log("Loaded userId from file: " + _userId);
                return;
            }
        }
        // Generate new userId
        _userId = Guid.NewGuid().ToString();
        CurrentUserId = _userId; // Set static property
        Debug.Log("Generated new userId: " + _userId);
        // Save immediately so it's persisted
        SaveDataToJson();
    }

    private void LoadStatsFromSave()
    {
        if (File.Exists(_filePath))
        {
            string jsonData = File.ReadAllText(_filePath);
            SaveFileData loaded = JsonUtility.FromJson<SaveFileData>(jsonData);
            if (loaded != null)
            {
                // Set values in managers if possible
                if (MoneyManager.Instance != null)
                    MoneyManager.Instance.money = loaded.money;
                if (PowerManager.Instance != null)
                    PowerManager.Instance.power = loaded.electricity;
                if (EmissionManager.Instance != null)
                    EmissionManager.Instance.emission = loaded.polution;

                Debug.Log($"Loaded stats from save: money={loaded.money}, electricity={loaded.electricity}, polution={loaded.polution}");
                // Load prefab data
                prefabDataList = loaded.prefabData ?? new PrefabDataList();
            }
        }
    }

    public void SaveDataToJson()
    {
        Money = MoneyManager.Instance.GetCurrentValue();
        Electricity = PowerManager.Instance.GetCurrentValue();
        Polution = EmissionManager.Instance.GetCurrentValue();

        // Fetch prefab data from PrefabSerializer
        PrefabSerializer prefabSerializer = FindFirstObjectByType<PrefabSerializer>();
        if (prefabSerializer != null)
        {
            prefabDataList = prefabSerializer.GetAllPrefabInstances();
        }
        else
        {
            Debug.LogWarning("PrefabSerializer component not found in the scene.");
            prefabDataList = new PrefabDataList();
        }

        try
        {
            var data = new SaveFileData
            {
                userId = _userId,
                money = Money,
                electricity = Electricity,
                polution = Polution,
                prefabData = prefabDataList // Save prefab data in the same file
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

        // Fetch prefab data from PrefabSerializer
        PrefabSerializer prefabSerializer = FindFirstObjectByType<PrefabSerializer>();
        if (prefabSerializer != null)
        {
            prefabDataList = prefabSerializer.GetAllPrefabInstances();
        }
        else
        {
            Debug.LogWarning("PrefabSerializer component not found in the scene.");
            prefabDataList = new PrefabDataList();
        }

        var data = new Sendtodb.UserDataPayload
        {
            userId = _userId,
            money = Money,
            electricity = Electricity,
            polution = Polution,
            prefabData = prefabDataList // <-- Add this line
        };

        Sendtodb dbSender = new Sendtodb();
        dbSender.SendData("cityStats", _userId, data); // collectionName, userId, data
    }

    public void SetPrefabData(PrefabDataList data)
    {
        prefabDataList = data;
        SaveDataToJson();
    }

    public PrefabDataList GetPrefabData()
    {
        return prefabDataList;
    }

    public string GetUserId()
    {
        return _userId;
    }
}

// ============================================================
// Gemaakt door: Miro Vaassen
// ============================================================
