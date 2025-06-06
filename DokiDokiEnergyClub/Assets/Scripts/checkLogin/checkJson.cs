// ============================================================
// Created by: Miro Vaassen
// File: CheckJson.cs
// Description: Checks JSON for username and hashed user ID.
// ============================================================

using UnityEngine;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

public class checkJson : MonoBehaviour
{
    private class SaveFileData
    {
        public string userId;
        public int money;
        public int electricity;
        public int polution;
        public bool x;
        public string y;
    }
    private string _filePath;
    private object _userId;

    void Start()
    {
        // Set the file path to the JSON file
        _filePath = Path.Combine(Application.persistentDataPath, "localData.json");
        Debug.Log("JSON file path: " + _filePath);

        if (!File.Exists(_filePath))
        {
            Debug.LogWarning("Local JSON file not found. creating a new one.");
            return;
        }
        else if (File.Exists(_filePath))
        {
            Debug.Log("Local JSON file found. checking db for changes.");
            // Load the local JSON file and check user credentials
            string jsonData = File.ReadAllText(_filePath);
            SaveFileData loaded = JsonUtility.FromJson<SaveFileData>(jsonData);
            if (!string.IsNullOrEmpty(loaded.userId))
            {
                _userId = loaded.userId;
                Debug.Log("Loaded userId from file: " + _userId);
                FetchAndSaveDataFromDb();
                return;
            }
        }
        else
        {
            Debug.LogError("Failed to find or create the local JSON file.");
        }

        // Load prefabs from JSON when the game starts
        PrefabLoader prefabLoader = FindFirstObjectByType<PrefabLoader>();
        if (prefabLoader != null)
        {
            prefabLoader.LoadPrefabsFromJson();
        }
        else
        {
            Debug.LogWarning("PrefabLoader component not found in the scene.");
        }
    }

    private async void FetchAndSaveDataFromDb()
    {
        // Example logic to fetch data from the database
        GetDataFromDb dbFetcher = new GetDataFromDb();
        string jsonData = await dbFetcher.FetchData("cityStats", _userId.ToString());

        if (!string.IsNullOrEmpty(jsonData))
        {
            File.WriteAllText(_filePath, jsonData);
            Debug.Log("Fetched data from DB and saved locally.");
        }
        else
        {
            Debug.LogError("Failed to fetch data from the database.");
        }
    }

    public bool CheckUser(string username, string userId)
    {
        try
        {
            if (!File.Exists(_filePath))
            {
                Debug.LogWarning("JSON file not found.");
                return false;
            }

            string jsonData = File.ReadAllText(_filePath);
            Debug.Log("Loaded JSON: " + jsonData);

            UserData data = JsonUtility.FromJson<UserData>(jsonData);

            if (data != null && data.username == username && data.hashedId == HashUserId(userId))
            {
                Debug.Log("User validated successfully.");
                return true;
            }
            else
            {
                Debug.LogWarning("User validation failed.");
                return false;
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error checking user: " + ex.Message);
            return false;
        }
    }

    private string HashUserId(string userId)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(userId));
            StringBuilder builder = new StringBuilder();
            foreach (byte b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }
            return builder.ToString();
        }
    }

    [System.Serializable]
    private class UserData
    {
        public string username;
        public string hashedId;
    }
}
