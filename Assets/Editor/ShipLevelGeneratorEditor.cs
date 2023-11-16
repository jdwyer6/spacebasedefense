using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Ship_Level_Generator))]
public class ShipLevelGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Check if there's more than one target (multi-object editing)
        if (targets.Length > 1)
        {
            GUILayout.Label("Multi-object editing not supported.");
            return;
        }

        DrawDefaultInspector();

        Ship_Level_Generator levelGenerator = (Ship_Level_Generator)target;

        if (GUILayout.Button("Generate Level"))
        {
            levelGenerator.GenerateLevel();
        }
    }
}
