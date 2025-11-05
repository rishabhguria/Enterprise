package prana.esperCalculator.esperCEP;

import com.espertech.esper.common.client.hook.exception.ExceptionHandler;
import com.espertech.esper.common.client.hook.exception.ExceptionHandlerFactory;
import com.espertech.esper.common.client.hook.exception.ExceptionHandlerFactoryContext;

public class EsperExceptionHandlerFactory implements ExceptionHandlerFactory {
	@Override
	public ExceptionHandler getHandler(ExceptionHandlerFactoryContext context) {
		return EsperStatementExceptionHandler.getInstance();
	}
}
