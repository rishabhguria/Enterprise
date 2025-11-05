package com.nirvana.Helper;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;
import java.util.Set;
import com.nirvana.TestCases.BaseClass;

import org.apache.log4j.Logger;
import org.apache.poi.xssf.usermodel.XSSFRow;
import org.openqa.selenium.Alert;
import org.openqa.selenium.By;
import org.openqa.selenium.WebElement;


public class WindowHelper extends BaseClass {
	
	public static void switchTo(int index) {
		try {
			List<String> win = new ArrayList<String>(driver.getWindowHandles());
			driver.switchTo().window(win.get(index));
		} catch (IndexOutOfBoundsException e) {
			throw new IndexOutOfBoundsException("Invalid Window Index : " + index);
		}
		
	}
	
	public static void switchToParentWithClose() {
		List<String> win = new ArrayList<String>(driver.getWindowHandles());
		
		for(int i = 1; i < win.size(); i++){
			driver.switchTo().window(win.get(i));
			driver.close();
		}
		driver.switchTo().window(win.get(0));
		
	}
	
	public static void back() {
		driver.navigate().back();
		
	}
	
	public static  void forward() {
		driver.navigate().forward();
		
	}
	
	public static void refresh() {
		driver.navigate().refresh();
		logger.info( " page is refreshed");
		
	}
	
	public static void navigateToPage(String url) {
		driver.navigate().to(url);
		logger.info("Navigated to "+url);
	}
	
	public static void windowMaximize() {
		driver.manage().window().maximize();
	}
	
	public static void NavigateToLatestPage() {

		try {
			 Set<String> windowHandles = driver.getWindowHandles();

		        // Switch to the latest window handle
		        String latestHandle = null;
		        for (String handle : windowHandles) {
		            latestHandle = handle;
		        }
		        driver.switchTo().window(latestHandle);
		}catch(Exception e)
		{
			//ExceptionLog.info(e.toString());
			logger.error(e.toString());
			isPassed=false;
		}
	}
	
	public static void SwitchToIFrame(String element) {

		try {
			String iFrameXpath=XPathDict.get(element);
            WebElement iframeElement = driver.findElement(By.xpath(iFrameXpath));
            driver.switchTo().frame(iframeElement);
            logger.info( " driver switched to Iframe "+ element);
		}catch(Exception e)
		{
			//ExceptionLog.info(e.toString());
			logger.error(e.toString());
			isPassed=false;
		}
	}
	
	public static void SwitchOutIFrame(String element) {

		try {
			driver.switchTo().defaultContent();
			logger.info( " driver switched out of Iframe "+ element);
		}catch(Exception e)
		{
			//ExceptionLog.info(e.toString());
			logger.error(e.toString());
			isPassed=false;
		}
	}

	public static void NavigateToNextPage(String element, String arg) {


		try {
			ClickHelper.CanvasClick("CTC Launch Button");
	        // Switch to the new tab
	        for (String handle : driver.getWindowHandles()) {
	            driver.switchTo().window(handle);
	        }
		}catch(Exception e)
		{
			//ExceptionLog.info(e.toString());
			logger.error(e.toString());
			isPassed=false;
		}

	}
	public static void NavigateUsingBatchFile(String arg) {
		try {
			runBatchFile(arg);
			
		}catch(Exception e)
		{
			//ExceptionLog.info(e.toString());
			logger.error(e.toString());
			isPassed=false;
		}

	}
	
	 public static void runBatchFile(String url) {
	        try {
	            String command = "cmd /c start \"\" \"chrome.exe\" -kiosk \"" + url + "\"";
	            Process process = Runtime.getRuntime().exec(command);
	            process.waitFor();
	        } catch (IOException | InterruptedException e) {
	            e.printStackTrace();
	        }
	    }
	
public static void Navigate(XSSFRow row) {
		
		
		navigateToPage(row.getCell(3).getStringCellValue());
		
		
	}
public static void SwitchToAlert(String element) {

    try {
        // Switch to the alert
        Alert alert = driver.switchTo().alert();

        // Click the OK button on the alert
        alert.accept();
    }catch(Exception e)
    {
        //ExceptionLog.info(e.toString());
        logger.error(e.toString());
        isPassed=false;
    }
}

public static void closeLatestTab() {
    try {
        Set<String> windowHandles = driver.getWindowHandles();

        // If there is more than one tab, close the latest one
        if (windowHandles.size() > 1) {
            ArrayList<String> tabs = new ArrayList<>(windowHandles);
            String latestHandle = tabs.get(tabs.size() - 1);

            // Switch to the latest tab
            driver.switchTo().window(latestHandle);

            // Close the latest tab
            driver.close();

            // Switch back to the original tab (the first one in the list)
            String originalHandle = tabs.get(0);
            driver.switchTo().window(originalHandle);
        }
    } catch (Exception e) {
        // Log the exception
        logger.error(e.toString());
        isPassed = false;
    }
}
	

}
