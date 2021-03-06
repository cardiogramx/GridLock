﻿using System;

namespace GridLock.Events
{
    /// <summary>
    /// Represents the base class that every GridLockEventArgs item must inherit from.
    /// </summary>
    public class GridLockEventArgs : EventArgs
    {
        public GridLockItem Item { get; set; }
    }
}
