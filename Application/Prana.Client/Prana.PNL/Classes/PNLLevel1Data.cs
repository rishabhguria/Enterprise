using System;

namespace Nirvana.PNL
{
	/// <summary>
	/// Summary description for RowColumns.
	/// </summary>
	public class PNLLevel1Data: PNLData
	{
		PNLLevel2DataCollection _pnlLevel2DataCollection = new PNLLevel2DataCollection();

		public PNLLevel1Data()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public PNLLevel2DataCollection Level2DataCollection
		{
			get
			{
				return this._pnlLevel2DataCollection;
			}

			set
			{
               this._pnlLevel2DataCollection = value;
			}
		}
	}
}
