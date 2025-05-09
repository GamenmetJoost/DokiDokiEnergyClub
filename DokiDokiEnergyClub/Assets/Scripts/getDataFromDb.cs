// ============================================================
// Created by: Miro Vaassen
// File: getDataFromDb.cs
// Description: Handles fetching data from the database.
// ============================================================

using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;

public class GetDataFromDb : MonoBehaviour
{
    private string _apiUrl;

    public GetDataFromDb()
    {
        // Replace with your backend service URL
        _apiUrl = "http://38.242.134.8:5001/data";
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
                return null;
            }
        }
    }
}
