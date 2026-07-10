using System.Threading.Tasks;
using Infrastructure.Services;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Infrastructure.AssetsManagement
{
    public interface IAssetsProvider : IService
    {
        Task<GameObject> InstantiateFromAddressables(string address);
        Task<GameObject> InstantiateFromAddressables(string address, Vector3 position);
        Task<T> Load<T>(AssetReference assetReference) where T : class;
        Task<T> Load<T>(string address) where T : class;
        void Initialize();
        void Dispose();
    }
}