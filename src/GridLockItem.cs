using System;

namespace GridLock
{
    public class GridLockItem
    {
        public string Id { get; set; }

        public int Level { get; set; } = 0;

        public DateTime ExpiresOn { get; set; }
    }

}
