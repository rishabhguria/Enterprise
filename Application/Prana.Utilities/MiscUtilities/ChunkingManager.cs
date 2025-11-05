using Prana.BusinessObjects;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prana.Utilities.MiscUtilities
{
    public class ChunkingManager
    {
        public static List<List<T>> CreateChunksForClosing<T>(List<T> collection, int chunkSize)
        {
            List<List<T>> chunkList = new List<List<T>>();
            Type typeParameterType = typeof(T);
            try
            {
                if (typeParameterType.Equals(typeof(TaxLot)))
                {
                    List<TaxLot> Tradelist = collection.Cast<TaxLot>().ToList();
                    int records = Tradelist.Count;
                    int recordsParsed = 0;
                    int i = 0;
                    string ClosingID = string.Empty;

                    while (records > recordsParsed)
                    {
                        List<TaxLot> chunk = new List<TaxLot>();
                        i = recordsParsed;
                        bool F_record = true;
                        for (; i < Tradelist.Count; i++)
                        {
                            var TradeVariable = Tradelist[i];
                            if (chunk.Count == chunkSize)
                                continue;

                            if (F_record)
                            {
                                ClosingID = TradeVariable.TaxLotClosingId;
                                chunk.Add(TradeVariable);
                                recordsParsed++;
                                F_record = false;
                                continue;
                            }
                            if (ClosingID == TradeVariable.TaxLotClosingId)
                            {
                                chunk.Add(TradeVariable);
                                recordsParsed++;
                                F_record = true;
                            }
                        }

                        List<T> NewChunk = chunk.Cast<T>().ToList();
                        chunkList.Add(NewChunk);
                    }
                }
                else
                {
                    int records = collection.Count;
                    int recordsParsed = 0;
                    int i = 0;

                    while (records > recordsParsed)
                    {
                        List<T> chunk = new List<T>();
                        i = recordsParsed;
                        for (; i < collection.Count; i++)
                        {
                            var TradeVariable = collection[i];
                            if (chunk.Count == chunkSize)
                                break;

                            chunk.Add(TradeVariable);
                            recordsParsed = recordsParsed + 1;
                        }

                        List<T> NewChunk = new List<T>(chunk);
                        chunkList.Add(NewChunk);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

            return chunkList;
        }

        public static List<List<T>> CreateChunks<T>(List<T> collection, int chunkSize)
        {

            List<List<T>> chunkList = new List<List<T>>();
            try
            {
                //modified by Omshiv, March 2014, when collection is null it throw error
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-3591
                if (collection != null && collection.Count > 0)
                {
                    int records = collection.Count;
                    int recordsParsed = 0;

                    while (records > recordsParsed)
                    {
                        List<T> chunk = new List<T>();

                        if (chunkSize >= (records - recordsParsed))
                        {
                            chunkSize = (records - recordsParsed);
                        }

                        chunk = collection.GetRange(recordsParsed, chunkSize);

                        //  string key = (recordsParsed) + "-" + (recordsParsed + chunkSize);
                        chunkList.Add(chunk);

                        recordsParsed += chunkSize;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return chunkList;
        }

    }
}
