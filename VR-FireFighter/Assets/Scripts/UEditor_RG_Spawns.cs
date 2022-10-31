using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(RG_Spawns))]
public class UEditor_RG_Spawns : Editor
{
    private RG_Spawns script;
    int spawnType = 0;

    private void OnEnable() {
        // Method 1
        script = (RG_Spawns)target;
    }

    public override void OnInspectorGUI() {
        // Draw default inspector
        base.OnInspectorGUI();

        // For creating a new spawn
        GUILayout.Space(25);
        string[] str = { "Player", "FireHazard", "RescueEnt" };
        spawnType = GUILayout.Toolbar(spawnType, str);
        if (GUILayout.Button("Create New Spawnpoint")) {
            script.CreateNewSpawn(spawnType);
        }
        GUILayout.Space(10);
        // For placing newly created spawns into the list
        if (GUILayout.Button("Autosort Spawnpoints")) {
            script.AutosortSpawnpoints();
        }
        // For clearing the list, if you want to do that for some reason
        if (GUILayout.Button("Empty Spawn Array")) {
            script.ClearSpawnpoints();
        }
    }
}
