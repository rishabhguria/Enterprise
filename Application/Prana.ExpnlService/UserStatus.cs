namespace Prana.ExpnlService
{
    public class UserStatus
    {
        bool userBusyStatus = false;
        long userFreeUpTime = 100;

        public bool UserBusyStatus
        {
            get { return userBusyStatus; }
            set { userBusyStatus = value; }
        }
        public long UserFreeUpTime
        {
            get { return userFreeUpTime; }
            set { userFreeUpTime = value; }
        }
        System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
        public System.Diagnostics.Stopwatch UserSW
        {
            get
            {

                return stopWatch;

            }
        }
    }
}
