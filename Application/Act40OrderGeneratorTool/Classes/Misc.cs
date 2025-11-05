using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Text;

namespace Act40OrderGeneratorTool
{
    internal class Misc
    {
        internal static void SetUpComboBox(UltraCombo ultraCombo, List<EnumerationValue> lst)
        {
            ultraCombo.DataSource = lst;
            ultraCombo.DataBind();
            ultraCombo.DisplayMember = "DisplayText";
            ultraCombo.ValueMember = "Value";
            ultraCombo.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;
            ultraCombo.DisplayLayout.Bands[0].ColHeadersVisible = false;

            if (lst.Count > 0)
            {
                ultraCombo.Value = lst[0].Value;
                ultraCombo.Text = lst[0].DisplayText;
            }
            else
            {
                ultraCombo.Value = "";
                ultraCombo.Text = "";
            }
        }

        internal static String DataTabeleAsFilterQuery(DataTable dt)
        {
            StringBuilder query = new StringBuilder();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow condition = dt.Rows[i];
                String column = GetEnumDescription((FilterFields)Enum.Parse(typeof(FilterFields), condition["Column"].ToString(), true));
                String filterOperator = GetOperator(condition["Condition"].ToString());
                String value = condition["Value"].ToString();

                if (i == dt.Rows.Count - 1)
                    query.AppendFormat(" {0} {1} {2} ", column, filterOperator, value);
                else
                    query.AppendFormat(" {0} {1} {2} AND ", column, filterOperator, value);
            }
            return query.ToString();
        }

        internal static String GetOperator(String filterOperator)
        {
            FilterOperators x = (FilterOperators)Enum.Parse(typeof(FilterOperators), filterOperator, true);
            switch (x)
            {
                case FilterOperators.EqualTo:
                    return "=";
                case FilterOperators.GreaterThan:
                    return ">";
                case FilterOperators.GreaterThanAndEqualTo:
                    return ">=";
                case FilterOperators.LessThan:
                    return "<";
                case FilterOperators.LessThanAndEqualTo:
                    return "<=";
            }
            return "";
        }

        internal static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
    }
}
