using UnityEngine;
using UnityEditor;

public class SceneViewCameraWindow : EditorWindow
{
    [MenuItem("Tools/Relliesaar/Scene View Camera Settings")]
    public static void OpenWindow()
    {
        GetWindow<SceneViewCameraWindow>();
    }

    void OnGUI() 
    {
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("0.1x"))
        {
            SceneViewCameraSetup.SetCameraSpeed(0.1f);
            GetWindow<SceneView>().ShowNotification(new GUIContent("0.1x speed"));
        }

        if (GUILayout.Button("0.25x"))
        {
            SceneViewCameraSetup.SetCameraSpeed(0.25f);
            GetWindow<SceneView>().ShowNotification(new GUIContent("0.25x speed"));
        }

        if (GUILayout.Button("0.5x"))
        {
            SceneViewCameraSetup.SetCameraSpeed(0.5f);
            GetWindow<SceneView>().ShowNotification(new GUIContent("0.5x speed"));
        }

        if (GUILayout.Button("0.75x"))
        {
            SceneViewCameraSetup.SetCameraSpeed(0.75f);
            GetWindow<SceneView>().ShowNotification(new GUIContent("0.75x speed"));
        }

        EditorGUILayout.EndHorizontal();
    }
}