using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FindBrokenMesh : MonoBehaviour
{
	[MenuItem("Tools/Relliesaar/Fix Meshes With Broken Indexes")]
	static void RepairAllMeshes()
	{
		Vector2[] uvVectors;
		bool hasFoundBrokenMesh;
		int brokenMeshesCount = 0;
		int brokenIndexesCount = 0;
		MeshFilter[] meshes;

		meshes = FindObjectsOfType<MeshFilter>();

		foreach (MeshFilter mesh in meshes)
		{
			hasFoundBrokenMesh = false;
			uvVectors = mesh.sharedMesh?.uv;

			for (int i = 0; i < uvVectors?.Length; i++)
				if (float.IsNaN(uvVectors[i].x) || float.IsNaN(uvVectors[i].y))
				{
					if (!hasFoundBrokenMesh) brokenMeshesCount++;
					hasFoundBrokenMesh = true;
					brokenIndexesCount++;
					uvVectors[i] = Vector2.zero;
				}

			if (mesh.sharedMesh != null)
				mesh.sharedMesh.uv = uvVectors;
			else Debug.Log($"Cannot get shared mesh from {mesh.gameObject.name}");
		}
	}
}
