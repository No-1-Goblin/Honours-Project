using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private GeneratorSettings settings;
    private List<GameObject> generatedObjects = new();
    
    public GeneratorSettings getSettings()
    {
        return settings;
    }

    public void setGeneratorSettings(GeneratorSettings newSettings)
    {
        settings = newSettings;
    }

    public void generateLevel()
    {
        // Clean up old level first
        deleteLevel();
        generatedObjects.Add(Instantiate(settings.tileset.startPieces[0]).gameObject);
    }

    public void deleteLevel()
    {
        // Destroy all generated level elements
        while (generatedObjects.Count > 0)
        {
            // Destroy immediate so it works in editor
            DestroyImmediate(generatedObjects[0]);
            generatedObjects.RemoveAt(0);
        }
    }
}

[CustomEditor(typeof(LevelGenerator))]
public class LevelGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var levelGenerator = (LevelGenerator)target;
        if (GUILayout.Button("Generate Level"))
        {
            levelGenerator.generateLevel();
        }
        if (GUILayout.Button("Delete Level"))
        {
            levelGenerator.deleteLevel();
        }
    }
}
