using UnityEngine;
using UnityEditor;
using System.IO;

public class PlantClassFix : EditorWindow
{
    // Define the component type and field name you want to modify
    private const string FieldNameToModify = "_class"; // Replace with the name of your field
    private string folderPath = "Assets/Prefabs/PlantCards"; // Replace with your specific folder path

    [MenuItem("Tools/Plant Card Prefab Modifier")]
    public static void ShowWindow()
    {
        GetWindow<PlantClassFix>("Plant Class Fixes");
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

                property = serializedObject.FindProperty("description");

                if (property != null)
                {
                    // Modify the value based on the property type
                    // This example assumes a float; adjust for other types (intValue, boolValue, stringValue, etc)
                    property.stringValue = property.stringValue.Replace("Gravestone", "Repression");

                    property.stringValue = property.stringValue.Replace("sun", "energy");
                    property.stringValue = property.stringValue.Replace("brains", "energy");
                    property.stringValue = property.stringValue.Replace("brain", "energy");

                    property.stringValue = property.stringValue.Replace("a zombie", "an enemy");
                    property.stringValue = property.stringValue.Replace("A zombie", "An enemy");
                    property.stringValue = property.stringValue.Replace("zombies", "enemies");
                    property.stringValue = property.stringValue.Replace("Zombies", "Enemies");
                    property.stringValue = property.stringValue.Replace("zombie", "enemy");
                    property.stringValue = property.stringValue.Replace("Zombie", "Enemy");

                    property.stringValue = property.stringValue.Replace("a plant", "an ally");
                    property.stringValue = property.stringValue.Replace("A plant", "An ally");
                    property.stringValue = property.stringValue.Replace("plants", "allies");
                    property.stringValue = property.stringValue.Replace("Plants", "Allies");
                    property.stringValue = property.stringValue.Replace("plant", "ally");
                    property.stringValue = property.stringValue.Replace("Plant", "Ally");
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
