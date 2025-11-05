package com.nirvana.Mapping;

import com.nirvana.Helper.*;
import com.nirvana.TestCases.BaseClass;

import java.io.IOException;
import java.util.HashMap;
import java.util.Map;
import java.util.function.Consumer;

public class Mapping3 extends BaseClass {
    private static final Map<String, Consumer<Object[]>> actionMap = new HashMap<>();

    static {
        actionMap.put("NavigateURL", args -> WindowHelper.navigateToPage((String) args[0]));
        actionMap.put("SendKeys", args -> TextBoxHelper.typeInTextBox((String) args[0], (String) args[1]));
        actionMap.put("Click", args -> {
			try {
				ClickHelper.clickButton((String) args[0]);
			} catch (InterruptedException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		});
       
    }

    public static void performActionWithKeyword(String action, Object... args) throws IOException {
        if (actionMap.containsKey(action)) {
		    actionMap.get(action).accept(args);
		} else {
		    logger.error("Unsupported action: " + action);
		}
    }
}
