using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FindInLightmap : ScriptableObject
{
    Renderer[] renderers;
	const string ObjListPropertyString = "foundObjects";
    LightmapPresetsData lightmapPresetsData;

    public SerializedObject serObj;
    public SerializedProperty objListProperty;
	public List<GameObject> foundObjects;
	public List<int> lightmapIndexes;
    public List<string> lightmapNames;
    public static FindInLightmap instance;
    public int selectedLightmapIndex;

    public bool additionalParams;
    public bool includeInactiveSearch;
    
    public List<string> lightmapParamsNames;
    public List<int> lightmapParamsIndexes;
    public int selectedLightmapParamsIndex;

    public void FindObjectsByLMIndex()
    {
        renderers = Object.FindObjectsOfType<Renderer>(includeInactiveSearch);
        foundObjects = new List<GameObject>();

        foreach (var renderer in renderers)
            if (renderer.lightmapIndex == selectedLightmapIndex)
                foundObjects.Add(renderer.gameObject);

        UpdateSerializedProperty(ref objListProperty, ObjListPropertyString);
    }
    public void PrepareLightmapPopupField()
    {
        lightmapIndexes = new List<int>();
        lightmapNames = new List<string>();

        for (int i = 0; i < GetLightmapCount(); i++)
        {
            lightmapNames.Add($"Lightmap {i}");
            lightmapIndexes.Add(i);
        }
    }
    public void PrepareLightmapParamsField()
    {
        lightmapPresetsData = AssetDatabase.LoadAssetAtPath<LightmapPresetsData>("Assets/_Scripts/Editor/LD_Tools/FindInLightmap/LightmapPresetsData.asset");

        lightmapParamsNames = new List<string>();
        lightmapParamsIndexes = new List<int>();

        for (int i = 0; i < lightmapPresetsData.lightmapParameters.Count; i++)
        {
            lightmapParamsNames.Add(lightmapPresetsData.lightmapParameters[i].name);
            lightmapParamsIndexes.Add(i);
        }
    }
    public void PrepareObjectsList()
    {
        foundObjects = new List<GameObject>();
        FindSerializedProperty(ref objListProperty, ObjListPropertyString);
    }

    public void FindObjectsByLightmapParameters(bool invertComparisonSearch)
    {
		renderers = Object.FindObjectsOfType<Renderer>(includeInactiveSearch);
        foundObjects = new List<GameObject>();
        SerializedObject rendererSerialized;
        SerializedProperty lightmapParamProperty;

        if (!invertComparisonSearch) // if we search this LM parameter
        {
            foreach (var renderer in renderers)
            {
                rendererSerialized = new SerializedObject(renderer);
                lightmapParamProperty = rendererSerialized.FindProperty("m_LightmapParameters");

                if (lightmapParamProperty.objectReferenceValue == null)
                    continue;
                else
                {
					if (lightmapParamProperty.objectReferenceValue.name == lightmapPresetsData.lightmapParameters[selectedLightmapParamsIndex].name)
						foundObjects.Add(renderer.gameObject);
				}
            }
			UpdateSerializedProperty(ref objListProperty, ObjListPropertyString);
		}
        else // if we search other LM parameters exclude selected
        {
            foreach (var renderer in renderers)
            {
			    rendererSerialized = new SerializedObject(renderer);
			    lightmapParamProperty = rendererSerialized.FindProperty("m_LightmapParameters");

				if (lightmapParamProperty.objectReferenceValue == null)
					continue;
				else
				{
					if (lightmapParamProperty.objectReferenceValue.name != lightmapPresetsData.lightmapParameters[selectedLightmapParamsIndex].name)
						foundObjects.Add(renderer.gameObject);
				}
			}
			UpdateSerializedProperty(ref objListProperty, ObjListPropertyString);
		}
	}

    int GetLightmapCount()
    {
        return LightmapSettings.lightmaps.Length;
    }
	void FindSerializedProperty(ref SerializedProperty property, string propertyValue)
    {
        serObj = new SerializedObject(this);
        if (serObj != null)
            property = serObj.FindProperty(propertyValue);
        else Debug.Log("cannot find serialized object");
    }
	void UpdateSerializedProperty(ref SerializedProperty property, string propertyValue)
    {
        serObj.Update();
        property = serObj.FindProperty(propertyValue);
        serObj.ApplyModifiedProperties();
    }
    public void SelectPreviousObject()
    {
        if (foundObjects.IndexOf(Selection.activeGameObject) > 0)
            Selection.activeGameObject = foundObjects[foundObjects.IndexOf(Selection.activeGameObject) - 1].gameObject;
        else
            Selection.activeGameObject = foundObjects[foundObjects.Count - 1];
    }
    public void SelectNextObject()
    {
        if (foundObjects.IndexOf(Selection.activeGameObject) < foundObjects.Count - 1)
            Selection.activeGameObject = foundObjects[foundObjects.IndexOf(Selection.activeGameObject) + 1].gameObject;
        else
            Selection.activeGameObject = foundObjects[0];
    }
    public void SelectAllObjects()
    {
        Selection.objects = foundObjects.ToArray();
    }
}
