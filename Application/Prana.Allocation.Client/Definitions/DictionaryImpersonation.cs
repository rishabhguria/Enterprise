// ***********************************************************************
// Assembly         : Prana.Allocation.Client
// Author           : Disha Sharma
// Created          : 06-14-2016
// ***********************************************************************
// <copyright file="DictionaryImpersonation.cs" company="Nirvana">
//     Copyright (c) Nirvana. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Prana.BusinessObjects;
using System.ComponentModel;

namespace Prana.Allocation.Client.Definitions
{
    /// <summary>
    /// This is generic class to impersonate dictionary for binding dictionary like collections to WPF controls
    /// In Dictionary, Key and Value are readonly properties so if we bind it to control we cannot edit their value
    /// So, to enable editing and having Key,Value structure I have created  this class
    /// </summary>
    /// <typeparam name="T">Object Type for Key</typeparam>
    /// <typeparam name="U">Object Type for Value</typeparam>
    /// <seealso cref="Prana.BusinessObjects.INotifyPropertyChangedCustom" />
    /// <seealso cref="Prana.BusinessObjects.IKeyable" />
    class DictionaryImpersonation<T, U> : INotifyPropertyChangedCustom, IKeyable
    {
        #region Members

        /// <summary>
        /// The _key
        /// </summary>
        private T _key;

        /// <summary>
        /// The _value
        /// </summary>
        private U _value;

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public T Key
        {
            get { return _key; }
            set
            {
                _key = value;
                PropertyHasChanged();
            }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public U Value
        {
            get { return _value; }
            set
            {
                _value = value;
                PropertyHasChanged();
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryImpersonation{T, U}"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public DictionaryImpersonation(T key, U value)
        {
            Key = key;
            Value = value;
        }

        #endregion Constructors

        #region INotifyPropertyChangedCustom Members

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Properties the has changed.
        /// </summary>
        public void PropertyHasChanged()
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(string.Empty));
            }
        }

        #endregion INotifyPropertyChangedCustom Members

        #region IKeyable Members

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <returns></returns>
        public string GetKey()
        {
            return _key.ToString();
        }

        /// <summary>
        /// Updates the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Update(IKeyable item)
        {
            DictionaryImpersonation<T, U> newItem = (DictionaryImpersonation<T, U>)item;
            _key = newItem.Key;
            _value = newItem.Value;
        }

        #endregion IKeyable Members
    }
}
