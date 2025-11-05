namespace Prana.Analytics
{
    public class ComponentRiskData
    {
        private double _quantity;
        public double Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }

        private double _componentRisk;
        public double ComponentRisk
        {
            get { return _componentRisk; }
            set { _componentRisk = value; }
        }
    }
}