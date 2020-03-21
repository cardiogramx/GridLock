using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;

using Microsoft.Extensions.Caching.Distributed;

using GridLock.Events;

namespace GridLock
{
    public interface IGridLock
    {
        DistributedCacheEntryOptions Options { get; set; }

        /// <summary>
        /// Asynchronously returns the list of all the specified type stored and tracked in GridLock.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<T>> ListAsync<T>(CancellationToken cancellationToken = default) where T : GridLockItem;

        /// <summary>
        /// Asynchronously finds an object of specified type stored and tracked by GridLock.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T> FindAsync<T>(string Id, CancellationToken cancellationToken = default) where T : GridLockItem;

        /// <summary>
        /// Asynchronously finds an object of specified type stored and tracked by GridLock.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T> FindAsync<T>(T item, CancellationToken cancellationToken = default) where T : GridLockItem;

        /// <summary>
        /// Asynchronously adds an item of specified type to GridLock for tracking.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T> AddAsync<T>(T item, CancellationToken cancellationToken = default) where T : GridLockItem;

        /// <summary>
        /// Asynchronously updates a tracked item of specified type in GridLock.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T> UpdateAsync<T>(T item, CancellationToken cancellationToken = default) where T : GridLockItem;

        /// <summary>
        /// Asynchronously destroys and removes an object tracked by GridLock using the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DestroyAsync<T>(string Id, CancellationToken cancellationToken = default) where T : GridLockItem;

        /// <summary>
        /// Asynchronously destroys and removes an object tracked by GridLock.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DestroyAsync<T>(T item, CancellationToken cancellationToken = default) where T : GridLockItem;

        /// <summary>
        /// Asynchronously validates if an object is tracked by GridLock using the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> ValidateAsync<T>(string key, CancellationToken cancellationToken = default) where T : GridLockItem;

        /// <summary>
        /// Asynchronously validates if an object is tracked by GridLock.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> ValidateAsync<T>(T item, CancellationToken cancellationToken = default) where T : GridLockItem;


        /// <summary>
        /// Returns the list of all the specified type stored and tracked in GridLock.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        List<T> List<T>() where T : GridLockItem;

        /// <summary>
        /// Finds an object of specified type stored and tracked by GridLock.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="Id"></param>
        /// <returns></returns>
        T Find<T>(string Id) where T : GridLockItem;

        /// <summary>
        /// Finds an object of specified type stored and tracked by GridLock.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        T Find<T>(T item) where T : GridLockItem;

        /// <summary>
        /// Adds an item of specified type to GridLock for tracking.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        T Add<T>(T item) where T : GridLockItem;

        /// <summary>
        /// Updates a tracked item of specified type in GridLock.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        T Update<T>(T item) where T : GridLockItem;

        /// <summary>
        /// Destroys and removes an object tracked by GridLock using the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        void Destroy<T>(string key) where T : GridLockItem;

        /// <summary>
        /// Destroys and removes an object tracked by GridLock.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        void Destroy<T>(T item) where T : GridLockItem;

        /// <summary>
        /// Validates if an object is tracked by GridLock using the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Validate<T>(string key) where T : GridLockItem;

        /// <summary>
        /// Validates if an object is tracked by GridLock.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        bool Validate<T>(T item) where T : GridLockItem;

        /// <summary>
        /// Triggers before adding a new object to GridList.
        /// </summary>
        event EventHandler<GridLockEventArgs> OnAdding;

        /// <summary>
        /// Triggers after an object is added to GridList.
        /// </summary>
        event EventHandler<GridLockEventArgs> OnAdded;

        /// <summary>
        /// Triggers before destroying an object tracked by GridList.
        /// </summary>
        event EventHandler<GridLockEventArgs> OnDestroying;

        /// <summary>
        /// Triggers after an object is removed from GridList.
        /// </summary>
        event EventHandler<GridLockEventArgs> OnDestroyed;

        /// <summary>
        /// Triggers before a tracked object is updated in GridList.
        /// </summary>
        event EventHandler<GridLockEventArgs> OnUpdating;

        /// <summary>
        /// Triggers after a tracked object is updated in GridList.
        /// </summary>
        event EventHandler<GridLockEventArgs> OnUpdated;
    }
}
