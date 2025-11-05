using System;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class Asset
    {
        int _assetID = int.MinValue;
        string _name = string.Empty;

        /// <summary>
        /// constructor
        /// </summary>
        public Asset(int assetID, string name)
        {
            _assetID = assetID;
            _name = name;
        }

        public int AssetID
        {
            get
            {
                return _assetID;
            }

            set
            {
                _assetID = value;
            }
        }


        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }

        public override string ToString()
        {
            return Name;
        }

    }
}
