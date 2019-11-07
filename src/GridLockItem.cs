using System;

namespace GridLock
{
    public class GridLockItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        public int Level { get; set; } = 0;

        public DateTime ExpiresBy { get; internal set; } = DateTime.UtcNow;
    }

}
