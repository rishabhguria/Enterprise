//using System;
//using System.Collections.Generic;
//using System.Text;
//using AdapterClient.util;
//using Prana.Global;
//using Prana.Logging;

//namespace Prana.FixAdapters
//{
//    public class FixConnLogger:ILogger 
//    {
//        int logLevel;
//        private FixEngineConnection _underlyinGConn = null;
//        public FixConnLogger(FixEngineConnection conn)
//        {
//            _underlyinGConn = conn;
//        }

//        public bool DebugEnabled { get { return true; } }
//        public int LogLevel { get { return logLevel; }
//            set{logLevel=value;} }

//        public void debug(string p)
//        {
//            //_underlyinGConn.OnLogError(p);
//        }
//        public void error(string p)
//        {
//            //_underlyinGConn.OnLogError(p);
//        }
//        public void error(string p, Exception ex)
//        {
//            //_underlyinGConn.OnLogError(p);
//        }

//        public void info(string p)
//        {
//            //_underlyinGConn.OnLogError(p);
//        }
//        public void warn(string p, Exception f)
//        {
//           // _underlyinGConn.OnLogError(p);
//        }

//    }
//}
