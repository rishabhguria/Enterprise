using System;

namespace Prana.LiveFeed.UI
{
    /// <summary>
    /// Summary description for NewsCategoryNode.
    /// </summary>
    public class NewsCategoryNode
    {
        private int iID;
        private string strFullName;
        private string strCodeName;
        private int iParentID;

        public NewsCategoryNode()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public int ID
        {
            get
            {
                return this.iID;
            }
            set
            {
                if (value < 0)
                {
                    throw new Exception("ID can't be less than zero");
                }

                this.iID = value;
            }
        }

        public int ParentID
        {
            get
            {
                return this.iParentID;
            }
            set
            {
                if (value < 0)
                {
                    throw new Exception("iParentID can't be less than zero");
                }

                this.iParentID = value;
            }
        }

        public string Name
        {
            get
            {
                return this.strFullName;
            }
            set
            {
                if (value == null || value.Equals(""))
                {
                    throw new Exception("strFullName can't be nothing");
                }

                this.strFullName = value;
            }
        }

        public string Code
        {
            get
            {
                return this.strCodeName;
            }
            set
            {
                if (value == null)
                {
                    value = "";
                    //					throw new Exception("strCodeName can't be nothing");
                }

                this.strCodeName = value;
            }
        }



    }
}