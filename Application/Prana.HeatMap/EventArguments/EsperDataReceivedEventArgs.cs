using System.Data;

namespace Prana.HeatMap.EventArguments
{
    /// <summary>
    /// Event Arguments for when data is received from Esper
    /// </summary>
    public class EsperDataReceivedEventArgs
    {
        public DataSet Data { get; set; }
    }
}
