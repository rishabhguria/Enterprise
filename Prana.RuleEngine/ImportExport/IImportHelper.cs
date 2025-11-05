using Prana.RuleEngine.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Prana.RuleEngine.ImportExport
{

    public delegate void ImportExportHandler(DataSet ds,RuleCategory ruleCategory);
    

    interface IImportHelper:IDisposable
    {
        event ImportExportHandler ImportExportActionReceived;

        void ExportRule(Dictionary<String, String> requestDict);

        void ImportRule(Dictionary<String, String> requestDict);
        void IntialiseAmqpPlugins();
    }
}
