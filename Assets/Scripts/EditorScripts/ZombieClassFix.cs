using UnityEngine;
using UnityEditor;
using System.IO;

public class ZombieClassFix : EditorWindow
{
    // Define the component type and field name you want to modify
    private const string FieldNameToModify = "_class"; // Replace with the name of your field
    private string folderPath = "Assets/Prefabs/ZombieCards"; // Replace with your specific folder path

    [MenuItem("Tools/Batch Prefab Modifier")]
    public static void ShowWindow()
    {
        GetWindow<ZombieClassFix>("Zombie Class Fixes");
    }

    void OnGUI()
    {
        GUILayout.Label("Batch Modify Prefab Values", EditorStyles.boldLabel);
        folderPath = EditorGUILayout.TextField("Folder Path", folderPath);

        if (GUILayout.Button("Update All Prefabs in Folder"))
        {
            UpdatePrefabs();
        }
    }

    private void UpdatePrefabs()
    {
        if (!Directory.Exists(folderPath))
        {
            Debug.LogError("Folder not found at path: " + folderPath);
            return;
        }

        string[] prefabPaths = Directory.GetFiles(folderPath, "*.prefab", SearchOption.AllDirectories);
        AssetDatabase.StartAssetEditing(); // Speeds up the process by batching imports

        foreach (string prefabPath in prefabPaths)
        {
            // Load the prefab contents into an isolated scene for editing
            GameObject prefabRoot = PrefabUtility.LoadPrefabContents(prefabPath);

            // Get the component you want to modify (replace 'YourComponentType' with your actual type)
            Card component = prefabRoot.GetComponent<Card>();

            if (component != null)
            {
                // Use SerializedObject to modify serialized properties for proper saving and undo functionality
                SerializedObject serializedObject = new SerializedObject(component);
                SerializedProperty property = serializedObject.FindProperty(FieldNameToModify);

                if (property != null)
                {
                    // Modify the value based on the property type
                    // This example assumes a float; adjust for other types (intValue, boolValue, stringValue, etc)
                    property.enumValueIndex = property.enumValueIndex switch
                    {
                        7 => 1, // Example: if the value is 0, set it to 1
                        6 => 2, // Example: if the value is 1, set it to 2
                        8 => 0,
                        9 => 3,
                        _ => property.enumValueIndex // Keep the same value for other cases
                    };
                    serializedObject.ApplyModifiedProperties(); // Apply the changes to the serialized object
                    Debug.Log($"Updated {FieldNameToModify} on {prefabRoot.name}");
                }
                else
                {
                    Debug.LogWarning($"Property {FieldNameToModify} not found on component in prefab {prefabRoot.name}");
                }
            }
            else
            {
                Debug.LogWarning($"Component not found in prefab {prefabRoot.name}");
            }

            // Save the changes back to the prefab asset and unload the contents
            PrefabUtility.SaveAsPrefabAsset(prefabRoot, prefabPath);
            PrefabUtility.UnloadPrefabContents(prefabRoot);
        }

        AssetDatabase.StopAssetEditing(); // Stops batching imports
        AssetDatabase.Refresh(); // Refresh the Asset Database to show changes in the editor
        Debug.Log("Finished updating prefabs.");
    }
}
