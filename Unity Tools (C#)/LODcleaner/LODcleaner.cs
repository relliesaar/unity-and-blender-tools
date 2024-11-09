using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LODcleaner : Editor
{
    [MenuItem("Tools/Relliesaar/Clear LOD component")]
    public static void CleanLOD()
    {
        List<LOD> lods = new List<LOD>();

        List<GameObject> childrenToReparent = new List<GameObject>();

        if (Selection.activeGameObject.TryGetComponent<LODGroup>(out var LODcomp))
        {
            foreach (LOD lodobj in LODcomp.GetLODs())
                lods.Add(new LOD { screenRelativeTransitionHeight = lodobj.screenRelativeTransitionHeight } );

            Undo.RecordObject(LODcomp, "clear lod component");
            LODcomp.SetLODs(lods.ToArray());
            PrefabUtility.RecordPrefabInstancePropertyModifications(LODcomp);
        }
    }
}