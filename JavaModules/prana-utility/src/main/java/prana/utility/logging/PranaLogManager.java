package prana.utility.logging;

import java.io.File;
import java.io.PrintWriter;
import java.io.StringWriter;
import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.HashMap;
import java.util.Iterator;
import java.util.Map;

import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.core.LoggerContext;
import org.fusesource.jansi.AnsiConsole;

import prana.utility.configuration.ConfigurationHelper;

import static org.fusesource.jansi.Ansi.*;
import static org.fusesource.jansi.Ansi.Color.*;

public class PranaLogManager {

	private static HashMap<String,Long> errorTimeElapsed = new HashMap<String,Long>();
	
	public static void initializeLogger(String loggerConfigPath) {
		AnsiConsole.systemInstall();
		File file = new File(loggerConfigPath);
		LoggerContext context = (LoggerContext) LogManager.getContext(false);
		context.setConfigLocation(file.toURI());
		
		PranaLogManager.info("Logger started");
		DateFormat dateFormat = new SimpleDateFormat("yyyy/MM/dd HH:mm:ss.SSS");
		Date date = new Date();
		PranaLogManager.info("1. Start :" + dateFormat.format(date));		
	}

	public static void error(String message, Throwable ex) {

		try {
			System.out.println(ansi().fg(RED));
			LogManager.getLogger("file").error(message, ex);
			LogManager.getLogger("consoleprint").error(message, ex);
			System.out.println(ansi().reset());

		} catch (Exception internalEx) {
			PranaLogManager.info("Error in error logger"+internalEx.getMessage());
			internalEx.printStackTrace();
			// throw internalEx;

		}
	}

	public static void info(String message) {

		try {
			System.out.print(ansi().fg(GREEN));
			LogManager.getLogger("file").info(message);

			LogManager.getLogger("consoleprint").info(message);
			System.out.print(ansi().reset());

		} catch (Exception internalEx) {
			PranaLogManager.error(internalEx);
			internalEx.printStackTrace();
			// throw internalEx;

		}
	}

	public static void debug(String message) {

		try {
			LogManager.getLogger("file").debug(message);

			LogManager.getLogger("consoleprint").debug(message);

		} catch (Exception internalEx) {
			PranaLogManager.error(internalEx);
			internalEx.printStackTrace();
			// throw internalEx;

		}
	}

	public static void warn(String message) {

		try {
			System.out.println(ansi().fg(YELLOW));
			LogManager.getLogger("file").warn(message);
			LogManager.getLogger("consoleprint").warn(message);
			System.out.println(ansi().reset());

		} catch (Exception internalEx) {
			PranaLogManager.error(internalEx);
			internalEx.printStackTrace();
			// throw internalEx;

		}
	}

public static void logOnly(String message) {
		try {
			LogManager.getLogger("file").info(message);
		} catch (Exception internalEx) {
			internalEx.printStackTrace();
			PranaLogManager.error(internalEx);		}
	}

	public static void showOnly(String message) {
		try {
			LogManager.getLogger("consoleprint").info(message);
		} catch (Exception internalEx) {
			internalEx.printStackTrace();
			PranaLogManager.error(internalEx);	
        	}
	}
	
	/*Currently this logger is only used for logging esper events,
	 * with the help of event-reporter appender in log4j.xml of prana-logingTool project.
	 */
	public static void report(String message) {
		try {
			LogManager.getLogger("report").info(message);
		} catch (Exception internalEx) {
			internalEx.printStackTrace();
			PranaLogManager.error(internalEx);
		}
	}
	
	/**
	 * <li>This is the overloaded version of error logger method.</li>
	 * <li>This method sends email notification for errors and logs them to file as well</li>
	 * @param ex Exception
	 * @param sendEmail Permission to send email. If it is 'false' error will only be logged in file.
	 * @param timeOut Time window in seconds. Mail notification for same error will be sent only once in this time interval.
	 */
	public static void error(Throwable ex) {

		try {
			//mail to be sent only if configuration is set to true
			if(ConfigurationHelper.EMAIL_ERROR_NOTIFICATION_PERMISSION)
			{
				StringWriter sw = new StringWriter();
				ex.printStackTrace(new PrintWriter(sw));
				//to log only once in the specified time interval. Time is sent in millis format.
				logOnce(ConfigurationHelper.EMAIL_ERROR_NOTIFICATION_TIMEOUT_SECONDS*1000,sw.toString(), ex,ex.getMessage());
				sw.close();
			}
			//good old error logger
			error(ex.getMessage(),ex);
		} catch (Exception internalEx) {
			internalEx.printStackTrace();
			PranaLogManager.error(internalEx.getMessage(),internalEx);
		}
	}
	
	/**
	 * This is anothher overloaded version which includes a custom message.
	 * @param ex
	 * @param message
	 */
	public static void error(Throwable ex,String message) {

		try {
			//mail to be sent only if configuration is set to true
			if(ConfigurationHelper.EMAIL_ERROR_NOTIFICATION_PERMISSION)
			{
				StringWriter sw = new StringWriter();
				ex.printStackTrace(new PrintWriter(sw));
				//to log only once in the specified time interval. Time is sent in millis format.
				logOnce(ConfigurationHelper.EMAIL_ERROR_NOTIFICATION_TIMEOUT_SECONDS*1000,sw.toString(), ex,message);
				sw.close();
			}
			//good old error logger
			error(message,ex);
		} catch (Exception internalEx) {
			internalEx.printStackTrace();
			PranaLogManager.error(internalEx.getMessage(),internalEx);
		}
	}
	
	/**
	 * <li>This method E-Mails the message and error once in given interval of time.</li>
	 * <li>It stores/updates the last time of mailing of a particular message.</li>
	 * @param timeSec Given Interval ofTime
	 * @param errorStackTrace Stack trace which differentiates every error.
	 * @param ex Exception Object
	 */
	/**
	 * @param timeSec
	 * @param errorStackTrace
	 * @param ex
	 * @param message
	 */
	public static void logOnce(int timeSec, String errorStackTrace, Throwable ex,String message)
	{
		try {
			long currentTime = System.currentTimeMillis();
			
			if(!errorTimeElapsed.containsKey(errorStackTrace))
			{
				errorTimeElapsed.put(errorStackTrace,currentTime);
				LogManager.getLogger("mailError").error(message, ex);
			}
			
			long lastLoggedTime = errorTimeElapsed.get(errorStackTrace);
			
			if(currentTime-lastLoggedTime > timeSec )
			{
				LogManager.getLogger("mailError").error(ex.getMessage(), ex);
				errorTimeElapsed.put(errorStackTrace,currentTime);
			}		
			
			cleanElapsedErrorKeys(timeSec);
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			PranaLogManager.error(e.getMessage(),e);
		}
	}
	
	/**
	 * <li>This function clears the entries and timestamps for
	 * errors which were mailed before the given time interval</li>
	 * @param timeSec in milliseconds
	 */
	public static void cleanElapsedErrorKeys(int timeSec)
	{
		try {
			long currentTime = System.currentTimeMillis();
			
			Iterator<Map.Entry<String, Long>> iterator = errorTimeElapsed.entrySet().iterator();
			while (iterator.hasNext()) 
			{
				 Map.Entry<String, Long> entry = iterator.next();
			     if(currentTime - entry.getValue() > timeSec)
			     {
			          iterator.remove();
			     }
			}
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			PranaLogManager.error(e.getMessage(),e);

		}
	}
}
