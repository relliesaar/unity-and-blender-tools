using UnityEngine;
using UnityEditor;

public class PlaceObjectWindow : EditorWindow
{
	[MenuItem("Tools/Relliesaar/Place Object Window")]
	public static void OpenWindow()
	{
		GetWindow<PlaceObjectWindow>("Place Object Setup");
	}
	void OnGUI()
	{
		EditorGUILayout.LabelField($"Direction vector is {PlaceObject.PlacementDirection}");

		EditorGUILayout.Space(10f);

		EditorGUILayout.LabelField("Select object's direction vector", EditorStyles.boldLabel);

		EditorGUILayout.BeginHorizontal();

		if (GUILayout.Button("X axis"))
		{
			PlaceObject.ChangeDirectionVector(Vector3.right);
			DirectionChangeNotification();
		}

		if (GUILayout.Button("Y axis"))
		{
			PlaceObject.ChangeDirectionVector(Vector3.up);
			DirectionChangeNotification();
		}

		if (GUILayout.Button("Z axis"))
		{
			PlaceObject.ChangeDirectionVector(Vector3.forward);
			DirectionChangeNotification();
		}

		EditorGUILayout.EndHorizontal();
	}
	void DirectionChangeNotification()
	{
		string axisName;

		switch (PlaceObject.PlacementDirection)
		{
			case Vector3 vector when vector.Equals(Vector3.right):
				axisName = "Right (X)";
				break;

			case Vector3 vector when vector.Equals(Vector3.up):
				axisName = "Up (Y)";
				break;

			case Vector3 vector when vector.Equals(Vector3.forward):
				axisName = "Forward (Z)";
				break;

			default:
				axisName = "Error: couldn't get direction vector";
				break;
		}

		GetWindow<SceneView>().ShowNotification(new GUIContent($"Direction vector is {axisName}"));
	}
}
