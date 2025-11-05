namespace Prana.ServerCommon
{
    public class UserSettingConstants
    {
        private static bool _isDebugModeEnabled = false;
        public static bool IsDebugModeEnabled
        {
            set { _isDebugModeEnabled = value; }
            get
            {
                return _isDebugModeEnabled;
            }
        }
    }
}
