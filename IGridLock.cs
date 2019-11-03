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

        Task<List<GridLockItem>> ListAsync(CancellationToken cancellationToken = default);
        Task<GridLockItem> FindAsync(string Id, CancellationToken cancellationToken = default);
        Task<GridLockItem> FindAsync(GridLockItem item, CancellationToken cancellationToken = default);
        Task<GridLockItem> AddAsync(GridLockItem item, CancellationToken cancellationToken = default);
        Task<GridLockItem> UpdateAsync(GridLockItem item, CancellationToken cancellationToken = default);
        Task DestroyAsync(string key, CancellationToken cancellationToken = default);
        Task DestroyAsync(GridLockItem item, CancellationToken cancellationToken = default);
        Task<bool> ValidateAsync(string key, CancellationToken cancellationToken = default);
        Task<bool> ValidateAsync(GridLockItem item, CancellationToken cancellationToken = default);


        List<GridLockItem> List();
        GridLockItem Find(string Id);
        GridLockItem Find(GridLockItem item);
        GridLockItem Add(GridLockItem itemt);
        GridLockItem Update(GridLockItem item);
        void Destroy(string key);
        void Destroy(GridLockItem item);
        bool Validate(string key);
        bool Validate(GridLockItem item);


        event EventHandler<GridLockEventArgs> OnComitting;
        event EventHandler<GridLockEventArgs> OnComitted;

        event EventHandler<GridLockEventArgs> OnDeleting;
        event EventHandler<GridLockEventArgs> OnDeleted;

        event EventHandler<GridLockEventArgs> OnUpdating;
        event EventHandler<GridLockEventArgs> OnUpdated;
    }
}
