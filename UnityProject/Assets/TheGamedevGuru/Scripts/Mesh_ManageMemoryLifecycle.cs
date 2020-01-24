using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TheGamedevGuru
{
[Serializable]
public class AssetReferenceMesh : AssetReferenceT<Mesh>
{
    public AssetReferenceMesh(string guid) : base(guid) { }
}

[Serializable]
public class AssetReferenceMaterial : AssetReferenceT<Material>
{
    public AssetReferenceMaterial(string guid) : base(guid) { }
}

public class Mesh_ManageMemoryLifecycle : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer = null;
    [SerializeField] private MeshFilter meshFilter = null;
    [SerializeField] private AssetReferenceMesh meshReference = null;
    [SerializeField] private List<AssetReferenceMaterial> materialReferences = null;
    [SerializeField] private bool manageMeshModel = false;
    [SerializeField] private bool manageMeshMaterials = false;
    private object[] _materialRuntimeKeys;
    private Material[] _savedMaterials;
    private AsyncOperationHandle<IList<Material>> _materialHandle;

    private void Awake()
    {
        _materialRuntimeKeys = new object[materialReferences.Count];
        for (var i = 0; i < materialReferences.Count; i++)
        {
            _materialRuntimeKeys[i] = materialReferences[i].RuntimeKey;
        }

        _savedMaterials = new Material[materialReferences.Count];
    }

    void OnEnable()
    {
        if (manageMeshModel)
        {
            meshReference.LoadAssetAsync<Mesh>().Completed += handle => meshFilter.sharedMesh = handle.Result;
        }

        if (manageMeshMaterials)
        {
            _materialHandle =
                Addressables.LoadAssetsAsync<Material>(_materialRuntimeKeys, null, Addressables.MergeMode.Union);
            _materialHandle.Completed += handle =>
            {
                handle.Result.CopyTo(_savedMaterials, 0);
                meshRenderer.sharedMaterials = _savedMaterials;
            };
        }
    }

    void OnDisable()
    {
        if (manageMeshModel)
        {
            var model = meshFilter.sharedMesh;
            meshFilter.sharedMesh = null;
            Addressables.Release(model);
        }

        if (manageMeshMaterials)
        {
            Addressables.Release(_materialHandle);
        }
    }
}
}