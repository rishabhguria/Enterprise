package prana.basketComplianceService.customRule;

import java.util.ArrayList;

import prana.esperCalculator.commonCode.CEPManager;
import prana.utility.logging.PranaLogManager;

class RuleDeploymentManager {
	
	static void generateAndDeployCustomRuleEom(ArrayList<String> preTradeEomEvent) {
		try {

			PranaLogManager.info("Generating custom rule EOM");

			StringBuilder sbEom = new StringBuilder();
			sbEom.append("insert into WhatIfCustomRuleEndMessage\n");
			sbEom.append("\tselect distinct \n\t\tD.basketId as basketId,'EomBasket' as compressionLevel \n\tfrom\n");
			sbEom.append("pattern[every\n\t(D = RowCalculationBaseWindowWhatIfModified ");

			if (preTradeEomEvent.size() > 0)
				sbEom.append("->(\n");

			for (int enu = 0; enu < preTradeEomEvent.size(); enu++) {
				String eventName = preTradeEomEvent.get(enu);
				if (eventName != null && eventName.compareTo("") != 0) {
					sbEom.append(eventName);
					sbEom.append("(basketId = D.basketId)\n");
					if (enu < preTradeEomEvent.size() - 1)
						sbEom.append("\tand ");
					else
						sbEom.append(")");
				}
			}

			sbEom.append("\t)]\ngroup by D.basketId");
			String eom = sbEom.toString();
			PranaLogManager.info("Custom rule EOM genereted. Generated EOM is:\n" + eom);
			CEPManager.createEPL(eom, "WhatIfCustomRuleEndMessageEOM");
			PranaLogManager.info("Custom rule EOM deployed");
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}

	}
}