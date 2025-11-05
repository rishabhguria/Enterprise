package prana.utility.memory;

import prana.utility.logging.PranaLogManager;

public class MemoryHelper {

	private static int mb = 1024 * 1024;

	private static long fSLEEP_INTERVAL = 100;

	public static double getMaxMemory() throws Exception {
		try {
			putOutTheGarbage();
			Runtime runtime = Runtime.getRuntime();

			return runtime.maxMemory() / mb;
		} catch (Exception e) {
			
			PranaLogManager.error(e.getMessage(), e);
			throw e;
		}
	}

	public static double getFreeMemory() throws Exception {
		try {
			putOutTheGarbage();
			Runtime runtime = Runtime.getRuntime();

			return runtime.freeMemory() / mb;
		} catch (Exception e) {
			
			PranaLogManager.error(e.getMessage(), e);
			throw e;
		}
	}
	
	public static double getUsedMemory() throws Exception {
		try {
			putOutTheGarbage();
			Runtime runtime = Runtime.getRuntime();

			return (runtime.totalMemory() - runtime.freeMemory()) / mb;
		} catch (Exception e) {

			PranaLogManager.error(e.getMessage(), e);
			throw e;
		}
	}


	public static String getMemoryLoggings() throws Exception {
		try {
			Runtime runtime = Runtime.getRuntime();
		    double UsedMemory =(double) ((runtime.totalMemory()-  runtime.freeMemory())/ mb );
		    double UsedMemoryPercentage = (double)Math.ceil(((UsedMemory/ ((double)runtime.maxMemory()/mb)) *100));
		    PranaLogManager.logOnly("\n" +  " Max Heap Memory="+runtime.maxMemory()/ mb + ", Allocated Heap Memory="+runtime.totalMemory()/ mb + ", Used Heap Memory="+ UsedMemory + ", Used Memory% = " + UsedMemoryPercentage);
		    
		   
	return " Used Memory% = " + UsedMemoryPercentage;
	
		} catch (Exception e) {

			PranaLogManager.error(e.getMessage(), e);
			throw e;
		}
	}
	
	
	public static int getUsedHeapMemoryPercentage() throws Exception {
		try {
            putOutTheGarbage();
			Runtime runtime = Runtime.getRuntime();
			double UsedMemory = (double) ((runtime.totalMemory()-  runtime.freeMemory())/ mb );
			return (int) (UsedMemory/(runtime.maxMemory() / mb) * 100);
		} catch (Exception e) {

			PranaLogManager.error(e.getMessage(), e);
			throw e;
		}
	}

	private static void putOutTheGarbage() throws Exception {
		try {
			collectGarbage();
			collectGarbage();
		} catch (Exception e) {

			PranaLogManager.error(e.getMessage(), e);
			throw e;
		}
	}

	private static void collectGarbage() throws Exception {
		try {
			System.gc();
			Thread.currentThread();
			Thread.sleep(fSLEEP_INTERVAL);
			System.runFinalization();
			Thread.currentThread();
			Thread.sleep(fSLEEP_INTERVAL);
		} catch (Exception e) {

			PranaLogManager.error(e.getMessage(), e);
			throw e;
		}

	}
}
