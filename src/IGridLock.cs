using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Caching.Distributed;

namespace GridLock
{
    public interface IGridLock
    {
        DistributedCacheEntryOptions Options { get; set; }

        Task<List<T>> ListAsync<T>(CancellationToken cancellationToken = default) where T : GridLockItem;
        Task<T> FindAsync<T>(string Id, CancellationToken cancellationToken = default) where T : GridLockItem;
        Task<T> FindAsync<T>(T item, CancellationToken cancellationToken = default) where T : GridLockItem;
        Task<T> AddAsync<T>(T item, CancellationToken cancellationToken = default) where T : GridLockItem;
        Task<T> UpdateAsync<T>(T item, CancellationToken cancellationToken = default) where T : GridLockItem;
        Task DestroyAsync<T>(string key, CancellationToken cancellationToken = default) where T : GridLockItem;
        Task DestroyAsync<T>(T item, CancellationToken cancellationToken = default) where T : GridLockItem;
        Task<bool> ValidateAsync<T>(string key, CancellationToken cancellationToken = default) where T : GridLockItem;
        Task<bool> ValidateAsync<T>(T item, CancellationToken cancellationToken = default) where T : GridLockItem;


        List<T> List<T>() where T : GridLockItem;
        T Find<T>(string Id) where T : GridLockItem;
        T Find<T>(T item) where T : GridLockItem;
        T Add<T>(T item) where T : GridLockItem;
        T Update<T>(T item) where T : GridLockItem;
        void Destroy<T>(string key) where T : GridLockItem;
        void Destroy<T>(T item) where T : GridLockItem;
        bool Validate<T>(string key) where T : GridLockItem;
        bool Validate<T>(T item) where T : GridLockItem;


        event EventHandler<GridLockEventArgs> OnComitting;
        event EventHandler<GridLockEventArgs> OnComitted;

        event EventHandler<GridLockEventArgs> OnDeleting;
        event EventHandler<GridLockEventArgs> OnDeleted;

        event EventHandler<GridLockEventArgs> OnUpdating;
        event EventHandler<GridLockEventArgs> OnUpdated;
    }
}
