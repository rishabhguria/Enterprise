using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Middleware.Reference.Check
{
    public class QueryResults
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        /// <remarks></remarks>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        /// <remarks></remarks>
        public string Type { get; set; }
        /// <summary>
        /// Gets or sets the rename.
        /// </summary>
        /// <value>The rename.</value>
        /// <remarks></remarks>
        public string Rename { get; set; }
        /// <summary>
        /// Gets or sets the SQL.
        /// </summary>
        /// <value>The SQL.</value>
        /// <remarks></remarks>
        public string Sql { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<QueryResults> Depends = new List<QueryResults>();
        /// <summary>
        /// 
        /// </summary>
        public List<QueryResults> NoReferences = new List<QueryResults>();

        /// <summary>
        /// 
        /// </summary>
        public List<QueryResults> References = new List<QueryResults>();
        /// <summary>
        /// 
        /// </summary>
        public List<QueryResults> UsedBy = new List<QueryResults>();
        /// <summary>
        /// 
        /// </summary>
        public List<QueryResults> DependsOn = new List<QueryResults>();
        /// <summary>
        /// 
        /// </summary>
        public List<string> CommentRefs = new List<string>();

        public string GetName()
        {
            return string.IsNullOrEmpty(Rename) == false ? Rename : Name;
        }

        public static bool IsTable(string XType)
        {
            return XType == "U";
        }
        public static bool IsView(string XType)
        {
            return XType == "V";
        }
        public static bool IsProcedure(string XType)
        {
            string[] items = { "P", "PC" };

            return items.Contains(XType);
        }
        public static bool IsFunction(string XType)
        {
            string[] items = { "FN", "IF", "TF", "FS", "FT" };

            return items.Contains(XType);
        }
        public static bool IsFunctionTable(string XType)
        {
            string[] items = { "TF", "IF" };
            return items.Contains(XType);
        }
        public static bool IsFunctionScalar(string XType)
        {
            string[] items = { "FN" };
            return items.Contains(XType);
        }
        public string DropStatement()
        {
            string d = string.Format("IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[{0}]'))", Name);

            if (Type.Equals("P"))
                return d + "\nDrop Procedure " + Name;
            else if (Type.Equals("F"))
                return d + "\nDrop Function " + Name;
            else if (Type.Equals("V"))
                return d + "\nDrop View " + Name;
            else
               return string.Empty;
        }
     
    }
}
