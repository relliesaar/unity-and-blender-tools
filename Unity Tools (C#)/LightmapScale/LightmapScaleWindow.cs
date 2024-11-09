using UnityEngine;
using UnityEditor;

public class LightmapScaleWindow : EditorWindow
{
    [MenuItem("Tools/Relliesaar/Lightmap Scale Window")]
    public static void OpenWindow()
    {
        GetWindow<LightmapScaleWindow>();
    }
    void OnEnable() 
    {
        LightmapScale.multiplier = 1f;
    }
    void OnGUI() 
    {
        LightmapScale.multiplier = EditorGUILayout.FloatField("Multiplier", LightmapScale.multiplier, EditorStyles.boldLabel);

        EditorGUILayout.Space();

        if (GUILayout.Button("Multiply children's lightmap scale"))
            LightmapScale.MultiplyLightmapScale();
    }
}
