using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class MouseClickSpawner : MonoBehaviour
{
    public AssetReference humanPrefabReference;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SpawnPrefabAtMousePosition();
        }

        if (Input.GetMouseButtonDown(1))
        {
            SpawnResourcePrefabAtMousePosition();
        }
    }

    private void SpawnPrefabAtMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 spawnPosition = hit.point;
            spawnPosition.y += 1; 

            AssetManager.Instance.Inst(humanPrefabReference, spawnPosition, Quaternion.identity,
                (instGO) =>
                {
                    if (instGO != null)
                    { }
                });
        }
    }

    private void SpawnResourcePrefabAtMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 spawnPosition = hit.point;
            spawnPosition.y += 1; 

            GameObject treePrefab = Resources.Load<GameObject>("Tree");
            Instantiate(treePrefab, spawnPosition, Quaternion.identity);
        }
    }
}