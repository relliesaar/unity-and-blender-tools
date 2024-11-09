using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "LightmapPresetsData", menuName = "ScriptableObjects/LightmapPresetsData")] // create custom asset in editor
public class LightmapPresetsData : ScriptableObject
{
    public List<LightmapParameters> lightmapParameters;
}
