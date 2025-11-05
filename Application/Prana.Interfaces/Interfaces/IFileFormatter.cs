using System.Collections;
using System.Collections.Generic;

namespace Prana.Interfaces
{
    public interface IFileFormatter
    {
        bool CreateFile(IList list, IList summary, string Location, Dictionary<string, string> columnsWithSpecifiedNames);
    }
}
