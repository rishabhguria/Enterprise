package com.nirvana.Helper;

import java.io.IOException;
import java.util.NoSuchElementException;
import com.nirvana.TestCases.BaseClass;
import org.apache.poi.xssf.usermodel.XSSFRow;
import org.openqa.selenium.By;
import org.openqa.selenium.JavascriptExecutor;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.support.ui.ExpectedConditions;
import org.openqa.selenium.support.ui.WebDriverWait;
import org.testng.Assert;

public class CheckTextHelper extends BaseClass{
	public static void WaitforText(String locator,String locatorType,String Value ) throws IOException
	{
	
	
	WebDriverWait wait=new WebDriverWait(driver,6000);
		
	
	if(locatorType.contains("id")){
		wait.until(ExpectedConditions.textToBe(By.id(locator),Value));
		
		return;
	}	
	
	else if(locatorType.contains("name")){
		wait.until(ExpectedConditions.textToBe(By.name(locator),Value));
		
		return;	
	}
	else if(locatorType.contains("xpath")){
		wait.until(ExpectedConditions.textToBe(By.xpath(locator),Value));
		
		return;
	}
	
	else if(locatorType.contains("linkText")){
		wait.until(ExpectedConditions.textToBe(By.linkText(locator),Value));

		return;
	}
	
	else
		throw new NoSuchElementException("Element Not Found : " + locator);
	
	}
	
	
	
	
	public static void ContainsText(String locator,String locatorType,String Value ) throws IOException
	{
	
		WebDriverWait wait=new WebDriverWait(driver,6000);
		
		
		if(locatorType.contains("id")){
			wait.until(ExpectedConditions.textToBePresentInElementLocated(By.id(locator),Value));
		
			return;
		}	
		
		else if(locatorType.contains("name")){
			wait.until(ExpectedConditions.textToBePresentInElementLocated(By.name(locator),Value));
			
			return;	
		}
		else if(locatorType.contains("xpath")){
			wait.until(ExpectedConditions.textToBePresentInElementLocated(By.xpath(locator),Value));
		
			return;
		}
		
		else if(locatorType.contains("linkText")){
			wait.until(ExpectedConditions.textToBePresentInElementLocated(By.linkText(locator),Value));
			
			return;
		}
		
		else
			throw new NoSuchElementException("Element Not Found : " + locator);
		
		}

	
	
	
	public static void GetText(String locator,String locatorType,String Value,String WaitType) throws IOException {
	
	 if(WaitType.contains("TextToBe")) {
		 
		 WaitforText(locator, locatorType, Value);
		 
		 WebElement ele = getElement(locator,locatorType );
			
			
			
			String b = (ele.getText());
			com.nirvana.Helper.HelperClass.PrintConsole(ele.getText());
			
			
		if (b.equalsIgnoreCase(Value))
		{
			com.nirvana.Helper.HelperClass.PrintConsole(ele.getText());
			Assert.assertTrue(true);
			logger.info("StatusText:" +b);
			captureScreen(driver,"Text");


		}
		else
		{
			com.nirvana.Helper.HelperClass.PrintConsole("Fail");
			
			logger.info("test case failed....");
			captureScreen(driver,"Test Failed");
			Assert.assertTrue(false);
		}

	 }
		
	 else if(WaitType.contains("ContainsText")) {
		ContainsText(locator, locatorType, Value);
		WebElement ele = getElement(locator,locatorType );
		
		
		
		String b = (ele.getText());
		com.nirvana.Helper.HelperClass.PrintConsole(ele.getText());
		
		
	if (b.contains(Value))
	{
		com.nirvana.Helper.HelperClass.PrintConsole(ele.getText());
		Assert.assertTrue(true);
		logger.info("StatusText:" +b);
		captureScreen(driver,"Text");


	}
	else
	{
		com.nirvana.Helper.HelperClass.PrintConsole("Fail");
		
		logger.info("test case failed....");
		captureScreen(driver,"Test Failed");
		Assert.assertTrue(false);
	}

		}
		
		
	 }
	 
	

	public static void GetText(XSSFRow row) throws IOException {
		
		
	
		
		GetText(row.getCell(2).getStringCellValue(), row.getCell(1).getStringCellValue(),row.getCell(3).getStringCellValue(), row.getCell(4).getStringCellValue() );
			
			
		}

		
	
	
	
	
	
}
	
	
