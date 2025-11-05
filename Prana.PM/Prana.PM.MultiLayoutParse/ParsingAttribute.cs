using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Prana.PM.BLL;
using Prana.BusinessObjects.AppConstants;
namespace Prana.PM.MultiLayoutParse
{

    /// <summary>
    /// Enum keeping all the type of file layouts which we can get. 
    /// </summary>
   


    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ParsingAttribute : Attribute
    {
        private DataSourceFileLayout _dataSourceFileLayout;

        public ParsingAttribute(DataSourceFileLayout dataSourceFileLayout)
        {
            this._dataSourceFileLayout = dataSourceFileLayout;
        }

        public DataSourceFileLayout DataSourceFileLayout
        {
            get { return _dataSourceFileLayout; }
        }
    }

}
