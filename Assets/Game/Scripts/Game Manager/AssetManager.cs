using System.Collections.Generic;
using UnityEngine;

public class AssetManager : Singleton<AssetManager>
{
    [SerializeField] private Dictionary<string, GameObject> _prefabs = new Dictionary<string, GameObject>();

    public GameObject LoadPrefab(string prefabName)
    {
        if (!_prefabs.ContainsKey(prefabName))
        {
            _prefabs[prefabName] = Resources.Load<GameObject>($"Prefabs/{prefabName}");
        }

        return _prefabs[prefabName];
    }

    public GameObject InstantiatePrefab(string prefabName, Vector3 position, Quaternion rotation)
    {
        GameObject prefab = LoadPrefab(prefabName);
        if (prefab != null)
        {
            return Instantiate(prefab, position, rotation);
        }
        return null;
    }

    public GameObject InstantiatePrefab(string prefabName, Transform parent)
    {
        GameObject prefab = LoadPrefab(prefabName);
        if (prefab != null)
        {
            return Instantiate(prefab, parent.position, Quaternion.identity, parent);
        }
        return null;
    }

    public void UnloadPrefab(string prefabName)
    {
        if (_prefabs.ContainsKey(prefabName))
        {
            Resources.UnloadAsset(_prefabs[prefabName]);
            _prefabs.Remove(prefabName);
        }
    }

    public void PreloadPrefabs(string[] prefabNames)
    {
        foreach (string name in prefabNames)
        {
            LoadPrefab(name);
        }
    }
}
