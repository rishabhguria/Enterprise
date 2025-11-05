namespace Prana.NirvanaQualityChecker
{
    class CheckBoxItem
    {

        public int Key
        {
            //    get;
            set { }
        }

        public string Name { get; set; }

        public CheckBoxItem()
        {
        }

        public CheckBoxItem(string name, int val)
        {
            Name = name;
            Key = val;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
