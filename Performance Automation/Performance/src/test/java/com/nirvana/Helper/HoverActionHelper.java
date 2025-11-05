package com.nirvana.Helper;
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
	WebDriverWait wait = new WebDriverWait(driver, 120);
	wait.until(ExpectedConditions.presenceOfElementLocated(By.xpath(xpath)));
	WebElement ele=getWebElement(xpath);

	sb.moveToElement(ele).click().build().perform();
	logger.info("Hover on "+element);
	}
	catch(Exception e)
	{
		ExceptionLog.info(e.toString());
	}
//	executeScript(ele);

}

}


