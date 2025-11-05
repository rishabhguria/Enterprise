package prana.ruleEngineMediator.communication;

public class CommunicationManager {

	public static void initializeCommunication() throws Exception {

		AmqpListenerHelper.intializeAllAmqpListeners();
	}

}
