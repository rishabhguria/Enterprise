using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;
using System.Data.SqlClient;

namespace Middleware.Reference.Check
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public class Search
    {
      
        string[] TS_Tables = { "T_TS_GenericPNL", /*"T_TS_GenericPNLV7",*/ "T_TS_Transactions", "T_TS_DerivedData", "T_TS_DailyReturns", "T_TS_DailyReturns_Fund" };
        string[] MW_Tables = { "T_MW_GenericPNL", /*"T_TS_GenericPNLV7",*/ "T_MW_Transactions", "T_MW_DerivedData", "T_MW_DailyReturns", "T_MW_DailyReturns_Fund" };
     
        //readonly static string ConnectString = "Data Source=localhost;Initial Catalog=MonarchV1.7;Persist Security Info=True;User ID=sa;Password=NIRvana2@@6";

        readonly static string ConnectStringL = "Data Source=englander2.nirvanasolutions.com;Initial Catalog=MonarchV1.7.1;Persist Security Info=True;User ID=sa;Password=NIRvana2@@6";
               
        readonly List<SqlObject> SqlObjects = new List<SqlObject>();
             
        Server server = null;    
        Database db = null;     
     

        public static List<string> Exclude = new List<string>();

       
        readonly static List<QueryResults> Depends = new List<QueryResults>();


        /// <summary>
        /// Initializes a new instance of the <see cref="Search"/> class.
        /// </summary>
        /// <param name="LoadFromDatabase">if set to <c>true</c> [load from database].</param>
        /// <remarks></remarks>
        public Search(bool LoadFromDatabase = false)
        {
           
            ServerConnection cn = new ServerConnection { ConnectionString = ConnectStringL };

            server = new Server(cn);
            //db = server.Databases["MonarchV1.7"];
            db = server.Databases["MonarchV1.7.1"];

            if (LoadFromDatabase)
            {
                PersistAllObject();                
            }
            
            LoadAllObjectFromFile();
            GetDepends(TS_Tables);
            
            List<QueryResults> NonDepends = GetReferences(Depends);

            
            while (NonDepends.Count != 0)
            {
               Depends.AddRange(NonDepends);
               NonDepends = GetReferences(NonDepends);
            }
                                

            DumpOutput();
            IntegrityUpdate();
            CopyDependencies();
            UpdateDatabase();
            
        }

        /// <summary>
        /// Integrities the update.
        /// </summary>
        /// <remarks></remarks>
        public void IntegrityUpdate()
        {
            foreach (var item in Depends)
            {               
                if (string.IsNullOrEmpty(item.Rename) == false)
                {
                    if (item.Name.Contains("MW_"))
                    {
                    }
                    item.Sql = Regex.Replace(item.Sql, String.Format(@"\b{0}\b", item.Name), item.GetName(), RegexOptions.Singleline | RegexOptions.IgnoreCase);
                }

                TS_Tables.ToList().ForEach(table => item.Sql = Regex.Replace(item.Sql, String.Format(@"\b{0}\b", table), table.Replace("T_TS", "T_MW"), RegexOptions.Singleline | RegexOptions.IgnoreCase));

                foreach (var subitem in item.UsedBy)
                {
                    subitem.Sql = Regex.Replace(subitem.Sql, String.Format(@"\b{0}\b", item.Name), item.GetName(), RegexOptions.Singleline | RegexOptions.IgnoreCase);

                    //var match = NonDepends.Where(w => w.Name == subitem.Name).SingleOrDefault();

                    //if (match != null)
                    //{
                    //}
                    foreach (var table in item.DependsOn)
                    {
                        subitem.Sql = Regex.Replace(subitem.Sql, String.Format(@"\b{0}\b", table.Name), table.GetName(), RegexOptions.Singleline | RegexOptions.IgnoreCase);

                    }
                   // item.DependsOn.ForEach(table => subitem.Sql = Regex.Replace(subitem.Sql, String.Format(@"\b{0}\b", table.Name ), table.Rename, RegexOptions.Singleline | RegexOptions.IgnoreCase));                 
                    //item.DependsOn.ForEach(table => subitem.Sql = subitem.Sql.Replace("T_TS_", "T_MW_"));
                }
               
            }
        }
        /// <summary>
        /// Copies the dependencies.
        /// </summary>
        /// <remarks></remarks>
        public void CopyDependencies()
        {
            if (Directory.Exists(".\\Depends") == false)
                Directory.CreateDirectory(".\\Depends");
            
            if (Directory.Exists(".\\Depends\\F") == false)
            {
                Directory.CreateDirectory(".\\Depends\\F");
                Directory.CreateDirectory(".\\Depends\\P");
                Directory.CreateDirectory(".\\Depends\\V");
            }            
            Directory.GetFiles(".\\Depends\\", "*.sql", SearchOption.AllDirectories).ToList().ForEach(item => File.Delete(item));

            foreach (var item in Depends)
            {                
                string dst = string.Format(".\\Depends\\{0}\\{1}.sql", item.Type, item.GetName());

                //Debug.Assert(File.Exists(dst) == false);
                File.WriteAllText(dst, item.Sql);
               
                foreach (var subitem in item.UsedBy)
                {                    
                    dst = string.Format(".\\Depends\\{0}\\{1}.sql", subitem.Type, subitem.GetName());

                    //Debug.Assert(File.Exists(dst) == false);
                    File.WriteAllText(dst, subitem.Sql);

                }
            }
        }

        /// <summary>
        /// Dumps the depends.
        /// </summary>
        /// <remarks></remarks>
        public void DumpDepends()
        {
            List<string> Unique = new List<string>();

            Dictionary<string, List<string>> tmp = new Dictionary<string, List<string>>();

            foreach (var item in Depends)
            {
                Unique.Add(item.Name);
                foreach (var subitem in item.UsedBy)
                {
                    Unique.Add(subitem.Name);
                    List<string> results;
                    if (tmp.TryGetValue(subitem.Name, out results))
                    {
                        results.Add(item.Name);
                    }
                    else
                    {
                        List<string> x = new List<string>();
                        x.Add(item.Name);
                        tmp.Add(subitem.Name, x);
                    }
                }
            }
            
            foreach(string row in Unique.Distinct())
            {
                System.Diagnostics.Debug.Print(row.ToString());
            }
            foreach (var kvp in tmp)
            {
                string s = string.Format("{0}\t{1}", kvp.Key, string.Join("\t", kvp.Value));
                System.Diagnostics.Debug.Print(s);
            }
        }
        /// <summary>
        /// Dumps the output.
        /// </summary>
        /// <remarks></remarks>
        public void DumpOutput()
        {
            foreach (var item in Depends)
            {
                string q = string.Format("{0}\t{1}\t{2}\t{3}\t{4}", item.Name, item.Rename, item.Type, string.Join("\t", item.DependsOn.Select(s=>s.Name)), string.Join("\t", item.UsedBy.Select(s=>s.Name)));
                //string q = string.Format("{0}\t{1}\t{2}\t{3}\t{4}", item.Name, item.Rename, item.Type, "", string.Join("\t", item.UsedBy.Select(s => s.Name)));
                System.Diagnostics.Debug.Print(q);
            }
            
        }
        /// <summary>
        /// Persists all object.
        /// </summary>
        /// <remarks></remarks>
        public void PersistAllObject()
        {

            if (Directory.Exists(".\\SqlObjects") == false)
                Directory.CreateDirectory(".\\SqlObjects");
           
            Directory.GetFiles(".\\SqlObjects\\", "*.sql").ToList().ForEach(item => File.Delete(item));           

            if (Directory.Exists(".\\SqlObjects\\F") == false)
            {
                Directory.CreateDirectory(".\\SqlObjects\\F");
                Directory.CreateDirectory(".\\SqlObjects\\P");
                Directory.CreateDirectory(".\\SqlObjects\\V");
            }

            foreach (UserDefinedFunction function in db.UserDefinedFunctions)
            {
                if (function.IsSystemObject) continue;

                function.Script(new ScriptingOptions() { FileName = String.Format(".\\SqlObjects\\F\\{0}.sql", function.Name), IncludeIfNotExists = true, ScriptDrops = true, IncludeDatabaseContext = false });
                function.Script(new ScriptingOptions() { FileName = String.Format(".\\SqlObjects\\F\\{0}.sql", function.Name), AppendToFile = true, IncludeDatabaseContext = false });

                //File.WriteAllText(String.Format(".\\SqlObjects\\F\\{0}.sql", function.Name), function.TextBody);                
            }
            foreach (StoredProcedure procedure in db.StoredProcedures)
            {
                if (procedure.IsSystemObject) continue;

                try
                {
                    procedure.Script(new ScriptingOptions() { FileName = String.Format(".\\SqlObjects\\P\\{0}.sql", procedure.Name), IncludeIfNotExists = true,ScriptDrops = true, IncludeDatabaseContext = false });
                    procedure.Script(new ScriptingOptions() { FileName = String.Format(".\\SqlObjects\\P\\{0}.sql", procedure.Name), AppendToFile = true, IncludeDatabaseContext = false });

                    //File.WriteAllText(String.Format(".\\SqlObjects\\P\\{0}.sql", procedure.Name), procedure.TextBody);
                }
                catch (Exception)
                {
                    continue;
                }
            }
            foreach (View view in db.Views)
            {
                if (view.IsSystemObject) continue;
                try
                {
                    view.Script(new ScriptingOptions() { FileName = String.Format(".\\SqlObjects\\V\\{0}.sql", view.Name), IncludeIfNotExists = true,ScriptDrops = true, IncludeDatabaseContext = false });
                    view.Script(new ScriptingOptions() { FileName = String.Format(".\\SqlObjects\\V\\{0}.sql", view.Name), AppendToFile = true, IncludeDatabaseContext = false });
                    //File.WriteAllText(String.Format(".\\SqlObjects\\V\\{0}.sql", view.Name), view.TextBody);
                }
                catch (Exception)
                {
                    continue;
                }
            }


        }

        /// <summary>
        /// Loads all object from file.
        /// </summary>
        /// <remarks></remarks>
        public void LoadAllObjectFromFile()
        {
            string[] files = Directory.GetFiles(".\\sqlobjects", "*.sql", SearchOption.AllDirectories);

            foreach (string file in files)
            {
                string name = Path.GetFileNameWithoutExtension(file);
                string[] path = file.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);

                string type = path[2];

                if (type == "F" || type == "V" || type == "P")                   
                    SqlObjects.Add(new SqlObject() { Name = name, Type = type, Sql = File.ReadAllText(file) });
                else
                    return;

            }
        }
        
        /// <summary>
        /// Gets the depends.
        /// </summary>
        /// <param name="filters">The filters.</param>
        /// <remarks></remarks>
        private void GetDepends(string[] filters)
        {
            foreach (SqlObject obj in SqlObjects)
            {
                string sql = RemoveComment(obj.Sql);

                if (Exclude.Contains(obj.Name)) continue;
                QueryResults result = null;
                foreach (var filter in filters)
                {  
                    if (Regex.IsMatch(sql, String.Format(@"\b{0}\b", filter), RegexOptions.Singleline | RegexOptions.IgnoreCase))
                    {
                        if (result == null)
                        {
                            result = new QueryResults() { Name = obj.Name, Type = obj.Type, Rename = Rename(obj.Name, obj.Type), Sql = obj.Sql };
                        }
                        result.DependsOn.Add(new QueryResults() { Name = filter, Type = "T", Rename = filter.Replace("T_TS_", "T_MW") });
                    }                    
                }
                if (result != null)
                    Depends.Add(result);
            }
        }

     
        /// <summary>
        /// Gets the references.
        /// </summary>
        /// <remarks></remarks>
        private List<QueryResults> GetReferences(List<QueryResults> ds)
        {
            List<QueryResults> NonDepends = new List<QueryResults>();

            foreach (QueryResults result in ds)
            {
                foreach (SqlObject obj in SqlObjects)
                {
                    if (Exclude.Contains(obj.Name)) continue;

                    if (obj.Name == result.Name) continue;

                    if (Regex.IsMatch(RemoveComment(obj.Sql), String.Format(@"\b{0}\b", result.Name), RegexOptions.Singleline | RegexOptions.IgnoreCase))
                    {
                        if (result.UsedBy.Contains(new QueryResults() { Name = obj.Name }, new ContainsName()) == false)
                        {
                            var xref = Depends.Where(w => w.Name == obj.Name).SingleOrDefault();
                                                       
                            if (xref != null)
                            {
                                result.UsedBy.Add(xref);
                                result.References.Add(xref);
                            }
                            else if (NonDepends.Contains(new QueryResults() { Name = obj.Name }, new ContainsName()) == false)
                            {
                                QueryResults non = new QueryResults() { Rename = Rename(obj.Name, obj.Type), Name = obj.Name, Type = obj.Type, Sql = obj.Sql };

                                NonDepends.Add(non);
                                result.NoReferences.Add(non);
                                result.UsedBy.Add(non);
                            }
                            else
                            {
                                result.NoReferences.Add(NonDepends.Where(w=> w.Name == obj.Name).SingleOrDefault());
                                result.UsedBy.Add(NonDepends.Where(w => w.Name == obj.Name).SingleOrDefault());
                            }
                        }
                    }           
                }
            }
            return NonDepends;
        }

        /// <summary>
        /// Renames the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private string Rename(string name, string type)
        {
            string rename = string.Empty;
            string[] cols = name.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
            if (cols.Length == 1) return string.Empty;

            if (cols[1] != "W")
            {
                rename = type.Substring(0, 1) + "_MW";
                for (int i = 1; i < cols.Length; i++)
                {
                    rename += String.Format("_{0}{1}", cols[i].Substring(0, 1).ToUpper(), cols[i].Substring(1));
                }
            }
            return rename.Replace("D_", "");

        }
        /// <summary>
        /// Removes the comment.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string RemoveComment(string value)
        {
            List<string> blocks = value.Split(new string[] { "/*" }, StringSplitOptions.RemoveEmptyEntries).ToList();

            //C Block Style comments //* *//
            for (int i = 0; i < blocks.Count; i++)
            {
                int offset = blocks[i].IndexOf("*/");
                if (offset >= 0)
                {
                    blocks[i] = blocks[i].Substring(offset + 2);
                }

            }
            value = string.Join(" ", blocks);


            // SQL Comments --
            List<string> rows = value.
                Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();

            for (int i = rows.Count() - 1; i >= 0; i--)
            {
                if (rows[i].Contains("--"))
                {
                    for (int idx = 0; idx < rows[i].Count(); idx++)
                    {
                        if (rows[i].Substring(idx, 2) == "--")
                        {
                            rows[i] = rows[i].Substring(0, idx);
                            break;
                        }
                    }

                }
            }
            return string.Join(Environment.NewLine, rows.ToArray());
        }

        private void UpdateDatabase()
        {
            try
            {

                RemoveOldReferences();

               // server.ConnectionContext.BeginTransaction();
                CreateNewReferences();
              //  server.ConnectionContext.CommitTransaction();

               // server.ConnectionContext.BeginTransaction();
              
              //  server.ConnectionContext.CommitTransaction();

                TestExisting();
                
            
            }
            catch (Exception)
            {
                server.ConnectionContext.RollBackTransaction();
            }
        }

        private void RemoveOldReferences()
        {
            
            foreach (var table in TS_Tables)
            {
                try
                {
                    Debug.Write(string.Format("Dropping {0}", table));
                    server.ConnectionContext.ExecuteNonQuery("Drop Table " + table);
                    Debug.WriteLine("\tOK");
                }
                catch (Exception ex)
                {
                    Debug.Write("\tFailed\t");
                    if (ex.InnerException != null)
                    {
                        Debug.WriteLine(string.Format(ex.InnerException.Message.Replace(Environment.NewLine, "\t")));
                    }
                    else
                    {
                        Debug.WriteLine(string.Format(ex.Message.Replace(Environment.NewLine, "\t")));
                    }

                }

            }
            foreach (var item in Depends)
            {
                try
                {
                    Debug.Write(string.Format("Dropping {0}", item.Name));
                    server.ConnectionContext.ExecuteNonQuery(item.DropStatement());
                    Debug.WriteLine("\tOK");
                }
                catch (Exception ex)
                {
                    Debug.Write("\tFailed\t");
                    if (ex.InnerException != null)
                    {
                        Debug.WriteLine(string.Format(ex.InnerException.Message.Replace(Environment.NewLine, "\t")));
                    }
                    else
                    {
                        Debug.WriteLine(string.Format(ex.Message.Replace(Environment.NewLine, "\t")));
                    }

                }
              
            }
        }

    
        private void CreateNewReferences()
        {
            foreach (var item in Depends)
            {
                try
                {
                    server.ConnectionContext.ServerMessage += ConnectionContext_ServerMessage;
                    Debug.Write(string.Format("Creating {0}", item.GetName()));
                    server.ConnectionContext.ExecuteNonQuery(item.Sql);
                    Debug.WriteLine("\tOK");
                }
                catch (Exception ex)
                {
                    Debug.Write("\tFailed\t");
                    if (ex.InnerException != null)
                    {
                        Debug.WriteLine(string.Format(ex.InnerException.Message.Replace(Environment.NewLine, "\t")));
                    }
                    else
                    {
                        Debug.WriteLine(string.Format(ex.Message.Replace(Environment.NewLine, "\t")));
                    }

                }
            }
        }
        private void TestExisting()
        {
            server.ConnectionContext.ServerMessage += ConnectionContext_ServerMessage;
            List<string> Sqls = new List<string>();
            foreach (var item in Depends)
            {
                string sqlp = string.Empty;  
                try
                {
                    string sql = string.Format("select type from sys.objects where object_id = OBJECT_ID(N'[dbo].[{0}]')", item.GetName());

                    Console.WriteLine("Testing Object {0}", item.GetName());
                    SqlDataReader reader = server.ConnectionContext.ExecuteReader(sql);
                    reader.Read();
                    string xtype = reader[0].ToString();
                    reader.Close();

                    sqlp = GetSampleParms(item.GetName(), xtype);

                    reader = server.ConnectionContext.ExecuteReader(sqlp);
                    reader.Close();
                }
                catch (Exception)
                {
                    Debug.Print(sqlp);
                    Sqls.Add(sqlp);
                    //Console.WriteLine("Error Executing {0}\n\n{1}", item.Name, ex.Message);                        
                }
            }
           
        }

        void ConnectionContext_ServerMessage(object sender, ServerMessageEventArgs e)
        {
            if (e.Error.Number == 5701) return;

           // Debug.Print("Line {0}, Number {1}, Proc {2}, Msg {3}", e.Error.LineNumber, e.Error.Number, e.Error.Procedure, e.Error.Message);
            Console.WriteLine("Line {0}, Number {1}, Proc {2}, Msg {3}\n", e.Error.LineNumber, e.Error.Number, e.Error.Procedure, e.Error.Message);
        }

        /// <summary>
        /// Gets the parameter string.
        /// </summary>
        /// <param name="col">The col.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static string GetParameterString(ParameterCollectionBase col, bool usePrefix)
        {
            string buffer = string.Empty;

            List<string> buffers = new List<string>();

            foreach (Parameter parm in col)
            {
                switch (parm.DataType.SqlDataType)
                {
                    case SqlDataType.Xml:
                        buffers.Add("<xml/>");
                        break;

                    case SqlDataType.VarBinary:
                        buffers.Add("0");
                        break;

                    case SqlDataType.UniqueIdentifier:
                        buffers.Add(DateTime.Now.Ticks.ToString());
                        break;

                    case SqlDataType.Text:
                    case SqlDataType.NText:
                        buffers.Add("");
                        break;

                    case SqlDataType.Bit:
                        buffers.Add("1");
                        break;

                    case SqlDataType.Timestamp:
                        buffers.Add(string.Format("'{0}'", DateTime.Now.ToLongTimeString()));
                        break;

                    case SqlDataType.DateTime:
                    //  case SqlDataType.DateTime2:

                    case SqlDataType.SmallDateTime:
                        buffers.Add(string.Format("'{0}'", DateTime.Today.ToShortDateString()));
                        break;

                    //    case SqlDataType.Date:
                    //        buffers.Add(string.Format("'{0}'", DateTime.Today.ToShortDateString()));
                    //        break;

                    case SqlDataType.Char:
                    case SqlDataType.NVarChar:
                    case SqlDataType.VarChar:
                    case SqlDataType.VarCharMax:
                    case SqlDataType.NVarCharMax:
                        buffers.Add(string.Format("'{0}'", ""));
                        break;

                    case SqlDataType.Int:
                    case SqlDataType.BigInt:
                    case SqlDataType.SmallInt:
                    case SqlDataType.TinyInt:
                        buffers.Add(string.Format("{0}", 0));
                        break;

                    case SqlDataType.Float:
                    case SqlDataType.Decimal:
                    case SqlDataType.Real:
                        buffers.Add(string.Format("{0}", "0.0"));
                        break;

                    default:
                        Debug.Print(parm.DataType.ToString());
                        break;
                }

            }

            buffer = string.Join(",", buffers.ToArray());
            return buffer;

        }

        /// <summary>
        /// Gets the sample parms.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="xType">Type of the x.</param>
        /// <param name="database">The database.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string GetSampleParms(string name, string xType)
        {
        

            name = name.Replace(Environment.NewLine, "");

            if (QueryResults.IsProcedure(xType))
            {
                StoredProcedureParameterCollection col = db.StoredProcedures[name].Parameters;
                return String.Format("-- EXEC dbo.[{0}] {2}\r\n\r\n EXEC dbo.[{0}] {1}", name, GetParameterString((ParameterCollectionBase)col, true), GetParameterHelp(col));
            }
            else if (QueryResults.IsFunctionScalar(xType))
            {
                UserDefinedFunctionParameterCollection udcol = db.UserDefinedFunctions[name].Parameters;
                return String.Format("-- SELECT dbo.[{0}] {2}\r\n\r\nSELECT dbo.[{0}] ({1})", name, GetParameterString((ParameterCollectionBase)udcol, false), GetParameterHelp(udcol));
            }
            else if (QueryResults.IsFunctionTable(xType))
            {
                UserDefinedFunctionParameterCollection udcol = db.UserDefinedFunctions[name].Parameters;
                return String.Format("-- SELECT * from dbo.[{0}] {2}\r\n\r\nSELECT * from dbo.[{0}] ({1})", name, GetParameterString((ParameterCollectionBase)udcol, false), GetParameterHelp(udcol));
            }
            else
                return String.Format("SELECT TOP 1000 * FROM dbo.[{0}]", name);
                                    
        }

        /// <summary>
        /// Gets the parameter help.
        /// </summary>
        /// <param name="col">The col.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static string GetParameterHelp(ParameterCollectionBase col)
        {
            List<string> buffers = new List<string>();
            foreach (Parameter parm in col)
            {
                buffers.Add(String.Format("{0}=[{1}]", parm.Name, parm.DataType));
            }

            string buffer = string.Join(",", buffers.ToArray());
            return buffer;
        }

    }
}
