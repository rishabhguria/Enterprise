package com.nirvana.Helper;

import org.apache.poi.xssf.usermodel.XSSFRow;
import org.openqa.selenium.By;
import org.openqa.selenium.Keys;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.interactions.Actions;
import org.openqa.selenium.interactions.Keyboard;
import org.openqa.selenium.support.ui.ExpectedConditions;
import org.openqa.selenium.support.ui.Select;
import org.openqa.selenium.support.ui.WebDriverWait;

import com.nirvana.TestCases.BaseClass;
public class TextBoxHelper extends BaseClass {

	public static void typeInTextBox(String element,String arg) {
		String xpath=XPathDict.get(element);
		waitForElement(xpath);

		WebElement ele=getWebElement(xpath);
			Backspace(driver, ele);
		HelperClass helperClass = new HelperClass(driver);
		helperClass.ActionSendKeys(element,arg);
		/*try {

		String xpath=XPathDict.get(element);
		waitForElement(xpath);

		WebElement ele=getWebElement(xpath);
			Backspace(driver, ele);
		ele.clear();
		ele.click();
		ele.sendKeys(arg);
		logger.info("Typed "+arg+" in "+element);
		ele.sendKeys(Keys.ENTER);
		}
		catch(Exception e)
		{
			//ExceptionLog.info(e.toString());
			logger.error(e.toString());
			isPassed=false;
		}*/
	
	}
	private static void Backspace(WebDriver driver, WebElement element) {
		Actions action = new Actions(driver);
		for (int j = 0; j < 10; j++) {
			action.click(element).sendKeys(Keys.DELETE).perform();
		}
		for(int i = 0; i<10; i++){
			action.click(element).sendKeys(Keys.BACK_SPACE).perform();
		}
	}
	public static void clear(String locator,String locatorType) {
		WebElement ele = getElement(locator, locatorType);
		ele.clear();
		
	}
	
	public static void SelectDropDown(String element,String text)
	{
		try
		{
		String xpath=XPathDict.get(element);
		waitForElement(xpath);	
		Select s = new Select(driver.findElement(By.xpath(xpath)));
		s.selectByVisibleText(text);
		logger.info("Selected "+text+" from DropDown "+element);
		waitForElement(xpath);
		}
		catch(Exception e)
		{
			//ExceptionLog.info(e.toString());
			logger.error(e.toString());
			isPassed=false;
		}
	}
	

	public static void WaitForText(String element,String text)
	{
		try
		{
		String xpath=XPathDict.get(element);
		WebDriverWait wait=new WebDriverWait(driver,600);
		wait.until(ExpectedConditions.textToBePresentInElementLocated(By.xpath(xpath), text));
		}
		catch(Exception e)
		{
			//ExceptionLog.info(e.toString());
			logger.error(e.toString());
			isPassed=false;
		}
		
	}
	
	
	public static void WaitForCompleteText(String element,String text)
	{
		try
		{
		String xpath=XPathDict.get(element);
		WebDriverWait wait=new WebDriverWait(driver,600);
		wait.until(ExpectedConditions.textToBe(By.xpath(xpath), text));
		}
		catch(Exception e)
		{
			//ExceptionLog.info(e.toString());
			logger.error(e.toString());
			isPassed=false;
		}
		
	}
	//public static void typeUsingKeyword(XSSFRow row) {
		
	
		//typeInTextBox(row.getCell(2).getStringCellValue(),row.getCell(3).getStringCellValue(), row.getCell(1).getStringCellValue());
		
		
	//}
	
	
}

