using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class PrefabSerializer : MonoBehaviour
{
    public string fileName = "prefabsData.json";

    // Auto-detect all objects with a specific tag or name pattern
    public void SaveAllPrefabInstances()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        List<PrefabData> savedPrefabs = new List<PrefabData>();

        foreach (GameObject obj in allObjects)
        {
            // Skip non-active or editor-only objects
            if (!obj.activeInHierarchy || obj.scene.name == null) continue;

            // Skip built-in Unity objects (like Directional Light, Main Camera)
            if (obj.name.Contains("Camera") || obj.name.Contains("Light")) continue;

            string prefabName = obj.name.Replace("(Clone)", "").Trim();

            PrefabData data = new PrefabData
            {
                prefabName = prefabName,
                position = obj.transform.position,
                rotation = obj.transform.rotation
            };

            savedPrefabs.Add(data);
        }

        PrefabDataList list = new PrefabDataList { prefabs = savedPrefabs.ToArray() };

        string json = JsonUtility.ToJson(list, true);
        string path = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllText(path, json);
        Debug.Log($"Saved {savedPrefabs.Count} prefabs to: {path}");
    }
}
