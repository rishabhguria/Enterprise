using System;
using System.Collections.Generic;
using System.Text;
using Nirvana.Admin.PositionManagement.Classes;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{

    public enum RunUploadStatus
    {
        Failed,
        Successful,
        Awaiting
    }

    public enum ReconStatus
    {
        Closed,
        Open
    }

    public enum EntryType
    {
        Optional,
        Required
    }

    public enum AcceptDataFrom
    { 
        Source,
        Application
    }

    public enum SelectColumnsType
    {
        Numeric,
        Text,
        Both
    }

    public enum ImpactOnCash
    {
        Positive,
        Negative,
        None
    }

    public enum CloseTradeMethodology
    {
        Manual,
        Automatic
    }

    public enum CloseTradeAlogrithm
    {
        None,
        LIFO,
        FIFO,
        HIFO
    }

    public enum PositionType
    {
        Long,
        Short
    }

    public enum EntityStateEnum
    {
        Unchanged,
        Added,
        Deleted,
        Modified,
    }

}
