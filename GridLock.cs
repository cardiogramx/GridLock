using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using GridLock.Extensions.Storage.Distributed;
using Microsoft.Extensions.Caching.Distributed;

namespace GridLock
{
    /// <summary>
    /// The default implementation of <see cref="IGridLock"/>
    /// </summary>
    public class GridLock : IGridLock
    {
        private readonly ISharedStorage _storage;

        public DistributedCacheEntryOptions Options {
            get => _storage.Options;
            set => _storage.Options = value; }

        public GridLock(SharedStorage storage)
        {
            _storage = storage;
        }

        public GridLock(ISharedStorage storage)
        {
            _storage = storage;
        }

       

        public async Task<List<GridLockItem>> ListAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _storage.ReadAllObjectAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<GridLockItem> FindAsync(string Id, CancellationToken cancellationToken = default)
        {
            try
            {
                var _item = await _storage.ReadObjectAsync(Id, cancellationToken);
                return _item == null ? null : _item;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<GridLockItem> FindAsync(GridLockItem item, CancellationToken cancellationToken = default)
        {
            try
            {
                var items = await _storage.ReadAllObjectAsync(cancellationToken);
                var _item = items.Find(c => c.Equals(item));
                return _item == null ? null : _item;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<GridLockItem> AddAsync(GridLockItem item, CancellationToken cancellationToken = default)
        {
            try
            {
                OnComitting?.Invoke(null, new GridLockEventArgs() { Item = item });

                await _storage.SaveObjectAsync(item, cancellationToken);

                OnComitted?.Invoke(null, new GridLockEventArgs() { Item = item });

                return item;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<GridLockItem> UpdateAsync(GridLockItem item, CancellationToken cancellationToken = default)
        {
            try
            {
                OnUpdating?.Invoke(null, new GridLockEventArgs() { Item = item });

                await _storage.UpdateObjectAsync(item, cancellationToken);

                OnUpdated?.Invoke(null, new GridLockEventArgs() { Item = item });

                return item;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ValidateAsync(string Id, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _storage.ReadObjectAsync(Id, cancellationToken) switch
                {
                    null => false,
                    _ => true
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ValidateAsync(GridLockItem item, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _storage.ReadObjectAsync(item.Id, cancellationToken) switch
                {
                    null => false,
                    _ => true
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DestroyAsync(string Id, CancellationToken cancellationToken = default)
        {
            var _item = await _storage.ReadObjectAsync(Id, cancellationToken);

            if (_item != null)
            {
                OnDeleting?.Invoke(null, new GridLockEventArgs() { Item = _item });

                await _storage.RemoveObjectAsync(Id, cancellationToken);

                OnDeleted?.Invoke(null, new GridLockEventArgs() { Item = _item });
            }
        }

        public async Task DestroyAsync(GridLockItem item, CancellationToken cancellationToken = default)
        {
            var items = await _storage.ReadAllObjectAsync(cancellationToken);

            var _item = items.Find(c => c.Equals(item));

            if (_item != null)
            {
                OnDeleting?.Invoke(null, new GridLockEventArgs() { Item = _item });

                await _storage.RemoveObjectAsync(item.Id, cancellationToken);

                OnDeleted?.Invoke(null, new GridLockEventArgs() { Item = _item });
            }
        }



        public List<GridLockItem> List()
        {
            try
            {
                return _storage.ReadAllObject();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public GridLockItem Find(string Id)
        {
            try
            {
                var _item = _storage.ReadObject(Id);

                return _item switch
                {
                    null => null,
                    _ => _item
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public GridLockItem Find(GridLockItem item)
        {
            try
            {
                var _item = _storage.ReadAllObject().Find(c => c.Equals(item));

                return _item switch
                {
                    null => null,
                    _ => _item
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public GridLockItem Add(GridLockItem item)
        {
            try
            {
                OnComitting?.Invoke(null, new GridLockEventArgs() { Item = item });

                _storage.SaveObject(item);

                OnComitted?.Invoke(null, new GridLockEventArgs() { Item = item });

                return item;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public GridLockItem Update(GridLockItem item)
        {
            try
            {
                OnUpdating?.Invoke(null, new GridLockEventArgs() { Item = item });

                _storage.UpdateObject(item);

                OnUpdated?.Invoke(null, new GridLockEventArgs() { Item = item });

                return item;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Validate(string Id)
        {
            try
            {
                return _storage.ReadObjectAsync(Id) switch
                {
                    null => false,
                    _ => true
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Validate(GridLockItem item)
        {
            try
            {
                return _storage.ReadObject(item.Id) switch
                {
                    null => false,
                    _ => true
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Destroy(string Id)
        {
            var _item = _storage.ReadObject(Id);

            if (_item != null)
            {
                OnDeleting?.Invoke(null, new GridLockEventArgs() { Item = _item });

                _storage.RemoveObject(Id);

                OnDeleted?.Invoke(null, new GridLockEventArgs() { Item = _item });
            }
        }

        public void Destroy(GridLockItem item)
        {
            var _item = _storage.ReadAllObject().Find(c => c.Equals(item));

            if (_item != null)
            {
                OnDeleting?.Invoke(null, new GridLockEventArgs() { Item = _item });

                _storage.RemoveObject(item.Id);

                OnDeleted?.Invoke(null, new GridLockEventArgs() { Item = _item });
            }
        }


        public event EventHandler<GridLockEventArgs> OnComitting;
        public event EventHandler<GridLockEventArgs> OnComitted;
        public event EventHandler<GridLockEventArgs> OnDeleting;
        public event EventHandler<GridLockEventArgs> OnDeleted;
        public event EventHandler<GridLockEventArgs> OnUpdating;
        public event EventHandler<GridLockEventArgs> OnUpdated;
    }

    public class GridLockItem
    {
        public string Id { get; set; }

        public int Level { get; set; }
    }

}
