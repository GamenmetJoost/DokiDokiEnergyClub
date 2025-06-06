using System;
using UnityEngine;

[Serializable]
public class PrefabData
{
    public string prefabName;
    public Vector3 position;
    public Quaternion rotation;
}

[Serializable]
public class PrefabDataList
{
    public PrefabData[] prefabs;
}
