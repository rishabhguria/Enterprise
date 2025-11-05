using System;
using System.Collections.Generic;
using System.Text;
using Nirvana.Admin.PositionManagement.Classes;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
    class ImportMethodList:SortableSearchableList<ImportMethod>
    {
        /// <summary>
        /// Retrieves ImportMethods.
        /// </summary>
        /// <value>The retrieve.</value>
        public static SortableSearchableList<ImportMethod> Retrieve
        {
            get { return GetAllImportMethods(); }
        }

        /// <summary>
        /// Gets all import methods.
        /// </summary>
        /// <returns></returns>
        private static SortableSearchableList<ImportMethod> GetAllImportMethods()
        {
            //To Do: Add Method in DataSourceManager to fetch Import Methods!
            SortableSearchableList<ImportMethod> importMethodList = new SortableSearchableList<ImportMethod>();//DataSourceManager.GetAllDataSourceNames();
            
            importMethodList.Insert(0, new ImportMethod(0, Constants.C_COMBO_SELECT));


            //manual insertion of data -- later will be  moved to the DB
            importMethodList.Insert(1, new ImportMethod(1, "ftp"));
            importMethodList.Insert(2, new ImportMethod(2, "fix"));           
            return importMethodList;
        }
    }
}
