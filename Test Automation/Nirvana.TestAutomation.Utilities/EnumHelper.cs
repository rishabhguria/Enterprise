using Nirvana.TestAutomation.BussinessObjects.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace Nirvana.TestAutomation.Utilities
{
    public static class EnumHelper
    {
        /// <summary>
        /// Gets the <see cref=�DescriptionAttribute� /> of an <see cref=�Enum� /> type value.
        /// </summary>
        /// <param name=�value�>
        public static string GetDescription(Enum value)
        {
            string description = value.ToString();
            try
            {
                FieldInfo fieldInfo = value.GetType().GetField(description);
                EnumDescriptionAttribute[] attributes = (EnumDescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(EnumDescriptionAttribute), false);
                if (attributes != null && attributes.Length > 0)
                {
                    description = attributes[0].Description;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return description;
        }
    }
}
