package prana.basketComplianceService.communication;

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
	 * prana.basketComplianceService.amqpCollectors.BasketComplianceOrderCollector
	 * 
	 * @author dewashish
	 * 
	 */
	static void initializeBasketComplianceOrderCollector() throws Exception {
		try {
			String hostName = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS, ConfigurationConstants.KEY_APP_SETTINGS_AMQPSERVER);
			String vhost = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS, ConfigurationConstants.KEY_APP_SETTINGS_AMQP_VHOST);
			String userId = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS, ConfigurationConstants.KEY_APP_SETTINGS_AMQP_USERID);
			String password = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS, ConfigurationConstants.KEY_APP_SETTINGS_AMQP_PASSWORD);

			String basketComplianceQueue = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_QUEUE_LIST, ConfigurationConstants.KEY_QUEUE_BASKET_COMPLIANCE);

			String epl = "Create Dataflow AMQPInDataFlowBasketCompliance \n"
					+ "Create schema BasketComplianceQueue as (symbol string), \n" + "AMQPSource -> Outstream<BasketComplianceQueue> \n"
					+ "{host: '" + hostName + "',vhost:'" + vhost + "',userName:'" + userId + "',password:'" + password
					+ "',queueName: '" + basketComplianceQueue + "'," + "declareDurable:true," + "declareAutoDelete:false,"
					+ "collector: {class: 'prana.basketComplianceService.amqpCollectors.BasketComplianceCollector'}, logMessages: true } \n"
					+ "LogSink(Outstream){}";

			EPDeployment deployment = CEPManager.compileDeploy(epl, "AMQPInDataFlowBasketCompliance");
			EPDataFlowInstance instance = CEPManager.getEPRuntime().getDataFlowService()
					.instantiate(deployment.getDeploymentId(), "AMQPInDataFlowBasketCompliance");
			instance.start();
			PranaLogManager.info("--- BasketComplianceCollector Started ---");
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}
}
