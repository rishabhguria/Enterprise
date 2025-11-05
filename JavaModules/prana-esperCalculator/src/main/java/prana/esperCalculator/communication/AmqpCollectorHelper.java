package prana.esperCalculator.communication;

import prana.esperCalculator.constants.ConfigurationConstants;
import prana.esperCalculator.commonCode.CEPManager;
import prana.utility.configuration.ConfigurationHelper;
import prana.utility.logging.PranaLogManager;

import com.espertech.esper.common.client.dataflow.core.EPDataFlowInstance;
import com.espertech.esper.runtime.client.EPDeployment;

/**
 * Use this class to initialize specified data flow to receive data from amqp
 * 
 * @author dewashish
 * 
 */
class AmqpCollectorHelper {

	/**
	 * This method initializes LiveFeed data flow. Consumer class is
	 * prana.esperCalculator.amqpCollectors.SymbolDataCollector
	 * 
	 * @author dewashish
	 * 
	 */
	static void initializeLiveFeedCollector() throws Exception {
		try {
			String hostName = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS, ConfigurationConstants.KEY_APP_SETTINGS_AMQPSERVER);
			String vhost = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS, ConfigurationConstants.KEY_APP_SETTINGS_AMQP_VHOST);
			String userId = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS, ConfigurationConstants.KEY_APP_SETTINGS_AMQP_USERID);
			String password = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS, ConfigurationConstants.KEY_APP_SETTINGS_AMQP_PASSWORD);
			String liveFeedExchange = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_EXCHANGE_LIST, ConfigurationConstants.KEY_EXCHANGE_LIVEFEED);
			String liveFeedQueue = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_QUEUE_LIST, ConfigurationConstants.KEY_QUEUE_LIVEFEED);
			String ttlForQueue = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS, ConfigurationConstants.KEY_APP_SETTINGS_TTL_LIVEFEED);

			String epl = "Create Dataflow AMQPInDataFlowLiveFeed \n" + "Create schema SymbolData as (symbol string), \n"
					+ "AMQPSource -> Outstream<SymbolData> \n" + "{host: '" + hostName + "',vhost:'" + vhost
					+ "',userName:'" + userId + "',password:'" + password + "',exchange: '" + liveFeedExchange
					+ "',queueName: '" + liveFeedQueue + "'," + "declareDurable:true," + "declareAutoDelete:true,"
					+ "routingKey:'All'," + "declareAdditionalArgs:{'x-message-ttl':" + ttlForQueue + ",qos:20},"
					+ "collector: {class: 'prana.esperCalculator.amqpCollectors.SymbolDataCollector'}, logMessages: true } \n"
					+ "LogSink(Outstream){}";

			EPDeployment deployment = CEPManager.compileDeploy(epl, "AMQPInDataFlowLiveFeed");
			EPDataFlowInstance instance = CEPManager.getEPRuntime().getDataFlowService()
					.instantiate(deployment.getDeploymentId(), "AMQPInDataFlowLiveFeed");
			instance.start();
			PranaLogManager.info("--- LiveFeedCollector Started ---");

		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}

	/**
	 * This method initializes LiveFeed data flow. Consumer class is
	 * prana.esperCalculator.amqpCollectors.WhatIfTaxlotCollector
	 * 
	 * @author dewashish
	 * 
	 */
	static void initializeWhatIfOrderCollector() throws Exception {
		try {
			String hostName = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS, ConfigurationConstants.KEY_APP_SETTINGS_AMQPSERVER);
			String vhost = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS, ConfigurationConstants.KEY_APP_SETTINGS_AMQP_VHOST);
			String userId = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS, ConfigurationConstants.KEY_APP_SETTINGS_AMQP_USERID);
			String password = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS, ConfigurationConstants.KEY_APP_SETTINGS_AMQP_PASSWORD);

			String whatIfOrderQueue = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_QUEUE_LIST, ConfigurationConstants.KEY_QUEUE_WHATIF_ORDER);

			String epl = "Create Dataflow AMQPInDataFlowWhatIfTaxlot \n"
					+ "Create schema WhatIfTaxlot as (symbol string), \n" + "AMQPSource -> Outstream<WhatIfTaxlot> \n"
					+ "{host: '" + hostName + "',vhost:'" + vhost + "',userName:'" + userId + "',password:'" + password
					+ "',queueName: '" + whatIfOrderQueue + "'," + "declareDurable:true," + "declareAutoDelete:false,"
					+ "collector: {class: 'prana.esperCalculator.amqpCollectors.WhatIfTaxlotCollector'}, logMessages: true } \n"
					+ "LogSink(Outstream){}";

			EPDeployment deployment = CEPManager.compileDeploy(epl, "AMQPInDataFlowWhatIfTaxlot");
			EPDataFlowInstance instance = CEPManager.getEPRuntime().getDataFlowService()
					.instantiate(deployment.getDeploymentId(), "AMQPInDataFlowWhatIfTaxlot");
			instance.start();
			PranaLogManager.info("--- WhatIfTaxlotCollector Started ---");
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}
	
	/**
	 * This method initializes SecurityDetails data flow. Consumer class is
	 * prana.esperCalculator.amqpCollectors.SecurityDetailsCollector
	 * 
	 */
	static void initializeSecurityDetailsCollector() throws Exception {
		try {
			String hostName = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS, ConfigurationConstants.KEY_APP_SETTINGS_AMQPSERVER);
			String vhost = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS, ConfigurationConstants.KEY_APP_SETTINGS_AMQP_VHOST);
			String userId = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS, ConfigurationConstants.KEY_APP_SETTINGS_AMQP_USERID);
			String password = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS, ConfigurationConstants.KEY_APP_SETTINGS_AMQP_PASSWORD);

			String securityQueue = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_QUEUE_LIST, ConfigurationConstants.KEY_QUEUE_SECURITY_DETAILS);

			String epl = "Create Dataflow AMQPSecurityDetailsFlow \n"
					+ "Create schema SecurityDetails as (symbol string), \n" + "AMQPSource -> Outstream<SecurityDetails> \n"
					+ "{host: '" + hostName + "',vhost:'" + vhost + "',userName:'" + userId + "',password:'" + password
					+ "',queueName: '" + securityQueue + "'," + "declareDurable:true," + "declareAutoDelete:false,"
					+ "collector: {class: 'prana.esperCalculator.amqpCollectors.SecurityDetailsCollector'}, logMessages: true } \n"
					+ "LogSink(Outstream){}";

			EPDeployment deployment = CEPManager.compileDeploy(epl, "AMQPSecurityDetailsFlow");
			EPDataFlowInstance instance = CEPManager.getEPRuntime().getDataFlowService()
					.instantiate(deployment.getDeploymentId(), "AMQPSecurityDetailsFlow");
			instance.start();
			PranaLogManager.info("--- SecurityDetailsCollector Started ---");
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}
	
	/**
	 * This method initializes SymbolData data flow. Consumer class is
	 * prana.esperCalculator.amqpCollectors.ValidatedSymbolDataCollector
	 * 
	 */
	static void initializeValidatedSymbolDataCollector() throws Exception {
		try {
			String hostName = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS, ConfigurationConstants.KEY_APP_SETTINGS_AMQPSERVER);
			String vhost = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS, ConfigurationConstants.KEY_APP_SETTINGS_AMQP_VHOST);
			String userId = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS, ConfigurationConstants.KEY_APP_SETTINGS_AMQP_USERID);
			String password = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS, ConfigurationConstants.KEY_APP_SETTINGS_AMQP_PASSWORD);

			String symbolDataQueue = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_QUEUE_LIST, ConfigurationConstants.KEY_QUEUE_SYMBOL_DATA);

			String epl = "Create Dataflow AMQPValidatedSymbolDataFlow \n"
					+ "Create schema ValidatedSymbolData as (symbol string), \n" + "AMQPSource -> Outstream<ValidatedSymbolData> \n"
					+ "{host: '" + hostName + "',vhost:'" + vhost + "',userName:'" + userId + "',password:'" + password
					+ "',queueName: '" + symbolDataQueue + "'," + "declareDurable:true," + "declareAutoDelete:false,"
					+ "collector: {class: 'prana.esperCalculator.amqpCollectors.ValidatedSymbolDataCollector'}, logMessages: true } \n"
					+ "LogSink(Outstream){}";

			EPDeployment deployment = CEPManager.compileDeploy(epl, "AMQPValidatedSymbolDataFlow");
			EPDataFlowInstance instance = CEPManager.getEPRuntime().getDataFlowService()
					.instantiate(deployment.getDeploymentId(), "AMQPValidatedSymbolDataFlow");
			instance.start();
			PranaLogManager.info("--- ValidatedSymbolDataCollector Started ---");
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}

	/**
	 * This method initializes LiveFeed data flow. Consumer class is
	 * prana.esperCalculator.amqpCollectors.TaxlotCollector
	 * 
	 * @author dewashish
	 * 
	 */
	static void initializeOrderCollector() throws Exception {
		try {
			String hostName = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS, ConfigurationConstants.KEY_APP_SETTINGS_AMQPSERVER);
			String vhost = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS, ConfigurationConstants.KEY_APP_SETTINGS_AMQP_VHOST);
			String userId = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS, ConfigurationConstants.KEY_APP_SETTINGS_AMQP_USERID);
			String password = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS, ConfigurationConstants.KEY_APP_SETTINGS_AMQP_PASSWORD);
			String orderQueue = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_QUEUE_LIST, ConfigurationConstants.KEY_QUEUE_ORDER);

			String epl = "Create Dataflow AMQPInDataFlowPnLOrder \n" + "Create schema Taxlot as (symbol string), \n"
					+ "AMQPSource -> Outstream<Taxlot> \n" + "{host: '" + hostName + "',vhost:'" + vhost
					+ "',userName:'" + userId + "',password:'" + password + "',queueName: '" + orderQueue + "',"
					+ "declareDurable:true," + "declareAutoDelete:false,"
					+ "collector: {class: 'prana.esperCalculator.amqpCollectors.TaxlotCollector'}, logMessages: true } \n"
					+ "LogSink(Outstream){}";

			EPDeployment deployment = CEPManager.compileDeploy(epl, "AMQPInDataFlowPnLOrder");
			EPDataFlowInstance instance = CEPManager.getEPRuntime().getDataFlowService()
					.instantiate(deployment.getDeploymentId(), "AMQPInDataFlowPnLOrder");
			instance.start();
			PranaLogManager.info("--- OrderCollector Started ---");
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}
}
