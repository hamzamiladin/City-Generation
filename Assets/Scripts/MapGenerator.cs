using System;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    
    public enum DrawMode{ NoiseMap, ColorMap};
    public DrawMode drawMode;
    public int width;
    public int height;
    public int seed;
    public float scale;
    public int octaves;
    public float persistence;
    public float lacunarity;
    public Vector2 offset;
    public TerrainType[] regions;
    
    public bool autoUpdate = false;
    

    public void Generate()
    {
        float [,] noiseMap = Noise.GenerateNoiseMap(width, height, seed, scale, octaves, persistence, lacunarity, offset);
        

        Color[] colorMap = new Color[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float currentHeight = noiseMap[x, y];
                for(int i=0; i<regions.Length; i++)
                {
                    if(currentHeight <= regions[i].height)
                    {
                        colorMap[y * width + x] = regions[i].color;
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
            drawMap.DrawTexture(TextureGenerator.TextureColorMap(colorMap, width, height));
        }
    }

    private void OnValidate()
    {
        if(width < 1)
        {
            width = 1;
        }
        if(height < 1)
        {
            height = 1;
        }
        if(lacunarity < 1)
        {
            lacunarity = 1;
        }
        if(octaves < 0)
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