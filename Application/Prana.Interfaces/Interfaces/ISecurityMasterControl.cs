using Prana.BusinessObjects;
using System;

namespace Prana.Interfaces
{
    public interface ISecurityMasterControl
    {
        event EventHandler LaunchPricingInput;
        event EventHandler SymbolDoubleClicked;
        void HandleOnLoadRequest(ListEventAargs args);
        void SetClosingUIFrmExternally(bool isCLosingFromExt);
    }
}
