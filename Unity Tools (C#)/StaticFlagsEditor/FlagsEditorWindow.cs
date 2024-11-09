using UnityEngine;
using UnityEditor;

public class StaticFlagsWindow : EditorWindow
{
    Vector2 scroll;

    [MenuItem("Tools/Relliesaar/Static Flags Editor")]
    static void OpenWindow()
    {
        GetWindow<StaticFlagsWindow>("Static Flags Editor");
        FlagsEditor.applyToSelection = true;
    }

    void OnGUI() 
    {
        scroll = EditorGUILayout.BeginScrollView(scroll);

        FlagsEditor.staticFlags = (StaticEditorFlags)EditorGUILayout.EnumFlagsField("Static flags", FlagsEditor.staticFlags);

        EditorGUILayout.Space();

        if (FlagsEditor.applyToSelection)
            EditorGUILayout.LabelField("Changes will be applied to selected objects", EditorStyles.boldLabel);
        else
            EditorGUILayout.LabelField("Changes will be applied to children objects", EditorStyles.boldLabel);

            EditorGUILayout.Space();

        FlagsEditor.applyToSelection = EditorGUILayout.Toggle("Apply flags to selection", FlagsEditor.applyToSelection);
        
        if (!FlagsEditor.applyToSelection)
        {
            EditorGUILayout.Space();

            FlagsEditor.includeInactiveChildren = EditorGUILayout.Toggle("Include inactive children", FlagsEditor.includeInactiveChildren);
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Assign all selected flags"))
            FlagsEditor.ApplyFlags();

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Enable only selected flags"))
            FlagsEditor.ToggleSelectedFlags(true);
        
        if (GUILayout.Button("Disable only selected flags"))
            FlagsEditor.ToggleSelectedFlags(false);

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndScrollView();
    }
}
