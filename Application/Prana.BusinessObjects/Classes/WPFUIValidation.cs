using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace Prana.BusinessObjects
{
    [ComVisible(false)]
    public class ValidateEnumerationValue : ValidationAttribute
    {
        private Type _typeOfEnum;
        public Type TypeOfEnum
        {
            get { return _typeOfEnum; }
        }
        public ValidateEnumerationValue(Type typeOfEnum, string errorMessage)
            : base(errorMessage)
        {
            _typeOfEnum = typeOfEnum;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            EnumerationValue enumValue = (EnumerationValue)value;
            bool isValid;
            try
            {
                if (enumValue != null)
                {
                    Enum.Parse(TypeOfEnum, enumValue.Value.ToString());
                    isValid = true;
                }
                else
                {
                    isValid = false;
                }
            }
            catch (ArgumentException)
            {
                isValid = false;
            }
            return isValid ? ValidationResult.Success : new ValidationResult(this.ErrorMessage);
        }
    }

    [ComVisible(false)]
    public class EnsureMinimumElementsAttribute : ValidationAttribute
    {
        private readonly int _minElements;
        public EnsureMinimumElementsAttribute(int minElements)
        {
            _minElements = minElements;
        }

        public override bool IsValid(object value)
        {
            var list = value as IList;
            if (list != null)
            {
                return list.Count >= _minElements;
            }
            return false;
        }
    }
    [ComVisible(false)]
    public class ValidateErrorMessage : ValidationAttribute
    {
        private string _errorDesription;
        public string ErrorDesription
        {
            get { return _errorDesription; }
            set { _errorDesription = value; }
        }

        public ValidateErrorMessage(string errorDescription)
        {
            _errorDesription = errorDescription;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty(_errorDesription);
            if (property == null)
            {
                return new ValidationResult(
                    string.Format("Unknown property: {0}", _errorDesription)
                );
            }
            var otherValue = property.GetValue(validationContext.ObjectInstance, null);
            if (otherValue != null)
            {
                string errorMessages = otherValue.ToString();
                if (!String.IsNullOrEmpty(errorMessages))
                {
                    return new ValidationResult(errorMessages);
                }
            }
            return ValidationResult.Success;
        }
    }

    [ComVisible(false)]
    public class ValidateGreaterThanZero : ValidationAttribute
    {
        private string _displayName;
        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }

        public ValidateGreaterThanZero(string displayName)
        {
            _displayName = displayName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            bool isValid = !(Decimal.Parse(value.ToString()) <= decimal.Zero);

            if (isValid)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult(string.Format("The value is too small for field {0}", _displayName));
        }
    }

    [ComVisible(false)]
    public class EnsureServiceConnected : ValidationAttribute
    {
        string _errormessage = string.Empty;
        public EnsureServiceConnected(string error)
        {
            _errormessage = error;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            bool result = bool.Parse(value.ToString());
            if (result)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult(string.Format(_errormessage));
        }
    }

}
