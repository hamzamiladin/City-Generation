using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGenerator))] 
public class MapGenEditor : Editor
{

    public override void OnInspectorGUI()
    {
        MapGenerator generator = (MapGenerator)target;

        if (DrawDefaultInspector())
        {
            if (generator.autoUpdate)
            {
                generator.Generate();
            }
        }
        
        if (GUILayout.Button("Generate"))
        {
            generator.Generate();
        }
    }
}