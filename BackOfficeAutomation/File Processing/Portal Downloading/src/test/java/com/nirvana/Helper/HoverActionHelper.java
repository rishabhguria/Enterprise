package com.nirvana.Helper;
import java.time.Duration;

import org.apache.poi.xssf.usermodel.XSSFRow;
import org.openqa.selenium.By;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.interactions.Actions;
import org.openqa.selenium.support.ui.ExpectedConditions;
import org.openqa.selenium.support.ui.WebDriverWait;

import com.nirvana.TestCases.BaseClass;
public class HoverActionHelper extends BaseClass{

	public static void Hover(String element) {
	try {
		Actions sb=new Actions(driver);
	String xpath=XPathDict.get(element);
	WebDriverWait wait = new WebDriverWait(driver, 10);
	wait.until(ExpectedConditions.presenceOfElementLocated(By.xpath(xpath)));
	WebElement ele=getWebElement(xpath);

	sb.moveToElement(ele).click().build().perform();
	logger.info("Hover on "+element);
	}
	catch(Exception e)
	{
		//ExceptionLog.info(e.toString());
		logger.error(e.toString());
	}
//	executeScript(ele);

}
	
	public static void HoverOnLink(String element) {
		try {
			Thread.sleep(1000);
			Actions sb=new Actions(driver);
		String xpath=XPathDict.get(element);
		WebDriverWait wait = new WebDriverWait(driver, 10);
		wait.until(ExpectedConditions.presenceOfElementLocated(By.xpath(xpath)));
		WebElement ele=getWebElement(xpath);

		sb.moveToElement(ele).perform();
		logger.info("Hover on "+element);
		}
		catch(Exception e)
		{
			//ExceptionLog.info(e.toString());
			logger.error(e.toString());
		}

	}

}


