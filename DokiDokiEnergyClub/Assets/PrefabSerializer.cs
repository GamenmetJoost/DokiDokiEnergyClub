using UnityEngine;
using System.Collections.Generic;

public class PrefabSerializer : MonoBehaviour
{
    // Returns the prefab data list instead of saving to file
    public PrefabDataList GetAllPrefabInstances()
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

        return new PrefabDataList { prefabs = savedPrefabs.ToArray() };
    }
}
