using System;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
	/// <summary>
	/// Summary description for Currency.
	/// </summary>
	public class Currency
	{

		#region private members

		private int _id = Int32.MinValue;
		private string _Name = string.Empty;
		
		#endregion

		public Currency()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		#region

        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        /// <value>The ID.</value>
		public int ID
		{
			get
			{
				return this._id;
			}

			set
			{
				this._id = value;
			}
		}

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
		public string Name
		{
			get
			{
				return this._Name;
			}

			set
			{
				this._Name = value;
			}
		}

        public override string ToString()
        {
            return Name;
        }

		#endregion
	}
}
