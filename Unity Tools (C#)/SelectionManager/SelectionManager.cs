using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SelectionManager
{
	static List<GameObject> filterList;
	public static StaticEditorFlags flags;

	public static void FilterByLayer(int selectedLayer, bool excludeFromSelection)
	{
		if (Selection.gameObjects != null)
		{
			filterList = new List<GameObject>();

			foreach (GameObject obj in Selection.gameObjects)
				if (obj.layer == selectedLayer)
					filterList.Add(obj);

			if (excludeFromSelection)
				Selection.objects = System.Linq.Enumerable.ToList(System.Linq.Enumerable.Except(Selection.gameObjects, filterList)).ToArray();
			else
				Selection.objects = filterList.Count != 0 ? filterList.ToArray() : Selection.gameObjects;
		}
	}
	public static void FilterByTag(string selectedTag, bool excludeFromSelection)
	{
		if (Selection.gameObjects != null)
		{
			filterList = new List<GameObject>();

			foreach (GameObject obj in Selection.gameObjects)
				if (obj.tag == selectedTag)
					filterList.Add(obj);

			if (excludeFromSelection)
				Selection.objects = System.Linq.Enumerable.ToList(System.Linq.Enumerable.Except(Selection.gameObjects, filterList)).ToArray();
			else
				Selection.objects = filterList.Count != 0 ? filterList.ToArray() : Selection.gameObjects;
		}
	}
	public static void SearchSceneForLayer(bool includeInactive, int selectedLayer)
	{
		filterList = new List<GameObject>();

		var foundObjects = GameObject.FindObjectsOfType<MeshRenderer>(includeInactive);

		foreach (MeshRenderer meshRenderer in foundObjects)
			if (meshRenderer.gameObject.layer == selectedLayer)
				filterList.Add(meshRenderer.gameObject);

		Selection.objects = filterList.Count != 0 ? filterList.ToArray() : Selection.gameObjects;
	}
	public static void SearchSceneForTag(bool includeInactive, string selectedTag)
	{
		filterList = new List<GameObject>();

		var foundObjects = GameObject.FindObjectsOfType<GameObject>(includeInactive); // FindGameObjectsWithTag() doesn't work with untagged objects for some reason

		foreach (GameObject obj in foundObjects)
			if (obj.tag == selectedTag)
				filterList.Add(obj);

		Selection.objects = filterList.Count != 0 ? filterList.ToArray() : Selection.gameObjects;
	}
	public static void FilterActive(bool findActive)
	{
		if (Selection.gameObjects != null)
		{
			filterList = new List<GameObject>();

			foreach (GameObject obj in Selection.gameObjects)
				if (obj.activeInHierarchy == findActive)
					filterList.Add(obj);

			Selection.objects = filterList.Count != 0 ? filterList.ToArray() : Selection.gameObjects;
		}
	}
	public static void FilterGI(bool giEnabled)
	{
		if (Selection.gameObjects != null)
		{
			filterList = new List<GameObject>();

			foreach (GameObject obj in Selection.gameObjects)
				if (giEnabled == GameObjectUtility.GetStaticEditorFlags(obj).HasFlag(StaticEditorFlags.ContributeGI))
					filterList.Add(obj);

			Selection.objects = filterList.Count != 0 ? filterList.ToArray() : Selection.gameObjects;
		}
	}
	public static void FilterFlags(bool filterMultipleFlags)
	{
		if (Selection.gameObjects != null)
		{
			filterList = new List<GameObject>();

			foreach (GameObject obj in Selection.gameObjects)
			{
				var objFlags = GameObjectUtility.GetStaticEditorFlags(obj);
				int flagsHashCode;

				// this is a workaround for the case when flags that marked as "everything" have value of -1. it probably will be fixed in unity's api later 
				if (flags.GetHashCode() == -1)
					flagsHashCode = 2147483647;
				else
					flagsHashCode = flags.GetHashCode();


				switch (filterMultipleFlags)
				{
					case true:
						if (objFlags.GetHashCode() == flagsHashCode)
							filterList.Add(obj);
					break;

					case false:
						if (objFlags.HasFlag(flags))
							filterList.Add(obj);
							
						// this is another workaround for the case when flags marked as "everything"
						if (flagsHashCode == 2147483647 && objFlags.GetHashCode() > 0)
							filterList.Add(obj);
					break;
				}
			}

			Selection.objects = filterList.Count != 0 ? filterList.ToArray() : null;

			if (filterList.Count == 0)
				Debug.Log("There's no matches, removing selection");
		}
	}
	public static void FilterRenderers()
	{
		List<GameObject> requiredObjects = new List<GameObject>();

		foreach (var obj in Selection.gameObjects)
			if (obj.TryGetComponent(out Renderer renderer))
				requiredObjects.Add(obj);

		Selection.objects = requiredObjects.ToArray();
	}
	public static void ActivateRenderers(bool active)
	{
		foreach (var obj in Selection.gameObjects)
		{
			if (obj.TryGetComponent(out Renderer renderer))
			{
				Undo.RecordObject(renderer, "renderer status");
				renderer.enabled = active;
			}
		}
	}
	public static void ToggleRenderers()
	{
		foreach (var obj in Selection.gameObjects)
		{
			if (obj.TryGetComponent(out Renderer renderer))
			{
				Undo.RecordObject(renderer, "toggle renderer");
				renderer.enabled = !renderer.enabled;
			}
		}
	}
}
