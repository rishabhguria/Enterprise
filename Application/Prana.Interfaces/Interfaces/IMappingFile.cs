using System;
using System.Collections.Generic;

namespace Prana.Interfaces
{
    public interface IMappingFile
    {
        System.Windows.Forms.Form Reference();
        event EventHandler MappingClosed;
        ISecurityMasterServices SecurityMaster
        {
            set;
        }
        List<string> activityType
        {
            set;
        }

    }
}
