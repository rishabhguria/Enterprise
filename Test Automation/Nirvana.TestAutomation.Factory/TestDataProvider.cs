using Nirvana.TestAutomation.TestDataProvider;
using Nirvana.TestAutomation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nirvana.TestAutomation.Factory
{
    public class TestDataProvider
    {
        public static ITestDataProvider GetProvider(ProviderType type)
        {
            switch (type)
            {
                case ProviderType.Excel:
                    return new ExcelDataProvider();
                case ProviderType.GoogleSheets:
                    return new GoogleSheetsDataProvider();
                case ProviderType.OpenXml:
                    return new OpenXmlDataProvider();
                case ProviderType.Xls:
                    return new XlsDataProvider();

            }
            return null;
        }
    }
}