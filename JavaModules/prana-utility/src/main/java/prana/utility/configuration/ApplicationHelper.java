package prana.utility.configuration;

import java.io.IOException;
import java.lang.management.ManagementFactory;
import java.net.InetSocketAddress;
import java.net.ServerSocket;

import prana.utility.logging.PranaLogManager;
import prana.utility.memory.MemoryHelper;

public class ApplicationHelper {

	/**
	 * Server socket instance to keep lock on port
	 */
	private static ServerSocket _lockServerSocket;
	private static Object _lockerObject = new Object();

	public static String getApplicationinfo() throws Exception {
		try {
			synchronized (_lockerObject) {
				/*Runtime runtime = Runtime.getRuntime();
				int mb = 1024 * 1024;
				double heapMaxSize = runtime.maxMemory() / mb;*/
				// PranaLogManager.info("Heap Size: " + heapMaxSize);
				return "ProcessSignature: "
						+ ManagementFactory.getRuntimeMXBean().getName()
						+ "\nPort no used: " + _lockServerSocket.getLocalPort()
						+ "\nHeap Size: " + MemoryHelper.getMaxMemory();

			}

		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}

	/**
	 * Lock the socket port so that only one instance of application runs
	 * 
	 * @throws Exception
	 *             throws Exception if already running
	 */
	public static void lockPortForSingleInstance(String portNo)
			throws Exception {

		try {
			synchronized (_lockerObject) {
				_lockServerSocket = new ServerSocket();
				_lockServerSocket.bind(new InetSocketAddress(Integer
						.parseInt(portNo)));
				PranaLogManager.info(getApplicationinfo());
			}
		} catch (NumberFormatException ex) {
			PranaLogManager.error("Port No is in INCORRECT format", ex);
			throw ex;
		} catch (IOException ex) {
			PranaLogManager.error("Check if already running", ex);
			throw ex;
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}

	}

	public static boolean checkIfStartupCorrectly(String[] args)
			throws Exception {
		try {
			if (args != null && args.length > 0) {
				boolean argVal = Boolean.parseBoolean(args[0]);
				if (argVal)
					return true;
				else
					return false;
			} else
				return false;
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}
}
