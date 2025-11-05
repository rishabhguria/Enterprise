using Prana.BusinessObjects.Compliance.Enums;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Prana.BusinessObjects.Compliance.Alerting
{
    [DataContract]
    [KnownType(typeof(RuleOverrideType))]
    public class SimulationResult
    {
        #region PrivateFields
        private List<Alert> _alerts;
        private Boolean _isAllowed;
        private RuleOverrideType _overrideType;
        private String _simulationId;
        #endregion

        /// <summary>
        /// List of triggered alerts
        /// </summary>
        [DataMember]
        public List<Alert> Alerts
        {
            get { return _alerts; }
            set { _alerts = value; }
        }

        /// <summary>
        /// Whether a trade is allowed
        /// </summary>
        [DataMember]
        public Boolean Allowed
        {
            get { return _isAllowed; }
            set { _isAllowed = value; }
        }

        /// <summary>
        /// type of the override for trade.
        /// </summary>
        [DataMember]
        public RuleOverrideType OverrideType
        {
            get { return _overrideType; }
            set { _overrideType = value; }
        }

        /// <summary>
        /// simulationId for trade.
        /// </summary>
        [DataMember]
        public String SimulationId
        {
            get { return _simulationId; }
            set { _simulationId = value; }
        }

        public SimulationResult(Boolean isAllowed, List<Alert> alerts)
        {
            _alerts = alerts;
            _isAllowed = isAllowed;
        }

        public SimulationResult(Boolean isAllowed, List<Alert> alerts, RuleOverrideType overrideType, String simulationId)
        {
            _alerts = alerts;
            _isAllowed = isAllowed;
            _overrideType = overrideType;
            _simulationId = simulationId;
        }

    }
}
