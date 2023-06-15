using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureGenerator : MonoBehaviour
{
    public int width;
    public int height;
    public int seed;
    public float scale;
    public int octaves;
    public float persistence;
    public float lacunarity;
    public Vector2 offset;

    public float[,] NoiseMap { get; private set; }

    public void Start()
    {
        NoiseMap = Noise.GenerateNoiseMap(width, height, seed, scale, octaves, persistence, lacunarity, offset);

        Texture2D texture = new Texture2D(width, height);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // Use the noiseMap values to generate a grayscale texture.
                Color color = new Color(NoiseMap[x, y], NoiseMap[x, y], NoiseMap[x, y]);
                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();

        // Set the texture to the Renderer of the GameObject this script is attached to.
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = texture;
    }
}