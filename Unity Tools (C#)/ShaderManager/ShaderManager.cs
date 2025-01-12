using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ShaderManager : ScriptableObject
{
	public static ShaderManager instance;

	public Shader reqShader;
	public bool multipleShaders;
	public bool showFoundMaterials;
	public List<Shader> shadersSelectionList;
	public List<Object> shaderObjectsList;
	public List<Object> foundMaterials;

	List<Material> materialsList;
	List<Material> materialsToSelect;

	List<string> toggleNames;
	List<Shader> toggleShaders;

	List<Material> materialsToGenerateOnScene;
	List<ShaderManagerBase> reqShadersList;

	public void RecreateShadersList(ref List<Object> listToRecreate) 
	{
		listToRecreate = new List<Object>();
	}
	public void DeleteEmptySlots(List<Object> listToClear)
	{
		foreach (var obj in listToClear.ToArray())
			if (obj == null)
				listToClear.Remove(obj);

		listToClear.TrimExcess();
	}
	public void AddSelectedShadersToList()
	{
		foreach (Shader shader in Selection.objects)
			shaderObjectsList.Add(shader);
	}
	public void AddOneSelectedShader()
	{
		reqShader = (Shader)Selection.activeObject;
	}
	public void ClearShaderField()
	{
		reqShader = null;
	}

	public void SelectTheseShaders()
	{
		if (multipleShaders)
			Selection.objects = shaderObjectsList.ToArray();
		else
			Selection.objects = new[] { reqShader };
	}
	public void FindMatsForTheseShaders()
	{
		materialsList = new List<Material>();
		materialsToSelect = new List<Material>();
		foundMaterials = new List<Object>();

		var materialsGUIIDs = AssetDatabase.FindAssets("t:material");

		foreach (var guid in materialsGUIIDs)
		{
			var path = AssetDatabase.GUIDToAssetPath(guid);
			var materialAsset = AssetDatabase.LoadAssetAtPath<Material>(path);

			materialsList.Add(materialAsset);
		}

		CheckMultipleShadersBool();

		foreach (Shader shader in shadersSelectionList)
			foreach (Material mat in materialsList)
				if (mat.shader.name == shader.name)
					materialsToSelect.Add(mat);

		foreach (var mat in materialsToSelect)
			foundMaterials.Add(mat);

		showFoundMaterials = true;
	}
	public void SelectFoundMaterials()
	{
		Selection.objects = foundMaterials.ToArray();
	}
	public void SelectShaderForThisMat()
	{
		if (Selection.activeObject.GetType() == typeof(Material))
		{
			Material mat = (Material)Selection.activeObject;
			Selection.objects = new[] { mat.shader };
		}
	}
	public void SetReqShader()
	{
		if (Selection.activeObject.GetType() == typeof(Shader))
			reqShader = (Shader)Selection.activeObject;
	}
	void CheckMultipleShadersBool()
	{
		shadersSelectionList = new List<Shader>();

		if (multipleShaders)
			foreach (Shader shader in shaderObjectsList)
				shadersSelectionList.Add(shader);
		else
			shadersSelectionList.Add(reqShader);
	}
	public void GenerateMaterials(string folderPath, string folderName)
	{
		CheckMultipleShadersBool();
		reqShadersList = new List<ShaderManagerBase>();

		foreach (var shader in shadersSelectionList)
		{
			if (shader != null)
			{
				toggleNames = new List<string>();
				toggleShaders = new List<Shader>();

				SerializedObject shaderSerializedObject = new SerializedObject(shader);

				var serializedPropertiesArray = shaderSerializedObject.FindProperty("m_ParsedForm.m_PropInfo.m_Props.Array");
				var serializedPropertiesArrayCount = serializedPropertiesArray.arraySize;
			
				for (int i = 0; i < serializedPropertiesArrayCount; i++)
				{
					var propElementArray = serializedPropertiesArray.FindPropertyRelative($"data[{i}].m_Attributes.Array");

					foreach (SerializedProperty element in propElementArray)
					{
						if (element.stringValue.Contains("Toggle"))
						{
							var togglePropertyName = serializedPropertiesArray.FindPropertyRelative($"data[{i}].m_Name").stringValue;

							toggleNames.Add(togglePropertyName);
							toggleShaders.Add(shader); // keep the actual shader instead of serialized object
						}
					}
				}
				reqShadersList.Add(new ShaderManagerBase(toggleNames, shader));
			}
			else
				Debug.Log("required shader is null");
		}

		if (multipleShaders)
		{
			for (int i = 0; i < reqShadersList.Count; i++)
			{
				GenerateAssets(reqShadersList[i], folderPath, folderName); // generate materials for all shaders in the list
				GenerateObjectsOnScene(reqShadersList[i].togglePropNames.Count, i);
			}
		}
		else
		{
			GenerateAssets(reqShadersList[0], folderPath, folderName); // generate one set of materials
			GenerateObjectsOnScene(reqShadersList[0].togglePropNames.Count, 0);
		}
	}
	void GenerateAssets(ShaderManagerBase reqShaderElement, string path, string folderName)
	{
		string folderPath = path;
		string binIteration;
		string binToggles;
		Material matToCreate;
		string assetPath;

		materialsToGenerateOnScene = new List<Material>();

		for (int i = 0; i < Mathf.Pow(2, reqShaderElement.togglePropNames.Count); i++) // generate materials for one specific shader at the moment
		{
			binIteration = System.Convert.ToString(i, 2);
			binToggles = System.Int16.Parse(binIteration).ToString($"D{reqShaderElement.togglePropNames.Count}");

			if (!AssetDatabase.IsValidFolder($"{folderPath}/{folderName}"))
			{
				AssetDatabase.CreateFolder(folderPath, folderName);
				Debug.Log($"Folder didn't exist, created new folder at path {folderPath}/{folderName}");
			}

			matToCreate = new Material(reqShaderElement.shader);
			assetPath = $"{folderPath}/{folderName}/{reqShaderElement.shader.name.Replace('/', '_')}_M_{i + 1}.mat";

			AssetDatabase.CreateAsset(matToCreate, assetPath);
			var generatedMaterial = (Material)AssetDatabase.LoadAssetAtPath(assetPath, typeof(Material));
			materialsToGenerateOnScene.Add(generatedMaterial);

			for (int k = 0; k < reqShaderElement.togglePropNames.Count; k++) // reverse binaries elements for toggles to be set from the start, not the end
			{
				var reversedChars = binToggles.ToCharArray();
				System.Array.Reverse(reversedChars);

				var intChar = CheckChar(reversedChars[k]);
				generatedMaterial.SetFloat(reqShaderElement.togglePropNames[k], intChar);
			}
			AssetDatabase.SaveAssetIfDirty(generatedMaterial);
		}
		AssetDatabase.Refresh();
	}
	int CheckChar(char symbol)
	{
		if (symbol == '1')
			return 1;
		else return 0;
	}
	int GetPreviousToggleCount(int currentIndex)
	{
		int allPreviousToggles = 0;

		if (currentIndex != 0)
		{
			for (int i = 0; i < currentIndex; i++)
				allPreviousToggles += reqShadersList[i].togglePropNames.Count;

			return allPreviousToggles;
		}
		else return 0;
	}
	void GenerateObjectsOnScene(int togglesCount, int parentIndex)
	{
		int generatorColumns = togglesCount;
		var combinations = Mathf.Pow(2, generatorColumns);
		int generatorRows = togglesCount > 0 ? (int)combinations / togglesCount + 1 : 1;
		int generatorIncrement = 0;
		Vector3 parentsGeneratorPosition;
		Vector3 spheresGeneratorPosition;

		var parent = new GameObject($"Parent_{reqShadersList[parentIndex].shader.name}");
		parentsGeneratorPosition = new Vector3(GetPreviousToggleCount(parentIndex) * 1.5f + parentIndex, 0, 0);
		parent.transform.position = parentsGeneratorPosition;

		for (int row = 0; row < generatorRows; row++)
		{
			if (row == generatorRows - 1)
			{
				if (row > 0)
					generatorColumns = (int)combinations % togglesCount;
				else
					generatorColumns = 1;
			}

			for (int col = 0; col < generatorColumns; col++)
			{
				var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

				spheresGeneratorPosition = new Vector3(col * 1.5f * sphere.transform.localScale.x, 0, row * 1.5f * sphere.transform.localScale.x);

				sphere.transform.position = spheresGeneratorPosition;
				sphere.transform.SetParent(parent.transform, false);
				sphere.GetComponent<Renderer>().sharedMaterial = materialsToGenerateOnScene[generatorIncrement];
				sphere.name = $"{materialsToGenerateOnScene[generatorIncrement].name}";

				generatorIncrement++;
			}
		}
	}
	public void DestroyRenderersOnScene()
	{
		var objectsToDelete = FindObjectsOfType<GameObject>();

		for (int i = 0; i < objectsToDelete.Length; i++)
			if (objectsToDelete[i].GetComponent<Renderer>() || objectsToDelete[i].name.Contains("Parent"))
				DestroyImmediate(objectsToDelete[i]);
	}
	public void SelectMaterialsFromList()
	{
		Selection.objects = foundMaterials.ToArray();
	}
}
