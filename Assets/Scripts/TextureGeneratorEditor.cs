using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TextureGenerator))]
[CanEditMultipleObjects]
public class TextureGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TextureGenerator generator = (TextureGenerator)target;
        
        generator.width = EditorGUILayout.IntField("Width", generator.width);
        generator.height = EditorGUILayout.IntField("Height", generator.height);
        generator.seed = EditorGUILayout.IntField("Seed", generator.seed);
        generator.scale = EditorGUILayout.FloatField("Scale", generator.scale);
        generator.octaves = EditorGUILayout.IntField("Octaves", generator.octaves);
        generator.persistence = EditorGUILayout.FloatField("Persistence", generator.persistence);
        generator.lacunarity = EditorGUILayout.FloatField("Lacunarity", generator.lacunarity);
        generator.offset = EditorGUILayout.Vector2Field("Offset", generator.offset);
        
        if (GUILayout.Button("Generate Texture"))
        {
            generator.Start();
        }
    }
}