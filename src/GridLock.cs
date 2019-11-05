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

       

        public async Task<List<T>> ListAsync<T>(CancellationToken cancellationToken = default) where T : GridLockItem
        {
            try
            {
                return await _storage.ReadAllObjectAsync<T>(cancellationToken);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<T> FindAsync<T>(string Id, CancellationToken cancellationToken = default) where T : GridLockItem
        {
            try
            {
                var _item = await _storage.ReadObjectAsync<T>(Id, cancellationToken);
                return _item == null ? null : _item;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<T> FindAsync<T>(T item, CancellationToken cancellationToken = default) where T : GridLockItem
        {
            try
            {
                var items = await _storage.ReadAllObjectAsync<T>(cancellationToken);
                var _item = items.Find(c => c.Equals(item));
                return _item == null ? null : _item;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<T> AddAsync<T>(T item, CancellationToken cancellationToken = default) where T : GridLockItem
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

        public async Task<T> UpdateAsync<T>(T item, CancellationToken cancellationToken = default) where T : GridLockItem
        {
            try
            {
                OnUpdating?.Invoke(null, new GridLockEventArgs() { Item = item });

                await _storage.UpdateObjectAsync<T>(item, cancellationToken);

                OnUpdated?.Invoke(null, new GridLockEventArgs() { Item = item });

                return item;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ValidateAsync<T>(string Id, CancellationToken cancellationToken = default) where T : GridLockItem
        {
            try
            {
                return await _storage.ReadObjectAsync<T>(Id, cancellationToken) switch
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

        public async Task<bool> ValidateAsync<T>(T item, CancellationToken cancellationToken = default) where T : GridLockItem
        {
            try
            {
                return await _storage.ReadObjectAsync<T>(item.Id, cancellationToken) switch
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

        public async Task DestroyAsync<T>(string Id, CancellationToken cancellationToken = default) where T : GridLockItem
        {
            var _item = await _storage.ReadObjectAsync<T>(Id, cancellationToken);

            if (_item != null)
            {
                OnDeleting?.Invoke(null, new GridLockEventArgs() { Item = _item });

                await _storage.RemoveObjectAsync(Id, cancellationToken);

                OnDeleted?.Invoke(null, new GridLockEventArgs() { Item = _item });
            }
        }

        public async Task DestroyAsync<T>(T item, CancellationToken cancellationToken = default) where T : GridLockItem
        {
            var items = await _storage.ReadAllObjectAsync<T>(cancellationToken);

            var _item = items.Find(c => c.Equals(item));

            if (_item != null)
            {
                OnDeleting?.Invoke(null, new GridLockEventArgs() { Item = _item });

                await _storage.RemoveObjectAsync(item.Id, cancellationToken);

                OnDeleted?.Invoke(null, new GridLockEventArgs() { Item = _item });
            }
        }



        public List<T> List<T>() where T : GridLockItem
        {
            try
            {
                return _storage.ReadAllObject<T>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public T Find<T>(string Id) where T : GridLockItem
        {
            try
            {
                var _item = _storage.ReadObject<T>(Id);

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

        public T Find<T>(T item) where T : GridLockItem
        {
            try
            {
                var _item = _storage.ReadAllObject<T>().Find(c => c.Equals(item));

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

        public T Add<T>(T item) where T : GridLockItem
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

        public T Update<T>(T item) where T : GridLockItem
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

        public bool Validate<T>(string Id) where T : GridLockItem
        {
            try
            {
                return _storage.ReadObjectAsync<T>(Id) switch
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

        public bool Validate<T>(T item) where T : GridLockItem
        {
            try
            {
                return _storage.ReadObject<T>(item.Id) switch
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

        public void Destroy<T>(string Id) where T : GridLockItem
        {
            var _item = _storage.ReadObject<T>(Id);

            if (_item != null)
            {
                OnDeleting?.Invoke(null, new GridLockEventArgs() { Item = _item });

                _storage.RemoveObject(Id);

                OnDeleted?.Invoke(null, new GridLockEventArgs() { Item = _item });
            }
        }

        public void Destroy<T>(T item) where T : GridLockItem
        {
            var _item = _storage.ReadAllObject<T>().Find(c => c.Equals(item));

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

        public DateTime ExpireBy { get; set; }
    }

}
