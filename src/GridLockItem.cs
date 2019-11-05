﻿using System;

namespace GridLock
{
    public class GridLockItem
    {
        public string Id { get; set; }

        public int Level { get; set; }

        public DateTime ExpiresBy { get; internal set; }
    }

}
