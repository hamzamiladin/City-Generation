using System;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode
    {
        NoiseMap,
        ColorMap,
        Mesh
    };

    public DrawMode drawMode;
    
    const int mapChunkSize = 241;
    [Range(0,6)] public int LOD;
    public int seed;
    public float scale;
    public int octaves;
    public float persistence;
    public float lacunarity;
    public Vector2 offset;
    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;
    public TerrainType[] regions;
    public int cityGridSize;
    public int cityGridSpacing;
    public GameObject[] buildingPrefabs;

    public bool autoUpdate = false;


    public void Generate()
    {
        float[,] noiseMap =
            Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, scale, octaves, persistence, lacunarity, offset);

        TerrainType[] terrainMap = new TerrainType[mapChunkSize * mapChunkSize];
        Color[] colorMap = new Color[mapChunkSize * mapChunkSize];
        for (int y = 0; y < mapChunkSize; y++)
        {
            for (int x = 0; x < mapChunkSize; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight <= regions[i].height)
                    {
                        colorMap[y * mapChunkSize + x] = regions[i].color;
                        terrainMap[y * mapChunkSize + x] = regions[i];
                        break;
                    }
                }
            }
        }

        for (int y = 0; y < mapChunkSize; y++)
        {
            for (int x = 0; x < mapChunkSize; x++)
            {
                if(terrainMap[y * mapChunkSize + x].isCityRegion)
                {
                    GenerateCity(x, y);
                }
            }
        }

        DrawMap drawMap = FindObjectOfType<DrawMap>();
        if (drawMode == DrawMode.NoiseMap)
        {
            drawMap.DrawTexture(TextureGenerator.TextureHeightMap(noiseMap));
        }
        else if (drawMode == DrawMode.ColorMap)
        {
            drawMap.DrawTexture(TextureGenerator.TextureColorMap(colorMap, mapChunkSize, mapChunkSize));
        }
        else if (drawMode == DrawMode.Mesh)
        {
            drawMap.DrawMesh(MeshGen.GenerateMesh(noiseMap, meshHeightMultiplier, meshHeightCurve, LOD),
                TextureGenerator.TextureColorMap(colorMap, mapChunkSize, mapChunkSize));
        }
    }

    private void OnValidate()
    {
        if (lacunarity < 1)
        {
            lacunarity = 1;
        }

        if (octaves < 0)
        {
            octaves = 0;
        }
    }
    
    private void GenerateCity(int x, int y)
    {
        Debug.Log($"Generating city at ({x},{y})");
    
        for (int i = -cityGridSize / 2; i < cityGridSize / 2; i++)
        {
            for (int j = -cityGridSize / 2; j < cityGridSize / 2; j++)
            {
                // Select a random prefab from the array.
                GameObject buildingPrefab = buildingPrefabs[UnityEngine.Random.Range(0, buildingPrefabs.Length)];
            
                Vector3 position = new Vector3(x + i, 0, y + j);
                GameObject newBuilding = Instantiate(buildingPrefab, position, Quaternion.identity, transform);
            
                if (newBuilding == null)
                {
                    Debug.LogError($"Failed to instantiate building at ({position.x},{position.y},{position.z})");
                }
                else
                {
                    Debug.Log($"Building created at ({position.x},{position.y},{position.z})");
                }
            }
        }
    }
}

[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color color;
    public bool isCityRegion;
}
