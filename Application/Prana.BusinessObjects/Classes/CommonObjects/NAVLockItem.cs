using Prana.LogManager;
using System;

namespace Prana.BusinessObjects
{
    public class NAVLockItem
    {
        public NAVLockItem()
        {
            ClientID = 0;
            AccountID = 0;
            LastLockDate = DateTime.MinValue;
            LockAppliedDate = DateTime.MinValue;
            LockUserID = 0;
            UnlockDate = DateTime.MinValue;
            UnlockUserID = 0;
            SugLockDate = DateTime.MinValue;
            LockSchedule = 0;
        }

        public int NavLockID { get; set; }

        public int ClientID { get; set; }

        public int AccountID { get; set; }

        public DateTime LastLockDate { get; set; }

        public DateTime LockAppliedDate { get; set; }

        public int LockUserID { get; set; }

        public DateTime UnlockDate { get; set; }

        public int UnlockUserID { get; set; }

        public DateTime SugLockDate { get; set; }

        public string LockMethod { get; set; }

        public int LockSchedule { get; set; }






        public static NAVLockItem fillData(object[] row, int offSet)
        {
            NAVLockItem accountNAVlock = new NAVLockItem();
            try
            {


                int clientID = 0 + offSet;
                int AccountID = 2 + offSet;
                int LockSchedule = 4 + offSet;
                int LockUserID = 5 + offSet;
                int LastLockDate = 6 + offSet;

                int UnlockUserID = 7 + offSet;
                int UnlockDate = 8 + offSet;
                int LockAppliedDate = 9 + offSet;
                //   int SugLockDate = 9 + offSet;

                if (row[clientID] != System.DBNull.Value)
                {
                    accountNAVlock.ClientID = int.Parse(row[clientID].ToString());
                }

                if (row[AccountID] != System.DBNull.Value)
                {
                    accountNAVlock.AccountID = int.Parse(row[AccountID].ToString());
                }
                if (row[LockSchedule] != System.DBNull.Value)
                {
                    accountNAVlock.LockSchedule = int.Parse(row[LockSchedule].ToString());
                }

                if (row[LockUserID] != System.DBNull.Value)
                {
                    accountNAVlock.LockUserID = int.Parse(row[LockUserID].ToString());
                }

                if (row[LastLockDate] != System.DBNull.Value)
                {
                    accountNAVlock.LastLockDate = DateTime.Parse(row[LastLockDate].ToString());
                }

                if (row[UnlockUserID] != System.DBNull.Value)
                {
                    accountNAVlock.UnlockUserID = int.Parse(row[UnlockUserID].ToString());
                }

                if (row[UnlockDate] != System.DBNull.Value)
                {
                    accountNAVlock.UnlockDate = DateTime.Parse(row[UnlockDate].ToString());
                }

                if (row[LockAppliedDate] != System.DBNull.Value)
                {
                    accountNAVlock.LockAppliedDate = DateTime.Parse(row[LockAppliedDate].ToString());
                }


            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return accountNAVlock;

        }

        public enum LockAction
        {
            LOCK = 1,
            UNLOCK = 2
        }
    }
}
