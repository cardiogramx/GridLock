using System;
using System.Collections.Generic;

namespace GridLock
{
    public static class GridLockExtensions
    {
        public static T ToObject<T>(this GridLockItem gridLockItem) where T : GridLockItem
        {
            return gridLockItem as T;
        }

        public static T ToObjects<T>(this List<GridLockItem> gridLockItems) where T : List<GridLockItem>
        {
            return gridLockItems as T;
        }
    }
}
