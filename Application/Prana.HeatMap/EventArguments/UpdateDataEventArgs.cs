using System.Data;

namespace Prana.HeatMap.EventArguments
{
    /// <summary>
    /// Event Arguments for when data is received from Esper
    /// </summary>
    public class UpdateDataEventArgs
    {
        public DataTable Data { get; set; }
    }
}
