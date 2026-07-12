using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Infrastructure.AssetsManagement
{
    public class AssetsProvider : IAssetsProvider
    {
        private Dictionary<string, AsyncOperationHandle> _completedHandles = new();
        private Dictionary<string, List<AsyncOperationHandle>> _cachedHandles = new();

        public Task<GameObject> InstantiateFromAddressables(string address)
        {
            return Addressables.InstantiateAsync(address).Task;
        }

        public Task<GameObject> InstantiateFromAddressables(string address, Vector3 position)
        {
            return Addressables.InstantiateAsync(address, position, Quaternion.identity).Task;
        }

        public Task<GameObject> InstantiateFromAddressables(string address, Transform parent)
        {
            return Addressables.InstantiateAsync(address, parent).Task;
        }

        public void Initialize()
        {
            Addressables.InitializeAsync();
        }

        public async Task<T> Load<T>(AssetReference assetReference) where T : class
        {
            if (_completedHandles.TryGetValue(assetReference.AssetGUID, out AsyncOperationHandle cachedHandle))
            {
                return cachedHandle.Result as T;
            }

            return await RunCached(Addressables.LoadAssetAsync<T>(assetReference), cacheKey: assetReference.AssetGUID); 
        }

        public async Task<T> Load<T>(string address) where T : class
        {
            if (_completedHandles.TryGetValue(address, out AsyncOperationHandle cachedHandle))
            {
                return cachedHandle.Result as T;
            }
            
            return await RunCached(Addressables.LoadAssetAsync<T>(address), cacheKey: address); 
        }

        public void Dispose()
        {
            foreach (List<AsyncOperationHandle> cachedHandles in _cachedHandles.Values)
            {
                foreach (AsyncOperationHandle handle in cachedHandles)
                {
                    Addressables.Release(handle);
                }
            }
            
            _cachedHandles.Clear();
            _completedHandles.Clear();
        }

        private async Task<T> RunCached<T>(AsyncOperationHandle<T> handle, string cacheKey) where T : class
        {
            handle.Completed += completedHandle =>
            {
                Debug.Log($"Add cached handle {cacheKey}");
                _completedHandles[cacheKey] = completedHandle;
            };

            AddHandleToCached(cacheKey, handle);

            return await handle.Task;
        }

        private void AddHandleToCached<T>(string key, AsyncOperationHandle<T> handle) where T : class
        {
            if (!_cachedHandles.TryGetValue(key, out List<AsyncOperationHandle> pendingList))
            {
                pendingList = new List<AsyncOperationHandle>();
                _cachedHandles[key] = pendingList;
            }
            
            pendingList.Add(handle);
        }
    }
}