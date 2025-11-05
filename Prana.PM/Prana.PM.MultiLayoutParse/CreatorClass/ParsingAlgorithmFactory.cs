using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data;
using Prana.PM.BLL;
using Prana.BusinessObjects.AppConstants;

namespace Prana.PM.MultiLayoutParse
{
    
    public class ParsingAlgorithmFactory
    {
        private static ArrayList _registeredImplementations;

        /// <summary>
        /// Initializes the <see cref="ParsingAlgorithmFactory"/> class.
        /// </summary>
        static ParsingAlgorithmFactory()
        {
            _registeredImplementations = new ArrayList();
            RegisterClass(typeof(HeaderBlankDataStrategy));
            RegisterClass(typeof(HeaderDataStrategy));
            RegisterClass(typeof(NoHeaderDataStrategy));
            RegisterClass(typeof(SummaryHeaderDataStrategy));
        }

        /// <summary>
        /// Registers the class.
        /// </summary>
        /// <param name="requestStrategyImpl">The request strategy impl.</param>
        public static void RegisterClass(Type requestStrategyImpl)
        {
            if (!requestStrategyImpl.IsSubclassOf(typeof(FileParsingStrategy)))
                throw new Exception(requestStrategyImpl.ToString() + " must inherit from " +
                                                  "class " + typeof(FileParsingStrategy).ToString());

            _registeredImplementations.Add(requestStrategyImpl);
        }

        /// <summary>
        /// Creates the specified layout type.
        /// </summary>
        /// <param name="layoutType">Type of the layout.</param>
        /// <returns></returns>
        public static FileParsingStrategy Create(DataSourceFileLayout layoutType)
        {
            // loop thru all registered implementations
            foreach (Type impl in _registeredImplementations)
            {
                // get attributes for this type                            
                object[] attrlist = impl.GetCustomAttributes(true);

                // loop thru all attributes for this class
                foreach (object attr in attrlist)
                {
                    if (attr is ParsingAttribute)
                    {
                        if (((ParsingAttribute)attr).DataSourceFileLayout.Equals(layoutType))
                        {
                            return
                             (FileParsingStrategy)System.Activator.CreateInstance(impl);
                        }
                    }
                }
            }
            throw new Exception("Could not find a FileParsingStrategy " +
                                       "implementation for this layoutType");
        }
    }
}
