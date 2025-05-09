// ============================================================
// Created by: Miro Vaassen
// File: getDataFromDb.cs
// Description: Handles fetching data from the database.
// ============================================================

using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System.IO;

public class GetDataFromDb : MonoBehaviour
{
    private string _apiUrl;
    private string _localFilePath;

    public GetDataFromDb()
    {
        // Replace with your backend service URL
        _apiUrl = "http://38.242.134.8:5001/data";
        _localFilePath = Path.Combine(Application.persistentDataPath, "unsentData.json");
    }

    public async Task<string> FetchData(string collectionName)
    {
        if (string.IsNullOrEmpty(collectionName))
        {
            Debug.LogError("Collection name is required.");
            return null;
        }

        string url = $"{_apiUrl}/{collectionName}";
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SetRequestHeader("Content-Type", "application/json");

            var operation = request.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Data fetched successfully: " + request.downloadHandler.text);
                return request.downloadHandler.text;
            }
            else
            {
                Debug.LogError("Error fetching data: " + request.error);
                return LoadDataLocally();
            }
        }
    }

    private string LoadDataLocally()
    {
        try
        {
            if (File.Exists(_localFilePath))
            {
                string jsonData = File.ReadAllText(_localFilePath);
                Debug.Log("Loaded local data: " + jsonData);
                return jsonData;
            }
            else
            {
                Debug.LogWarning("No local data file found.");
                return null;
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error loading local data: " + ex.Message);
            return null;
        }
    }
}
