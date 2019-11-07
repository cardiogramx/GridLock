using System;

using GridLock;

namespace aspnetcore3
{
    public class Client : GridLockItem  //An object to be tracked by GridLock must inherit from GridLockItem
    {
        public DateTime DateTimeAdded { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;
    }
}
