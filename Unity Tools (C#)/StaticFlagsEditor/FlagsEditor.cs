using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class FlagsEditor
{
    public static StaticEditorFlags staticFlags;
    public static bool applyToSelection;
    public static bool includeInactiveChildren;
    static List<GameObject> preparedObjects;

    public static void ApplyFlags()
    {
        PrepareObjectsList(applyToSelection);

        if (preparedObjects.Count < 1)
            return;

        foreach (var obj in preparedObjects)
        {
            Undo.RecordObject(obj, "edit static flags");
            GameObjectUtility.SetStaticEditorFlags(obj, staticFlags);
        }
    }

    static void PrepareObjectsList(bool applyToSelection)
    {
        preparedObjects = new List<GameObject>();

        if (applyToSelection)
        {
            preparedObjects = Selection.gameObjects.ToList<GameObject>();
        }
        else
        {
            var childrenTransforms = Selection.activeGameObject.GetComponentsInChildren<Transform>(includeInactiveChildren);

            foreach (var transform in childrenTransforms)
                preparedObjects.Add(transform.gameObject);
        }
    }
    public static void ToggleSelectedFlags(bool addFlags)
    {
        StaticEditorFlags objectFlags;
        PrepareObjectsList(applyToSelection);

        if (preparedObjects.Count < 1)
            return;

        foreach (var obj in preparedObjects)
        {
            Undo.RecordObject(obj, "edit static flags");

            objectFlags = GameObjectUtility.GetStaticEditorFlags(obj);
            
            if (addFlags)
                objectFlags |= staticFlags;
            else
                objectFlags &= ~staticFlags;

            GameObjectUtility.SetStaticEditorFlags(obj, objectFlags);
        }
    }
}
