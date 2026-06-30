using Infrastructure.Services;
using UnityEngine;

namespace Infrastructure.AssetsManagement
{
    public interface IAssetsProvider : IService
    {
        GameObject InstantiatePrefabFromResources(string path);
        GameObject InstantiatePrefabFromResources(string path, Vector3 position);
    }
}