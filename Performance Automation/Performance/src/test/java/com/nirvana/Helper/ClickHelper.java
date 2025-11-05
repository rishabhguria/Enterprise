package com.nirvana.Helper;

import org.apache.poi.xssf.usermodel.XSSFRow;
import org.openqa.selenium.*;
import org.openqa.selenium.interactions.Actions;

import com.nirvana.TestCases.BaseClass;
import com.nirvana.TestCases.ExecuteTestCase_1;

import java.util.List;

public class ClickHelper extends BaseClass {
	
	public static void clickButton(String element) throws InterruptedException {
		try {

			HelperClass helperClass = new HelperClass(driver);
			helperClass.ActionClick(element);
		}
		catch (StaleElementReferenceException ex){
			try{
				HelperClass helperClass = new HelperClass(driver);
				helperClass.ActionClick(element);
			}
			catch (Exception e){
				com.nirvana.Helper.HelperClass.PrintConsole("Not Found element In DOM");
			}
		}
		catch (Exception e){
			com.nirvana.Helper.HelperClass.PrintConsole(e);
		}
		/*try
		{
		String xpath=XPathDict.get(element);
		waitForElement(xpath);
		WebElement ele=getWebElement(xpath);
		 executeScript(ele);
		 logger.info(element +" Clicked");
		}
		catch(Exception e)
		{
			//ExceptionLog.info(e.toString());
			isPassed=false;
		}*/
		
		
	}
	public static void ClickCheckbox(String element){
		try {
			String xpath = XPathDict.get(element);
			waitForElement(xpath);
			WebElement checkbox = driver.findElement(By.xpath(xpath));
			checkbox.click();
			com.nirvana.Helper.HelperClass.PrintConsole("Checkbox clicked successfully");
		}
		catch (Exception e){
			com.nirvana.Helper.HelperClass.PrintConsole(e);
		}

	}
public static void clickDynamicButton(String element,String arg) throws InterruptedException {
		
	try {	
	String xpath=XPathDict.get(element);
		xpath=xpath.replace("arg", arg);
		waitForElement(xpath);
		WebElement ele=getWebElement(xpath);
		 executeScript(ele);
		 HelperClass.PrintConsole(element +" Clicked");
	}
	catch(Exception e)
	{
		//ExceptionLog.info(e.toString());
		HelperClass.PrintConsole(e.toString());
		isPassed=false;
	}
		
		
	}
	
	public static void conditionalClick(String element) throws InterruptedException
	{
		try
		{
			String xpath=XPathDict.get(element);
			waitForElement(xpath);
			WebElement ele=getWebElement(xpath);
			if(!ele.getText().contains("Hide"))
			{	executeScript(ele);
				System.out.println(element+" Clicked");
				HelperClass.PrintConsole(element +" Clicked");
			}
			
			
		}
		
		catch(Exception e)
		{
			//ExceptionLog.info(e.toString());
			HelperClass.PrintConsole(e.toString());
			isPassed=false;
		}
	}
	public static void rightClick(String element) throws  InterruptedException{
		try {
			String xpath = XPathDict.get(element);
			Actions action = new Actions(driver);
			//action.contextClick(elementLocator).perform()
			WebElement element1 = driver.findElement(By.xpath(xpath));


			action.contextClick(element1).perform();
			// patternWithValue = "//*[@IsLegacyIAccessiblePatternAvailable='True']";
		}
		catch (Exception e){
			com.nirvana.Helper.HelperClass.PrintConsole(e);
		}

	}



	public static void CanvasClick(String element) throws InterruptedException
	{
		try {
			String xpath = XPathDict.get(element);
			waitForElement(xpath);
			WebElement ele = getWebElement(xpath);
			Actions actions = new Actions(driver);
			//actions.moveToElement(ele,0,0).moveByOffset(500,100).click().build().perform();


			Dimension canvas_dimensions = ele.getSize();

			int center_x = canvas_dimensions.getWidth() / 2;

			int center_y = canvas_dimensions.getHeight() / 2;

			int x = canvas_dimensions.getWidth() / 23;


			int button_x = (int) ((center_x / 3));
			int button_y = (int) ((center_y / 3));

			JavascriptExecutor js = ((JavascriptExecutor) driver);
			js.executeScript("window.scrollTo(0, document.body.scrollHeight)");
			Thread.sleep(5000);
			//actions.moveToElement(ele,button_x,button_y)
			//.click().build().perform();

			actions.moveToElement(ele, 0, 0).moveByOffset(x, 1).click().build().perform();
			Thread.sleep(5000);
		}
		catch (Exception e){
			com.nirvana.Helper.HelperClass.PrintConsole(e);
		}
		
	}
	

	
	

	}

