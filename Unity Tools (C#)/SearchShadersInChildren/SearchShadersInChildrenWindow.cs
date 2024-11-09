using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SearchShadersInChildrenWindow : EditorWindow
{
    const string foundObjectsPropertyName = "foundObjects";
    const string shadersListPropertyName = "shadersList";
    const string materialsListPropertyName = "materialsList";
    SerializedObject serObj;
    SerializedProperty foundObjectsProperty;
    SerializedProperty shadersListProperty;
    SerializedProperty materialsListProperty;
    Vector2 scroll;

    [MenuItem("Tools/Relliesaar/Search Shaders In Children")]
    static void OpenWindow()
    {
        GetWindow<SearchShadersInChildrenWindow>();
    }

    void OnEnable() 
    {
        SearchShadersInChildren.instance = CreateInstance<SearchShadersInChildren>();
        SearchShadersInChildren.instance.foundObjects = new List<GameObject>();
        SearchShadersInChildren.instance.shadersList = new List<Shader>();
        SearchShadersInChildren.instance.materialsList = new List<Material>();
        
        FindSerializedProperty(ref foundObjectsProperty, foundObjectsPropertyName);
        FindSerializedProperty(ref shadersListProperty, shadersListPropertyName);
        FindSerializedProperty(ref materialsListProperty, materialsListPropertyName);
    }

    void FindSerializedProperty(ref SerializedProperty property, string propertyName)
    {
        serObj = new SerializedObject(SearchShadersInChildren.instance);
        property = serObj.FindProperty(propertyName);
    }
    void UpdateSerializedProperty(ref SerializedProperty property, string propertyName)
    {
        serObj.Update();
        property = serObj.FindProperty(propertyName);
        serObj.ApplyModifiedProperties();
    }

    void OnGUI()
    {
        scroll = EditorGUILayout.BeginScrollView(scroll);

        SearchShadersInChildren.instance.shaderForSearch = EditorGUILayout.ObjectField("Shader", SearchShadersInChildren.instance.shaderForSearch, typeof(Shader), true) as Shader;

        EditorGUILayout.Space();

        if (GUILayout.Button("Find objects with this shader"))
        {
            SearchShadersInChildren.instance.SearchShaders();
            UpdateSerializedProperty(ref foundObjectsProperty, foundObjectsPropertyName);
            UpdateSerializedProperty(ref materialsListProperty, materialsListPropertyName);
        }

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Select all objects"))
            SearchShadersInChildren.instance.SelectStuff("objects");

        if (GUILayout.Button("Select all materials"))
            SearchShadersInChildren.instance.SelectStuff("materials");

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(foundObjectsProperty, true);
        
        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(materialsListProperty, true);

        EditorGUILayout.Space();

        if (GUILayout.Button("Display shaders"))
        {
            SearchShadersInChildren.instance.DisplayShaders();
            UpdateSerializedProperty(ref shadersListProperty, shadersListPropertyName);
        }

        EditorGUILayout.PropertyField(shadersListProperty, true);

        EditorGUILayout.EndScrollView();
    }
}
