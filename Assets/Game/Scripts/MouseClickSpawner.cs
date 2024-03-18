using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class MouseClickSpawner : MonoBehaviour
{
    public AssetReference humanPrefabReference;

    void Update()
    {
        // Check for left mouse button click
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
            // Calculate the spawn position for the human prefab
            Vector3 spawnPosition = hit.point;
            spawnPosition.y += 1; // Adjust the height if necessary

            // Instantiate the human prefab using the AssetManager
            AssetManager.Instance.Inst(humanPrefabReference, spawnPosition, Quaternion.identity,
                (instGO) =>
                {
                    if (instGO != null)
                    {
                        // Optional: Perform any initialization on the prefab here
                    }
                });
        }
    }

    private void SpawnResourcePrefabAtMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Calculate the spawn position for the tree prefab
            Vector3 spawnPosition = hit.point;
            spawnPosition.y += 1; // Adjust the height if necessary

            // Load the tree prefab from the Resources folder
            GameObject treePrefab = Resources.Load<GameObject>("Tree");
            Instantiate(treePrefab, spawnPosition, Quaternion.identity);
        }
    }
}