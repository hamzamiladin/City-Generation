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

    public bool autoUpdate = false;


    public void Generate()
    {
        float[,] noiseMap =
            Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, scale, octaves, persistence, lacunarity, offset);


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
                        break;
                    }
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
}

[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color color;
}