using UnityEngine;
using System.Collections.Generic;

public class CityGenerator : MonoBehaviour
{
    [Header("City Settings")]
    public int gridSizeX = 5;
    public int gridSizeZ = 5;
    public float streetWidth = 10f;
    public float blockSize = 30f;

    [Header("Prefabs")]
    public GameObject[] buildingPrefabs;
    public GameObject streetPrefab;
    public GameObject intersectionPrefab;
    public GameObject sidewalkPrefab;

    // Create city layout
    public void GenerateCity()
    {
        // Create parent objects
        GameObject streetsParent = new GameObject("Streets");
        streetsParent.transform.SetParent(transform);
        
        GameObject buildingsParent = new GameObject("Buildings");
        buildingsParent.transform.SetParent(transform);

        // Generate streets (simplified)
        for (int x = 0; x <= gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                float posX = x * (blockSize + streetWidth);
                float posZ = z * (blockSize + streetWidth) + (blockSize / 2);

                if (streetPrefab != null)
                {
                    GameObject street = Instantiate(streetPrefab, new Vector3(posX, 0, posZ), Quaternion.Euler(0, 90, 0));
                    street.transform.SetParent(streetsParent.transform);
                    street.name = $"Street_V_{x}_{z}";
                }
            }
        }

        // Generate horizontal streets
        for (int z = 0; z <= gridSizeZ; z++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                float posX = x * (blockSize + streetWidth) + (blockSize / 2);
                float posZ = z * (blockSize + streetWidth);

                if (streetPrefab != null)
                {
                    GameObject street = Instantiate(streetPrefab, new Vector3(posX, 0, posZ), Quaternion.identity);
                    street.transform.SetParent(streetsParent.transform);
                    street.name = $"Street_H_{x}_{z}";
                }
            }
        }

        // Generate buildings
        if (buildingPrefabs != null && buildingPrefabs.Length > 0)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int z = 0; z < gridSizeZ; z++)
                {
                    float blockX = x * (blockSize + streetWidth) + (blockSize / 2);
                    float blockZ = z * (blockSize + streetWidth) + (blockSize / 2);

                    // Create building block
                    GameObject block = new GameObject($"Block_{x}_{z}");
                    block.transform.position = new Vector3(blockX, 0, blockZ);
                    block.transform.SetParent(buildingsParent.transform);

                    // Place buildings on block corners
                    PlaceBuildingAt(block.transform, blockX - blockSize/4, blockZ - blockSize/4);
                    PlaceBuildingAt(block.transform, blockX + blockSize/4, blockZ - blockSize/4);
                    PlaceBuildingAt(block.transform, blockX - blockSize/4, blockZ + blockSize/4);
                    PlaceBuildingAt(block.transform, blockX + blockSize/4, blockZ + blockSize/4);
                }
            }
        }
    }

    private void PlaceBuildingAt(Transform parent, float x, float z)
    {
        if (buildingPrefabs.Length == 0) return;
        
        GameObject buildingPrefab = buildingPrefabs[Random.Range(0, buildingPrefabs.Length)];
        if (buildingPrefab != null)
        {
            GameObject building = Instantiate(buildingPrefab, 
                new Vector3(x, 0, z), 
                Quaternion.Euler(0, Random.Range(0, 4) * 90, 0));
            
            building.transform.SetParent(parent);
        }
    }
}
