package com.nirvana.Helper;

import org.openqa.selenium.JavascriptExecutor;
import org.openqa.selenium.Keys;
import org.openqa.selenium.NoAlertPresentException;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.concurrent.TimeUnit;
import java.util.regex.Matcher;
import java.util.regex.Pattern;
import java.util.stream.Collectors;
import java.text.DateFormat;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.time.DayOfWeek;
import java.time.Duration;
import java.time.LocalDate;
import java.time.format.DateTimeFormatter;
import java.time.temporal.ChronoUnit;
import java.util.Date;

import org.apache.poi.xssf.usermodel.XSSFRow;
import org.openqa.selenium.By;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.support.ui.ExpectedConditions;
import org.openqa.selenium.support.ui.Select;
import org.openqa.selenium.support.ui.WebDriverWait;
import org.openqa.selenium.TimeoutException;
import com.nirvana.TestCases.BaseClass;
import com.nirvana.Utilities.XLUtils;

import org.openqa.selenium.StaleElementReferenceException;
import org.openqa.selenium.NoSuchElementException;
import org.openqa.selenium.chrome.ChromeDriver;
import org.openqa.selenium.interactions.Actions;


public class TextBoxHelper extends BaseClass {

	public static int mailNo=1;
	public static  Map <String,String> groupings=new HashMap <String,String>();
	public static  Map <String,String> eleType=new HashMap <String,String>();
	
	private static boolean isAlertPresent() {
	    try {
	        driver.switchTo().alert();
	        return true;
	    } catch (NoAlertPresentException e) {
	        return false;
	    }
	}

	
	public static void typeInTextBox(String element,String arg) {
		
		try {
		String xpath=XPathDict.get(element);
		WebDriverWait wait = new WebDriverWait(driver, 10);
		wait.until(ExpectedConditions.visibilityOfElementLocated(By.xpath(xpath)));
		WebElement ele = driver.findElement(By.xpath(xpath));
		ele.click();
		ele.clear();
		ele.click();
		ele.sendKeys(arg);
		logger.info("Typed "+arg+" in "+element);
		}
		catch(Exception e)
		{
			//ExceptionLog.info(e.toString());
			logger.error(e.toString());
			isPassed=false;
		}
	
	}
	
public static void typeInTextBoxWithoutClear(String element,String arg) {
		
		try {
		String xpath=XPathDict.get(element);
		WebDriverWait wait = new WebDriverWait(driver, 10);
		wait.until(ExpectedConditions.visibilityOfElementLocated(By.xpath(xpath)));
		WebElement ele = driver.findElement(By.xpath(xpath));
		//ele.clear();
		
		ele.sendKeys(arg);
		ele.click();
		logger.info("Typed "+arg+" in "+element);
		}
		catch(Exception e)
		{
			//ExceptionLog.info(e.toString());
			logger.error(e.toString());
			isPassed=false;
		}
	
	}
	
public static void TypeDateInGs(String element,String arg) {
		
		try {
			arg=replaceDateTimePlaceholders(arg);
		String xpath=XPathDict.get(element);
		WebDriverWait wait = new WebDriverWait(driver, 10);
		wait.until(ExpectedConditions.visibilityOfElementLocated(By.xpath(xpath)));
		WebElement ele = driver.findElement(By.xpath(xpath));
		ele.click();
		ele.sendKeys(arg);
		logger.info("Typed "+arg+" in "+element);
		}
		catch(Exception e)
		{
			//ExceptionLog.info(e.toString());
			logger.error(e.toString());
			isPassed=false;
		}
	
	}


public static void TypeDateInGsWithClear(String element,String arg) {
	
	try {
	arg=replaceDateTimePlaceholders(arg);
	String xpath=XPathDict.get(element);
	WebDriverWait wait = new WebDriverWait(driver, 10);
	wait.until(ExpectedConditions.visibilityOfElementLocated(By.xpath(xpath)));
	WebElement ele = driver.findElement(By.xpath(xpath));
	ele.click();
	ele.clear();
	ele.click();
	ele.sendKeys(arg);
	logger.info("Typed "+arg+" in "+element);
	}
	catch(Exception e)
	{
		//ExceptionLog.info(e.toString());
		logger.error(e.toString());
		isPassed=false;
	}

}

public static void TypeDateInGsWithClear_V2(String element,String arg) {
	
	try {
	arg=replaceDateTimePlaceholders(arg);
	String xpath=XPathDict.get(element);
	WebDriverWait wait = new WebDriverWait(driver, 10);
	wait.until(ExpectedConditions.visibilityOfElementLocated(By.xpath(xpath)));
	WebElement ele = driver.findElement(By.xpath(xpath));
	ele.clear();
	ele.sendKeys(arg);
	logger.info("Typed "+arg+" in "+element);
	}
	catch(Exception e)
	{
		//ExceptionLog.info(e.toString());
		logger.error(e.toString());
		isPassed=false;
	}

}

public static void TypeDateWithClear_CITI(String element,String arg) {
    
    try {
    	String arr[]=arg.split("#");
    	arg=replaceDateTimePlaceholders(arg);
    	if(arr.length>1)
    	{
    		if(arr[1].equals("1 Week Prior"))
    		{
    			arg=extractAndSubtract7Days(arg);
    		}else if(arr[1].equals("1 Week After"))
    		{
    			arg=extractAndAdd7Days(arg);
    		}else if(arr[1].equals("1 Month Prior")) {
    			arg=extractAndSubtract1Month(arg);
    		}else if(arr[1].equals("1 Month After")) {
    			arg=extractAndAdd1Month(arg);
    		}
    	}
    String xpath=XPathDict.get(element);
    WebDriverWait wait = new WebDriverWait(driver, 10);
    wait.until(ExpectedConditions.visibilityOfElementLocated(By.xpath(xpath)));
    WebElement ele = driver.findElement(By.xpath(xpath));
    ele.click();
    JavascriptExecutor js = (JavascriptExecutor) driver;
    js.executeScript("arguments[0].value = '';", ele);

    ele.sendKeys(arg);
    ele.sendKeys(Keys.ENTER);
    logger.info("Typed "+arg+" in "+element);
    }
    catch(Exception e)
    {
        //ExceptionLog.info(e.toString());
        logger.error(e.toString());
        isPassed=false;
    }

}

	
public static void TypeDate(String element,String arg) {
		
		try {
			//String stringValue = String.valueOf(arg);
			arg=replaceDateTimePlaceholders(arg);
		String xpath=XPathDict.get(element);
		 DateFormat inputFormat = new SimpleDateFormat("MM/dd/yyyy");
         Date date = inputFormat.parse(arg);
         
         // Format the date as "MM/DD/YYYY"
         DateFormat outputFormat = new SimpleDateFormat("MM/dd/yyyy");
         String formattedDate = outputFormat.format(date);
		driver.manage().timeouts().implicitlyWait(10, TimeUnit.SECONDS);
		WebElement calendarInput = driver.findElement(By.xpath(xpath));
		calendarInput.click();
		calendarInput.clear();
		((JavascriptExecutor) driver).executeScript("arguments[0].value = arguments[1]", calendarInput, formattedDate);
		calendarInput.click();
		logger.info("Typed "+arg+" in "+element);
		}
		catch(Exception e)
		{
			//ExceptionLog.info(e.toString());
			logger.error(e.toString());
			isPassed=false;
		}
	
	}

	
public static void Scrollup() {
		try {
			 JavascriptExecutor js = (JavascriptExecutor) driver;
			 new WebDriverWait(driver, 10).until(
	                    webDriver -> ((JavascriptExecutor) webDriver).executeScript("return document.readyState").equals("complete"));

	            // Scroll up to the top of the page
	            js.executeScript("window.scrollTo(0, 0);");
		}
		catch(Exception e)
		{
			//ExceptionLog.info(e.toString());
			logger.error(e.toString());
			isPassed=false;
		}
	
	}

public static void ScrollDown() {
    try {
        JavascriptExecutor js = (JavascriptExecutor) driver;
        new WebDriverWait(driver, 10).until(
                webDriver -> ((JavascriptExecutor) webDriver).executeScript("return document.readyState").equals("complete"));

        // Scroll down to the bottom of the page
        js.executeScript("window.scrollTo(0, document.body.scrollHeight);");
    } catch (Exception e) {
        // Log the exception
        logger.error(e.toString());
        // Set a flag indicating that the scrolling failed
        isPassed = false;
    }
}
	public static void clear(String locator,String locatorType) {
		WebElement ele = getElement(locator, locatorType);
		ele.clear();
		
	}
	
//	Code for Sucbscription by Kislay
	
	public static void selectOptionByText( String optionText) {
	    try {
	        WebElement elementToClick = driver.findElement(By.cssSelector("b.caret"));
	        ((JavascriptExecutor) driver).executeScript("arguments[0].click();", elementToClick);
	     
	        logger.info("clicked dropdown Select Email group  " + optionText);

	    } catch (Exception e) {
	        logger.error(e.toString());
	        isPassed = false;
	    }
	}
	
	public static void selectParametersByText( String optionText,int rowsCount,String reportName, String val,int paramNo ,int i, int j) {
	    try {
	    	String tempVal=val.replaceAll("\\s", "").toLowerCase();
	    	for(int k=2;k<=rowsCount;k++)
	    	{
	    		String temp = "//*[@id=\""+reportName+"_"+paramNo+"\"]/table[2]/tbody/tr["+ i + "]/td["+j+"]/div/ul/li["+k+"]/a/label";
				WebElement element = driver.findElement(By.xpath(temp));
			String pName=	element.getText();
			pName = pName.replaceAll("\\s", "").toLowerCase();
			if(pName.equals(tempVal))
			{
				String temp2="//*[@id=\""+reportName+"_"+paramNo+"\"]/table[2]/tbody/tr["+ i + "]/td["+j+"]/div/ul/li["+k+"]/a/label/input";
				XPathDict.put(val, temp2);
	    		ClickHelper.clickButton(val);
			}
	    	}

	    } catch (Exception e) {
	        logger.error(e.toString());
	        isPassed = false;
	    }
	}
	
	public static void UnselectDefaultParameters( String className, String xpath) {
	    try {
	    	String str="selectAllcheckboxSubsParam";
	    	if(className.contains("active"))
	    	{
	    		XPathDict.put(str, xpath);
	    		ClickHelper.clickButton(str);
	    	}
	    	else {
	    		WebElement element = driver.findElement(By.xpath(xpath));

	        // Perform a double click using Actions class
	        Actions actions = new Actions(driver);
	        actions.doubleClick(element).build().perform();
	    	}

	    } catch (Exception e) {
	        logger.error(e.toString());
	        isPassed = false;
	    }
	}
	
	public static void selectMailTextBox( String optionText,int rowNo) {
	    try {
	    	mailNo++;
	    	String xpath = "//*[@id=\"tblEmailSettings\"]/tbody/tr[" + mailNo + "]/td/input[1]";
	    	waitForElement(xpath);
			WebDriverWait wait = new WebDriverWait(driver, 10);
			wait.until(ExpectedConditions.visibilityOfElementLocated(By.xpath(xpath)));
			WebElement ele = driver.findElement(By.xpath(xpath));
			
			ele.sendKeys(optionText);
			logger.info("Typed "+optionText);
			
	    } catch (Exception e) {
	        logger.error(e.toString());
	        isPassed = false;
	    }
	}
	
	public static void editParameters(String reportNameWithSpace,String reportName,String newReportName, int paramNo) {
	    try {
	    	String argForFileFormat="";
	    	String ParametersList = ParamList.get(newReportName);
	    	String[] arr = ParametersList.split("#");
	    	String paramName="";
	    	String paramValue="";
	    	String ElementType="";
	    	for(int i=0;i<arr.length;i++)
	    	{
	    		String []temp1;
	    		String []arg =arr[i].split("\\^");
	    		if(!arg[0].contains("Corp."))
	    		{
	    			temp1=arg[0].split("\\.(?=[^.]+)");
	    		}
	    		else {
	    			temp1 =arg[0].split("\\.(?=\\S)");
	    		}
	    		paramName=temp1[0];
	    		ElementType=temp1[1];
	    		paramValue = arg[1];
	    		groupings.put(paramName, paramValue);
	    		eleType.put(paramName, ElementType);
	    	}
	    	WebDriverWait wait = new WebDriverWait(driver, 10);
		        List<WebElement> rows = wait.until(ExpectedConditions.visibilityOfAllElementsLocatedBy(By.xpath("//*[@id=\""+reportName+"_"+paramNo+"\"]/table[2]/tbody/tr")));
		        int row =rows.size();
		        int count=1;
		        for(int i=1;i<=row;i++)
		        {
		        	List<WebElement> cols = wait.until(ExpectedConditions.visibilityOfAllElementsLocatedBy(By.xpath("//*[@id=\""+reportName+"_"+paramNo+"\"]/table[2]/tbody/tr["+ i + "]/td")));
		        	int col = cols.size();
		        	int j=1;
		        	while(j<=col)
		        	{
		        		String pName="";
		        		if(i==1)
		        		{
		        			String temp = "//*[@id=\""+reportName+"_"+paramNo+"\"]/table[2]/tbody/tr["+ i + "]/td["+j+"]";
	        				WebElement element = driver.findElement(By.xpath(temp));
	        				 pName=	element.getText();
		        		}
		        		else
		        		{
		        			String temp = "//*[@id=\""+reportName+"_"+paramNo+"\"]/table[2]/tbody/tr["+ i + "]/td["+j+"]/span";
	        				WebElement element = driver.findElement(By.xpath(temp));
	        				pName =element.getText();
		        		}
		        		
		        			if(pName.equals("Period"))
		        			{
		        				if(groupings.containsKey(pName))
		        				{
		        					String period=groupings.get(pName);
			        				String xpathPeriod="//*[@id=\"Period_ctl_"+reportName+"_"+paramNo+"-ctl_1\"]";
			        				XPathDict.put(period,xpathPeriod);
			        				SelectDropDown(period,period);
		        				}
		        				
		        				j+=2;
		        				continue;
		        			}else if(pName.equals("File Format"))
		        			{
		        				
		        				if(groupings.containsKey(pName))
		        				{
		        					String format=groupings.get(pName);
		        					if(format.equals("Excel"))
		        					{
		        						argForFileFormat=reportNameWithSpace+"_"+format;
			        					SelectFileFormat(argForFileFormat,paramNo);
		        					}
		        					
		        				}
		        				
		        				j+=2;
		        				continue;
		        			}
		        			int s =groupings.size();
		        			if(groupings.containsKey(pName))
		        			{
		        				j++;
		        				String pVal= groupings.get(pName);
		        				if(pVal.equals("Default"))
		        					{
		        						j++;
		        						count++;	
		        						continue;
		        					}
		        				else if(pVal.equals("True")|| pVal.equals("False"))
		        				{
		        					//count++;
		        					
		        					String defaultCheckboxTF ="//*[@id=\""+reportName+"_"+paramNo+"\"]/table[2]/tbody/tr["+ i + "]/td["+j+"]/input[3]";
		        					XPathDict.put(pName, defaultCheckboxTF);
		        					ClickHelper.clickButton(pName);
		        					if(pVal.equals("True"))
		        					{
		        						String strTemp ="//*[@id=\""+reportName+"_"+paramNo+"\"]/table[2]/tbody/tr["+ i + "]/td["+j+"]/input[1]";
			        					XPathDict.put(pName, strTemp);
			        					ClickHelper.clickButton(pName);
		        					}
		        					else {
		        						String strTemp ="//*[@id=\""+reportName+"_"+paramNo+"\"]/table[2]/tbody/tr["+ i + "]/td["+j+"]/input[2]";
			        					XPathDict.put(pName, strTemp);
			        					ClickHelper.clickButton(pName);
		        					}
		        					j++;
		        				}
		        				else
		        				{
		        					
		        					String [] valList = pVal.split(",");
		        					if(!pName.equals("Accounts"))
		        					{
		        						
//		        						boolean checked=false;
		        						String defaultCheckbox1 ="//*[@id=\""+reportName+"_"+paramNo+"\"]/table[2]/tbody/tr["+ i + "]/td["+j+"]/input";
		        						
//		        						WebElement element =driver.findElement(By.xpath(defaultCheckbox1));
//		        						java.util.List<WebElement> attributes = element.findElements(By.xpath(".//*"));
//		        						for (WebElement attribute : attributes) {
//		        				            String attributeName = attribute.getTagName();
//		        				            if(attributeName.equals("disabled"))
//		        				            	checked=true;
//		        				        }
		        						
		        							XPathDict.put(pName, defaultCheckbox1);
				        					ClickHelper.clickButton(pName);
		        						
		        						
		        					}
		        					
		        					String type= eleType.get(pName);
		        					if(type.equals("sd"))
		        					{
		        						String xpathTemp= "//*[@id=\"ddl_"+reportName+"_"+paramNo+"_"+count+"\"]";
		        						XPathDict.put(valList[0], xpathTemp);
		        						SelectDropDown(valList[0],valList[0]);
		        						
		        					}
		        					else
		        					{
		        						String t1 ="//*[@id=\""+reportName+"_"+paramNo+"\"]/table[2]/tbody/tr["+ i + "]/td["+j+"]/div/button";
			        					//XPathDict.put(pName, t1);
		        						waitForElement(t1);
			        					 WebElement elem = wait.until(ExpectedConditions.elementToBeClickable(By.xpath(t1)));

			        		                // Evaluate JavaScript click
			        		                JavascriptExecutor jsExecutor2 = (JavascriptExecutor) driver;
			        		                jsExecutor2.executeScript("arguments[0].click();", elem);
			        		                logger.info("Clicked :" +pName + " dropdown.");
			        					//ClickHelper.clickButton(pName);
			        					String selectAllLi="//*[@id=\""+reportName+"_"+paramNo+"\"]/table[2]/tbody/tr["+ i + "]/td["+j+"]/div/ul/li[2]";
			        					WebElement ele = driver.findElement(By.xpath(selectAllLi));
			        					String className=ele.getAttribute("class");
			        					String selectAllcheckbox="//*[@id=\""+reportName+"_"+paramNo+"\"]/table[2]/tbody/tr["+ i + "]/td["+j+"]/div/ul/li[2]/a/label/input";
			        					UnselectDefaultParameters(className,selectAllcheckbox);
			        					String t3 ="//*[@id=\""+reportName+"_"+paramNo+"\"]/table[2]/tbody/tr["+ i + "]/td["+j+"]/div/ul/li";
			        					List<WebElement> paramsRows = wait.until(ExpectedConditions.visibilityOfAllElementsLocatedBy(By.xpath(t3)));
			        			        int rowCount =paramsRows.size();
		        						for(int k=0;k<valList.length;k++)
			        					{
				        					String val =valList[k];
				        					if(val.equals("None selected"))continue;
				        					selectParametersByText(val, rowCount,reportName,val, paramNo ,i,j);
				        					
			        					}
		        					}
		        					//count++;
		        					j++;
		        				}
		        			}
		        			else {
		        				j+=2;
		        			}
		        			count++;
		        	}
		        }
			
	    } catch (Exception e) {
	        logger.error(e.toString());
	        isPassed = false;
	    }
	}
	
	public static void SelectSubscriptionFromTable(String element,String text)
	{
		try
		{
//			String xpath=XPathDict.get(element);
//	        waitForElement(xpath);
//	        WebDriverWait wait1 = new WebDriverWait(driver, 10);
//	        WebElement buttonElement = wait1.until(ExpectedConditions.elementToBeClickable(By.xpath(xpath)));
//	       // WebElement buttonElement = driver.findElement(By.xpath(xpath));
//	        JavascriptExecutor jsExecutor = (JavascriptExecutor) driver;
//		    jsExecutor.executeScript("arguments[0].click();", buttonElement);
	       
			WebDriverWait wait = new WebDriverWait(driver, 10);
	        List<WebElement> rows = wait.until(ExpectedConditions.visibilityOfAllElementsLocatedBy(By.xpath("//*[@id=\"iggridSubscription_table\"]/tbody/tr")));
	        int row =rows.size();
	        for (int i = 1; i <= row; i++) {
	            String actualtext = wait.until(ExpectedConditions.visibilityOfElementLocated(By.xpath("//*[@id=\"iggridSubscription_table\"]/tbody/tr[" + i + "]/td[1]"))).getText();
	            if (actualtext.equals(text)) {
	                String temp = "//*[@id=\"iggridSubscription_table\"]/tbody/tr[" + i + "]/th/span[2]";
	                waitForElement(temp);
	                WebElement elem = wait.until(ExpectedConditions.elementToBeClickable(By.xpath(temp)));

	                // Evaluate JavaScript click
	                JavascriptExecutor jsExecutor2 = (JavascriptExecutor) driver;
	                jsExecutor2.executeScript("arguments[0].click();", elem);
	                logger.info("Selected option '" + text + "' from the dropdown.");
	                break;
//	                JavascriptExecutor jsExecutor2 = (JavascriptExecutor) driver;
//				      jsExecutor2.executeScript("arguments[0].click();", elem);
	                //elem.click(); // Click the element
	               // logger.info("Selected option '" + text + "' from the dropdown.");
	               // break;
	            }
	        }

		}
		catch(Exception e)
		{
			//ExceptionLog.info(e.toString());
			logger.error(e.toString());
			isPassed=false;
		}
	}

	
	public static void SelectFileFormat(String text, int templateNo)
	{
		try
		{
			text= text.replace(" ", "");
			text= text.replace("-", "");
			String[] arr = text.split("_"); 
			String reportName="";
			String fileFormat="";
			if(arr.length==2)
			{
				reportName=arr[0];
				//reportName=reportName.replace(" ", "");
				fileFormat=arr[1];
			}
			else {
				 reportName="";
				for(int i=0;i<arr.length-1;i++)
				{
					reportName+=arr[i];
				}
				
				 fileFormat= arr[arr.length-1];
			}
			
		String xpath="//*[@id=\"" +reportName + "_"+templateNo+"\"]/table[2]/tbody/tr[1]/td[4]/div/button";
		waitForElement(xpath);
		WebElement element = driver.findElement(By.xpath(xpath));
		JavascriptExecutor jsExecutor = (JavascriptExecutor) driver;
        jsExecutor.executeScript("arguments[0].click();", element);
        
		String inputXpath= "//*[@id=\"" + reportName + "_"+ templateNo + "\"]/table[2]/tbody/tr[1]/td[4]/div/ul/li[1]/div/input";
		XPathDict.put(reportName,inputXpath);
		typeInTextBox( reportName, fileFormat);
		
		String temp = "//*[@id=\"" + reportName + "_"+ templateNo + "\"]/table[2]/tbody/tr[1]/td[4]/div/ul/li[4]/a/label";
		waitForElement(temp);	
		WebElement element2 = driver.findElement(By.xpath(temp));
		JavascriptExecutor jsExecutor2 = (JavascriptExecutor) driver;
        jsExecutor2.executeScript("arguments[0].click();", element2);
		}
		catch(Exception e)
		{
			//ExceptionLog.info(e.toString());
			logger.error(e.toString());
			isPassed=false;
		}
	}
	
	
	public static void RenameReportsParameters(String text)
	{
		try
		{
			String original[] = text.split("#");
			String OriginalReportName="";
			text= text.replace(" ", "");
			text= text.replace("-", "");
			text= text.replace("_", "");
			String arg[] = text.split("#");
		//	String argForFileFormat= original[0];
				OriginalReportName=original[0];
			///String[] arr = text.split("_"); 
			String reportName="";
			reportName=arg[0];
//			if(arr.length==2)
//			{
//				reportName=arr[0];
//			}
//			else {
//				 reportName="";
//				for(int i=0;i<arr.length-1;i++)
//				{
//					reportName+=arr[i];
//				}
//			}
			
	       
			ArrayList<String> PeriodList = new ArrayList<>();
			String newReportName="";
			for(int i=1;i<original.length;i++)
			{
				
				newReportName=original[i];
				String inputXpath= "//*[@id=\"header_ctl_"+reportName +"_"+i+"\"]/tbody/tr/th/input";
				XPathDict.put(newReportName,inputXpath);
				typeInTextBox( newReportName, newReportName);
				
				// Select Period from dropdown
//				String xpathPeriod="//*[@id=\"Period_ctl_"+reportName+"_"+i+"-ctl_1\"]";
//				XPathDict.put(ReportNameWithPeriod,xpathPeriod);
//				SelectDropDown(ReportNameWithPeriod,original[i]);
//				SelectFileFormat(argForFileFormat,i);
				editParameters(OriginalReportName,reportName,newReportName, i);
				groupings.clear();
				
				String element="GridSaveButton";
				ClickHelper.clickButton(element);
				
				
				if(i<arg.length-1)
				{
					String xpathToAddParameters="//*[@id=\"blockCreateSubscription\"]/div[3]/div[1]/input";
					WebElement element2 = driver.findElement(By.xpath(xpathToAddParameters));
					JavascriptExecutor jsExecutor = (JavascriptExecutor) driver;
			        jsExecutor.executeScript("arguments[0].click();", element2);
				}
				
				
			}
			
		
		}
		catch(Exception e)
		{
			//ExceptionLog.info(e.toString());
			logger.error(e.toString());
			isPassed=false;
		}
	}
	
	
	public static void SelectMfForSubscription(String element, String text)
	{
		try
		{
			if(text.equals("All"))
			{
				element="SelectAll";
				ClickHelper.clickButton(element);
				return;
			}
			String[] splitStrings = text.split("_");
			List<String> mfList = new ArrayList<>();
	        for (String s : splitStrings) {
	        	mfList.add(s);
	        }
			
			
	        WebDriverWait wait = new WebDriverWait(driver, 10);
	       
			 List<WebElement> rows = wait.until(ExpectedConditions.visibilityOfAllElementsLocatedBy(By.xpath("//tr[@class=\"csMasterFunds\"]/th")));
		
			int rowCount =rows.size();
	        for (WebElement row : rows) {
	            String actualtext = row.getText().toString(); 
	            if (mfList.contains(actualtext)) {
	            	WebElement inputElement = row.findElement(By.xpath("./preceding-sibling::td//input[@class='masterFunds']"));
                    if (inputElement != null) {
                        JavascriptExecutor jsExecutor2 = (JavascriptExecutor) driver;
                        jsExecutor2.executeScript("arguments[0].click();", inputElement);
                        System.out.println("Selected option '" + actualtext + "' from the dropdown.");
                    }
	               
	            }
	            
	        }
		}
		catch(Exception e)
		{
			//ExceptionLog.info(e.toString());
			logger.error(e.toString());
			isPassed=false;
		}
	}
	
	 public static void SelectMfWithLocalDownload(String element, String text) {
	        try {
	            String[] splitStrings = text.split("#");
	            String mfName= splitStrings[0];
	            List<String> accountList = new ArrayList<>();
	            for (String s : splitStrings) {
	            	if(!s.equals(mfName))
	            	{
	            		accountList.add(s);
	            	}
	                
	            }

	            WebDriverWait wait = new WebDriverWait(driver, 10);
	            List<WebElement> rows = wait.until(ExpectedConditions.visibilityOfAllElementsLocatedBy(By.xpath("//tr[@class='csMasterFunds']/th")));
	            for (WebElement row : rows) {
	                String actualtext = row.getText();
	                if (actualtext.equals(mfName)) {
	                    WebElement inputElement = row.findElement(By.xpath("./preceding-sibling::td//input[@class='masterFunds']"));
	                    if (inputElement != null) {
	                        inputElement.click();
	                        System.out.println("Selected master fund: '" + mfName + "'");

	                        // Find corresponding account and click
	                        List<WebElement> accountElements = wait.until(ExpectedConditions.visibilityOfAllElementsLocatedBy(By.xpath("//tr[@class='csFunds']/td")));
	                        int count1=accountElements.size();
	                        for (WebElement accountElement : accountElements) {
	                        	String accountName=accountElement.getText();
	                        	if (accountList.contains(accountName))
	                        	{
	                        		 WebElement correspondingInputElement = accountElement.findElement(By.xpath("./preceding-sibling::td//input[@type='checkbox']"));
	                                 if (correspondingInputElement != null) {
	                                     correspondingInputElement.click();
	                                     System.out.println("Clicked corresponding input for account: " + accountName);
	                                 }
	                        	}
	                           // accountElement.click();
	                           // System.out.println("Clicked corresponding account: " + accountElement.getText());
	                        }
	                    }
	                }
	            }
	        } catch(Exception e)
			{
				//ExceptionLog.info(e.toString());
				logger.error(e.toString());
				isPassed=false;
			}
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
		}
		catch(Exception e)
		{
			//ExceptionLog.info(e.toString());
			logger.error(e.toString());
			isPassed=false;
		}
	}
	WebDriverWait wait = new WebDriverWait(driver, 10);

	public static void WaitForText1(String element, String text) {
	    try {
	        String xpath = XPathDict.get(element);
	        WebDriverWait wait = new WebDriverWait(driver, 600); // Example: Wait for up to 10 minutes
 // 12 seconds timeout, polling every 100 milliseconds
	        wait.until(ExpectedConditions.textToBePresentInElementLocated(By.xpath(xpath), text));
	    } catch (TimeoutException e) {
	        logger.error("Timed out waiting for text: " + text);
	        isPassed = false;
	    } catch (NoSuchElementException e) {
	        logger.error("Element not found with xpath: " + XPathDict.get(element));
	        isPassed = false;
	    } catch (Exception e) {
	        logger.error("An unexpected error occurred: " + e.toString());
	        isPassed = false;
	    }
	}

	

	
	public static void WaitForText(String element,String text)
	{
		 try {
	            String xpath = XPathDict.get(element);
	            WebDriverWait wait = new WebDriverWait(driver, 180); // 180 seconds

	            // Custom wait condition to wait for "text" or 12 seconds timeout
	           try
	           {
	        	   WebElement webElement = wait.until((WebDriver d) -> {
		                WebElement elementToCheck = d.findElement(By.xpath(xpath));
		                String textValue = elementToCheck.getText().trim();
		                System.out.println(textValue);
		                return textValue.contains(text) ? elementToCheck : null;
		            });
		            
	           }
	           catch (StaleElementReferenceException e) {
	        	   System.out.println("StaleElementReferenceException caught, retrying to find the element...");
	           }
	            while (true) {
	            	try {
	            		 WebElement elementToCheck;
	            	        try {
	            	            elementToCheck = driver.findElement(By.xpath(xpath));
	            	        } catch (NoSuchElementException e) {
	            	            System.out.println("Element not found. Retrying to find the element...");
	            	            // Add any necessary wait or retry mechanism here
	            	            continue; // Continue to the next iteration of the loop
	            	        }
	                    String elementText = elementToCheck.getText().trim();
	                    System.out.println(elementText);
	                    if (!elementText.contains(text)) {
	                        // The expected text has disappeared
	                        // System.out.println("'" + text + "' has disappeared!");
	                        break; // Exit the loop
	                    }
	                } catch (StaleElementReferenceException e) {
	                    System.out.println("StaleElementReferenceException caught, retrying to find the element...");
	                }
	                
	                // Wait for a short interval before checking again
	                Thread.sleep(1000); // Sleep for 1 second
	            }

	        } catch (Exception e) {
	            // Handle the exception (e.g., log the error, set isPassed to false)
	        	//logger.error(webElement.getText().toString());
	        	logger.error(e.toString());
	        	
				isPassed=false;
	            System.out.println("Error: " + e.getMessage());
	        }

	}
	
//	public static void WaitForCompleteText(String element,String text)
//	{
//		try
//		{
//		String xpath=XPathDict.get(element);
//		WebDriverWait wait=new WebDriverWait(driver,12000);
//		wait.until(ExpectedConditions.textToBe(By.xpath(xpath), text));
//		}
//		catch(Exception e)
//		{
//			//ExceptionLog.info(e.toString());
//			logger.error(e.toString());
//			isPassed=false;
//		}
//		
//	}
	
	public static void FinalStatus(String element,String text)
	{
		try {
	        String xpath = XPathDict.get(element);
	        WebElement elementToCheck = driver.findElement(By.xpath(xpath));
	        String actualText = elementToCheck.getText().trim();
	        System.out.println("FinalStatus: " + actualText);
	        

	        // The text "completed" is present
	        // Continue with the next statement
	        // ...

	    } catch (Exception e) {
	        // Handle other exceptions if needed
	        logger.error(e.toString());
	        isPassed = false;
	    }
		
	}
	public static void WaitForCompleteText(String element,String text)
	{
		try {
	        String xpath = XPathDict.get(element);
	        WebElement elementToCheck = driver.findElement(By.xpath(xpath));
	        String actualText = elementToCheck.getText().trim();

	        if (!actualText.equals(text)) {
	            // The text "completed" is not present, throw an exception
	            throw new AssertionError("Expected text: "+ text+ ", Actual text: '" + actualText + "'");
	        }

	        // The text "completed" is present
	        // Continue with the next statement
	        // ...

	    } catch (Exception e) {
	        // Handle other exceptions if needed
	        logger.error(e.toString());
	        isPassed = false;
	    }
	}
	 
	//public static void typeUsingKeyword(XSSFRow row) {
		
	
		//typeInTextBox(row.getCell(2).getStringCellValue(),row.getCell(3).getStringCellValue(), row.getCell(1).getStringCellValue());
		
		
	//}
	
public static void SendDate(String element,String arg) {
		
		try {
			arg=replaceDateTimePlaceholders(arg);
		String xpath=XPathDict.get(element);
		 WebDriverWait wait = new WebDriverWait(driver, 10);
		//ClickHelper.doubleClickButton(element);
		WebElement ele = getWebElement(xpath);
		WebElement calendarInput = driver.findElement(By.xpath("//div[@class='display-flex justify-content-between paddingtop-md minwidth-0']//div[@class='report-settings__label align-self-center' and text()='"+element+"']/following-sibling::div//button[contains(@class, 'datepicker__inputicon')]"));
		ele.click();
		ele.clear();
		ele.sendKeys(arg);
		//calendarInput.click();
		//calendarInput.click();
		ele.click();
		logger.info("Typed "+arg+" in "+element);
		Thread.sleep(2000);
		}
		catch(Exception e)
		{
			//ExceptionLog.info(e.toString());
			logger.error(e.toString());
			isPassed=false;
		}
	
	}
	
	public static void WaitToBecomeInVisible(String element,String arg)
	{
		try {
		String xpath=XPathDict.get(element);
		 WebDriverWait wait = new WebDriverWait(driver, 10);
        By loadingMessageLocator = By.xpath(xpath);
        WebElement loadingMessage = wait.until(ExpectedConditions.visibilityOfElementLocated(loadingMessageLocator));
        wait.until(ExpectedConditions.invisibilityOfElementLocated(loadingMessageLocator));
		logger.info(element+" become invisble.");
		}
		catch(Exception e)
		{
			//ExceptionLog.info(e.toString());
			logger.error(e.toString());
			isPassed=false;
		}
	}
	
public static void GetStartDate(String element,String arg) {
		
		try {
			arg=replaceDateTimePlaceholders(arg);
			// Parse the date string into a LocalDate object
	        DateTimeFormatter formatter = DateTimeFormatter.ofPattern("MM/dd/yyyy");
	        LocalDate date = LocalDate.parse(arg, formatter);
	        
	        // Get the date of 1 month before
	        LocalDate oneMonthBefore = date.minusMonths(1);
	        
	        // Adjust the date if it falls on a weekend
	        if (oneMonthBefore.getDayOfWeek() == DayOfWeek.SATURDAY) {
	            oneMonthBefore = oneMonthBefore.minusDays(1); // Move to Friday
	        } else if (oneMonthBefore.getDayOfWeek() == DayOfWeek.SUNDAY) {
	            oneMonthBefore = oneMonthBefore.minusDays(2); // Move to Friday
	        }
	        
	        // Format the adjusted date back to "MM/dd/yyyy"
	        String startDate = oneMonthBefore.format(formatter);
	        logger.info("Start date is :"+startDate);
		String xpath=XPathDict.get(element);
		 WebDriverWait wait = new WebDriverWait(driver, 10);
		ClickHelper.doubleClickButton(element);
		WebElement ele = getWebElement(xpath);
		WebElement calendarInput = driver.findElement(By.xpath("//div[@class='display-flex justify-content-between paddingtop-md minwidth-0']//div[@class='report-settings__label align-self-center' and text()='"+element+"']/following-sibling::div//button[contains(@class, 'datepicker__inputicon')]"));
		ele.click();
		ele.clear();
		ele.sendKeys(startDate);
		calendarInput.click();
		//calendarInput.click();
		//ele.click();
		logger.info("Typed "+arg+" in "+element);
		}
		catch(Exception e)
		{
			//ExceptionLog.info(e.toString());
			logger.error(e.toString());
			isPassed=false;
		}
	
	}

public static void TypeDateWithoutSleep(String element,String arg) {
	
	try {
		arg=replaceDateTimePlaceholders(arg);
	String xpath=XPathDict.get(element);
	WebDriverWait wait = new WebDriverWait(driver, 10);

	ClickHelper.doubleClickButton(element);
	WebElement ele = getWebElement(xpath);
	ele.click();
	ele.clear();
	ele.sendKeys(arg);
	//ele.click();
	logger.info("Typed "+arg+" in "+element);
	}
	catch(Exception e)
	{
		//ExceptionLog.info(e.toString());
		logger.error(e.toString());
		isPassed=false;
	}

}
public static void typeInTextBoxAfterJSClear(String element,String arg) {
	
	try {
	String xpath=XPathDict.get(element);
	WebDriverWait wait = new WebDriverWait(driver, 10);

	wait.until(ExpectedConditions.visibilityOfElementLocated(By.xpath(xpath)));
	WebElement ele = driver.findElement(By.xpath(xpath));
	ele.click();
	 JavascriptExecutor js = (JavascriptExecutor) driver;
     js.executeScript("arguments[0].value = '';", ele);
     // Trigger the input and change events
     js.executeScript("arguments[0].dispatchEvent(new Event('input'));", ele);
     js.executeScript("arguments[0].dispatchEvent(new Event('change'));", ele);
	ele.click();
	ele.sendKeys(arg);
	logger.info("Typed "+arg+" in "+element);
	}
	catch(Exception e)
	{
		//ExceptionLog.info(e.toString());
		logger.error(e.toString());
		isPassed=false;
	}

}


public static String extractAndSubtract7Days(String text) {
    // Define regex patterns for MM/dd/yyyy and yyyy-MM-dd
    String datePattern1 = "\\b(0[1-9]|1[0-2])/([0-2][0-9]|3[01])/(\\d{4})\\b";
    String datePattern2 = "\\b(\\d{4})-(0[1-9]|1[0-2])-([0-2][0-9]|3[01])\\b";
    
    // Combine the patterns
    String combinedPattern = datePattern1 + "|" + datePattern2;
    Pattern pattern = Pattern.compile(combinedPattern);
    Matcher matcher = pattern.matcher(text);

    // Define date formatters for both patterns
    DateTimeFormatter formatter1 = DateTimeFormatter.ofPattern("MM/dd/yyyy");
    DateTimeFormatter formatter2 = DateTimeFormatter.ofPattern("yyyy-MM-dd");
    
    StringBuilder result = new StringBuilder();
    boolean found = false;

    while (matcher.find()) {
        found = true;
        // Extract the matched date part
        String dateStr = matcher.group(0);
        try {
            LocalDate date;
            if (dateStr.contains("/")) {
                // Parse the date string in MM/dd/yyyy format
                date = LocalDate.parse(dateStr, formatter1);
            } else {
                // Parse the date string in yyyy-MM-dd format
                date = LocalDate.parse(dateStr, formatter2);
            }

            // Subtract 7 days from the date
            LocalDate newDate = date.minus(7, ChronoUnit.DAYS);

            // Format the new date back to the original format
            String newDateStr;
            if (dateStr.contains("/")) {
                newDateStr = newDate.format(formatter1);
            } else {
                newDateStr = newDate.format(formatter2);
            }

            // Append the new date to the result
            result.append(newDateStr).append(" ");
        } catch (Exception e) {
            return "Error processing date: " + dateStr;
        }
    }
    
    if (!found) {
        return "No date found in the text";
    }

    return result.toString().trim();
}

public static String extractAndSubtract1Month(String text) {
    // Define regex patterns for MM/dd/yyyy and yyyy-MM-dd
    String datePattern1 = "\\b(0[1-9]|1[0-2])/([0-2][0-9]|3[01])/(\\d{4})\\b";
    String datePattern2 = "\\b(\\d{4})-(0[1-9]|1[0-2])-([0-2][0-9]|3[01])\\b";
    
    // Combine the patterns
    String combinedPattern = datePattern1 + "|" + datePattern2;
    Pattern pattern = Pattern.compile(combinedPattern);
    Matcher matcher = pattern.matcher(text);

    // Define date formatters for both patterns
    DateTimeFormatter formatter1 = DateTimeFormatter.ofPattern("MM/dd/yyyy");
    DateTimeFormatter formatter2 = DateTimeFormatter.ofPattern("yyyy-MM-dd");
    
    StringBuilder result = new StringBuilder();
    boolean found = false;

    while (matcher.find()) {
        found = true;
        // Extract the matched date part
        String dateStr = matcher.group(0);
        try {
            LocalDate date;
            if (dateStr.contains("/")) {
                // Parse the date string in MM/dd/yyyy format
                date = LocalDate.parse(dateStr, formatter1);
            } else {
                // Parse the date string in yyyy-MM-dd format
                date = LocalDate.parse(dateStr, formatter2);
            }

            // Subtract 1 month from the date
            LocalDate newDate = date.minus(1, ChronoUnit.MONTHS);

            // Format the new date back to the original format
            String newDateStr;
            if (dateStr.contains("/")) {
                newDateStr = newDate.format(formatter1);
            } else {
                newDateStr = newDate.format(formatter2);
            }

            // Append the new date to the result
            result.append(newDateStr).append(" ");
        } catch (Exception e) {
            return "Error processing date: " + dateStr;
        }
    }
    
    if (!found) {
        return "No date found in the text";
    }

    return result.toString().trim();
}

public static String extractAndAdd1Month(String text) {
    // Define regex patterns for MM/dd/yyyy and yyyy-MM-dd
    String datePattern1 = "\\b(0[1-9]|1[0-2])/([0-2][0-9]|3[01])/(\\d{4})\\b";
    String datePattern2 = "\\b(\\d{4})-(0[1-9]|1[0-2])-([0-2][0-9]|3[01])\\b";

    // Combine the patterns
    String combinedPattern = datePattern1 + "|" + datePattern2;
    Pattern pattern = Pattern.compile(combinedPattern);
    Matcher matcher = pattern.matcher(text);

    // Define date formatters for both patterns
    DateTimeFormatter formatter1 = DateTimeFormatter.ofPattern("MM/dd/yyyy");
    DateTimeFormatter formatter2 = DateTimeFormatter.ofPattern("yyyy-MM-dd");

    StringBuilder result = new StringBuilder();
    boolean found = false;

    while (matcher.find()) {
        found = true;
        // Extract the matched date part
        String dateStr = matcher.group(0);
        try {
            LocalDate date;
            if (dateStr.contains("/")) {
                // Parse the date string in MM/dd/yyyy format
                date = LocalDate.parse(dateStr, formatter1);
            } else {
                // Parse the date string in yyyy-MM-dd format
                date = LocalDate.parse(dateStr, formatter2);
            }

            // Add 1 month to the date
            LocalDate newDate = date.plus(1, ChronoUnit.MONTHS);

            // Format the new date back to the original format
            String newDateStr;
            if (dateStr.contains("/")) {
                newDateStr = newDate.format(formatter1);
            } else {
                newDateStr = newDate.format(formatter2);
            }

            // Append the new date to the result
            result.append(newDateStr).append(" ");
        } catch (Exception e) {
            return "Error processing date: " + dateStr;
        }
    }

    if (!found) {
        return "No date found in the text";
    }

    return result.toString().trim();
}

public static String extractAndSubtract2Months(String text) {
    // Define regex patterns for MM/dd/yyyy and yyyy-MM-dd
    String datePattern1 = "\\b(0[1-9]|1[0-2])/([0-2][0-9]|3[01])/(\\d{4})\\b";
    String datePattern2 = "\\b(\\d{4})-(0[1-9]|1[0-2])-([0-2][0-9]|3[01])\\b";
    
    // Combine the patterns
    String combinedPattern = datePattern1 + "|" + datePattern2;
    Pattern pattern = Pattern.compile(combinedPattern);
    Matcher matcher = pattern.matcher(text);

    // Define date formatters for both patterns
    DateTimeFormatter formatter1 = DateTimeFormatter.ofPattern("MM/dd/yyyy");
    DateTimeFormatter formatter2 = DateTimeFormatter.ofPattern("yyyy-MM-dd");
    
    StringBuilder result = new StringBuilder();
    boolean found = false;

    while (matcher.find()) {
        found = true;
        // Extract the matched date part
        String dateStr = matcher.group(0);
        try {
            LocalDate date;
            if (dateStr.contains("/")) {
                // Parse the date string in MM/dd/yyyy format
                date = LocalDate.parse(dateStr, formatter1);
            } else {
                // Parse the date string in yyyy-MM-dd format
                date = LocalDate.parse(dateStr, formatter2);
            }

            // Subtract 2 months from the date
            LocalDate newDate = date.minus(2, ChronoUnit.MONTHS);

            // Format the new date back to the original format
            String newDateStr;
            if (dateStr.contains("/")) {
                newDateStr = newDate.format(formatter1);
            } else {
                newDateStr = newDate.format(formatter2);
            }

            // Append the new date to the result
            result.append(newDateStr).append(" ");
        } catch (Exception e) {
            return "Error processing date: " + dateStr;
        }
    }
    
    if (!found) {
        return "No date found in the text";
    }

    return result.toString().trim();
}

public static void TypeDateFromDateStateStreet1monthPrior(String element,String arg) {
	
	try {
	arg=replaceDateTimePlaceholders(arg);
	arg=extractAndSubtract1Month(arg);
	String xpath=XPathDict.get(element);
	WebDriverWait wait = new WebDriverWait(driver, 10);

	wait.until(ExpectedConditions.visibilityOfElementLocated(By.xpath(xpath)));
	WebElement ele = driver.findElement(By.xpath(xpath));
	ele.click();
	ele.clear();
	ele.click();
	ele.sendKeys(arg);
	logger.info("Typed "+arg+" in "+element);
	}
	catch(Exception e)
	{
		//ExceptionLog.info(e.toString());
		logger.error(e.toString());
		isPassed=false;
	}

}

public static void TypeDateFromDateStateStreet2monthPrior(String element,String arg) {
	
	try {
	arg=replaceDateTimePlaceholders(arg);
	arg=extractAndSubtract2Months(arg);
	String xpath=XPathDict.get(element);
	WebDriverWait wait = new WebDriverWait(driver, 10);
	wait.until(ExpectedConditions.visibilityOfElementLocated(By.xpath(xpath)));
	WebElement ele = driver.findElement(By.xpath(xpath));
	ele.click();
	ele.clear();
	ele.click();
	ele.sendKeys(arg);
	logger.info("Typed "+arg+" in "+element);
	}
	catch(Exception e)
	{
		//ExceptionLog.info(e.toString());
		logger.error(e.toString());
		isPassed=false;
	}

}

public static void TypeDateInBTIG(String element,String arg) {
	
	try {
		arg=replaceDateTimePlaceholders(arg);
	String xpath=XPathDict.get(element);
	WebDriverWait wait = new WebDriverWait(driver, 10);
	wait.until(ExpectedConditions.visibilityOfElementLocated(By.xpath(xpath)));
	WebElement ele = driver.findElement(By.xpath(xpath));
 	ele.click();
	ele.clear();
	ele.sendKeys(Keys.CONTROL + "a");
	ele.sendKeys(Keys.DELETE);
	ele.sendKeys(arg);
	logger.info("Typed "+arg+" in "+element);
	}
	catch(Exception e)
	{
		//ExceptionLog.info(e.toString());
		logger.error(e.toString());
		isPassed=false;
	}

}

public static void TypeDateFromDatePNC(String element,String arg) {
	
	try {
		arg=replaceDateTimePlaceholders(arg);
		arg=extractAndSubtract7Days(arg);
	String xpath=XPathDict.get(element);
	WebDriverWait wait = new WebDriverWait(driver, 10);
	wait.until(ExpectedConditions.visibilityOfElementLocated(By.xpath(xpath)));
	WebElement ele = driver.findElement(By.xpath(xpath));
	ele.click();
	ele.sendKeys(arg);
	logger.info("Typed "+arg+" in "+element);
	}
	catch(Exception e)
	{
		//ExceptionLog.info(e.toString());
		logger.error(e.toString());
		isPassed=false;
	}

}


public static void TypeDateFromDateCITI(String element,String arg) {
	
	try {
		arg=replaceDateTimePlaceholders(arg);
		arg=extractAndSubtract7Days(arg);
		String xpath = XPathDict.get(element);
        WebDriverWait wait = new WebDriverWait(driver, 10);
        WebElement inputElement = wait.until(ExpectedConditions.visibilityOfElementLocated(By.xpath(xpath)));
        
        JavascriptExecutor js = (JavascriptExecutor) driver;
        js.executeScript("arguments[0].removeAttribute('readonly')", inputElement); // Remove readonly attribute
        XLUtils.ClearInput(element, arg);// Clear any existing value
        inputElement.sendKeys(arg); // Set the new value
	logger.info("Typed "+arg+" in "+element);
	}
	catch(Exception e)
	{
		//ExceptionLog.info(e.toString());
		logger.error(e.toString());
		isPassed=false;
	}

}


public static String extractAndAdd7Days(String text) {
    // Define regex patterns for MM/dd/yyyy and yyyy-MM-dd
    String datePattern1 = "\\b(0[1-9]|1[0-2])/([0-2][0-9]|3[01])/(\\d{4})\\b";
    String datePattern2 = "\\b(\\d{4})-(0[1-9]|1[0-2])-([0-2][0-9]|3[01])\\b";
    
    // Combine the patterns
    String combinedPattern = datePattern1 + "|" + datePattern2;
    Pattern pattern = Pattern.compile(combinedPattern);
    Matcher matcher = pattern.matcher(text);

    // Define date formatters for both patterns
    DateTimeFormatter formatter1 = DateTimeFormatter.ofPattern("MM/dd/yyyy");
    DateTimeFormatter formatter2 = DateTimeFormatter.ofPattern("yyyy-MM-dd");
    
    StringBuilder result = new StringBuilder();
    boolean found = false;

    while (matcher.find()) {
        found = true;
        // Extract the matched date part
        String dateStr = matcher.group(0);
        try {
            LocalDate date;
            if (dateStr.contains("/")) {
                // Parse the date string in MM/dd/yyyy format
                date = LocalDate.parse(dateStr, formatter1);
            } else {
                // Parse the date string in yyyy-MM-dd format
                date = LocalDate.parse(dateStr, formatter2);
            }

            // Add 7 days to the date
            LocalDate newDate = date.plus(7, ChronoUnit.DAYS);

            // Format the new date back to the original format
            String newDateStr;
            if (dateStr.contains("/")) {
                newDateStr = newDate.format(formatter1);
            } else {
                newDateStr = newDate.format(formatter2);
            }

            // Append the new date to the result
            result.append(newDateStr).append(" ");
        } catch (Exception e) {
            return "Error processing date: " + dateStr;
        }
    }
    
    if (!found) {
        return "No date found in the text";
    }

    return result.toString().trim();
}


public static void TypeDateToDateCITI(String element,String arg) {
	
	try {
		arg=replaceDateTimePlaceholders(arg);
		arg=extractAndAdd7Days(arg);
	String xpath=XPathDict.get(element);
	WebDriverWait wait = new WebDriverWait(driver, 10);
	wait.until(ExpectedConditions.visibilityOfElementLocated(By.xpath(xpath)));
	WebElement ele = driver.findElement(By.xpath(xpath));
	ele.click();
	ele.sendKeys(arg);
	logger.info("Typed "+arg+" in "+element);
	}
	catch(Exception e)
	{
		//ExceptionLog.info(e.toString());
		logger.error(e.toString());
		isPassed=false;
	}

}

public static void TypeDateToDateCITI_New(String element,String arg) {
	
	try {
		arg=replaceDateTimePlaceholders(arg);
		arg=extractAndAdd7Days(arg);
		 String xpath = XPathDict.get(element);
	        WebDriverWait wait = new WebDriverWait(driver, 10);
	        WebElement inputElement = wait.until(ExpectedConditions.visibilityOfElementLocated(By.xpath(xpath)));
	        
	        JavascriptExecutor js = (JavascriptExecutor) driver;
	        js.executeScript("arguments[0].removeAttribute('readonly')", inputElement); // Remove readonly attribute
	        XLUtils.ClearInput(element, arg);// Clear any existing value
	        inputElement.sendKeys(arg); // Set the new value
	logger.info("Typed "+arg+" in "+element);
	}
	catch(Exception e)
	{
		//ExceptionLog.info(e.toString());
		logger.error(e.toString());
		isPassed=false;
	}

}



public static void SetDateReadOnlyInput(String element, String arg) {
    try {
    	arg=replaceDateTimePlaceholders(arg);
    		String[] dateParts = arg.split("-");
        	dateParts[1] = dateParts[1].toUpperCase();
        	arg = String.join("-", dateParts);
    	
    	
        String xpath = XPathDict.get(element);
        WebDriverWait wait = new WebDriverWait(driver, 10);
        WebElement inputElement = wait.until(ExpectedConditions.visibilityOfElementLocated(By.xpath(xpath)));
        
        JavascriptExecutor js = (JavascriptExecutor) driver;
        js.executeScript("arguments[0].removeAttribute('readonly')", inputElement); // Remove readonly attribute
        inputElement.clear();
        inputElement.sendKeys(arg); // Set the new value
        
        logger.info("Set value " + arg + " in " + element);
    } catch (Exception e) {
        logger.error("Error setting value in " + element + ": " + e.toString());
        isPassed = false;
    }
}

public static void SetDateReadOnlyInputCITI(String element, String arg) {
    try {
    	arg=replaceDateTimePlaceholders(arg);
        String xpath = XPathDict.get(element);
        WebDriverWait wait = new WebDriverWait(driver, 10);
        WebElement inputElement = wait.until(ExpectedConditions.visibilityOfElementLocated(By.xpath(xpath)));
        
        JavascriptExecutor js = (JavascriptExecutor) driver;
        js.executeScript("arguments[0].removeAttribute('readonly')", inputElement); // Remove readonly attribute
        XLUtils.ClearInput(element, arg);// Clear any existing value
        inputElement.sendKeys(arg); // Set the new value
        
        logger.info("Set value " + arg + " in " + element);
    } catch (Exception e) {
        logger.error("Error setting value in " + element + ": " + e.toString());
        isPassed = false;
    }
}

public static void TypeDateAfterClearingInput(String element,String arg) {
	
	try {
		arg=replaceDateTimePlaceholders(arg);
	String xpath=XPathDict.get(element);
	WebDriverWait wait = new WebDriverWait(driver, 10);
	wait.until(ExpectedConditions.visibilityOfElementLocated(By.xpath(xpath)));
	WebElement ele = driver.findElement(By.xpath(xpath));
	ele.click();
	 JavascriptExecutor js = (JavascriptExecutor) driver;
     js.executeScript("arguments[0].value = '';", ele);
     // Trigger the input and change events
     js.executeScript("arguments[0].dispatchEvent(new Event('input'));", ele);
     js.executeScript("arguments[0].dispatchEvent(new Event('change'));", ele);
	ele.sendKeys(arg);
	
	
	logger.info("Typed "+arg+" in "+element);
	}
	catch(Exception e)
	{
		//ExceptionLog.info(e.toString());
		logger.error(e.toString());
		isPassed=false;
	}

}

public static void TypeDateInParts_Clearstreet(String element, String arg) {
    try {
        // Replace placeholders in the date argument
        arg = replaceDateTimePlaceholders(arg);

        // Split the date argument into components
        String[] dateParts = arg.split("-");
        String month = dateParts[0];
        String day = dateParts[1];
        String year = dateParts[2];

        // Get the base XPath for the date picker
        String baseXPath = XPathDict.get(element);

        WebDriverWait wait = new WebDriverWait(driver, 10);
        JavascriptExecutor js = (JavascriptExecutor) driver;

        // Interact with the month field
        String monthXPath = baseXPath + "//div[@role='spinbutton' and contains(@aria-label, 'month')]"
                + "";
        wait.until(ExpectedConditions.visibilityOfElementLocated(By.xpath(monthXPath)));
        WebElement monthEle = driver.findElement(By.xpath(monthXPath));
        js.executeScript("arguments[0].innerText = arguments[1];", monthEle, month);

        // Interact with the day field
        String dayXPath = baseXPath + "//div[@role='spinbutton' and contains(@aria-label, 'day')]"
                + "";
        wait.until(ExpectedConditions.visibilityOfElementLocated(By.xpath(dayXPath)));
        WebElement dayEle = driver.findElement(By.xpath(dayXPath));
        js.executeScript("arguments[0].innerText = arguments[1];", dayEle, day);

        // Interact with the year field
        String yearXPath = baseXPath + "//div[@role='spinbutton' and contains(@aria-label, 'year')]"
                + "";
        wait.until(ExpectedConditions.visibilityOfElementLocated(By.xpath(yearXPath)));
        WebElement yearEle = driver.findElement(By.xpath(yearXPath));
        js.executeScript("arguments[0].innerText = arguments[1];", yearEle, year);

        logger.info("Typed " + arg + " in " + element);
    } catch (Exception e) {
        logger.error(e.toString());
        isPassed = false;
    }
}

public static void TypeDateInParts_Clearstreet_V2(String element, String arg) {
    try {
        // Replace placeholders in the date argument
        arg = replaceDateTimePlaceholders(arg);

        // Split the date argument into components
        String[] dateParts = arg.split("-");
        String month = dateParts[0];
        String day = dateParts[1];
        String year = dateParts[2];

        // Get the base XPath for the date picker
        String baseXPath = XPathDict.get(element);

        WebDriverWait wait = new WebDriverWait(driver, 10);
        JavascriptExecutor js = (JavascriptExecutor) driver;

        // Interact with the month field
        String monthXPath = baseXPath + "//div[@role='spinbutton' and contains(@aria-label, 'month')]"
                + "";
        wait.until(ExpectedConditions.visibilityOfElementLocated(By.xpath(monthXPath)));
        WebElement monthEle = driver.findElement(By.xpath(monthXPath));
        monthEle.sendKeys(month);
        

        // Interact with the day field
        String dayXPath = baseXPath + "//div[@role='spinbutton' and contains(@aria-label, 'day')]"
                + "";
        wait.until(ExpectedConditions.visibilityOfElementLocated(By.xpath(dayXPath)));
        WebElement dayEle = driver.findElement(By.xpath(dayXPath));
        dayEle.sendKeys(day);
       

        // Interact with the year field
        String yearXPath = baseXPath + "//div[@role='spinbutton' and contains(@aria-label, 'year')]"
                + "";
        wait.until(ExpectedConditions.visibilityOfElementLocated(By.xpath(yearXPath)));
        WebElement yearEle = driver.findElement(By.xpath(yearXPath));
        yearEle.sendKeys(year);

        logger.info("Typed " + arg + " in " + element);
    } catch (Exception e) {
        logger.error(e.toString());
        isPassed = false;
    }
}
}

