using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

public class FindInLightmapWindow : EditorWindow
{
	Vector2 scroll;

	[MenuItem("Tools/Relliesaar/Find In Lightmap Window")]
	public static void OpenWindow()
	{
		GetWindow<FindInLightmapWindow>();
	}
	void OnEnable()
	{
		EditorSceneManager.activeSceneChangedInEditMode += RefreshLightmapsIndexesOnChangingScene;

		FindInLightmap.instance = CreateInstance<FindInLightmap>();
		FindInLightmap.instance.PrepareLightmapParamsField();
		FindInLightmap.instance.PrepareLightmapPopupField();
		FindInLightmap.instance.PrepareObjectsList();
	}
	void OnDisable()
	{
		EditorSceneManager.activeSceneChangedInEditMode -= RefreshLightmapsIndexesOnChangingScene;
	}
	static void RefreshLightmapsIndexesOnChangingScene(Scene previousScene, Scene currentScene)
	{
		// only because i don't need to use previous and current scenes in OnEnable() method
		FindInLightmap.instance.PrepareLightmapPopupField();
	}
	void OnGUI()
	{
		scroll = EditorGUILayout.BeginScrollView(scroll);

		EditorGUILayout.Space();

		FindInLightmap.instance.selectedLightmapIndex = EditorGUILayout.IntPopup("Lightmap index: ", 
			FindInLightmap.instance.selectedLightmapIndex,
			FindInLightmap.instance.lightmapNames.ToArray(), 
			FindInLightmap.instance.lightmapIndexes.ToArray());

		EditorGUILayout.Space();

		if (GUILayout.Button("Find objects with selected LM index"))
			FindInLightmap.instance.FindObjectsByLMIndex();

		EditorGUILayout.Space();

		FindInLightmap.instance.additionalParams = EditorGUILayout.Toggle("Additional search params",
			FindInLightmap.instance.additionalParams);

		if (FindInLightmap.instance.additionalParams)
		{
			FindInLightmap.instance.includeInactiveSearch = EditorGUILayout.Toggle("Include inactive objects", 
				FindInLightmap.instance.includeInactiveSearch);

			EditorGUILayout.Space();

			FindInLightmap.instance.selectedLightmapParamsIndex = EditorGUILayout.IntPopup("Lightmap parameters: ",
				FindInLightmap.instance.selectedLightmapParamsIndex,
				FindInLightmap.instance.lightmapParamsNames.ToArray(),
				FindInLightmap.instance.lightmapParamsIndexes.ToArray());

			EditorGUILayout.Space();

			EditorGUILayout.BeginHorizontal();

			if (GUILayout.Button("Search this LM param"))
				FindInLightmap.instance.FindObjectsByLightmapParameters(false);

			if (GUILayout.Button("Search inverted LM param"))
				FindInLightmap.instance.FindObjectsByLightmapParameters(true);

			EditorGUILayout.EndHorizontal();
		}

		EditorGUILayout.Space();

		EditorGUILayout.LabelField("Iterate through objects", EditorStyles.boldLabel);

		EditorGUILayout.BeginHorizontal();

		if (GUILayout.Button("All objects"))
			FindInLightmap.instance.SelectAllObjects();
		
		if (GUILayout.Button("Next object"))
			FindInLightmap.instance.SelectNextObject();

		if (GUILayout.Button("Previous object"))
			FindInLightmap.instance.SelectPreviousObject();

		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space();

		EditorGUILayout.LabelField("List of found game objects", EditorStyles.boldLabel);

		EditorGUILayout.PropertyField(FindInLightmap.instance.objListProperty, true);
		FindInLightmap.instance.serObj.ApplyModifiedProperties();

		EditorGUILayout.EndScrollView();
	}
}
