namespace Prana.PortfolioReports
{
    class DataSetHelper
    {
        //public DataSet ds;
        //private System.Collections.ArrayList m_FieldInfo; 
        //private string m_FieldList;
        //private System.Collections.ArrayList GroupByFieldInfo; 
        //private string GroupByFieldList;

        //public DataSetHelper(ref DataSet DataSet)
        //{
        //    //ds = DataSet;
        //}
        public DataSetHelper()
        {
            //ds = null;
        }

        //private class FieldInfo
        //{
        //    public string RelationName;
        //    public string FieldName;	//source table field name
        //    public string FieldAlias;	//destination table field name
        //    public string Aggregate;
        //}


        //private void ParseFieldList(string FieldList, bool AllowRelation)
        //{
        //    /*
        //     * This code parses FieldList into FieldInfo objects  and then 
        //     * adds them to the m_FieldInfo private member
        //     * 
        //     * FieldList systax:  [relationname.]fieldname[ alias], ...
        //    */
        //    if (m_FieldList == FieldList) return;
        //    m_FieldInfo = new System.Collections.ArrayList();
        //    m_FieldList = FieldList;
        //    FieldInfo Field; string[] FieldParts; string[] Fields = FieldList.Split(',');
        //    int i;
        //    for (i = 0; i <= Fields.Length - 1; i++)
        //    {
        //        Field = new FieldInfo();
        //        //parse FieldAlias
        //        FieldParts = Fields[i].Trim().Split(' ');
        //        switch (FieldParts.Length)
        //        {
        //            case 1:
        //                //to be set at the end of the loop
        //                break;
        //            case 2:
        //                Field.FieldAlias = FieldParts[1];
        //                break;
        //            default:
        //                throw new Exception("Too many spaces in field definition: '" + Fields[i] + "'.");
        //        }
        //        //parse FieldName and RelationName
        //        FieldParts = FieldParts[0].Split('.');
        //        switch (FieldParts.Length)
        //        {
        //            case 1:
        //                Field.FieldName = FieldParts[0];
        //                break;
        //            case 2:
        //                if (AllowRelation == false)
        //                    throw new Exception("Relation specifiers not permitted in field list: '" + Fields[i] + "'.");
        //                Field.RelationName = FieldParts[0].Trim();
        //                Field.FieldName = FieldParts[1].Trim();
        //                break;
        //            default:
        //                throw new Exception("Invalid field definition: " + Fields[i] + "'.");
        //        }
        //        if (Field.FieldAlias == null)
        //            Field.FieldAlias = Field.FieldName;
        //        m_FieldInfo.Add(Field);
        //    }
        //}

        //private void ParseGroupByFieldList(string FieldList)
        //{
        //    /*
        //    * Parses FieldList into FieldInfo objects and adds them to the GroupByFieldInfo private member
        //    * 
        //    * FieldList syntax: fieldname[ alias]|operatorname(fieldname)[ alias],...
        //    * 
        //    * Supported Operators: count,sum,max,min,first,last
        //    */
        //    if (GroupByFieldList == FieldList) return;
        //    GroupByFieldInfo = new System.Collections.ArrayList();
        //    FieldInfo Field; string[] FieldParts; string[] Fields = FieldList.Split(',');
        //    for (int i = 0; i <= Fields.Length - 1; i++)
        //    {
        //        Field = new FieldInfo();
        //        //Parse FieldAlias
        //        FieldParts = Fields[i].Trim().Split(' ');
        //        switch (FieldParts.Length)
        //        {
        //            case 1:
        //                //to be set at the end of the loop
        //                break;
        //            case 2:
        //                Field.FieldAlias = FieldParts[1];
        //                break;
        //            default:
        //                throw new ArgumentException("Too many spaces in field definition: '" + Fields[i] + "'.");
        //        }
        //        //Parse FieldName and Aggregate
        //        FieldParts = FieldParts[0].Split('(');
        //        switch (FieldParts.Length)
        //        {
        //            case 1:
        //                Field.FieldName = FieldParts[0];
        //                break;
        //            case 2:
        //                Field.Aggregate = FieldParts[0].Trim().ToLower();    //we're doing a case-sensitive comparison later
        //                Field.FieldName = FieldParts[1].Trim(' ', ')');
        //                break;
        //            default:
        //                throw new ArgumentException("Invalid field definition: '" + Fields[i] + "'.");
        //        }
        //        if (Field.FieldAlias == null)
        //        {
        //            if (Field.Aggregate == null)
        //                Field.FieldAlias = Field.FieldName;
        //            else
        //                Field.FieldAlias = Field.Aggregate + "of" + Field.FieldName;
        //        }
        //        GroupByFieldInfo.Add(Field);
        //    }
        //    GroupByFieldList = FieldList;
        //}

        //public DataTable CreateGroupByTable(string TableName, DataTable SourceTable, string FieldList)
        //{
        //    /*
        //     * Creates a table based on aggregates of fields of another table
        //     * 
        //     * RowFilter affects rows before GroupBy operation. No "Having" support
        //     * though this can be emulated by subsequent filtering of the table that results
        //     * 
        //     *  FieldList syntax: fieldname[ alias]|aggregatefunction(fieldname)[ alias], ...
        //    */
        //    if (FieldList == null)
        //    {
        //        throw new ArgumentException("You must specify at least one field in the field list.");
        //        //return CreateTable(TableName, SourceTable);
        //    }
        //    else
        //    {
        //        DataTable dt = new DataTable(TableName);
        //        ParseGroupByFieldList(FieldList);
        //        foreach (FieldInfo Field in GroupByFieldInfo)
        //        {
        //            DataColumn dc = SourceTable.Columns[Field.FieldName];
        //            if (Field.Aggregate == null)
        //                dt.Columns.Add(Field.FieldAlias, dc.DataType, dc.Expression);
        //            else
        //                dt.Columns.Add(Field.FieldAlias, dc.DataType);
        //        }
        //        if (ds != null)
        //            ds.Tables.Add(dt);
        //        return dt;
        //    }
        //}


        //    public void InsertGroupByInto(DataTable DestTable, DataTable SourceTable, string FieldList,
        //string RowFilter, string GroupBy)
        //    {
        //        /*
        //         * Copies the selected rows and columns from SourceTable and inserts them into DestTable
        //         * FieldList has same format as CreateGroupByTable
        //        */
        //        if (FieldList == null)
        //            throw new ArgumentException("You must specify at least one field in the field list.");
        //        ParseGroupByFieldList(FieldList);	//parse field list
        //        ParseFieldList(GroupBy, false);			//parse field names to Group By into an arraylist
        //        DataRow[] Rows = SourceTable.Select(RowFilter, GroupBy);
        //        DataRow LastSourceRow = null, DestRow = null; bool SameRow; int RowCount = 0;
        //        foreach (DataRow SourceRow in Rows)
        //        {
        //            SameRow = false;
        //            if (LastSourceRow != null)
        //            {
        //                SameRow = true;
        //                foreach (FieldInfo Field in m_FieldInfo)
        //                {
        //                    if (!ColumnEqual(LastSourceRow[Field.FieldName], SourceRow[Field.FieldName]))
        //                    {
        //                        SameRow = false;
        //                        break;
        //                    }
        //                }
        //                if (!SameRow)
        //                    DestTable.Rows.Add(DestRow);
        //            }
        //            if (!SameRow)
        //            {
        //                DestRow = DestTable.NewRow();
        //                RowCount = 0;
        //            }
        //            RowCount += 1;
        //            foreach (FieldInfo Field in GroupByFieldInfo)
        //            {
        //                switch (Field.Aggregate)    //this test is case-sensitive
        //                {
        //                    case null:        //implicit last
        //                    case "":        //implicit last
        //                    case "last":
        //                        DestRow[Field.FieldAlias] = SourceRow[Field.FieldName];
        //                        break;
        //                    case "first":
        //                        if (RowCount == 1)
        //                            DestRow[Field.FieldAlias] = SourceRow[Field.FieldName];
        //                        break;
        //                    case "count":
        //                        DestRow[Field.FieldAlias] = RowCount;
        //                        break;
        //                    case "sum":
        //                        DestRow[Field.FieldAlias] = Add(DestRow[Field.FieldAlias], SourceRow[Field.FieldName]);
        //                        break;
        //                    case "max":
        //                        DestRow[Field.FieldAlias] = Max(DestRow[Field.FieldAlias], SourceRow[Field.FieldName]);
        //                        break;
        //                    case "min":
        //                        if (RowCount == 1)
        //                            DestRow[Field.FieldAlias] = SourceRow[Field.FieldName];
        //                        else
        //                            DestRow[Field.FieldAlias] = Min(DestRow[Field.FieldAlias], SourceRow[Field.FieldName]);
        //                        break;
        //                }
        //            }
        //            LastSourceRow = SourceRow;
        //        }
        //        if (DestRow != null)
        //            DestTable.Rows.Add(DestRow);
        //    }


        //    public DataTable SelectGroupByInto(string TableName, DataTable SourceTable, string FieldList,
        //string RowFilter, string GroupBy)
        //    {
        //        /*
        //         * Selects data from one DataTable to another and performs various aggregate functions
        //         * along the way. See InsertGroupByInto and ParseGroupByFieldList for supported aggregate functions.
        //         */
        //        DataTable dt = CreateGroupByTable(TableName, SourceTable, FieldList);
        //        InsertGroupByInto(dt, SourceTable, FieldList, RowFilter, GroupBy);
        //        return dt;
        //    }

    }
}
