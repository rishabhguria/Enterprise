using System;
using System.ComponentModel;
using System.Data;


namespace Prana.NirvanaQualityChecker
{
    class Script
    {
        private bool isSelected = true;

        public bool SelectScript
        {
            get { return isSelected; }
            set { isSelected = value; }
        }

        private String _parentFolder;
        public String ModuleName
        {
            get { return _parentFolder; }
            set { _parentFolder = value; }
        }



        private String _scriptName;

        public String ScriptName
        {
            get { return _scriptName; }
            set { _scriptName = value; }
        }



        private DataSet _databaseResult;
        [Browsable(false)]
        public DataSet DatabaseResult
        {
            get
            {
                return _databaseResult;
            }
            set
            {
                _databaseResult = value;


                if (_databaseResult != null && _databaseResult.Tables[_databaseResult.Tables.Count - 1].Columns.Count == 1 && _databaseResult.Tables[_databaseResult.Tables.Count - 1].Rows.Count == 1 && _databaseResult.Tables[_databaseResult.Tables.Count - 1].Columns.Contains("ErrorMsg"))
                {
                    _errorMessage = _databaseResult.Tables[_databaseResult.Tables.Count - 1].Rows[0].ItemArray[0].ToString();
                    _databaseResult.Tables.Remove(_databaseResult.Tables[_databaseResult.Tables.Count - 1]);
                }
                else
                {
                    _errorMessage = "BLANK";
                }
            }
        }

        private String _errorMessage = " ";
        public String ErrorMessage
        {
            get
            {
                if (_databaseResult == null)
                {
                    return " ";
                }
                return _errorMessage;
            }
        }




        private String _fullScriptPath;
        [Browsable(false)]
        public String FullScriptPath
        {
            get { return _fullScriptPath; }
            set { _fullScriptPath = value; }
        }

        // private string _description;

        //[Browsable(false)]
        //public string Description
        //{
        //    //get
        //    //{
        //    //    if (_description == null || _description.Equals(""))
        //    //        return ScriptName;
        //    //    else
        //    //        return _description;
        //    //}
        //    set { _description = value; }
        //}


        //[Browsable(false)]
        //public String FileName { get; set; }



        public Script(String scriptName, String fullScriptPath, String parentFolder)//, String description)
        {
            ScriptName = scriptName;
            FullScriptPath = fullScriptPath;
            ModuleName = parentFolder;
            //          Description = description;
        }


        public String Status
        {
            get
            {
                if (ErrorMessage.Equals(" "))
                    return "?";
                if (ErrorMessage.Equals("-"))
                    return "[ ]";
                if (ErrorMessage.Equals("") || ErrorMessage.Equals("BLANK"))
                {
                    return "✔";
                }
                return ErrorMessage.Substring(ErrorMessage.Length - 1).Equals("!") ? "!" : "✘";
            }
        }

        public string View
        {

            get
            {

                return "            Error View";
            }
        }

    }
}
