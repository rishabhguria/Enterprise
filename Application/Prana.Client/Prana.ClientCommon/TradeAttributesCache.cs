using Infragistics.Win;
using Prana.CommonDataCache;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Prana.ClientCommon
{
    public class TradeAttributesCache
    {
        private static readonly BindingSource[] bss = new BindingSource[] { new BindingSource(), new BindingSource(), new BindingSource(), new BindingSource(), new BindingSource(), new BindingSource() };
        private static bool[] _keepRecords = CachedDataManager.GetInstance.GetAttributeKeepRecords();
        private static readonly object _cacheLock = new object();

        public static bool[] KeepRecords
        {
            get { return _keepRecords; }
            set { _keepRecords = value; }
        }

        static TradeAttributesCache()
        {
            for (int i = 0; i < bss.Length; i++)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("C1");

                DataView dw = new DataView(dt);
                dw.Sort = "C1 ASC";
                bss[i].DataSource = dw;
            }
        }

        public static void updateCache(string[] attributes)
        {
            if (attributes.Length == bss.Length)
            {
                for (int i = 0; i < bss.Length; i++)
                {
                    if (_keepRecords[i] && attributes[i] != null && attributes[i] != string.Empty)
                    {
                        DataView dw = (DataView)bss[i].DataSource;
                        if (dw.Find(attributes[i]) < 0)
                        {
                            DataRowView drw = dw.AddNew();
                            drw.BeginEdit();
                            drw[0] = attributes[i];
                            drw.EndEdit();
                        }
                    }
                }
            }
        }

        public static void updateCache(List<string>[] attributes, bool isInitialize = false)
        {
            if (attributes == null || attributes.Length < bss.Length)
                return;

            lock (_cacheLock) // Lock to prevent concurrent access
            {
                for (int i = 0; i < bss.Length; i++)
                {
                    var currentAttributes = attributes[i];
                    if (currentAttributes == null)
                        continue;

                    DataView dw = (DataView)bss[i].DataSource;

                    if (isInitialize)
                    {
                        // Reinitialize: clear and repopulate with provided attributes
                        dw.Table.Rows.Clear();

                        foreach (string attribute in currentAttributes)
                        {
                            DataRow row = dw.Table.NewRow();
                            row[0] = attribute;
                            dw.Table.Rows.Add(row);
                        }
                    }
                    else if (_keepRecords[i])
                    {
                        foreach (string attribute in currentAttributes)
                        {
                            if (dw.Find(attribute) < 0)
                            {
                                DataRowView drw = dw.AddNew();
                                drw.BeginEdit();
                                drw[0] = attribute;
                                drw.EndEdit();
                            }
                        }
                    }
                }
            }
        }

        public static BindableValueList[] getValueList(Control control)
        {
            BindableValueList[] vls = new BindableValueList[bss.Length];
            for (int i = 0; i < bss.Length; i++)
            {
                vls[i] = new BindableValueList(bss[i], "", "C1", "C1", control);
            }
            return vls;
        }
    }
}
