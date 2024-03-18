using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

/// <summary>
/// Manages the loading, instantiation, and unloading of Addressable assets.
/// </summary>
public class AssetManager : Singleton<AssetManager>
{
    /// <summary>
    /// Stores references to loaded prefab assets.
    /// </summary>
    private Dictionary<string, AsyncOperationHandle<GameObject>> _handles = new();

    /// <summary>
    /// Loads a prefab asynchronously and executes a callback upon completion.
    /// </summary>
    /// <param name="name">The name of the prefab to load.</param>
    /// <param name="onLoaded">Action to execute when loading is complete.</param>
    public void Load(string name, Action<GameObject> onLoaded)
    {
        if (!_handles.ContainsKey(name))
        {
            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(name);
            handle.Completed += (AsyncOperationHandle<GameObject> op) =>
            {
                if (op.Status == AsyncOperationStatus.Succeeded)
                {
                    _handles[name] = op;
                    onLoaded?.Invoke(op.Result);
                }
                else
                {
                    Debug.LogError($"Failed to load prefab: {name}");
                }
            };
        }
        else
        {
            onLoaded?.Invoke(_handles[name].Result);
        }
    }

    /// <summary>
    /// Instantiates a prefab at the given position and rotation.
    /// </summary>
    /// <param name="prefab">Reference to the Addressable prefab to instantiate.</param>
    /// <param name="pos">The position to instantiate the prefab at.</param>
    /// <param name="rot">The rotation for the instantiated prefab.</param>
    /// <param name="onInst">Optional callback to execute after instantiation.</param>
    public void Inst(AssetReference prefab, Vector3 pos,
                     Quaternion rot, Action<GameObject> onInst = null)
    {
        Load(prefab.AssetGUID, (loadedPrefab) =>
        {
            GameObject inst = Instantiate(loadedPrefab, pos, rot);
            onInst?.Invoke(inst);
        });
    }

    public void Inst(string prefName, Vector3 pos,
                 Quaternion rot, Action<GameObject> onInst = null)
    {
        Load(prefName, (loadedPrefab) =>
        {
            GameObject inst = Instantiate(loadedPrefab, pos, rot);
            onInst?.Invoke(inst);
        });
    }

    /// <summary>
    /// Unloads a previously loaded prefab.
    /// </summary>
    /// <param name="name">The name of the prefab to unload.</param>
    public void Unload(string name)
    {
        if (_handles.ContainsKey(name))
        {
            Addressables.Release(_handles[name]);
            _handles.Remove(name);
        }
    }

    public void Unload(AssetReference prefab)
    {
        if (_handles.ContainsKey(prefab.AssetGUID))
        {
            Addressables.Release(_handles[prefab.AssetGUID]);
            _handles.Remove(prefab.AssetGUID);
        }
    }
}
