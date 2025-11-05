package prana.esperCalculator.esperCEP;

import com.espertech.esper.common.client.hook.exception.ExceptionHandler;
import com.espertech.esper.common.client.hook.exception.ExceptionHandlerContext;

import prana.utility.logging.PranaLogManager;

public class EsperStatementExceptionHandler implements ExceptionHandler {

	private static EsperStatementExceptionHandler _esperExceptionHandler;

	public static EsperStatementExceptionHandler getInstance() {
		if (_esperExceptionHandler == null)
			_esperExceptionHandler = new EsperStatementExceptionHandler();
		return _esperExceptionHandler;
	}

	private EsperStatementExceptionHandler() {
	}

	@Override
	public void handle(ExceptionHandlerContext context) {
		PranaLogManager.info("Statement:\n" + context.getEpl() + " has errors");
		PranaLogManager.error(context.getThrowable());
	}

}
