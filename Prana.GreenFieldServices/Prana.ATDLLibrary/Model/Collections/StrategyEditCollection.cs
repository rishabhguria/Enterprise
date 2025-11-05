using System.Collections.ObjectModel;
using System.Diagnostics;
using Prana.ATDLLibrary.Fix;
using Prana.ATDLLibrary.Model.Elements;
using Prana.ATDLLibrary.Model.Elements.Support;
using Prana.LogManager;

namespace Prana.ATDLLibrary.Model.Collections
{
    /// <summary>
    /// Collection class of <see cref="StrategyEdit_t">StrategyEdit</see>s.
    /// </summary>
    public class StrategyEditCollection : Collection<StrategyEdit_t>
    {  
        protected override void InsertItem(int index, StrategyEdit_t item)
        {
            Logger.LoggerWrite(string.Format("StrategyEdit added"), LoggingConstants.ATDL_LOGGING, 1, 1, TraceEventType.Verbose);

            base.InsertItem(index, item);
        }
    }
}
