using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ShaderManagerWindow : EditorWindow
{
	ScriptableObject targetSO;
	SerializedObject serObj;
	SerializedProperty shaderObjectsProperty;
	SerializedProperty materialProperty;

	string folderPath;
	string folderName;

	Vector2 scroll;
	const string ShaderPropertyString = "shaderObjectsList";
	const string MaterialsPropertyString = "foundMaterials";

	[MenuItem("Tools/Relliesaar/Shader Manager Window")]
	public static void OpenWindow()
	{
		GetWindow<ShaderManagerWindow>();
	}
	void FindSerializedProperty(ref SerializedProperty property, string value)
	{
		targetSO = ShaderManager.instance;
		serObj = new SerializedObject(targetSO);
		property = serObj.FindProperty(value);
	}
	void UpdateSerializedProperty(ref SerializedProperty property, string value)
	{
		serObj.Update();
		property = serObj.FindProperty(value);
		serObj.ApplyModifiedProperties();
	}
	void OnEnable()
	{
		ShaderManager.instance = CreateInstance<ShaderManager>();
		ShaderManager.instance.multipleShaders = false;
		ShaderManager.instance.showFoundMaterials = false;
		ShaderManager.instance.foundMaterials = new List<Object>();
		ShaderManager.instance.shaderObjectsList = new List<Object>();

		FindSerializedProperty(ref shaderObjectsProperty, ShaderPropertyString);
		FindSerializedProperty(ref materialProperty, MaterialsPropertyString);

		folderPath = "Assets/_Shaders/ShaderVariants";
		folderName = "ShaderVariants_Tool";

	}
	void OnGUI()
	{
		scroll = EditorGUILayout.BeginScrollView(scroll);

		EditorGUILayout.Space();

		ShaderManager.instance.multipleShaders = EditorGUILayout.ToggleLeft("Use multiple shaders", ShaderManager.instance.multipleShaders);

		if (ShaderManager.instance.multipleShaders)
		{
			EditorGUILayout.Space();

			EditorGUILayout.BeginHorizontal();

			if (GUILayout.Button("Add selected shaders to the list"))
			{
				ShaderManager.instance.AddSelectedShadersToList();
				UpdateSerializedProperty(ref shaderObjectsProperty, ShaderPropertyString);
			}

			if (GUILayout.Button("Recreate list"))
			{
				ShaderManager.instance.RecreateShadersList(ref ShaderManager.instance.shaderObjectsList);
				UpdateSerializedProperty(ref shaderObjectsProperty, ShaderPropertyString);
			}

			if (GUILayout.Button("Delete empty slots"))
			{
				ShaderManager.instance.DeleteEmptySlots(ShaderManager.instance.shaderObjectsList);
				UpdateSerializedProperty(ref shaderObjectsProperty, ShaderPropertyString);
			}

			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Space();

			EditorGUILayout.PropertyField(shaderObjectsProperty, true);
			serObj.ApplyModifiedProperties();

			EditorGUILayout.Space();

			EditorGUILayout.BeginHorizontal();

			if (GUILayout.Button("Generate materials"))
				ShaderManager.instance.GenerateMaterials(folderPath, folderName);

			EditorGUILayout.Space();

			if (GUILayout.Button("Clear the scene"))
				ShaderManager.instance.DestroyRenderersOnScene();

			EditorGUILayout.EndHorizontal();
		}
		else
		{
			EditorGUILayout.Space();

			EditorGUILayout.BeginHorizontal();

			if (GUILayout.Button("Add one selected shader"))
				ShaderManager.instance.AddOneSelectedShader();

			if (GUILayout.Button("Clear shader field"))
				ShaderManager.instance.ClearShaderField();

			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Space();

			ShaderManager.instance.reqShader = EditorGUILayout.ObjectField("Required shader: ", ShaderManager.instance.reqShader, typeof(Shader), false) as Shader;

			EditorGUILayout.Space();

			EditorGUILayout.BeginHorizontal();

			if (GUILayout.Button("Generate materials"))
				ShaderManager.instance.GenerateMaterials(folderPath, folderName);

			EditorGUILayout.Space();

			if (GUILayout.Button("Clear the scene"))
				ShaderManager.instance.DestroyRenderersOnScene();

			EditorGUILayout.EndHorizontal();
		}
			
		EditorGUILayout.Space();
		EditorGUILayout.Space();

		ShaderManager.instance.showFoundMaterials = EditorGUILayout.ToggleLeft("Show found materials", ShaderManager.instance.showFoundMaterials);

		if (ShaderManager.instance.showFoundMaterials)
		{
			EditorGUILayout.Space();

			EditorGUILayout.BeginHorizontal();

			if (GUILayout.Button("Select found materials"))
				ShaderManager.instance.SelectFoundMaterials();

			if (GUILayout.Button("Recreate list"))
			{
				ShaderManager.instance.RecreateShadersList(ref ShaderManager.instance.foundMaterials);
				UpdateSerializedProperty(ref materialProperty, MaterialsPropertyString);
			}

			if (GUILayout.Button("Delete empty slots"))
			{
				ShaderManager.instance.DeleteEmptySlots(ShaderManager.instance.foundMaterials);
				UpdateSerializedProperty(ref materialProperty, MaterialsPropertyString);
			}

			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Space();

			EditorGUILayout.PropertyField(materialProperty, true);
			serObj.ApplyModifiedProperties();
		}

		EditorGUILayout.Space();

		EditorGUILayout.LabelField("Different utils:", EditorStyles.boldLabel);

		EditorGUILayout.Space();

		folderPath = EditorGUILayout.TextField("Generator folder path: ", folderPath);
		folderName = EditorGUILayout.TextField("Generator folder name: ", folderName);

		EditorGUILayout.Space();

		if (GUILayout.Button("Find materials for required shaders"))
		{
			ShaderManager.instance.FindMatsForTheseShaders();
			UpdateSerializedProperty(ref materialProperty, MaterialsPropertyString);
		}
		
		EditorGUILayout.Space();

		if (GUILayout.Button("Select materials from the found list"))
			ShaderManager.instance.SelectMaterialsFromList();

		EditorGUILayout.Space();

		if (GUILayout.Button("Find shader for one selected material"))
			ShaderManager.instance.SelectShaderForThisMat();

		EditorGUILayout.Space();

		if (GUILayout.Button("Select required shaders"))
			ShaderManager.instance.SelectTheseShaders();

		EditorGUILayout.EndScrollView();
	}
}
