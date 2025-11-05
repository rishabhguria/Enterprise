namespace Prana.PM.BLL
{
    //public abstract class FileParsingStrategy
    //{
    //    public abstract int ParseFileAndStoreData(RunUpload runUploadItem);

    //    public abstract int ParseFileAndStoreData(RunUpload currentFtpProgress, DataTable datasourceData);

    //}

    //[ParsingAttribute(DataSourceFileLayout.HeaderBlankData)]
    //public class HeaderBlankDataStrategy : FileParsingStrategy
    //{

    //    public override int ParseFileAndStoreData(RunUpload runUploadItem)
    //    {
    //        return Convert.ToInt16(DataSourceFileLayout.HeaderBlankData);
    //    }

    //    public override int ParseFileAndStoreData(RunUpload runUploadItem, DataTable datasourceData)
    //    {
    //        return Convert.ToInt16(DataSourceFileLayout.HeaderBlankData);
    //    }
    //}

    //[ParsingAttribute(DataSourceFileLayout.HeaderData)]
    //public class HeaderDataStrategy : FileParsingStrategy
    //{
    //    public override int ParseFileAndStoreData(RunUpload runUploadItem)
    //    {
    //        return Convert.ToInt16(DataSourceFileLayout.HeaderData);
    //    }

    //    public override int ParseFileAndStoreData(RunUpload runUploadItem, DataTable datasourceData)
    //    {
    //        return Convert.ToInt16(DataSourceFileLayout.HeaderData);
    //    }
    //}

    //[ParsingAttribute(DataSourceFileLayout.NoHeaderData)]
    //public class NoHeaderDataStrategy : FileParsingStrategy
    //{
    //    public override int ParseFileAndStoreData(RunUpload runUploadItem)
    //    {
    //        return Convert.ToInt16(DataSourceFileLayout.NoHeaderData);
    //    }

    //    public override int ParseFileAndStoreData(RunUpload runUploadItem, DataTable datasourceData)
    //    {
    //        return Convert.ToInt16(DataSourceFileLayout.NoHeaderData);
    //    }
    //}

    //[ParsingAttribute(DataSourceFileLayout.SummaryHeaderData)]
    //public class SummaryHeaderDataStrategy : FileParsingStrategy
    //{
    //    /// <summary>
    //    /// Parses the file and store data.
    //    /// </summary>
    //    /// <param name="runUploadItem">The run upload item.</param>
    //    /// <returns></returns>
    //    public override int ParseFileAndStoreData(RunUpload runUploadItem)
    //    {
    //        return Convert.ToInt16(DataSourceFileLayout.SummaryHeaderData);
    //    }

    //    /// <summary>
    //    /// Parses the file and store data.
    //    /// </summary>
    //    /// <param name="runUploadItem">The run upload item.</param>
    //    /// <returns></returns>
    //    public override int ParseFileAndStoreData(RunUpload runUploadItem, DataTable datasourceData)
    //    {
    //        return Convert.ToInt16(DataSourceFileLayout.SummaryHeaderData);
    //    }
    //}

    //public class ParsingAlgorithmFactory
    //{
    //    private static ArrayList _registeredImplementations;

    //    /// <summary>
    //    /// Initializes the <see cref="ParsingAlgorithmFactory"/> class.
    //    /// </summary>
    //    static ParsingAlgorithmFactory()
    //    {
    //        _registeredImplementations = new ArrayList();
    //        RegisterClass(typeof(HeaderBlankDataStrategy));
    //        RegisterClass(typeof(HeaderDataStrategy));
    //        RegisterClass(typeof(NoHeaderDataStrategy));
    //        RegisterClass(typeof(SummaryHeaderDataStrategy));
    //    }

    //    /// <summary>
    //    /// Registers the class.
    //    /// </summary>
    //    /// <param name="requestStrategyImpl">The request strategy impl.</param>
    //    public static void RegisterClass(Type requestStrategyImpl)
    //    {
    //        if (!requestStrategyImpl.IsSubclassOf(typeof(FileParsingStrategy)))
    //            throw new Exception("ArithmiticStrategy must inherit from " +
    //                                              "class AbstractArithmiticStrategy");

    //        _registeredImplementations.Add(requestStrategyImpl);
    //    }

    //    /// <summary>
    //    /// Creates the specified layout type.
    //    /// </summary>
    //    /// <param name="layoutType">Type of the layout.</param>
    //    /// <returns></returns>
    //    public static FileParsingStrategy Create(DataSourceFileLayout layoutType)
    //    {
    //        // loop thru all registered implementations
    //        foreach (Type impl in _registeredImplementations)
    //        {
    //            // get attributes for this type
    //            object[] attrlist = impl.GetCustomAttributes(true);

    //            // loop thru all attributes for this class
    //            foreach (object attr in attrlist)
    //            {
    //                if (attr is ParsingAttribute)
    //                {
    //                    if (((ParsingAttribute)attr).DataSourceFileLayout.Equals(layoutType))
    //                    {
    //                        return
    //                         (FileParsingStrategy)System.Activator.CreateInstance(impl);
    //                    }
    //                }
    //            }
    //        }
    //        throw new Exception("Could not find a FileParsingStrategy " +
    //                                   "implementation for this layoutType");
    //    }
    //}
}
