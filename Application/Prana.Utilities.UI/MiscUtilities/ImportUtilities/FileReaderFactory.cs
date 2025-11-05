using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Prana.Utilities.ImportExportUtilities
{
    public class FileReaderFactory
    {
        private static ArrayList _registeredImplementations;

        static FileReaderFactory()
        {
            _registeredImplementations = new ArrayList();
            RegisterClass(typeof(ExcelReadingStrategy));          
            RegisterClass(typeof(TextReadingStrategy));
            RegisterClass(typeof(DefaultReadingStrategy)); 
        }

        /// <summary>
        /// Registers the class.
        /// </summary>
        /// <param name="requestStrategyImpl">The request strategy impl.</param>
        public static void RegisterClass(Type requestStrategyImpl)
        {
            if (!requestStrategyImpl.IsSubclassOf(typeof(FileFormatStrategy)))
                throw new Exception("ArithmiticStrategy must inherit from " +
                                                  "class AbstractArithmiticStrategy");

            _registeredImplementations.Add(requestStrategyImpl);
        }

        public static FileFormatStrategy Create(DataSourceFileFormat formatType)
        {
            // loop thru all registered implementations
            foreach (Type impl in _registeredImplementations)
            {
                // get attributes for this type
                object[] attrlist = impl.GetCustomAttributes(true);

                // loop thru all attributes for this class
                foreach (object attr in attrlist)
                {
                    if (attr is FormattingAttribute)
                    {
                        if (((FormattingAttribute)attr).DataSourceFileFormat.Equals(formatType))
                        {
                            return
                             (FileFormatStrategy)System.Activator.CreateInstance(impl);
                        }
                    }
                }
            }
            throw new Exception("Could not find a FileFormatStrategy " +
                                       "implementation for this fileFormatType");
        }
    }
}
