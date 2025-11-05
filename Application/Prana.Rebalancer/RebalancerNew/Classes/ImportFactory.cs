using Microsoft.Win32;
using Prana.BusinessObjects.Enumerators.RebalancerNew;
using Prana.Rebalancer.RebalancerNew.BussinessLogic.Interfaces;
using Prana.Utilities.ImportExportUtilities;
using System.Data;

namespace Prana.Rebalancer.RebalancerNew.Classes
{
    public class ImportFactory : IImportFactory
    {
        public IImport CreateObject(RebalancerEnums.ImportType type, ref string error)
        {
            DataTable dt = GetDataTable(ref error);
            IImport instance = null;
            switch (type)
            {
                case RebalancerEnums.ImportType.CustomGroupsImport:
                    instance = new ImportCustomGroup(dt);
                    break;
                case RebalancerEnums.ImportType.CashFlowImport:
                    instance = new ImportCashFlow(dt);
                    break;
                default:
                    instance = null;
                    break;

            }
            return instance;
        }

        private DataTable GetDataTable(ref string error)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "Desktop";
            openFileDialog.Title = "Select File to Import";
            openFileDialog.Filter = "Excel Files (*.xls)|*.xls|Excel Files (*.xlsx)|*.xlsx|CSV Files (*.csv)|*.csv";
            DataTable dt = new DataTable();
            if (openFileDialog.ShowDialog() == true)
            {
                dt = FileReaderFactory.GetDataTableFromDifferentFileFormatsNew(openFileDialog.FileName);
            }
            else
            {
                error = "User canceled the import operation.";
            }
            return dt;
        }
    }
}
