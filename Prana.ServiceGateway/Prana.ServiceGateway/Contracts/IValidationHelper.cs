// ***********************************************************************
// <copyright file="IValidationHelper.cs" company="Nirvana">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Prana.ServiceGateway.Contracts
{
    /// <summary>
    /// Interface for ValidationHelper class
    /// </summary>
    public interface IValidationHelper
    {
        /// <summary>
        /// Validates the data annotations.
        /// </summary>
        /// <param name="entityToValidate">The entity to validate.</param>
        /// <returns>System.String.</returns>
        string ValidateDataAnnotations(object entityToValidate);
    }
}