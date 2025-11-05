//using System;
//using Nirvana.Interfaces;
//using System.Collections;
//using Nirvana.LiveFeed;
//
//
//namespace Nirvana.PNL
//{
//	/// <summary>
//	/// Summary description for LiveFeedImpl.
//	/// </summary>
//	public class LiveFeedImpl:Nirvana.Interfaces.ILiveFeedManager
//	{
//		public LiveFeedImpl()
//		{
//		}
//		public Hashtable RequestSymbolData(ArrayList symbol)
//		{
//			Hashtable ht = new Hashtable();
//			Level1Data level1Data;
//					
//			level1Data = new Level1Data();
//				
//			level1Data.Last = 36.34;
//			level1Data.Ask = 37.12;
//			level1Data.Bid = 33.54;
//
//			ht.Add("T", level1Data);
//
//			level1Data = new Level1Data();
//				
//			level1Data.Last = 36.34;
//			level1Data.Ask = 37.12;
//			level1Data.Bid = 33.54;
//
//			ht.Add("HELL", level1Data);
//
//			level1Data = new Level1Data();
//				
//			level1Data.Last = 36.34;
//			level1Data.Ask = 37.12;
//			level1Data.Bid = 33.54;
//
//			ht.Add("DELL", level1Data);
//
//			level1Data = new Level1Data();
//				
//			level1Data.Last = 36.34;
//			level1Data.Ask = 37.12;
//			level1Data.Bid = 33.54;
//
//			ht.Add("Tell", level1Data);
//
//			level1Data = new Level1Data();
//				
//			level1Data.Last = 36.34;
//			level1Data.Ask = 37.12;
//			level1Data.Bid = 33.54;
//
//			ht.Add("bugs", level1Data);
//
//			level1Data.Last = 36.34;
//			level1Data.Ask = 37.12;
//			level1Data.Bid = 33.54;
//
//			ht.Add("Stillmorebugs", level1Data);
//
//			level1Data.Last = 36.34;
//			level1Data.Ask = 37.12;
//			level1Data.Bid = 33.54;
//
//			ht.Add("lessbugs", level1Data);
//
//			level1Data.Last = 36.34;
//			level1Data.Ask = 37.12;
//			level1Data.Bid = 33.54;
//
//			ht.Add("Te", level1Data);
//
//			level1Data.Last = 36.34;
//			level1Data.Ask = 37.12;
//			level1Data.Bid = 33.54;
//
//			ht.Add("rytry", level1Data);
//
//			level1Data.Last = 36.34;
//			level1Data.Ask = 37.12;
//			level1Data.Bid = 33.54;
//
//			ht.Add("tohell", level1Data);
//
//			level1Data.Last = 36.34;
//			level1Data.Ask = 37.12;
//			level1Data.Bid = 33.54;
//
//			ht.Add("new", level1Data);
//
//			level1Data.Last = 36.34;
//			level1Data.Ask = 37.12;
//			level1Data.Bid = 33.54;
//
//			ht.Add("SingleBand", level1Data);
//
//			level1Data.Last = 36.34;
//			level1Data.Ask = 37.12;
//			level1Data.Bid = 33.54;
//
//			ht.Add("e", level1Data);
//
//			level1Data.Last = 36.34;
//			level1Data.Ask = 37.12;
//			level1Data.Bid = 33.54;
//
//			ht.Add("asdf", level1Data);
//
//			level1Data.Last = 36.34;
//			level1Data.Ask = 37.12;
//			level1Data.Bid = 33.54;
//
//			ht.Add("IBM", level1Data);
//
//			level1Data.Last = 36.34;
//			level1Data.Ask = 37.12;
//			level1Data.Bid = 33.54;
//
//			ht.Add("erer", level1Data);
//
//
//			return ht;
//		}
//		
//		#region ILiveFeedManager Members
//		
//		public event System.EventHandler DataManagerDisconnected;
//		
//		public bool IsDataManagerConnected()
//		{
//			// TODO:  Add LiveFeedImpl.IsDataManagerConnected implementation
//			return false;
//		}
//		
//		public event System.EventHandler DataManagerConnected;
//		
//		public event System.EventHandler Level1DataResponse;
//		
//		public void RequestSymbolList(ArrayList symbolList)
//		{
//			// TODO:  Add LiveFeedImpl.RequestSymbolList implementation
//		}
//
//		#endregion
//	}
//}
