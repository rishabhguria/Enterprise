using System;

namespace  Nirvana.Utilities
{
	/// <summary>
	/// Summary description for EventHelper.
	/// </summary>
	public class EventHelper
	{
		public EventHelper()
		{
		}

		delegate void AsyncInvokeDelegate(Delegate del, params object[] args);

		public static void FireAsync(Delegate del, params object[] args)
		{
			if (del == null)
			{ 
				return; 
			}

			Delegate[] delegates = del.GetInvocationList();
			AsyncInvokeDelegate invoker = new AsyncInvokeDelegate(InvokeDelegate);

			foreach (Delegate sink in delegates)
			{
				if(sink.Target != null)
				{
					invoker.BeginInvoke(sink,args,null,null);
					System.Threading.Thread.Sleep(50);
				}
			}

		}

//		public static void FireAsyncSymbol(Delegate del, string symbol, params object[] args)
//		public static void FireAsyncSymbol(Delegate del, params object[] args)
//		{
//
//			if (del == null)
//			{ 
//				return; 
//			}
//
//			Delegate[] delegates = del.GetInvocationList();
//    
//
//			AsyncInvokeDelegate invoker = new AsyncInvokeDelegate(InvokeDelegate);
//
//			foreach (Delegate sink in delegates)
//			{
////				if(symbol.Equals(((Nirvana.WatchList.Quotes)(((System.Object)(sink.Target)))).Symbol))
//				invoker.BeginInvoke(sink,args,null,null);
//			}
//
//		}

		//
		//		public static void Fire(Delegate del,params object[] args)
		//
		//		{
		//
		//			if (del == null)
		//
		//			{ 
		//
		//				return; 
		//
		//			}
		//
		//			Delegate[] delegates = del.GetInvocationList();
		//
		//			foreach (Delegate sink in delegates)
		//
		//			{
		//
		//				InvokeDelegate(sink,args);
		//
		//			}
		//
		//		}



		private static void InvokeDelegate(Delegate sink, params object[] args)
		{
			try
			{
				sink.DynamicInvoke(args);
			}
			catch(Exception ex)
			{
				string s = ex.Message + ex.StackTrace;
			}
		}

	}
}
