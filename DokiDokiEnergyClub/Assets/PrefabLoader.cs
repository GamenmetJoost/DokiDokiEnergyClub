using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class PrefabLoader : MonoBehaviour
{
    public string fileName = "localData.json";
    public List<GameObject> availablePrefabs;

    void Awake()
    {
        // Set fileName based on current userId if available
        if (!string.IsNullOrEmpty(saveToLocal.CurrentUserId))
        {
            fileName = $"prefabsData_{saveToLocal.CurrentUserId}.json";
        }
    }

    void Start()
    {
        LoadPrefabsFromJson();
    }

    public void LoadPrefabsFromJson()
    {
        string path = Path.Combine(Application.persistentDataPath, "localData.json");
        PrefabDataList dataList = new PrefabDataList();

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            // Try to extract prefabData from the save file
            try
            {
                SaveFileData saveData = JsonUtility.FromJson<SaveFileData>(json);
                if (saveData != null && saveData.prefabData != null && saveData.prefabData.prefabs != null)
                {
                    dataList = saveData.prefabData;
                }
                else
                {
                    Debug.LogWarning("No prefab data found in localData.json");
                    dataList = new PrefabDataList();
                }
            }
            catch
            {
                Debug.LogWarning("Failed to parse prefab data from localData.json");
                dataList = new PrefabDataList();
            }
        }
        else
        {
            Debug.LogWarning("localData.json not found, using empty prefab list.");
            dataList = new PrefabDataList();
        }

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

        Debug.Log($"Loaded {dataList.prefabs?.Length ?? 0} prefab instances from localData.json.");
    }

    [System.Serializable]
    private class SaveFileData
    {
        public string userId;
        public int money;
        public int electricity;
        public int polution;
        public bool x;
        public string y;
        public PrefabDataList prefabData;
    }
}
