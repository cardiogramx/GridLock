using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace GridLock.Extensions.Storage.Distributed
{
    /// <summary>
    /// The default implementation of <see cref="ISharedStorage"/>
    /// </summary>
    public class SharedStorage : ISharedStorage
    {
        private readonly IDistributedCache _distributedCache;
        private protected DistributedCacheEntryOptions _options;

        public DistributedCacheEntryOptions Options
        {
            get => _options;
            set => _options = value;
        }

        /// <summary>
        /// The default constructor for Dependency Injection
        /// </summary>
        /// <param name="cache"></param>
        public SharedStorage(IDistributedCache cache)
        {
            _distributedCache = cache;

            _options = new DistributedCacheEntryOptions()
            {
                SlidingExpiration = TimeSpan.FromSeconds(10)
            };
        }


        public async Task<T> SaveObjectAsync<T>(T item, CancellationToken cancellationToken = default) where T : GridLockItem
        {
            var store = await GetStoredKeysAsync(cancellationToken);

            if (store.Contains(item.Id))
            {
                return item;
            }

            var json = JsonSerializer.Serialize(item);// JsonConvert.SerializeObject(item);

            await _distributedCache.SetStringAsync(item.Id, json, _options, cancellationToken);

            await StoreKeyAsync(item.Id, cancellationToken);

            return item;
        }

        public async Task<T> UpdateObjectAsync<T>(T item, CancellationToken cancellationToken = default) where T : GridLockItem
        {
            var store = await GetStoredKeysAsync(cancellationToken);

            if (store.Contains(item.Id))
            {
                await RemoveObjectAsync(item.Id, cancellationToken);
            }

            var json = JsonSerializer.Serialize(item);

            await _distributedCache.SetStringAsync(item.Id, json, _options, cancellationToken);

            await StoreKeyAsync(item.Id, cancellationToken);

            return item;
        }

        public async Task<T> ReadObjectAsync<T>(string key, CancellationToken cancellationToken = default) where T : GridLockItem
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return null;
            }

            var json = await _distributedCache.GetStringAsync(key, cancellationToken);

            if (string.IsNullOrWhiteSpace(json))
            {
                await RemoveKeyAsync(key, cancellationToken);
                return null;
            }

            var response = JsonSerializer.Deserialize<T>(json);

            return response;
        }

        public async Task<List<T>> ReadAllObjectAsync<T>(CancellationToken cancellationToken = default) where T : GridLockItem
        {
            var gridList = new List<T>();

            foreach (var key in await GetStoredKeysAsync(cancellationToken))
            {
                gridList.Add(await ReadObjectAsync<T>(key, cancellationToken));
            }

            return gridList;
        }

        public async Task RemoveObjectAsync(string key, CancellationToken cancellationToken = default)
        {
            await _distributedCache.RemoveAsync(key, cancellationToken);
            await RemoveKeyAsync(key, cancellationToken);
        }

        public async Task RemoveAllObjectAsync(CancellationToken cancellationToken = default)
        {
            foreach (var key in await GetStoredKeysAsync(cancellationToken))
            {
                await RemoveObjectAsync(key, cancellationToken);
            }

            await _distributedCache.RemoveAsync("keyList", cancellationToken);
        }



        public T SaveObject<T>(T item) where T : GridLockItem
        {
            if (GetStoredKeys().Contains(item.Id))
            {
                return item;
            }

            var json = JsonSerializer.Serialize(item);

            _distributedCache.SetString(item.Id, json, _options);

            StoreKey(item.Id);

            return item;
        }

        public T UpdateObject<T>(T item) where T : GridLockItem
        {
            if (GetStoredKeys().Contains(item.Id))
            {
                RemoveObject(item.Id);
            }
           
            var json = JsonSerializer.Serialize(item);

            _distributedCache.SetString(item.Id, json, _options);

            StoreKey(item.Id);

            return item;
        }

        public T ReadObject<T>(string key) where T : GridLockItem
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return null;
            }

            var json = _distributedCache.GetString(key);

            if (string.IsNullOrWhiteSpace(json))
            {
                RemoveKey(key);
                return null;
            }

            var response = JsonSerializer.Deserialize<T>(json);

            return response;
        }

        public List<T> ReadAllObject<T>() where T : GridLockItem
        {
            var gridList = new List<T>();

            foreach (var key in GetStoredKeys())
            {
                gridList.Add(ReadObject<T>(key));
            }

            return gridList;
        }

        public void RemoveObject(string key)
        {
            _distributedCache.Remove(key);
            RemoveKey(key);
        }

        public void RemoveAllObject()
        {
            foreach (var key in GetStoredKeys())
            {
                RemoveObject(key);
            }

            _distributedCache.Remove("keyList");
        }


        /// <summary>
        /// Stores a tracking key into the distributed cache storage using the specified key
        /// </summary>
        /// <param name="key"></param>
        private void StoreKey(string key)
        {
            var keyList = GetStoredKeys();

            if (!keyList.Contains(key))
            {
                keyList.Add(key);

                var json = JsonSerializer.Serialize(keyList); //JsonConvert.SerializeObject(keyList);

                _distributedCache.SetString("keyList", json);
            }
        }


        /// <summary>
        /// Asynchronously stores a tracking key into the distributed cache storage using the specified key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task StoreKeyAsync(string key, CancellationToken cancellationToken = default)
        {
            var keyList = await GetStoredKeysAsync(cancellationToken);

            if (!keyList.Contains(key))
            {
                keyList.Add(key);

                var json = JsonSerializer.Serialize(keyList);// JsonConvert.SerializeObject(keyList);

                await _distributedCache.SetStringAsync("keyList", json, cancellationToken);
            }
        }


        /// <summary>
        /// Gets the list of tracking keys from the distributed cache storage
        /// </summary>
        /// <returns></returns>
        private List<string> GetStoredKeys()
        {
            var json = _distributedCache.GetString("keyList");

            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<string>();
            }

            return JsonSerializer.Deserialize<List<string>>(json);
        }


        /// <summary>
        /// Asynchronously gets the list of tracking keys from the distributed cache storage
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<List<string>> GetStoredKeysAsync(CancellationToken cancellationToken = default)
        {
            var json = await _distributedCache.GetStringAsync("keyList", cancellationToken);

            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<string>();
            }

            return JsonSerializer.Deserialize<List<string>>(json);
        }


        /// <summary>
        /// Removes a tracking key from the distributed cache storage using the specified key
        /// </summary>
        /// <param name="key"></param>
        private void RemoveKey(string key)
        {
            var keyList = GetStoredKeys();

            if (keyList.Contains(key))
            {
                keyList.Remove(key);

                var json = JsonSerializer.Serialize(keyList);// JsonConvert.SerializeObject(keyList);

                _distributedCache.SetString("keyList", json);
            }
        }

        /// <summary>
        /// Asynchronously removes a tracking key from the distributed cache storage using the specified key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task RemoveKeyAsync(string key, CancellationToken cancellationToken = default)
        {
            var keyList = await GetStoredKeysAsync(cancellationToken);

            if (keyList.Contains(key))
            {
                keyList.Remove(key);

                var json = JsonSerializer.Serialize(keyList); //JsonConvert.SerializeObject(keyList);

                await _distributedCache.SetStringAsync("keyList", json, cancellationToken);
            }
        }
    }

}
