using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SearchShadersInChildren : ScriptableObject
{
    public static SearchShadersInChildren instance;
    public  Shader shaderForSearch;
    string shaderName;
    public List<GameObject> foundObjects;
    public List<Shader> shadersList;
    public List<Material> materialsList;

    public void SearchShaders()
    {
        if (shaderForSearch == null)
            return;

        shaderName = shaderForSearch.name;

        foundObjects = new List<GameObject>();
        materialsList = new List<Material>();
        
        var renderers = Selection.activeGameObject.GetComponentsInChildren<Renderer>();

        foreach (var renderer in renderers)
        {
            if (renderer.sharedMaterial != null && renderer.sharedMaterial.shader.name == shaderName)
            {
                foundObjects.Add(renderer.gameObject);
                
                if (!materialsList.Contains(renderer.sharedMaterial))
                    materialsList.Add(renderer.sharedMaterial);
            }
        }
    }

    public void DisplayShaders()
    {
        var renderers = Selection.activeGameObject.GetComponentsInChildren<Renderer>();
        shadersList = new List<Shader>();

        foreach (var renderer in renderers)
            if (renderer.sharedMaterial != null && !shadersList.Contains(renderer.sharedMaterial.shader))
                    shadersList.Add(renderer.sharedMaterial.shader);
    }

    public void SelectStuff(string type)
    {
        switch (type)
        {
            case "materials":
                Selection.objects = materialsList.ToArray(); 
                break;

            case "objects":
                Selection.objects = foundObjects.ToArray();
                break;
        }
    }
}
