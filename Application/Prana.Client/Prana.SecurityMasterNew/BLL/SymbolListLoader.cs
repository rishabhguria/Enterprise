namespace Prana.SecurityMasterNew
{
    class SymbolListLoader
    {
        //public static List<string> GetLiveFeedSymbols()
        //{
        //    List<string> symbolList = new List<string>();
        //    string startPath = System.Windows.Forms.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        //    string liveFeedPreferencesPath = startPath + @"\Prana Preferences\LiveFeedPreferences\WatchListPreferences.xml";

        //    XmlDocument xmldoc = new XmlDocument();
        //    if (!System.IO.File.Exists(liveFeedPreferencesPath))
        //    {
        //        return symbolList;
        //    }
        //    xmldoc.Load(liveFeedPreferencesPath);    
        //    if (xmldoc != null)
        //    {
        //        XmlNodeList xmlNodesSymbolList = xmldoc.SelectNodes("WatchListPreferences/ColumnPreferences/WatchList/SymbolList");
        //        int count = 0;
        //        foreach (XmlNode xmlNodesSymbols in xmlNodesSymbolList)
        //        {
        //            string symbolsCommaSeperated = xmlNodesSymbols.InnerText;

        //            List<string> symbolsListToAdd = GeneralUtilities.GetListFromString(symbolsCommaSeperated, ',');
        //            if (count == 0)
        //            {
        //                symbolList = symbolsListToAdd;
        //            }
        //            else
        //            {
        //                foreach (string symbol in symbolsListToAdd)
        //                {
        //                    if (!symbolList.Contains(symbol))
        //                    {
        //                        symbolList.Add(symbol);
        //                    }
        //                }
        //            }
        //            count++;

        //        }
        //    }
        //    return symbolList;
        //}
    }
}
