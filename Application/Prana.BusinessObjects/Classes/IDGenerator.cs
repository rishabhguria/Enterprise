using System;
using System.Text;

namespace Prana.BusinessObjects
{
    public class IDGenerator
    {
        private static Int64 _seed = Int64.Parse(DateTime.Now.ToString("yyyymmddHHmmssfff"));
        private static readonly object lockSeed = new object();
        public static string GenerateClientBasketID()
        {
            lock (lockSeed)
            {
                _seed++;
                return _seed.ToString() + "CB";
            }
        }
        public static string GenerateClientWaveID()
        {
            lock (lockSeed)
            {
                _seed++;
                return _seed.ToString() + "CW";
            }
        }
        public static string GenerateClientGroupID()
        {
            lock (lockSeed)
            {
                _seed++;
                return _seed.ToString() + "CG";
            }
        }
        public static string GenerateSavedBasketID()
        {
            lock (lockSeed)
            {
                _seed++;
                return _seed.ToString() + "CSB";
            }
        }
        public static string GenerateClientOrderID()
        {
            lock (lockSeed)
            {
                _seed++;
                return _seed.ToString() + "CO";

            }
        }
        public static string GenerateFilterID()
        {
            lock (lockSeed)
            {
                _seed++;
                return _seed.ToString() + "CF";
            }
        }
        public static string GetAUCVID(int assetID, int underlyingID, int counterPartyID, int venueID)
        {
            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append('A');
            sBuilder.Append(assetID.ToString());
            sBuilder.Append('U');
            sBuilder.Append(underlyingID.ToString());
            sBuilder.Append('C');
            sBuilder.Append(counterPartyID.ToString());
            sBuilder.Append('V');
            sBuilder.Append(venueID.ToString());
            return sBuilder.ToString();
        }
        public static string GetAUUID(int assetID, int underlyingID, int userID)
        {
            return "A" + assetID.ToString() + "U" + underlyingID.ToString() + "U" + userID.ToString();
        }

        public static string GenerateMultiTradeId()
        {
            lock (lockSeed)
            {
                _seed++;
                return _seed.ToString() + "MT";
            }
        }

        public static string GenerateSimulationId()
        {
            lock (lockSeed)
            {
                _seed++;
                return "SIMULATION_" + _seed.ToString();
            }
        }

        public static string GenerateCalculationRequestId()
        {
            lock (lockSeed)
            {
                _seed++;
                return "CalcRequest_" + _seed.ToString();
            }
        }
    }
}
