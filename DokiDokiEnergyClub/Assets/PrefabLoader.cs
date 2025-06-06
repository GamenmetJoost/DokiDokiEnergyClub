using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class PrefabLoader : MonoBehaviour
{
    public string fileName = "prefabsData.json";
    public List<GameObject> availablePrefabs;

    public void LoadPrefabsFromJson()
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);
        if (!File.Exists(path))
        {
            Debug.LogWarning("No JSON file found at " + path);
            return;
        }

        string json = File.ReadAllText(path);
        PrefabDataList dataList = JsonUtility.FromJson<PrefabDataList>(json);

        foreach (PrefabData data in dataList.prefabs)
        {
            GameObject prefab = availablePrefabs.Find(p => p.name == data.prefabName);
            if (prefab == null)
            {
                Debug.LogWarning("Prefab not found in list: " + data.prefabName);
                continue;
            }

            GameObject obj = Instantiate(prefab, data.position, data.rotation);
            obj.name = data.prefabName;
        }

        Debug.Log($"Loaded {dataList.prefabs.Length} prefab instances from file.");
    }
}
