package prana.loggingTool.communication;

public class CommunicationManager {

	public static void initializeCommunication() throws Exception {

		AmqpListenerHelper.intializeAllAmqpListeners();
	}

}
