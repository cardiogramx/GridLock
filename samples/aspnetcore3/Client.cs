using System;

using GridLock;

namespace aspnetcore3
{
    public class Client : GridLockItem
    {
        public DateTime DateTimeAdded { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;
    }
}
