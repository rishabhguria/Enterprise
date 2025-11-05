namespace Prana.Global
{
    public class StringSplitter
    {
        public static string[] SplitByString(string stringToSplit, string splitter)
        {
            int offset = 0;
            int index = 0;
            int[] offsets = new int[stringToSplit.Length + 1];

            while (index < stringToSplit.Length)
            {
                int indexOf = stringToSplit.IndexOf(splitter, index);
                if (indexOf != -1)
                {
                    offsets[offset++] = indexOf;
                    index = (indexOf + splitter.Length);
                }
                else
                {
                    index = stringToSplit.Length;
                }
            }

            string[] final = new string[offset + 1];
            if (offset == 0)
            {
                final[0] = stringToSplit;
            }
            else
            {
                offset--;
                final[0] = stringToSplit.Substring(0, offsets[0]);
                for (int i = 0; i < offset; i++)
                {
                    final[i + 1] = stringToSplit.Substring(offsets[i] + splitter.Length, offsets[i + 1] - offsets[i] - splitter.Length);
                }
                final[offset + 1] = stringToSplit.Substring(offsets[offset] + splitter.Length);
            }
            return final;
        }
    }
}
