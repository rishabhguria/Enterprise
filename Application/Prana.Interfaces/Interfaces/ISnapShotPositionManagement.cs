using System;

namespace Prana.Interfaces
{
    public interface ISnapShotPositionManagement
    {
        System.Windows.Forms.Form Reference();
        event EventHandler FormClosedHandler;
        void SetUp();
        event EventHandler LaunchPreferences;
    }
}
