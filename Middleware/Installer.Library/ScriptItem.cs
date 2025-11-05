using System;
    
namespace Installer.Library
{
    public class ScriptItem
    {
        public bool Execute { get; set; }
        public string ScriptType { get; set; }
        public string ScriptName { get; set; }
        public bool DropExisting { get; set; }
        public string Status { get; set; }
        public int Order { get; set; }
        public string FullPathName { get; set; }
    }
}
