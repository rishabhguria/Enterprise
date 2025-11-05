package com.nirvana.TestCases;


import com.nirvana.Helper.HelperClass;
import com.nirvana.Helper.WindowHelper;
import freemarker.core.Environment;
import org.apache.jena.base.Sys;
import org.testng.annotations.AfterClass;
import org.testng.annotations.BeforeClass;

import org.apache.commons.io.FileUtils;

import org.apache.log4j.Logger;
import org.apache.log4j.PropertyConfigurator;
import org.apache.poi.xssf.usermodel.XSSFRow;
import org.apache.poi.xssf.usermodel.XSSFSheet;
import org.apache.poi.xssf.usermodel.XSSFWorkbook;
import org.openqa.selenium.By;
import org.openqa.selenium.JavascriptExecutor;
import org.openqa.selenium.OutputType;
import org.openqa.selenium.TakesScreenshot;

import java.awt.*;
import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.sql.SQLOutput;
import java.text.SimpleDateFormat;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.*;
import java.util.List;

import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.chrome.ChromeDriver;
import org.openqa.selenium.chrome.ChromeOptions;
import org.openqa.selenium.support.ui.ExpectedConditions;
import org.openqa.selenium.support.ui.WebDriverWait;

public class BaseClass {

	public static WebDriver driver;
	public static Logger logger;
	public static Logger ExceptionLog;
	public static Boolean isPassed = true;
	public static String source;
	public static String Filepath = (System.getProperty("user.dir")+"//Data//Data.xlsx");
	public static Map <String,String> XPathDict=new HashMap <String,String>();
	public static Integer Iteration = 0;

	protected static void CloseWindow(String arg) throws InterruptedException {
		Set<String> handles = driver.getWindowHandles();
		com.nirvana.Helper.HelperClass.PrintConsole("handles count "+driver.getWindowHandles().size());
		com.nirvana.Helper.HelperClass.PrintConsole("Closing Window");
		int count = 1;
		while(true) {
			count++;
			if (count > 11)
				break;
			try {

				//com.nirvana.Helper.HelperClass.PrintConsole("Title is "+title);
				//com.nirvana.Helper.HelperClass.PrintConsole("Url is "+driver.getCurrentUrl());
				//com.nirvana.Helper.HelperClass.PrintConsole("Count is" + count);

				for (String handle : driver.getWindowHandles()) {
					driver.switchTo().window(handle);
					String url = driver.getCurrentUrl();
					//String newTitle = driver.getTitle();
					//com.nirvana.Helper.HelperClass.PrintConsole("newTitle is " + newTitle);
					//com.nirvana.Helper.HelperClass.PrintConsole("URL is "+url);
					//com.nirvana.Helper.HelperClass.PrintConsole("window is "+driver.getTitle());
					if (driver.getTitle().contains(arg)) {//Thread.sleep(5000);
						//if (url.contains("browser")) {
						com.nirvana.Helper.HelperClass.PrintConsole("Window Switch to "+driver.getTitle());
						HelperClass.PrintConsole("window Closed");
						driver.close();
						//if(count==10)
						break;
						/*} else {
							driver.close();
						driver.switchTo().window(handle);
						}*/
					} else
						driver.switchTo().window(handle);
				}
			}
			catch (Exception e){

			}
		}

	}

	protected static void CloseWindowRTPNL(String arg) throws InterruptedException {
		Set<String> handles = driver.getWindowHandles();
		com.nirvana.Helper.HelperClass.PrintConsole("handles count "+driver.getWindowHandles().size());
		com.nirvana.Helper.HelperClass.PrintConsole("Closing Window");
		int count = 1;
		while(true) {
			count++;
			if (count > 11)
				break;
			try {

				//com.nirvana.Helper.HelperClass.PrintConsole("Title is "+title);
				//com.nirvana.Helper.HelperClass.PrintConsole("Url is "+driver.getCurrentUrl());
				//com.nirvana.Helper.HelperClass.PrintConsole("Count is" + count);

				for (String handle : driver.getWindowHandles()) {
					driver.switchTo().window(handle);
					String url = driver.getCurrentUrl();
					//String newTitle = driver.getTitle();
					//com.nirvana.Helper.HelperClass.PrintConsole("newTitle is " + newTitle);
					//com.nirvana.Helper.HelperClass.PrintConsole("URL is "+url);
					//com.nirvana.Helper.HelperClass.PrintConsole("window is "+driver.getTitle());
					if (driver.getTitle().equals(arg)) {//Thread.sleep(5000);
						//if (url.contains("browser")) {
						com.nirvana.Helper.HelperClass.PrintConsole("Window Switch to "+driver.getTitle());
						HelperClass.PrintConsole("window Closed");
						driver.close();
						//if(count==10)
						break;
						/*} else {
							driver.close();
						driver.switchTo().window(handle);
						}*/
					} else
						driver.switchTo().window(handle);
				}
			}
			catch (Exception e){

			}
		}

	}
	protected static void CloseAllWindow(String arg) {
		com.nirvana.Helper.HelperClass.PrintConsole("handles count "+driver.getWindowHandles().size());
		com.nirvana.Helper.HelperClass.PrintConsole("Closing All Window");
		
			 Set<String> windowHandles = driver.getWindowHandles();
        
        // Loop through all the window handles
        for (String handle : windowHandles) {
            try {
                driver.switchTo().window(handle);
                com.nirvana.Helper.HelperClass.PrintConsole("Window title: " + driver.getTitle());
				if (driver.getTitle().contains("Dock"))
					continue;
                // Close the current window
				else if(driver.getTitle().equals(arg))
					driver.close();
            } catch (org.openqa.selenium.NoSuchWindowException e) {
                // If the window is already closed, skip to the next one
                com.nirvana.Helper.HelperClass.PrintConsole("Window already closed: " + handle);
            }
        }

	}
	public static void CheckRow(String args) throws InterruptedException {
		//String SymbolXpath = "/html/body/div/div[1]/div/div/main/div/div/div/div/div[1]/div/div[2]/div[1]/span/section/span/div[2]/div/div/div[1]/div/div[2]";
		String SymbolXpath = "/html/body/div/div[1]/div/div/main/div/div/div/div/div/div[1]/div/div[2]/div[1]/span/section/span/div[2]/div/div/div[1]/div/div[2]";
		//String StatusXpath = "/html/body/div/div[1]/div/div/main/div/div/div/div/div[1]/div/div[2]/div[1]/span/section/span/div[2]/div/div/div[1]/div/div[4]";
		String StatusXpath = "/html/body/div/div[1]/div/div/main/div/div/div/div/div/div[1]/div/div[2]/div[1]/span/section/span/div[2]/div/div/div[1]/div/div[4]";
		String[] expectedValues = args.split(",");
		String expectedSymbol = expectedValues[0];
		String expectedStatus = expectedValues[1];

		WebElement symbol = getWebElement(SymbolXpath);
		WebElement status = getWebElement(StatusXpath);

		Long CurrentTime = System.currentTimeMillis();

		while (System.currentTimeMillis() - CurrentTime <= 120000) {
			String symbolText = symbol.getText();
			String statusText = status.getText();

			if (symbolText.equals(expectedSymbol) && statusText.equals(expectedStatus)) {
				com.nirvana.Helper.HelperClass.PrintConsole("Matched");
				break;
			} else {
				com.nirvana.Helper.HelperClass.PrintConsole("Mismatch");
				Thread.sleep(1000);
			}
		}
	}

	protected static void CheckElement(String element) {
		String xpath = XPathDict.get(element);
		try{
			waitForElement(xpath);
			List<WebElement> element1 = driver.findElements(By.xpath(xpath));
			if(element1.size()<=0)
				isPassed = false;
		}
		catch (Exception e){
			isPassed =false;
		}
	}

	public String  readExcelData(String sheetName,int row,int column) throws IOException{
		String Path=(System.getProperty("user.dir")+"//Data//Data.xlsx");
		FileInputStream fs = new FileInputStream(new File(Path));
		XSSFWorkbook workbook = new XSSFWorkbook(fs);
		XSSFSheet sheet = workbook.getSheet(sheetName);	
		return sheet.getRow(row).getCell(column).getStringCellValue();
	}
	
    public int ExcelSheetLength(String SheetName) throws IOException {
	
	String Path=(System.getProperty("user.dir")+"//Data//Data.xlsx");
	FileInputStream fs = new FileInputStream(new File(Path));
	XSSFWorkbook workbook = new XSSFWorkbook(fs);
	XSSFSheet sheet = workbook.getSheet(SheetName);	
	return sheet.getLastRowNum();
    }

	@BeforeClass


	
		public void setup() throws IOException
		{
			try {

				Runtime.getRuntime().addShutdownHook(new Thread(() -> {
					com.nirvana.Helper.HelperClass.PrintConsole("Shutdown hook triggered");

				}));
			} catch (IllegalStateException e) {
				System.err.println("Error: Unable to register shutdown hook. JVM is already shutting down.");
			}
			String timestamp = LocalDateTime.now().format(DateTimeFormatter.ofPattern("HH"));
			if(timestamp.equals("4") || timestamp.equals("16")){
				com.nirvana.Helper.HelperClass.PrintConsole("Not able to run the automation because of 4AM time");
				com.nirvana.Helper.HelperClass.PrintConsole("Stopping Automation");
				System.exit(1);
			}
			//System.setProperty("webdriver.chrome.driver", "E:\\Performance Code\\Performance\\Drivers\\chromedriver.exe");
		System.setProperty("webdriver.chrome.driver",System.getProperty("user.dir")+"//Drivers//chromedriver.exe");
		logger = Logger.getLogger("BatchRun");
		PropertyConfigurator.configure("Log4j.properties");
		
		  Map<String, Object> prefs = new HashMap<String, Object>();
		 
	      
	        prefs.put("download.default_directory","E:\\");
	        
	        prefs.put("profile.content_settings.exceptions.automatic_downloads.*.setting", 1 );
	        // Adding cpabilities to ChromeOptions
	        ChromeOptions options = new ChromeOptions();
	        //options.addArguments("--headless");
	        //options.setExperimentalOption("prefs", prefs);
	        
	        options.setExperimentalOption("debuggerAddress", "localhost:8084");
	       //options.setExperimentalOption("debuggerAddress", "localhost:8084");
	        // Launching browser with desired capabilities
	        driver= new ChromeDriver(options);
	        //driver.manage().window().maximize();
	        
	        
	       // FileUtils.cleanDirectory(new File("E:\\ReportMatchingSource\\"));
		
	      //Creating XPath dictionary
	        String page,element,XPath;
	        for(int i=1;i<=ExcelSheetLength("XPath");i++)
	        {
	        	//page=readExcelData("XPath",i,0);
	        	element=readExcelData("XPath",i,1);
	        	XPath=readExcelData("XPath",i,2);
	        	XPathDict.put(element, XPath);
	        	
	        }
	        
		}
	public static void captureImage() throws AWTException, IOException {
		java.awt.Robot robot = new java.awt.Robot();
		java.awt.Rectangle screenRect = new java.awt.Rectangle(java.awt.Toolkit.getDefaultToolkit().getScreenSize());
		java.awt.image.BufferedImage capture = robot.createScreenCapture(screenRect);

		// Saving the captured image to a file
		java.io.File outputfile = new java.io.File("screenshot.png");
		javax.imageio.ImageIO.write(capture, "PNG", outputfile);
		com.nirvana.Helper.HelperClass.PrintConsole("Screenshot saved: " + outputfile.getAbsolutePath());
	}
	
	
	
	
	public static WebElement getElement(String locator,String locatorType) {
		

		if(locatorType.contains("id")){			
		return driver.findElement(By.id(locator));
		
		}else if(locatorType.contains("name") ){
			;
			return driver.findElement(By.name(locator));}
			
		else if(locatorType.contains("xpath")){
			
			return driver.findElement(By.xpath(locator));}
		else if(locatorType.contains("linkText")){ 
			
			return driver.findElement(By.linkText(locator));}
		
		else if(locatorType.contains("partialLinkText")){ 
			return driver.findElement(By.partialLinkText(locator));
		}
				
		else
			throw new NoSuchElementException("No Such Element : " + locator);		}
	
	//Get LOcatorType Name
	public static void getvalue (XSSFRow row) {
		
		getElement(row.getCell(2).getStringCellValue(),row.getCell(1).getStringCellValue());
		
	}

	
	public static void Sleep(long Value) throws InterruptedException {
		
	
	Thread.sleep(Value);
	}
	
	static long TotalSleepTimer = 0L;
	public static void Sleep() throws InterruptedException {
		Thread.sleep(20000);
		TotalSleepTimer+=20000;
	}

	
	
	
	public static Object executeScript(WebElement ele) {
		ele.click();
		/*try {
			JavascriptExecutor executor = (JavascriptExecutor) driver;
			return executor.executeScript("arguments[0].click();", ele);
		}
		catch (Exception e){
			com.nirvana.Helper.HelperClass.PrintConsole(e);
		}*/

		return null;
	}
	
	
	public static void waitForElement(String locator,String lo) {
		
		WebDriverWait wait = new WebDriverWait(driver, 60);
		

		if(lo.contains("id")){
			wait.until(ExpectedConditions.presenceOfElementLocated(By.id(locator)));
			
			return;
		}else if(lo.contains("name")){
			wait.until(ExpectedConditions.presenceOfElementLocated(By.name(locator)));
			
			return;
				}
		else if(lo.contains("xpath")){
			wait.until(ExpectedConditions.elementToBeClickable(By.xpath(locator)));
		
			return;
		}
		
		else if(lo.contains("linkText")){
			wait.until(ExpectedConditions.presenceOfElementLocated(By.linkText(locator)));
			
			return;
		}
		
		
		else if(lo.contains("partialLinkText")){
			wait.until(ExpectedConditions.presenceOfElementLocated(By.partialLinkText(locator)));
			return;
		}
		
		
		
		else
			throw new NoSuchElementException("Element Not Found : " + locator);
		
	}
	
	//Get LOcatorType Name
	
	public static void waitElement(String element) throws IOException, AWTException {

    String xpath=XPathDict.get(element);
	try {
		WebDriverWait wait = new WebDriverWait(driver, 120);
		wait.until(ExpectedConditions.elementToBeClickable(By.xpath(xpath)));
		HelperClass.PrintConsole("Clickable:" + element);
	}
	catch (Exception e){
		HelperClass.PrintConsole(e);
		HelperClass.PrintConsole(element +" Not clickable");
		WindowHelper.CaptureImage("Element Not Found",element);
	}
	}

	public static void waitUsingKeyword(XSSFRow row) {
		
		
		waitForElement(row.getCell(2).getStringCellValue(), row.getCell(1).getStringCellValue());
	
	}
	
	@AfterClass
	
	public void tearDown()
	{
	//driver.quit();
	}

	
	public static void captureScreen(WebDriver driver, String tname) throws IOException {
		TakesScreenshot ts = (TakesScreenshot) driver;
		File source = ts.getScreenshotAs(OutputType.FILE);
		String timeStamp = new SimpleDateFormat("yyyy.MM.dd.HH.mm.ss").format(new Date());
		File target = new File(System.getProperty("user.dir") + "/Screenshots/" +timeStamp +tname + ".png");
		FileUtils.copyFile(source, target);
		com.nirvana.Helper.HelperClass.PrintConsole("Screenshot taken");
	}


	public static void waitForElement(String xpath) {
      
	WebDriverWait wait = new WebDriverWait(driver, 120);
	wait.until(ExpectedConditions.elementToBeClickable(By.xpath(xpath)));	
	
	
}

	public static void waitForElement1(String xpath) {
	      
		WebDriverWait wait = new WebDriverWait(driver, 120);
		wait.until(ExpectedConditions.visibilityOfElementLocated(By.xpath(xpath)));	
		
		
	}


public static WebElement getWebElement(String xpath)
{
	return driver.findElement(By.xpath(xpath));
}
	
	
	
}



