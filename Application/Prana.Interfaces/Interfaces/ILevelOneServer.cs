using System;

namespace Prana.Interfaces
{
    public interface ILevelOneServer : Prana.Interfaces.ILiveFeedApplicationConstants
    {
        void GetLevel1SnapShot(string Symbol);
        event EventHandler Level1SnapshotResponse;
    }
}
