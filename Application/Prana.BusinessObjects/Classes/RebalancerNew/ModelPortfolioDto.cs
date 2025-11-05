using Prana.BusinessObjects.Enumerators.RebalancerNew;

namespace Prana.BusinessObjects.Classes.RebalancerNew
{
    public class ModelPortfolioDto
    {
        private string modelPortfolioName;

        public string ModelPortfolioName
        {
            get { return modelPortfolioName; }
            set { modelPortfolioName = value; }
        }

        private RebalancerEnums.ModelPortfolioType modelPortfolioType;

        public RebalancerEnums.ModelPortfolioType ModelPortfolioType
        {
            get { return modelPortfolioType; }
            set { modelPortfolioType = value; }
        }

        private int? referenceId;
        /// <summary>
        /// If ModelPortfolioType is master fund then ReferenceId will be master fund id.
        /// If ModelPortfolioType is Account then ReferenceId will be fund id.
        /// </summary>
        public int? ReferenceId
        {
            get { return referenceId; }
            set { referenceId = value; }
        }

        private int id;
        /// <summary>
        /// If ModelPortfolioType is master fund then ReferenceId will be master fund id.
        /// If ModelPortfolioType is Account then ReferenceId will be fund id.
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string modelPortfolioData;

        public string ModelPortfolioData
        {
            get { return modelPortfolioData; }
            set { modelPortfolioData = value; }
        }

        public RebalancerEnums.RebalancerPositionsType? PositionsType { get; set; }

        public RebalancerEnums.UseTolerance? UseTolerance { get; set; }

        public RebalancerEnums.ToleranceFactor? ToleranceFactor { get; set; }

        public RebalancerEnums.TargetPercentType? TargetPercentType { get; set; }

    }
}
