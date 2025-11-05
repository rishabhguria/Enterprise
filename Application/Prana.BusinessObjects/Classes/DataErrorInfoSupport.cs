using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;

namespace Prana.BusinessObjects
{
    public sealed class DataErrorInfoSupport : IDataErrorInfo
    {
        private readonly object instance;

        public DataErrorInfoSupport(object instance)
        {
            this.instance = instance;
        }

        public string Error { get { return this[""]; } }

        public string this[string memberName]
        {
            get
            {
                List<ValidationResult> validationResults = new List<ValidationResult>();

                if (string.IsNullOrEmpty(memberName))
                {
                    Validator.TryValidateObject(instance, new ValidationContext(instance, null, null), validationResults, true);
                }
                else
                {
                    PropertyDescriptor property = TypeDescriptor.GetProperties(instance)[memberName];
                    if (property == null)
                    {
                        throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                        "The specified member {0} was not found on the instance {1}", memberName, instance.GetType()));
                    }
                    Validator.TryValidateProperty(property.GetValue(instance),
                    new ValidationContext(instance, null, null) { MemberName = memberName }, validationResults);
                }

                StringBuilder errorBuilder = new StringBuilder();
                foreach (ValidationResult validationResult in validationResults)
                {
                    errorBuilder.AppendLine(validationResult.ErrorMessage);
                }

                return errorBuilder.ToString();
            }
        }
    }
}