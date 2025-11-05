/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package prana.basketComplianceService.main;

import prana.utility.configuration.ApplicationHelper;
import prana.utility.logging.PranaLogManager;

/**
 * Main class entry point of application
 * 
 */
public class BasketComplianceServiceMain {

	/**
	 * Start up function
	 * 
	 * @param args
	 */
	public static void main(String[] args) {
		try {
			// Exiting application if it is not correctly started
			if (!ApplicationHelper.checkIfStartupCorrectly(args))
				System.exit(-1);
			ServiceManager.initializeService();
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}
}
