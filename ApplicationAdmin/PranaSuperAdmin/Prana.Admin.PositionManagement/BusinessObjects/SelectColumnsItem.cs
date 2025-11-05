using System;
using System.Collections.Generic;
using System.Text;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
    public class SelectColumnsItem
    {
        private string _sourceColumnName;

        /// <summary>
        /// Gets or sets the name of the source column.
        /// </summary>
        /// <value>The name of the source column.</value>
        public string SourceColumnName
        {
            get { return _sourceColumnName; }
            set { _sourceColumnName = value; }
        }

        private string _description;

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        private SelectColumnsType _type;

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public SelectColumnsType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        private string _sampleValue;

        public string SampleValue
        {
            get { return _sampleValue; }
            set { _sampleValue = value; }
        }

        private string _notes;

        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        /// <value>The notes.</value>
        public string Notes
        {
            get { return _notes; }
            set { _notes = value; }
        }
    }
}
