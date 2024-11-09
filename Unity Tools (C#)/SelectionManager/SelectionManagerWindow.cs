using UnityEngine;
using UnityEditor;

public class SelectionManagerWindow : EditorWindow
{
	static string selectedTag;
	static int selectedLayer;
	static bool includeInactive;
	static Vector2 windowScroll;

	[MenuItem("Tools/Relliesaar/Selection Manager Window")]
	public static void OpenWindow()
	{
		GetWindow<SelectionManagerWindow>("Selection Manager");
	}

	void OnGUI()
	{
		windowScroll = EditorGUILayout.BeginScrollView(windowScroll);

		includeInactive = EditorGUILayout.Toggle("Search inactive objects in scene:", includeInactive);

		EditorGUILayout.Space();

		selectedLayer = EditorGUILayout.LayerField("Filter selection by layer:", selectedLayer);

		EditorGUILayout.BeginHorizontal();

		if (GUILayout.Button("Filter by layer"))
			SelectionManager.FilterByLayer(selectedLayer, false);

		if (GUILayout.Button("Exclude layer"))
			SelectionManager.FilterByLayer(selectedLayer, true);

		if (GUILayout.Button("Search layer in scene"))
			SelectionManager.SearchSceneForLayer(includeInactive, selectedLayer);

		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space();

		selectedTag = EditorGUILayout.TagField("Filter selection by tag:", selectedTag);

		EditorGUILayout.BeginHorizontal();

		if (GUILayout.Button("Filter by tag"))
			SelectionManager.FilterByTag(selectedTag, false);

		if (GUILayout.Button("Exclude tag"))
			SelectionManager.FilterByTag(selectedTag, true);

		if (GUILayout.Button("Search tag in scene"))
			SelectionManager.SearchSceneForTag(includeInactive, selectedTag);

		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space();

		EditorGUILayout.LabelField("Filter objects in selection");

		EditorGUILayout.BeginHorizontal();

		if (GUILayout.Button("Active objects"))
			SelectionManager.FilterActive(true);

		if (GUILayout.Button("Inactive objects"))
			SelectionManager.FilterActive(false);

		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space();

		EditorGUILayout.LabelField("Manage renderers");

		EditorGUILayout.BeginHorizontal();

		if (GUILayout.Button("Enable"))
			SelectionManager.ActivateRenderers(true);

		if (GUILayout.Button("Disable"))
			SelectionManager.ActivateRenderers(false);

		if (GUILayout.Button("Toggle"))
			SelectionManager.ToggleRenderers();

		if (GUILayout.Button("Filter"))
			SelectionManager.FilterRenderers();

		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space();

		EditorGUILayout.LabelField("Find objects with GI flag");

		EditorGUILayout.BeginHorizontal();

		if (GUILayout.Button("GI is on"))
			SelectionManager.FilterGI(true);

		if (GUILayout.Button("GI is off"))
			SelectionManager.FilterGI(false);

		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space();

		EditorGUILayout.LabelField("Find objects with selected flags");

		SelectionManager.flags = (StaticEditorFlags)EditorGUILayout.EnumFlagsField("Flags: ", SelectionManager.flags);

		EditorGUILayout.Space();

		if (GUILayout.Button("Find objects with flags (strict search)"))
			SelectionManager.FilterFlags(true);

		if (GUILayout.Button("Find objects with flags (not strict search)"))
			SelectionManager.FilterFlags(false);

		EditorGUILayout.EndScrollView();
	}
}
