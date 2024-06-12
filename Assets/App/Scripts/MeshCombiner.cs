using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace App.Scripts
{
    public class MeshCombiner : MonoBehaviour
    {
        [SerializeField] private List<MeshFilter> sourceMeshFilters;
        [SerializeField] private MeshFilter targetMeshFilter;

        [ContextMenu("Combine Meshes")]
        private void CombineMeshes()
        {
            var combine = new CombineInstance[sourceMeshFilters.Count];

            for (var i = 0; i < sourceMeshFilters.Count; i++)
            {
                combine[i].mesh = sourceMeshFilters[i].sharedMesh;
                combine[i].transform = sourceMeshFilters[i].transform.localToWorldMatrix;
            }

            var mesh = new Mesh();
            mesh.CombineMeshes(combine);
            targetMeshFilter.mesh = mesh;

            SaveMesh(targetMeshFilter.sharedMesh, gameObject.name, false, true);
        }

        private void SaveMesh(Mesh mesh, string objName, bool makeNewInstance, bool optimizeMesh)
        {
            string path = EditorUtility.SaveFilePanel("Save Separate Mesh Agent", "Assets/Media/Meshes", objName, "asset");

            path = FileUtil.GetProjectRelativePath(path);

            Mesh meshToSave = (makeNewInstance) ? Instantiate(mesh) : mesh;

            if (optimizeMesh)
            {
                MeshUtility.Optimize(meshToSave);
            }
        
            AssetDatabase.CreateAsset(meshToSave, path);
            AssetDatabase.SaveAssets();
        }
    }
}