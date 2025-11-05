using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Installer.Library;
using System.Text.RegularExpressions;


namespace Middleware.Installer
{
    class OrderByNaturalSort : IComparer<ScriptItem>
    {
        private Dictionary<string, string[]> table;

        public OrderByNaturalSort()
	{
		table = new Dictionary<string, string[]>();
	}

	public void Dispose()
	{
		table.Clear();
		table = null;
	}

    public int Compare(ScriptItem x, ScriptItem y)
	{
		if(x.ScriptName == y.ScriptName)
		{
			return 0;
		}
		string[] x1, y1;
		if(!table.TryGetValue(x.ScriptName, out x1))
		{
			x1 = Regex.Split(x.ScriptName.Replace(" ", ""), "([0-9]+)");
			table.Add(x.ScriptName, x1);
		}
		if(!table.TryGetValue(y.ScriptName, out y1))
		{
			y1 = Regex.Split(y.ScriptName.Replace(" ", ""), "([0-9]+)");
			table.Add(y.ScriptName, y1);
		}

		for(int i = 0; i < x1.Length && i < y1.Length; i++)
		{
			if(x1[i] != y1[i])
			{
				return PartCompare(x1[i], y1[i]);
			}
		}
		if(y1.Length > x1.Length)
		{
			return 1;
		}
		else if(x1.Length > y1.Length)
		{
			return -1;
		}
		else
		{
			return 0;
		}
	}

	private static int PartCompare(string left, string right)
	{
		int x, y;
		if(!int.TryParse(left, out x))
		{
			return left.CompareTo(right);
		}

		if(!int.TryParse(right, out y))
		{
			return left.CompareTo(right);
		}

		return x.CompareTo(y);
	}
    }
}
