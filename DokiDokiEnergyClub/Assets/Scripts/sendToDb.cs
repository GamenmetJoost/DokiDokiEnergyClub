using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class Sendtodb : MonoBehaviour
{
    private string _apiUrl;

    public Sendtodb()
    {
        // Replace with your backend service URL
        _apiUrl = "http://localhost:5001/data";
    }

    public void SendData<T>(string collectionName, T data)
    {
        var payload = new
        {
            collectionName = collectionName,
            data = data
        };

        string jsonData = JsonUtility.ToJson(payload);
        Debug.Log("Sending data: " + jsonData);
        SendDataToServer(jsonData);
    }

    private async void SendDataToServer(string jsonData)
    {
        using (UnityWebRequest request = new UnityWebRequest(_apiUrl, "POST"))
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
            }
        }
    }
}

