using Csla;
using System;
using System.ComponentModel;

namespace Prana.PM.BLL
{
    [Serializable()]
    public class DeskTrade : BusinessBase<DeskTrade>
    {

        public DeskTrade()
        {
            MarkAsChild();
        }



        private Guid _deskID = Guid.NewGuid();
        [Browsable(false)]
        public Guid DeskID
        {
            get
            {
                return _deskID;
            }
            set { _deskID = value; }
        }


        private string _side;

        /// <summary>
        /// Gets or sets the side.
        /// </summary>
        /// <value>The side.</value>
        public string Side
        {
            get { return _side; }
            set { _side = value; }
        }

        private string _symbol;

        /// <summary>
        /// Gets or sets the symbol.
        /// </summary>
        /// <value>The symbol.</value>
        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        private double _quantity;

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        /// <value>The quantity.</value>
        public double Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }

        private string _instructions;

        /// <summary>
        /// Gets or sets the instructions.
        /// </summary>
        /// <value>The instructions.</value>
        public string Instructions
        {
            get { return _instructions; }
            set { _instructions = value; }
        }



        /// <summary>
        /// Gets the id value.
        /// </summary>
        /// <returns></returns>
        protected override object GetIdValue()
        {
            return _deskID;
        }


        /// <summary>
        /// Gets a value indicating whether this instance is valid.
        /// </summary>
        /// <value><c>true</c> if this instance is valid; otherwise, <c>false</c>.</value>
        public override bool IsValid
        {
            get { return base.IsValid; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is dirty.
        /// </summary>
        /// <value><c>true</c> if this instance is dirty; otherwise, <c>false</c>.</value>
        public override bool IsDirty
        {
            get { return base.IsDirty; }
        }
    }
}
