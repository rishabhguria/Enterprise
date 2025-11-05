package prana.esperCalculator.main;

import java.util.Date;

import prana.esperCalculator.constants.ConfigurationConstants;
import prana.esperCalculator.commonCode.CEPManager;
import prana.utility.configuration.ConfigurationHelper;
import prana.utility.logging.PranaLogManager;
import prana.utility.memory.MemoryHelper;
import prana.utility.misc.Misc;

public class DivisorTimerManager implements Runnable {
	private int memoryCheckInterval;
	private int memoryCheckCounter = 0;
	private double timerIncrement;
	private double usedMemoryPercentage;
	private double percentageIncrement;
	private int maxIncrementLimit;
	private int timeToPercentageRatio;
	private int timerCount = 0;
	private int totalCount = 0;

	public DivisorTimerManager() {
		try {
			memoryCheckInterval = Integer.parseInt(ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_DIVISOR_MEMORY_MONITOR,
					ConfigurationConstants.KEY_DIVISOR_MEMORY_MONITOR_MEMORY_CHECK_INTERVAL)) * 1000;
			timerIncrement = Double.parseDouble(ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_DIVISOR_MEMORY_MONITOR,
					ConfigurationConstants.KEY_DIVISOR_MEMORY_MONITOR_TIMER_INCREMENT));
			usedMemoryPercentage = Double.parseDouble(ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_DIVISOR_MEMORY_MONITOR,
					ConfigurationConstants.KEY_DIVISOR_MEMORY_MONITOR_USED_MEMORY_PERCENTAGE));
			percentageIncrement = Double.parseDouble(ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_DIVISOR_MEMORY_MONITOR,
					ConfigurationConstants.KEY_DIVISOR_MEMORY_MONITOR_PERCENTAGE_INCREMENT)) / 100;
			maxIncrementLimit = Integer.parseInt(ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_DIVISOR_MEMORY_MONITOR,
					ConfigurationConstants.KEY_DIVISOR_MEMORY_MONITOR_MAX_INCREMENT_LIMIT));
			timeToPercentageRatio = Integer.parseInt(ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_DIVISOR_MEMORY_MONITOR,
					ConfigurationConstants.KEY_DIVISOR_MEMORY_MONITOR_TIME_TO_PERCENTAGE_RATIO));
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	@Override
	public void run() {
		try {
			double heapMaxSize = MemoryHelper.getMaxMemory();
			PranaLogManager.info("Heap Size: " + heapMaxSize);
			boolean isLimitExceed = false;
			while (!isLimitExceed) {
				Thread.sleep(memoryCheckInterval);
			
                boolean isMemoryExceed = checkIfMemoryExceed(MemoryHelper.getUsedHeapMemoryPercentage());
				
				PranaLogManager.info(MemoryHelper.getMemoryLoggings());
					
			        if (isMemoryExceed) {
					resetMemoryCheckInterval();
					isLimitExceed = updateTimerOrPercentageVariable();
				} else {
					increaseMemoryCheckInterval();
				}
			}
		} catch (Exception e) {
			PranaLogManager.error(e);
		}
	}

	private void increaseMemoryCheckInterval() {
		memoryCheckCounter++;
		if (memoryCheckCounter > 5 && memoryCheckInterval < 300000) {
			memoryCheckInterval += 20000;
		}
	}

	private void resetMemoryCheckInterval() {

		try {
			memoryCheckCounter = 0;
			memoryCheckInterval = Integer.parseInt(ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_DIVISOR_MEMORY_MONITOR,
					ConfigurationConstants.KEY_DIVISOR_MEMORY_MONITOR_MEMORY_CHECK_INTERVAL)) * 1000;
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	private boolean updateTimerOrPercentageVariable() {
		try {
			if (totalCount <= maxIncrementLimit) {
				timerCount++;
				if (timerCount <= timeToPercentageRatio) {
					double timerChange = (double) CEPManager.getVariableValue("TimerChange");

					CEPManager.setVariableValue("TimerChange", timerChange + timerIncrement);

					PranaLogManager.info("Timer value increased from " + timerChange + " to "
							+ (timerChange + timerIncrement) + " @ " + new Date());
				} else {
					timerCount = 0;
					double percentageChange = (double) CEPManager.getVariableValue("PercentageChange");

					CEPManager.setVariableValue("PercentageChange",
							Misc.formatDecimalTo((percentageChange + percentageIncrement), 4));

					PranaLogManager.info("Percentage value increased from " + percentageChange * 100 + " to "
							+ Misc.formatDecimalTo((percentageChange + percentageIncrement) * 100, 4) + " @ "
							+ new Date());
				}
				totalCount += 1;
				return false;
			} else {
				PranaLogManager.info("Reached Maximum limit. Please increase heap size");
				return true;
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return true;
		}
	}

	private boolean checkIfMemoryExceed(int usedHeapPercentage) {
		try {
			
			if (usedHeapPercentage >= (usedMemoryPercentage))
				return true;
			else
				return false;
		} catch (Exception e) {
			PranaLogManager.error(e);
			return false;
		}
	}
}
