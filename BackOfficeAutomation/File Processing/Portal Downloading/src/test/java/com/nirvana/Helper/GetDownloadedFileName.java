package com.nirvana.Helper;

import java.io.IOException;
import com.nirvana.TestCases.BaseClass;
import org.apache.poi.xssf.usermodel.XSSFRow;
import org.openqa.selenium.JavascriptExecutor;

public class GetDownloadedFileName extends BaseClass{
	
	
	public static   void GetReportName(String path) throws IOException, InterruptedException
	{
		driver.get("chrome://downloads");

		
		JavascriptExecutor js1 = (JavascriptExecutor)driver;
		// wait until the file is downloaded
		Long percentage = (long) 0;
		while ( percentage!= 100) {
		    try {
		        percentage = (Long) js1.executeScript("return document.querySelector('downloads-manager').shadowRoot.querySelector('#downloadsList downloads-item').shadowRoot.querySelector('#progress').value");
		        logger.info("File Download Status:" +percentage +'%');
		        //System.out.println(percentage);
		    }catch (Exception e) {
		      // Nothing to do just wait	
		    	logger.error(e);
		    	isPassed=false;
		  }
		  			  
		// get the latest downloaded file name
		String fileName = (String) js1.executeScript("return document.querySelector('downloads-manager').shadowRoot.querySelector('#downloadsList downloads-item').shadowRoot.querySelector('div#content #file-link').text");
	
		
		source = path +fileName;
		Thread.sleep(10000);
		
		}
		
	}
	
public static void GetNameFile(XSSFRow row) throws IOException, InterruptedException {
		
		
	try {
		
		GetReportName(row.getCell(3).getStringCellValue() );
	} catch (Exception e) {
		//ExceptionLog.info(e.toString());
		logger.error(e.toString());
		isPassed = false;
	}
			
		}
	
	

}
