namespace Prana.BusinessObjects
{

    public class OTCCustomFields
    {


        /// <summary>
        /// ID
        /// </summary>
        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// Name
        /// </summary>
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string defaultValue;
        public string DefaultValue
        {
            get { return defaultValue; }
            set { defaultValue = value; }
        }

        /// <summary>
        /// Data Type
        /// </summary>
        private string dataType;
        public string DataType
        {
            get { return dataType; }
            set
            {
                dataType = value;


            }
        }

        /// <summary>
        /// UI Order
        /// </summary>
        private int uIOrder;
        public int UIOrder
        {
            get { return uIOrder; }
            set { uIOrder = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string instrumentType;
        public string InstrumentType
        {
            get { return instrumentType; }
            set { instrumentType = value; }
        }


    }
}
