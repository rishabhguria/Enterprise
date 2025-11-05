using System;
using System.Data;

namespace Prana.ExposurePnlCache
{
    public interface IVariableColumnsTable
    {
        DataTable TableWithColums
        {
            get;
            set;
        }

        Type ObjectForDataTable
        {
            get;
            set;
        }
    }
}
