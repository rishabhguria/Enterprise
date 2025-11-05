// ***********************************************************************
// <copyright file="ValidationHelper.cs" company="Nirvana">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Prana.ServiceGateway.Contracts;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Prana.ServiceGateway.ExceptionHandling;
using Prana.ServiceGateway.Constants;

namespace Prana.ServiceGateway.Utility
{
    /// <summary>
    /// Class to help with validation
    /// </summary>
    /// <seealso cref="IValidationHelper" />
    public class ValidationHelper : IValidationHelper
    {
        private readonly ILogger<ValidationHelper> _logger;

        public ValidationHelper(ILogger<ValidationHelper> logger)
        {
            this._logger = logger;
        }
        /// <inheritdoc />
        public string ValidateDataAnnotations(object entityToValidate)
        {
            try
            {
                StringBuilder errors = new StringBuilder();
                ValidationContext context = new ValidationContext(entityToValidate, null, null);
                List<ValidationResult> results = new List<ValidationResult>();
                bool valid = Validator.TryValidateObject(entityToValidate, context, results, true);
                if (!valid)
                {
                    foreach (ValidationResult vr in results)
                    {
                        errors.AppendLine(vr.ErrorMessage);
                    }
                }

                return errors.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ValidateDataAnnotations of ValidationHelper");
                throw;
            }
        }
    }
}