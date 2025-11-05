package com.nirvana.Helper;

import org.apache.poi.xssf.usermodel.XSSFRow;
import org.openqa.selenium.By;
import org.openqa.selenium.WebElement;
import com.nirvana.TestCases.BaseClass;

public class CheckBoxHelper extends BaseClass {
	
	public static void clickCheckBox(String Value,String Type) throws InterruptedException {
		
//	waitForElement("","");
	
	if(Type.contains("Excel")) {
		WebElement ele = driver.findElement(By.xpath("//a[contains(text(),'"+Value+"')]/../../td/input[@class='exlCheckBox']"));
		executeScript(ele);
		
	}else if(Type.contains("PDF")) {
		WebElement ele = driver.findElement(By.xpath("//a[contains(text(),'"+Value+"')]/../../td/input[@class='pdfCheckBox']"));
		executeScript(ele);
		
	}
	
	else if(Type.contains("All Excel")) {
		WebElement ele = driver.findElement(By.xpath("//span[contains(text(),'All PDF')]/../../td/input[@id='chkAllExcel']"));
		executeScript(ele);
			
		}
	
	else if(Type.contains("All PDF")) {
		WebElement ele = driver.findElement(By.xpath("//span[contains(text(),'All Excel')]/../../td/input[@id='chkAllPdf']"));
		executeScript(ele);
		
	}
	
	else if(Type.contains("Middleware")) {
		
		WebElement ele = driver.findElement(By.xpath("//input[@type='checkbox' and @id='chkMiddleware']"));
		executeScript(ele);
	}
	
	
	else if(Type.contains("Touch Step")) {

		WebElement ele = driver.findElement(By.xpath("//span[contains(text(),'Touch')]/../input[@type='checkbox' and @id='chkTouchStep']"));
		executeScript(ele);
		
		
		
	}
	
	else if(Type.contains("Cash")) {
		
		WebElement ele = driver.findElement(By.xpath("//input[@type='checkbox' and @id='chkCash']"));
		executeScript(ele);
		
		
	}
	
	else if(Type.contains("Account")){
		WebElement ele = driver.findElement(By.xpath("//td[contains(text(),'"+Value+"')]/preceding-sibling::*[1][self::input]"));
		executeScript(ele);
		//td[contains(text(),'DCM MM Fund Series D')]/preceding-sibling::*[1][self::input]	
		
		
	}
	
	else if(Type.contains("All Accounts")) {
		WebElement ele = driver.findElement(By.xpath("//th[contains(text(),'All')]/preceding-sibling::td/*[1][self::input]"));
		executeScript(ele);
	}
	
	
	else {
		com.nirvana.Helper.HelperClass.PrintConsole("Reports Not Selected");
	}
		
	}
	
	public static boolean isChecked(String locator,String locatorType) {
		WebElement ele = getElement(locator, locatorType);
		return ele.isSelected();
		
	}

	public static void Checkbox(XSSFRow row) throws InterruptedException {
		
		
		clickCheckBox(row.getCell(6).getStringCellValue(),row.getCell(5).getStringCellValue());
	
}
}