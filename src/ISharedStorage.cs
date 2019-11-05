using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace GridLock.Extensions.Storage.Distributed
{
    public interface ISharedStorage
    {
        DistributedCacheEntryOptions Options { get; set; }

        /// <summary>
        /// Asynchronously saves the value of the generic type specified independently into the distributed cache storage.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T> SaveObjectAsync<T>(T item, CancellationToken cancellationToken = default) where T : GridLockItem;

        /// <summary>
        /// Asynchronously updates the value of the generic type specified in the distributed cache storage.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T> UpdateObjectAsync<T>(T item, CancellationToken cancellationToken = default) where T : GridLockItem;

        /// <summary>
        /// Asynchronously reads the value of the generic type from the distributed cache storage using the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T> ReadObjectAsync<T>(string key, CancellationToken cancellationToken = default) where T : GridLockItem;

        /// <summary>
        /// Asynchronously reads all values of the generic type from the distributed cache storage.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<T>> ReadAllObjectAsync<T>(CancellationToken cancellationToken = default) where T : GridLockItem;

        /// <summary>
        /// Asynchronously removes a value in the distributed cache storage using the specified key.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task RemoveObjectAsync(string key, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously removes all objects from the distributed cache storage.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task RemoveAllObjectAsync(CancellationToken cancellationToken = default);




        /// <summary>
        /// Saves the value of the generic type specified independently into the distributed cache storage.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        T SaveObject<T>(T item) where T : GridLockItem;

        /// <summary>
        /// Updates the value of the generic type specified in the distributed cache storage.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        T UpdateObject<T>(T item) where T : GridLockItem;

        /// <summary>
        /// Reads the value of the generic type from the distributed cache storage using the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T ReadObject<T>(string key) where T : GridLockItem;

        /// <summary>
        /// Reads all values of the generic type from the distributed cache storage.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        List<T> ReadAllObject<T>() where T : GridLockItem;

        /// <summary>
        /// Removes a value in the distributed cache storage using the specified key.
        /// </summary>
        /// <param name="key"></param>
        void RemoveObject(string key);

        /// <summary>
        /// Removes all objects from the distributed cache storage.
        /// </summary>
        void RemoveAllObject();


        //void StoreKey(string key);
        //Task StoreKeyAsync(string key, CancellationToken cancellationToken = default);

        //List<string> GetStoredKeys();
        //Task<List<string>> GetStoredKeysAsync(CancellationToken cancellationToken = default);

        //void RemoveKey(string key);
        //Task RemoveKeyAsync(string key, CancellationToken cancellationToken = default);
    }
}
