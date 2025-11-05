// ***********************************************************************
// Assembly         : Prana.Allocation.Common
// Author           : dewashish
// Created          : 07-26-2014
//
// Last Modified By : dewashish
// Last Modified On : 09-09-2014
// ***********************************************************************
// <copyright file="AllocationOutputResult.cs" company="Nirvana">
//     Copyright (c) Nirvana. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Prana.BusinessObjects;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// The Definitions namespace.
/// </summary>
namespace Prana.Allocation.Common.Definitions
{
    /// <summary>
    /// Class definition for AllocationOutputResult.
    /// It contains the dictionary of AllocationOutput for given list of AllocationGroup
    /// </summary>
    [Serializable]
    public class AllocationOutputResult
    {
        /// <summary>
        /// Private object which will either contains null or the raised exception while allocating the given group list
        /// </summary>
        private String _error = string.Empty;

        /// <summary>
        /// Property which will be either null (No exception generated) or will contain the raised exception
        /// </summary>
        /// <value>The error.</value>
        public String Error { get { return _error; } set { _error = value; } }

        /// <summary>
        /// Private locker object for given AllocationOutputCollection
        /// </summary>
        private readonly object _lockerObject = new object();

        /// <summary>
        /// AllocationOutputCollection cache allocationGroup wise.
        /// </summary>
        private List<AllocationOutput> _outputCollection = new List<AllocationOutput>();

        /// <summary>
        /// This read-only property returns the collection of the output
        /// </summary>
        /// <value>The output collection.</value>
        public IReadOnlyList<AllocationOutput> OutputCollection { get { return _outputCollection.AsReadOnly(); } }

        /// <summary>
        /// The allocation failed symbols
        /// </summary>
        private List<string> _allocationFailedSymbols = new List<string>();

        /// <summary>
        /// Gets or sets the allocation failed symbols.
        /// </summary>
        /// <value>
        /// The allocation failed symbols.
        /// </value>
        public List<string> AllocationFailedSymbols { get { return _allocationFailedSymbols; } }

        /// <summary>
        /// Returns the AllocationOutout of the given groupId
        /// </summary>
        /// <param name="groupId">Id of the group for which this allocation output belongs</param>
        /// <returns>Allocation output required</returns>
        public AllocationOutput GetAllocationOutput(string groupId)
        {
            try
            {
                return _outputCollection.Find(i => i.GroupId == groupId);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return null;
            }
        }

        /// <summary>
        /// Adds the given output to the collection
        /// </summary>
        /// <param name="output">AllocationOutput object which will be added to OutputCollection</param>
        /// <exception cref="System.Exception">GroupId already exists</exception>
        public void Add(AllocationOutput output)
        {
            try
            {
                lock (_lockerObject)
                {
                    AllocationOutput outP = _outputCollection.Find(i => i.GroupId == output.GroupId);
                    if (outP == null)
                        _outputCollection.Add(output);
                    else
                        throw new Exception("GroupId already exists");
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Adds the given dictionary to AllocationOutputCollection
        /// </summary>
        /// <param name="outputDictionary">Dictionary to be added</param>
        public void Add(SerializableDictionary<string, AllocationOutput> outputDictionary)
        {
            try
            {
                foreach (string key in outputDictionary.Keys)
                {
                    Add(outputDictionary[key]);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Adds a given allocationOutputResult to existing result
        /// </summary>
        /// <param name="result">Result which is to be merged</param>
        public void Add(AllocationOutputResult result)
        {
            try
            {
                if (result.Error != null)
                    this._error = result.Error;

                foreach (AllocationOutput output in result._outputCollection)
                {
                    Add(output);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Appends the error.
        /// </summary>
        /// <param name="p">The p.</param>
        public void AppendError(string errorMsg)
        {
            try
            {
                lock (_lockerObject)
                {
                    if (!this.Error.Contains(errorMsg))
                        this.Error += errorMsg.Trim() + "\n";
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Adds the allocation failed symbol.
        /// </summary>
        /// <param name="symbols">The symbol list.</param>
        public void AddAllocationFailedSymbols(List<string> symbols)
        {
            try
            {
                lock (_lockerObject)
                {
                    List<string> symbolList = symbols.Except(_allocationFailedSymbols).ToList();
                    _allocationFailedSymbols.AddRange(symbolList);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
    }
}
