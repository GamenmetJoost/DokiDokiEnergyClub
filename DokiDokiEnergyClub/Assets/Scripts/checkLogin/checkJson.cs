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
    private string _filePath;

    void Start()
    {
        // Set the file path to the JSON file
        _filePath = Path.Combine(Application.persistentDataPath, "localData.json");
        Debug.Log("JSON file path: " + _filePath);

        if (!File.Exists(_filePath))
        {
            Debug.LogWarning("Local JSON file not found. Attempting to fetch data from the database...");
            // Add logic to fetch data from the database and save it locally
            FetchAndSaveDataFromDb();
        }
    }

    private async void FetchAndSaveDataFromDb()
    {
        // Example logic to fetch data from the database
        GetDataFromDb dbFetcher = new GetDataFromDb();
        string jsonData = await dbFetcher.FetchData("users");

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
