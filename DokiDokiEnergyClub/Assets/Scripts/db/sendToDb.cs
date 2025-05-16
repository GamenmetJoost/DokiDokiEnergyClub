// ============================================================
// Created by: Miro Vaassen
// File: sendToDb.cs
// Description: Handles sending data to the database.
// ============================================================

using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.IO;

public class Sendtodb : MonoBehaviour
{
    private string _apiUrl;
    private string _localFilePath;

    public Sendtodb()
    {
        // Replace with your backend service URL
        _apiUrl = "https://mongodb.mirovaassen.nl/data";
        _localFilePath = Path.Combine(Application.persistentDataPath, "unsentData.json");
        Debug.Log("Local JSON file path: " + _localFilePath);
    }

    [System.Serializable]
    public class UserDataPayload
    {
        public string userId;
        public int money;
        public int electricity;
        public int polution;
        public bool x;
        public string y;
    }

    [System.Serializable]
    public class DbPayload
    {
        public string collectionName;
        public string userId; // <-- toegevoegd
        public UserDataPayload data;
    }

    public void SendData(string collectionName, string userId, UserDataPayload data)
    {
        string url = _apiUrl; // "https://mongodb.mirovaassen.nl/data"
        var payload = new DbPayload
        {
            collectionName = collectionName,
            userId = userId,
            data = data
        };

        // Log de payload als object
        Debug.Log("Payload object:");
        Debug.Log($"collectionName: {payload.collectionName}");
        Debug.Log($"userId: {payload.userId}");
        Debug.Log($"data.money: {payload.data.money}, data.electricity: {payload.data.electricity}, data.polution: {payload.data.polution}, data.x: {payload.data.x}, data.y: {payload.data.y}");

        string jsonData = JsonUtility.ToJson(payload, true);
        Debug.Log("JSON that will be sent to the server:");
        Debug.Log(jsonData);

        SaveDataLocally(jsonData);

        Debug.Log("Attempting to send data to the server.");
        SendDataToServer(url, jsonData);
    }

    private async void SendDataToServer(string url, string jsonData)
    {
        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            var operation = request.SendWebRequest();

            while (!operation.isDone)
                await System.Threading.Tasks.Task.Yield();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Data sent successfully: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Error sending data: " + request.error);
                SaveDataLocally(jsonData);
            }
        }
    }

    private void SaveDataLocally(string jsonData)
    {
        try
        {
            File.WriteAllText(_localFilePath, jsonData);
            Debug.Log("Data saved locally due to failed server request.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error saving data locally: " + ex.Message);
        }
    }
}

