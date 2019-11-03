using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace GridLock.Extensions.Storage.Distributed
{
    public interface ISharedStorage
    {
        DistributedCacheEntryOptions Options { get; set; }

        Task<T> SaveObjectAsync<T>(T item, CancellationToken cancellationToken = default) where T : GridLockItem;
        Task<T> UpdateObjectAsync<T>(T item, CancellationToken cancellationToken = default) where T : GridLockItem;
        Task<T> ReadObjectAsync<T>(string key, CancellationToken cancellationToken = default) where T : GridLockItem;
        Task<List<T>> ReadAllObjectAsync<T>(CancellationToken cancellationToken = default) where T : GridLockItem;
        Task RemoveObjectAsync(string key, CancellationToken cancellationToken = default);
        Task RemoveAllObjectAsync(CancellationToken cancellationToken = default);

        T SaveObject<T>(T item) where T : GridLockItem;
        T UpdateObject<T>(T item) where T : GridLockItem;
        T ReadObject<T>(string key) where T : GridLockItem;
        List<T> ReadAllObject<T>() where T : GridLockItem;
        void RemoveObject(string key);
        void RemoveAllObject();


        //void StoreKey(string key);
        //Task StoreKeyAsync(string key, CancellationToken cancellationToken = default);

        //List<string> GetStoredKeys();
        //Task<List<string>> GetStoredKeysAsync(CancellationToken cancellationToken = default);

        //void RemoveKey(string key);
        //Task RemoveKeyAsync(string key, CancellationToken cancellationToken = default);
    }
}
