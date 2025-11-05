using System;

namespace Prana.NavLockUI
{
    public class NavLockData
    {
        public int LockId { get; set; }
        public string LockDate { get; set; }
        public int LockedById { get; set; }
        public string LockedByName { get; set; }
        public string LockCreationDate { get; set; }
    }
}
