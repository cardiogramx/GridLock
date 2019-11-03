using System;
namespace GridLock
{
    public static class GridLockExtensions
    {
        public static T ToObject<T>(this GridLockItem gridLockItem) where T : GridLockItem
        {
            return gridLockItem as T;
        }
    }
}
