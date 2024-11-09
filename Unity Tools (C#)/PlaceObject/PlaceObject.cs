using UnityEngine;
using UnityEditor;

public class PlaceObject
{
	static Vector3 _placementDirection = Vector3.up;
	public static Vector3 PlacementDirection
	{
		get { return _placementDirection; }
		set
		{
			switch (value)
			{
				case Vector3 vector when vector.Equals(Vector3.right):
					_placementDirection = Vector3.right;
					break;

				case Vector3 vector when vector.Equals(Vector3.up):
					_placementDirection = Vector3.up;
					break;

				case Vector3 vector when vector.Equals(Vector3.forward):
					_placementDirection= Vector3.forward;
					break;

				default:
					_placementDirection = Vector3.up; // default direction
					break;
			}
		}
	}
	public static void ChangeDirectionVector(Vector3 direction)
	{
		PlacementDirection = direction;
	}

	[MenuItem("Tools/Relliesaar/Object To Cursor")] // menu item is only for assigning a shortcut in the editor
	public static void ObjectToCursor() // for proper use, assign shortcut in the editor
	{
		Vector2 cursorCoord;

		if (EditorWindow.mouseOverWindow
			&& Event.current.mousePosition.x <= EditorWindow.mouseOverWindow.position.width && Event.current.mousePosition.x >= 0
			&& Event.current.mousePosition.y <= EditorWindow.mouseOverWindow.position.height && Event.current.mousePosition.y >= 0)
		{
			cursorCoord = (Event.current.mousePosition - new Vector2(0, EditorWindow.mouseOverWindow.position.height)) * new Vector2(1f, -1f); // inverting height pixel coordinates

			Ray ray = SceneView.lastActiveSceneView.camera.ScreenPointToRay(new Vector3(cursorCoord.x, cursorCoord.y, 0));

			if (Physics.Raycast(ray, out RaycastHit hit) && Selection.activeGameObject != null && hit.transform.gameObject != Selection.activeGameObject)
			{
				Undo.RecordObject(Selection.activeGameObject.transform, "place the object");
				Selection.activeGameObject.transform.SetPositionAndRotation(hit.point, Quaternion.LookRotation(hit.normal));

				switch (PlacementDirection)
				{
					case Vector3 vector when vector.Equals(Vector3.right):
						Selection.activeGameObject.transform.Rotate(new Vector3(90f, 0, 90f));
						break;

					case Vector3 vector when vector.Equals(Vector3.up):
						Selection.activeGameObject.transform.Rotate(new Vector3(90f, 0, 0));
						break;

					default: break;
				}
			}
		}
	}
}
