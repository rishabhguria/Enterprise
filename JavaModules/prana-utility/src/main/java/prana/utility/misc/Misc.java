package prana.utility.misc;

import java.text.DecimalFormat;
import java.text.NumberFormat;
import java.util.Scanner;

import prana.utility.logging.PranaLogManager;

public class Misc {

	public static double formatDecimalTo(double value, int noOfPlaces) {
		try {
			double divMul = Math.pow(10, noOfPlaces);
			return Math.round(value * divMul) / divMul;
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}

	/**
	 * Extracting scientific number from string parameter and converting scientific number to double
	 * @param parameters String parameter name
	 * @return formatted string
	 */
	public static String formatStringParameters(String parameters) {
		try {
			NumberFormat fm1 = new DecimalFormat(",##0.00##");
			// String str = "Name of value 2.111E6 Next Name= 444.4 symbol MSFT";
			StringBuilder parameterBuilder = new StringBuilder();
			Scanner scanner = new Scanner(parameters);

			while (scanner.hasNext()) {
				if (scanner.hasNextDouble() && !scanner.hasNextInt()) {
					parameterBuilder.append(fm1.format(scanner.nextDouble()));
				} else {
					parameterBuilder.append(scanner.next() + " ");
				}
			}		
			scanner.close();
			return parameterBuilder.toString();
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}

}
