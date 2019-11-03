using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace GridLock.Extensions.Storage.Distributed
{
    public interface ISharedStorage
    {
        DistributedCacheEntryOptions Options { get; set; }

        Task<GridLockItem> SaveObjectAsync(GridLockItem item, CancellationToken cancellationToken = default);
        Task<GridLockItem> UpdateObjectAsync(GridLockItem item, CancellationToken cancellationToken = default);
        Task<GridLockItem> ReadObjectAsync(string key, CancellationToken cancellationToken = default);
        Task<List<GridLockItem>> ReadAllObjectAsync(CancellationToken cancellationToken = default);
        Task RemoveObjectAsync(string key, CancellationToken cancellationToken = default);
        Task RemoveAllObjectAsync(CancellationToken cancellationToken = default);

        GridLockItem SaveObject(GridLockItem item);
        GridLockItem UpdateObject(GridLockItem item);
        GridLockItem ReadObject(string key);
        List<GridLockItem> ReadAllObject();
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
