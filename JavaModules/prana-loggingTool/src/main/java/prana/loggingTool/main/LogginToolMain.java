package prana.loggingTool.main;

import prana.utility.configuration.ApplicationHelper;
import prana.utility.logging.PranaLogManager;

/**
 * Main entry point of application
 * 
 * @author dewashish
 * 
 */
public class LogginToolMain {

	/**
	 * Pass true to call start application
	 * 
	 * @param args
	 */
	public static void main(String[] args) {

		try {
			if (!ApplicationHelper.checkIfStartupCorrectly(args))
				System.exit(-1);
			ServiceManager.initializeService();
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
		}

	}

}
