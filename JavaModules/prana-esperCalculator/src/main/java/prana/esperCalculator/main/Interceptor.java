package prana.esperCalculator.main;

import static java.nio.file.StandardWatchEventKinds.ENTRY_CREATE;
import static java.nio.file.StandardWatchEventKinds.ENTRY_DELETE;
import static java.nio.file.StandardWatchEventKinds.ENTRY_MODIFY;
import static java.nio.file.StandardWatchEventKinds.OVERFLOW;

import java.io.File;
import java.io.FileReader;
import java.nio.file.FileSystems;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.nio.file.WatchEvent;
import java.nio.file.WatchKey;
import java.nio.file.WatchService;
import java.util.List;
import com.opencsv.bean.CsvToBeanBuilder;
import prana.esperCalculator.objects.InterceptorData;

import prana.esperCalculator.commonCode.CEPManager;
import prana.esperCalculator.constants.CollectorConstants;
import prana.esperCalculator.constants.ConfigurationConstants;
import prana.utility.configuration.ConfigurationHelper;
import prana.utility.logging.PranaLogManager;

public class Interceptor implements Runnable {

	Path _path;
	WatchService _watcher;
	String _fileName;
	boolean _isFileExist;

	public Interceptor(String directory) throws Exception {
		try {
			File folder = new File(directory);
			String absPath = folder.getAbsolutePath();
			PranaLogManager.info("\nLoading interceptor from " + absPath);
			this._path = Paths.get(absPath);
			this._fileName = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS, ConfigurationConstants.INTERCEPTOR_FILE_NAME);
			_isFileExist = new File(_path.toString() + "/" + _fileName).exists();
			if (_isFileExist) {
				this._watcher = FileSystems.getDefault().newWatchService();
				this._path.register(_watcher, ENTRY_CREATE, ENTRY_DELETE, ENTRY_MODIFY);
				readFile();
			} else {
				PranaLogManager.error("File does not exist, File Name: " + _fileName, new Exception());
			}
		} catch (Exception ex) {
			PranaLogManager.error("Error starting interceptor", ex);
			throw ex;
		}
	}

	@Override
	public void run() {
		try {
			if (_isFileExist) {
				while (true) {
					try {
						// Wait for key to be signaled
						WatchKey key;
						try {
							key = _watcher.take();
						} catch (InterruptedException x) {
							return;
						}
						Thread.sleep(1000);
						List<WatchEvent<?>> events = key.pollEvents();
						for (WatchEvent<?> event : events) {
							WatchEvent.Kind<?> kind = event.kind();
							if (kind == OVERFLOW) {
								continue;
							}
							@SuppressWarnings("unchecked")
							WatchEvent<Path> ev = (WatchEvent<Path>) event;
							Path filename = ev.context();
							if (filename.endsWith(_fileName)) {
								if (ev.kind() == ENTRY_CREATE) {
									PranaLogManager.info("file created: " + filename);
									readFile();
								} else if (ev.kind() == ENTRY_DELETE) {
									PranaLogManager.info("file deleted: " + filename);
									clearCustomSymbolWindow();
								} else if (ev.kind() == ENTRY_MODIFY) {
									PranaLogManager.info("file modified: " + filename);
									readFile();
								}
							}
							boolean valid = key.reset();
							if (!valid) {
								break;
							}
						}
					} catch (Exception ex) {
						PranaLogManager.error(ex, "Error in execution of interceptor");
					}
				}
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex, "Error executing interceptor");
		}
	}
	static int _count =0;
	/**
	 * Reads the file and loads the symbol
	 * 
	 * @param fileName
	 */
	private void readFile() {
		try {
			_count++;
			List<InterceptorData> beans = new CsvToBeanBuilder<InterceptorData>(
					new FileReader(_path.toString() + "/" + _fileName)).withType(InterceptorData.class).build().parse();

			// Sending CustomSymbolData to Esper
			for (InterceptorData data : beans) {
				double askPrice = data.getAskPrice();
				double bidPrice = data.getBidPrice();
				double lowPrice = data.getLowPrice();
				double highPrice = data.getHighPrice();
				double openPrice = data.getOpenPrice();
				double closePrice = data.getClosePrice();
				double lastPrice = data.getLastPrice();
				double selectedFeedPrice = data.getSelectedFeedPrice();
				double markPrice = data.getMarkPrice();
				double delta = data.getDelta();
				double beta5YearMonthly = data.getBeta5YearMonthly();
				int assetId = data.getAssetId();
				double openInterest = data.getOpenInterest();
				double getAvgVolume20Days = data.getAvgVolume20Days();
				String underlyingSymbol = data.getUnderlyingSymbol();
				String conversionMethod = data.getConversionMethod();

				CEPManager.getEPRuntime().getEventService().sendEventObjectArray(new Object[] { data.getSymbol(),
						underlyingSymbol != null ? (underlyingSymbol.isEmpty() ? null : underlyingSymbol) : null, askPrice == 0.0 ? null : askPrice,
						bidPrice == 0.0 ? null : bidPrice, lowPrice == 0.0 ? null : lowPrice,
						highPrice == 0.0 ? null : highPrice, openPrice == 0.0 ? null : openPrice,
						closePrice == 0.0 ? null : closePrice, lastPrice == 0.0 ? null : lastPrice,
						selectedFeedPrice == 0.0 ? null : selectedFeedPrice,
						conversionMethod != null ? (conversionMethod.isEmpty() ? null : conversionMethod) : null, markPrice == 0.0 ? null : markPrice,
						delta == 0.0 ? null : delta, beta5YearMonthly == 0.0 ? null : beta5YearMonthly,
						assetId == 0 ? null : assetId, openInterest == 0.0 ? null : openInterest,
						getAvgVolume20Days == 0.0 ? null : getAvgVolume20Days, data.getSharesOutstanding() },
						"CustomSymbolData");
			}

			if (CEPManager.getEPRuntime() != null && (PendingWhatIfCache.getInstance().isEmpty()
					&& !PendingWhatIfCache.getIsValidatedSymbolDataReceived())) {
				PranaLogManager.logOnly("Sending SymbolData cycle event (Interceptor) with: " + _count);
				CEPManager.getEPRuntime().getEventService().sendEventObjectArray(new Object[] { _count },
						CollectorConstants.SYMBOL_DATA_CYCLE_COMPLETED_EVENT_NAME);
				PranaLogManager.logOnly("SymbolData cycle completed (Interceptor) with: " + _count);
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex, "Failed to read file : " + _fileName);
		}
	}

	/**
	 * Sends an event to esper to clear the custom symbol window
	 */
	private void clearCustomSymbolWindow() {
		try {
			CEPManager.getEPRuntime().getEventService().sendEventObjectArray(new Object[0], "ClearCustomSymbolData");
			PranaLogManager.info("Custom symbol data cleared");
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}
}