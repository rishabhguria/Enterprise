namespace Prana.LiveFeed.UI
{

    public struct Filter
    {

        public string strSymbols;
        public string strService;
        public string strCategory;

        public Filter(string Symbols, string Service, string Category)
        {
            this.strSymbols = Symbols.ToUpper();
            this.strService = Service.ToUpper();
            this.strCategory = Category.ToLower();
        }

        public Filter(string s)
        {
            this.strSymbols = "";
            this.strService = "";
            this.strCategory = "";
        }


    }


    public struct FilterTab
    {
        public int iTabNumber;
        public string strTabKey;
        public Filter strctFilter;
        public string iSelectedID;
        public int iNumberDisplay;
        public int iNumberDays;
        public bool bAndOr;

        public FilterTab(int TabID, string TabKey, string Symbols, string Service, string Category, bool bAndOr, string SelectedID, int NumberDisplay, int NumberDays)
        {
            iTabNumber = TabID;
            strTabKey = TabKey;
            strctFilter = new Filter(Symbols, Service, Category);
            iSelectedID = SelectedID;
            iNumberDisplay = NumberDisplay;
            iNumberDays = NumberDays;
            this.bAndOr = bAndOr;
        }

        public FilterTab(int i)
        {
            iTabNumber = -1;
            strTabKey = "";
            strctFilter = new Filter("");
            iSelectedID = "";
            iNumberDisplay = 10;
            iNumberDays = 1;
            this.bAndOr = true;
        }
    }
    /// <summary>
    /// Summary description for NewsFilterTabs.
    /// </summary>
    /// 


    public class NewsFilterTabs
    {
        //		public NewsFilterTabs()
        //		{
        //			//
        //			// TODO: Add constructor logic here
        //			//
        //		}
        private System.Collections.Hashtable hashTabFilter = new System.Collections.Hashtable();
        //		System.Collections.IDictionaryEnumerator dicList;

        private static int iTabID = 0;
        private static NewsFilterTabs _classInstance;

        private NewsFilterTabs()
        {
            //
            // TODO: Add constructor logic here
            //
            //			hashTabFilter = new h


            //System.Collections.IDictionaryEnumerator
            //				dicList = hashTabFilter.GetEnumerator();
            //			System.Collections.DictionaryEntry _deHTKey = hashTabFilter;
            //			this.NewFilter(0,"All","",-1,10);

        }


        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// 

        public void Close(bool disposing)
        {
            this.Dispose(disposing);

        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                hashTabFilter.Clear();
                _classInstance = null;

            }

        }

        public static NewsFilterTabs Instance
        {
            get
            {

                if (_classInstance == null)
                {
                    _classInstance = new NewsFilterTabs();
                }
                return _classInstance;

            }

        }

        private string AddToHashtable(string Key, FilterTab filterTab)
        {
            if (hashTabFilter.ContainsKey(Key))
            {
                hashTabFilter.Remove(Key);
            }
            this.hashTabFilter.Add(Key, filterTab);

            return Key;
        }

        public int Count
        {
            get
            {
                return hashTabFilter.Count;
            }
        }

        public string AddFilter(string TabKey, string Symbols, string Service, string Category, bool bAndOr, string SelectedID, int NumberDisplay, int NumberDays)
        {
            TabKey = TabKey.Trim().ToLower();

            TabKey = TabKey.Replace(".", "-");

            for (int i = 0; i < TabKey.Length; i++)
            {
                if (!(char.IsLetterOrDigit(TabKey, i) || TabKey[i].Equals(',') || TabKey[i].Equals('-')))
                {
                    TabKey = TabKey.Replace(TabKey.Substring(i, 1), "");
                }
            }

            if (TabKey.Replace(",", "").Trim().Equals(""))
            {
                TabKey = "all";
            }


            Symbols = Symbols.Trim();
            char _cTemp;
            for (int i = 0; i < Symbols.Length; i++)
            {
                _cTemp = Symbols[i];
                if (!(char.IsLetterOrDigit(_cTemp) || _cTemp.Equals(',') || _cTemp.Equals('-') || _cTemp.Equals('.')))
                {
                    Symbols = Symbols.Replace(_cTemp.ToString(), "");
                }
            }
            if (Symbols.Replace(",", "").Trim().Equals(""))
            {
                Symbols = "";
            }





            Service = Service.Trim();

            for (int i = 0; i < Service.Length; i++)
            {
                _cTemp = Service[i];
                if (!(char.IsLetterOrDigit(_cTemp) || _cTemp.Equals(',') || _cTemp.Equals('-')))
                {
                    Service = Service.Replace(Service.Substring(i, 1), "");
                }
            }
            if (Service.Replace(",", "").Trim().Equals(""))
            {
                Service = "";
            }



            Category = Category.Trim().Replace(" ", ",");

            for (int i = 0; i < Category.Length; i++)
            {
                _cTemp = Category[i];
                if (!(char.IsLetterOrDigit(_cTemp) || _cTemp.Equals(',') || _cTemp.Equals('-')))
                {
                    Category = Category.Replace(Category.Substring(i, 1), "");
                }
            }
            if (Category.Replace(",", "").Trim().Equals(""))
            {
                Category = "";
            }

            if (NumberDays <= 0)
            {
                NumberDays = 1;
            }
            if (NumberDisplay <= 0)
            {
                NumberDisplay = 10;
            }


            string strKey = TabKey;//+":"+ iTabID.ToString();
            FilterTab _strucFilterTab;//= new FilterTab();
                                      //			if(hashTabFilter.ContainsKey(strKey))
                                      //			{
                                      ////				_deHTKey["Fg"]
                                      //				_strucFilterTab = (FilterTab)hashTabFilter[strKey];
                                      //
                                      //			}
                                      //			else
                                      //			{
                                      //				_strucFilterTab =  new FilterTab(1);
                                      //				this.hashTabFilter.Add(strKey,_strucFilterTab);
                                      //
                                      //			}
            _strucFilterTab = new FilterTab(iTabID, TabKey, Symbols, Service, Category, bAndOr, SelectedID, NumberDisplay, NumberDays);
            //			_strucFilterTab.iTabNumber = iTabID;
            //			_strucFilterTab.strTabKey = TabKey;
            //			_strucFilterTab.bAndOr = bAndOr;
            //			_strucFilterTab.strFilter = Filter;
            //			_strucFilterTab.iSelectedID = SelectedID;
            //			_strucFilterTab.iNumberDisplay = NumberDisplay;

            //			if(hashTabFilter.ContainsKey(strKey))
            //			{
            //				hashTabFilter.Remove(strKey);
            //			}
            //			this.hashTabFilter.Add(strKey,_strucFilterTab);

            this.AddToHashtable(strKey, _strucFilterTab);



            iTabID++;
            return strKey;
        }

        public string deleteFilter(string Key)
        {
            if (hashTabFilter.ContainsKey(Key))
            {
                hashTabFilter.Remove(Key);
            }

            return Key;
        }


        public FilterTab GetValue(string Key)
        {
            if (hashTabFilter.ContainsKey(Key))
            {
                return (FilterTab)hashTabFilter[Key];
            }
            else
            {
                return new FilterTab();
            }
        }

        //		public FilterTab GetValue(int Index)
        //		{		
        //			if(hashTabFilter.Count>Index)
        //			{
        //				return (FilterTab)hashTabFilter.Keys. ;
        //			}
        //			else
        //			{
        //				return new FilterTab(1);
        //			}
        //		}


        public void UpdateSymbols(string Key, string SymbolsNew)
        {
            SymbolsNew = SymbolsNew.Trim();

            for (int i = 0; i < SymbolsNew.Length; i++)
            {
                if (!(char.IsLetterOrDigit(SymbolsNew, i) || SymbolsNew[i].Equals(',') || SymbolsNew[i].Equals('-')))
                {
                    SymbolsNew = SymbolsNew.Replace(SymbolsNew.Substring(i, 1), "");
                }
            }
            if (SymbolsNew.Replace(",", "").Trim().Equals(""))
            {
                SymbolsNew = "";
            }


            FilterTab _structFilterTab = this.GetValue(Key);

            _structFilterTab.strctFilter.strSymbols = SymbolsNew;

            this.AddToHashtable(Key, _structFilterTab);

            //			if(hashTabFilter.ContainsKey(Key))
            //			{
            //				hashTabFilter.Remove(Key);
            //			}
            //			this.hashTabFilter.Add(Key,_structFilterTab);
        }



        public void UpdateService(string Key, string Service)
        {
            Service = Service.Trim();

            for (int i = 0; i < Service.Length; i++)
            {
                if (!(char.IsLetterOrDigit(Service, i) || Service[i].Equals(',') || Service[i].Equals('-')))
                {
                    Service = Service.Replace(Service.Substring(i, 1), "");
                }
            }
            if (Service.Replace(",", "").Trim().Equals(""))
            {
                Service = "";
            }


            FilterTab _structFilterTab = this.GetValue(Key);

            _structFilterTab.strctFilter.strService = Service;

            this.AddToHashtable(Key, _structFilterTab);

            //			if(hashTabFilter.ContainsKey(Key))
            //			{
            //				hashTabFilter.Remove(Key);
            //			}
            //			this.hashTabFilter.Add(Key,_structFilterTab);
        }

        public void UpdateCategory(string Key, string Category)
        {
            Category = Category.Trim();

            for (int i = 0; i < Category.Length; i++)
            {
                if (!(char.IsLetterOrDigit(Category, i) || Category[i].Equals(',') || Category[i].Equals('-')))
                {
                    Category = Category.Replace(Category.Substring(i, 1), "");
                }
            }
            if (Category.Replace(",", "").Trim().Equals(""))
            {
                Category = "";
            }


            FilterTab _structFilterTab = this.GetValue(Key);

            _structFilterTab.strctFilter.strCategory = Category;

            this.AddToHashtable(Key, _structFilterTab);
            //			if(hashTabFilter.ContainsKey(Key))
            //			{
            //				hashTabFilter.Remove(Key);
            //			}
            //			this.hashTabFilter.Add(Key,_structFilterTab);
        }

        public void UpdateAndOr(string Key, bool bAndOr)
        {

            FilterTab _structFilterTab = this.GetValue(Key);

            _structFilterTab.bAndOr = bAndOr;

            this.AddToHashtable(Key, _structFilterTab);

            //			if(hashTabFilter.ContainsKey(Key))
            //			{
            //				hashTabFilter.Remove(Key);
            //			}
            //			this.hashTabFilter.Add(Key,_structFilterTab);
        }

        public void UpdateSelectedID(string Key, string SelectedID)
        {

            FilterTab _structFilterTab = this.GetValue(Key);

            _structFilterTab.iSelectedID = SelectedID;

            this.AddToHashtable(Key, _structFilterTab);

            //			if(hashTabFilter.ContainsKey(Key))
            //			{
            //				hashTabFilter.Remove(Key);
            //			}
            //			this.hashTabFilter.Add(Key,_structFilterTab);
        }


        public void UpdateNumberDisplay(string Key, int NumberDisplay)
        {
            if (NumberDisplay <= 0)
            {
                NumberDisplay = 10;
            }

            FilterTab _structFilterTab = this.GetValue(Key);

            _structFilterTab.iNumberDisplay = NumberDisplay;

            this.AddToHashtable(Key, _structFilterTab);


        }

        public void UpdateNumberDays(string Key, int NumberDays)
        {
            if (NumberDays <= 0)
            {
                NumberDays = 1;
            }

            FilterTab _structFilterTab = this.GetValue(Key);

            _structFilterTab.iNumberDays = NumberDays;

            this.AddToHashtable(Key, _structFilterTab);


        }


    }
}







