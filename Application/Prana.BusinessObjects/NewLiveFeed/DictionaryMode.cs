using System.Collections.Generic;

namespace Prana.BusinessObjects
{
    public class DictionaryMode : IStore
    {
        private Dictionary<string, SymbolData> _liveFeedDataDictionary = new Dictionary<string, SymbolData>();
        private Dictionary<string, SymbolData> _alternateLiveFeedDataDictionary = new Dictionary<string, SymbolData>();
        private bool AlternateLevel1Store = false;
        readonly object _lockOnDict = new object();
        public void AddOrUpdateData(SymbolData liveFeedData)
        {
            try
            {
                lock (_lockOnDict)
                {
                    if (!AlternateLevel1Store)
                    {

                        // lock (_liveFeedDataDictionary)
                        //{
                        if (_liveFeedDataDictionary.ContainsKey(liveFeedData.Symbol))
                        {
                            _liveFeedDataDictionary[liveFeedData.Symbol].UpdateContinuousData(liveFeedData);

                        }
                        else
                        {
                            _liveFeedDataDictionary.Add(liveFeedData.Symbol, liveFeedData);
                        }
                        //}
                    }
                    else
                    {

                        // lock (_alternateLiveFeedDataDictionary)
                        //{
                        if (_alternateLiveFeedDataDictionary.ContainsKey(liveFeedData.Symbol))
                        {
                            _alternateLiveFeedDataDictionary[liveFeedData.Symbol].UpdateContinuousData(liveFeedData);

                        }
                        else
                        {
                            _alternateLiveFeedDataDictionary.Add(liveFeedData.Symbol, liveFeedData);
                        }
                        //}
                    }
                }


            }
            catch
            {

            }

        }

        //object _lockOnGetData = new object();
        public List<SymbolData> GetData()
        {
            List<SymbolData> list = null;
            try
            {
                lock (_lockOnDict)
                {

                    if (!AlternateLevel1Store)
                    {
                        AlternateLevel1Store = true;
                        //lock (_liveFeedDataDictionary)
                        //{
                        if (_liveFeedDataDictionary.Count > 0)
                        {
                            list = new List<SymbolData>(_liveFeedDataDictionary.Values);
                            _liveFeedDataDictionary.Clear();
                            return list;

                        }
                        else
                        {
                            return null;
                        }
                        //}
                    }
                    else
                    {
                        AlternateLevel1Store = false;
                        //lock (_alternateLiveFeedDataDictionary)
                        //{
                        if (_alternateLiveFeedDataDictionary.Count > 0)
                        {
                            list = new List<SymbolData>(_alternateLiveFeedDataDictionary.Values);
                            _alternateLiveFeedDataDictionary.Clear();
                            return list;

                        }
                        else
                        {
                            return null;
                        }
                        //}

                    }
                }
            }
            catch
            {
                return null;
            }

        }
        public void DeleteData(string symbol)
        {
            //try
            //{
            //    lock (_liveFeedDataDictionary)
            //    {
            //        if (_liveFeedDataDictionary.ContainsKey(symbol))
            //        {
            //            _liveFeedDataDictionary.Remove(symbol);
            //        }
            //    }

            //    lock (_alternateLiveFeedDataDictionary)
            //    {
            //        if (_alternateLiveFeedDataDictionary.ContainsKey(symbol))
            //        {
            //            _alternateLiveFeedDataDictionary.Remove(symbol);
            //        }
            //    }
            //}
            //catch 
            //{

            //}
        }
        public void ClearData()
        {
            //try
            //{
            //    lock (_liveFeedDataDictionary)
            //    {
            //       _liveFeedDataDictionary.Clear();
            //    }
            //    lock (_alternateLiveFeedDataDictionary)
            //    {
            //        _alternateLiveFeedDataDictionary.Clear();
            //    }
            //}
            //catch 
            //{

            //}
        }







    }
}
