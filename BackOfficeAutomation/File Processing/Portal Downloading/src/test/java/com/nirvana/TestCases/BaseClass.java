package com.nirvana.TestCases;

import org.testng.annotations.AfterClass;
import org.testng.annotations.BeforeClass;
import org.testng.annotations.Test;

import com.nirvana.Helper.WindowHelper;

import org.apache.commons.io.FileUtils;
import org.apache.log4j.FileAppender;
import org.apache.log4j.LogManager;
import org.apache.log4j.Logger;
import org.apache.log4j.PatternLayout;
import org.apache.log4j.PropertyConfigurator;
import org.apache.poi.xssf.usermodel.XSSFRow;
import org.apache.poi.xssf.usermodel.XSSFSheet;
import org.apache.poi.xssf.usermodel.XSSFWorkbook;
import org.openqa.selenium.By;
import org.openqa.selenium.JavascriptExecutor;
import org.openqa.selenium.OutputType;
import org.openqa.selenium.TakesScreenshot;
import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.NoSuchElementException;
import java.util.Properties;
import java.awt.AWTException;
import java.awt.Robot;
import java.awt.event.KeyEvent;
import java.awt.Frame;
import java.awt.Window;
import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;
import java.nio.file.Paths;
import java.nio.file.Files;
import java.nio.file.Path;
import java.time.Duration;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.regex.Matcher;
import java.util.regex.Pattern;
import java.time.LocalDateTime;
import java.io.StringReader;

import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import org.w3c.dom.Document;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;
import org.xml.sax.InputSource;

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
	public static Boolean isPassed;
	public static Boolean isBreakStep;
	public static Boolean isContinueStep;
	public static String source;
	public static List<String> cashAccountList = new ArrayList<>();
	public static String Filepath = (System.getProperty("user.dir") + "//Data//Data.xlsx");
	public static Map<String, String> XPathDict = new HashMap<String, String>();
	public static Map<String, String> ParamList = new HashMap<String, String>();
	public static List<String> ArgList = new ArrayList<>();
	public static HashMap<String, List<String>> ReportList1 = new HashMap<>();
	public static HashMap<String, List<String>> ReportList2 = new HashMap<>();
	public static HashMap<String, Integer> monthNumberMap = new HashMap<>();
	public static String clientNameForLogFile = "";
	public static HashMap<String, String> DownloadLogFilePath = new HashMap<>();
	// public static String DownloadLogFilePath="";
	public static LocalDate processDate = getProcessDate(
			Paths.get(System.getProperty("user.dir"), "Data", "ProcessDateXmlPath.txt").toString());

	public static class XMLDateExtractor {
		private String xmlContent;

		public XMLDateExtractor(String xmlContent) {
			this.xmlContent = xmlContent;
		}

		public LocalDate extractFilterValue() {
			try {
				System.out.println("XML Content: " + xmlContent); // Log XML content
				DocumentBuilderFactory factory = DocumentBuilderFactory.newInstance();
				DocumentBuilder builder = factory.newDocumentBuilder();
				Document doc = builder.parse(new InputSource(new StringReader(xmlContent)));

				if (doc != null) {
					System.out.println("Document parsed successfully."); // Log successful parsing
					Node filterValueNode = doc.getElementsByTagName("FilterValue").item(0);
					if (filterValueNode != null) {
						String filterValueText = filterValueNode.getTextContent();
						// Specify the date format 'MM/dd/yyyy'
						DateTimeFormatter formatter = DateTimeFormatter.ofPattern("MM/dd/yyyy");
//	                    // Parse the filter value as LocalDateTime
//	                    return LocalDateTime.parse(filterValueText, formatter);
						LocalDate date = LocalDate.parse(filterValueText, formatter);
						System.out.println("Parsed Date: " + date);
						return date;

					} else {
						System.out.println("FilterValue node not found in XML.");
					}
				} else {
					System.out.println("Document is null after parsing."); // Log if document is null after parsing
				}
			} catch (Exception ex) {
				ex.printStackTrace(); // Log any exceptions that occur during parsing
			}
			return null; // or handle accordingly, e.g., return LocalDateTime.MIN
		}
	}

	public static String replaceDateTimePlaceholders(String input) {
		String pattern = "\\(([^)]+)\\)";
		String pattern1 = "\\{([^}]+)\\}";
		Pattern regex = Pattern.compile(pattern);
		Pattern regex1 = Pattern.compile(pattern1);
		Matcher matcher = regex.matcher(input);
		Matcher matcher1 = regex1.matcher(input);
		StringBuffer result = new StringBuffer();
		String temp = "";
		while (matcher.find()) {
			String placeholder = matcher.group(1);
			String replacement = getFormattedDateTime(placeholder);
			matcher.appendReplacement(result, replacement);
			temp = "processDate";
		}

		while (matcher1.find()) {
			String placeholder = matcher1.group(1);
			String replacement = getFormattedCurrentDateTime(placeholder);
			matcher1.appendReplacement(result, replacement);
			temp = "currDate";
		}
		if (temp.equals("processDate"))
			matcher.appendTail(result);
		if (temp.equals("currDate"))
			matcher1.appendTail(result);
		return result.toString();
	}

	public static String getFormattedDateTime(String format) {
		DateTimeFormatter formatter = DateTimeFormatter.ofPattern(format);
		return processDate.format(formatter);
	}

	public static String getFormattedCurrentDateTime(String format) {
		// extract curren date in format 2024-09-10
		LocalDate currDate = LocalDate.now();
		DateTimeFormatter formatter = DateTimeFormatter.ofPattern(format);
		return currDate.format(formatter);
	}

	public static String readXmlPathFromConfig(String configPath) {
		try (BufferedReader reader = new BufferedReader(new FileReader(configPath))) {
			StringBuilder content = new StringBuilder();
			String line;
			while ((line = reader.readLine()) != null) {
				content.append(line);
			}
			return content.toString();
		} catch (IOException ex) {
			System.out.println("Error reading XML path from config: " + ex.getMessage());
			return null;
		}
	}

	public static LocalDate getProcessDate(String configPath) {
		try {
			String xmlFilePath = readXmlPathFromConfig(configPath);
			if (xmlFilePath != null) {
				String xmlContent = new String(Files.readAllBytes(Paths.get(xmlFilePath)));
				XMLDateExtractor parser = new XMLDateExtractor(xmlContent);
				LocalDate filterValue = parser.extractFilterValue();
				if (!filterValue.equals(LocalDate.MIN)) {
					return filterValue;
				} else {
					System.out.println("Failed to extract FilterValue.");
				}
			}
		} catch (IOException ex) {
			// Handle exception
		}
		return LocalDate.MIN;
	}

	public static String readExcelData(String sheetName, int row, int column) throws IOException {
		try {
			String Path = (System.getProperty("user.dir") + "//Data//Data.xlsx");
			FileInputStream fs = new FileInputStream(new File(Path));
			XSSFWorkbook workbook = new XSSFWorkbook(fs);
			XSSFSheet sheet = workbook.getSheet(sheetName);
			return sheet.getRow(row).getCell(column).getStringCellValue();
		} catch (IOException e) {
			// Handle the exception (log it, print a message, etc.)
			e.printStackTrace(); // Example: Print the stack trace
			return "Error: Unable to read Excel data";
		}

	}

	public static List<String> readLogFile(String filePath) {

		List<String> lines = new ArrayList<>();
		if (filePath.equals("") || filePath.equals("null"))
			return lines;
		try (BufferedReader reader = new BufferedReader(new FileReader(filePath))) {
			String line;
			while ((line = reader.readLine()) != null) {
				lines.add(line);
			}
		} catch (IOException e) {
			System.err.println("Error reading the file: " + e.getMessage());
		}

		return lines;
	}

	public static void LogDownloadedFile(String filePath, String content) {
		// Define the date-time format
		DateTimeFormatter formatter = DateTimeFormatter.ofPattern("dd-MM-yy HH:mm:ss");
		// Get the current date and time
		String timestamp = LocalDateTime.now().format(formatter);
		// Combine the timestamp with the content
		String logEntry = "[" + timestamp + "] " + content;

		try (BufferedWriter writer = new BufferedWriter(new FileWriter(filePath, true))) {
			writer.write(logEntry);
			writer.newLine();
		} catch (IOException e) {
			System.err.println("Error writing to the file: " + e.getMessage());
		}
	}

	public static void createDownloadeFilesLog(String directoryPath, String ele) {
		// Create the directory if it doesn't exist
		File directory = new File(directoryPath);
		if (!directory.exists()) {
			if (directory.mkdirs()) {
				System.out.println("Directory created: " + directory.getAbsolutePath());
			} else {
				System.out.println("Failed to create directory.");
				return; // Exit the method if directory creation fails
			}
		}

		// Specify the file name format
		String processDate = replaceDateTimePlaceholders("(yyyyMMdd)");
		String fileName = "FilesDownloaded_" + processDate + ".txt";
		DownloadLogFilePath.put(ele, directoryPath + fileName);
		// Create a File object with the specified directory and file name
		File file = new File(directoryPath + fileName);

		try {
			// Check if the file already exists
			if (!file.exists()) {
				// Create the file
				if (file.createNewFile()) {
					System.out.println("File created: " + file.getAbsolutePath());
				} else {
					System.out.println("Failed to create file.");
				}
			} else {
				System.out.println("File already exists.");
			}
		} catch (IOException e) {
			System.out.println("An error occurred.");
			e.printStackTrace();
		}

	}

	private static String generateLogFileName() {
		SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd-HH-mm-ss");
		String timestamp = sdf.format(new Date());
		return System.getProperty("user.dir") + "\\logs\\selenium-" + timestamp + ".log";
	}

	public static int ExcelSheetLength(String SheetName) throws IOException {
		try {
			String Path = (System.getProperty("user.dir") + "//Data//Data.xlsx");
			FileInputStream fs = new FileInputStream(new File(Path));
			XSSFWorkbook workbook = new XSSFWorkbook(fs);
			XSSFSheet sheet = workbook.getSheet(SheetName);
			if (sheet != null) {
				// Return the number of rows if the sheet exists
				return sheet.getLastRowNum(); // Adding 1 to account for 0-based indexing
			} else {
				// Handle the case when sheet is not found
				// System.out.println("Sheet '" + SheetName + "' not found.");
				return -1; // Or return 0, depending on how you want to handle it
			}
		} catch (IOException e) {
			// Handle the exception (log it, print a message, etc.)
			e.printStackTrace(); // Example: Print the stack trace
			return 0;
		}
	}

	@BeforeClass

	public void setup() throws IOException {
		System.setProperty("webdriver.chrome.driver", System.getProperty("user.dir") + "//Drivers//chromedriver.exe");
		String dir = System.getProperty("user.dir");
		// logger = Logger.getLogger("DownloadAutomation");
		// PropertyConfigurator.configure("Log4j.properties");
		// Generate log file name dynamically
		String logFileName = generateLogFileName();

		// Ensure directory exists
		File logDir = new File(logFileName).getParentFile();
		if (!logDir.exists()) {
			logDir.mkdirs();
		}

		// Initialize logger
		logger = Logger.getLogger(BaseClass.class);

		 // Create properties object
        Properties props = new Properties();

        // Set root logger to INFO level with both console and error appenders
        props.setProperty("log4j.rootLogger", "INFO,FILE, console, error");

        
        props.setProperty("log4j.appender.FILE", "org.apache.log4j.FileAppender");
        props.setProperty("log4j.appender.FILE.File", logFileName);
        props.setProperty("log4j.appender.FILE.layout", "org.apache.log4j.PatternLayout");
        props.setProperty("log4j.appender.FILE.layout.ConversionPattern", "%d{yyyy-MM-dd HH:mm:ss} %-5p %c{1}:%L - %m%n");
        // Configure the console appender
        props.setProperty("log4j.appender.console", "org.apache.log4j.ConsoleAppender");
        props.setProperty("log4j.appender.console.layout", "org.apache.log4j.PatternLayout");
        props.setProperty("log4j.appender.console.layout.ConversionPattern", "%d{yyyy-MM-dd HH:mm:ss} %-5p %C{1}:%L - %m%n");

        // Configure the error appender to print errors in red
        props.setProperty("log4j.appender.error", "org.apache.log4j.ConsoleAppender");
        props.setProperty("log4j.appender.error.layout", "org.apache.log4j.PatternLayout");
        props.setProperty("log4j.appender.error.layout.ConversionPattern", "\u001B[31m%d{yyyy-MM-dd HH:mm:ss} %-5p %C{1}:%L - %m%n\u001B[0m");
        props.setProperty("log4j.appender.error.Threshold", "ERROR");

        // Configure log4j using the properties
        PropertyConfigurator.configure(props);

       

        // This will be printed in normal color (INFO level)
        logger.info("Hover on Portfolio");

        // This will be printed in red (ERROR level)
        logger.error("This is an error message.");		

		Map<String, Object> prefs = new HashMap<String, Object>();

		// Adding cpabilities to ChromeOptions
		ChromeOptions options = new ChromeOptions();
		options.setExperimentalOption("debuggerAddress", "localhost:9998");
		driver = new ChromeDriver(options);
		

		String page, element, XPath, mfName, reportName, accountName;
		for (int i = 1; i <= ExcelSheetLength("XPath"); i++) {
			// page=readExcelData("XPath",i,0);
			element = readExcelData("XPath", i, 1);
			XPath = readExcelData("XPath", i, 2);
			XPathDict.put(element, XPath);

		}

		int excelLength = ExcelSheetLength("ReportList1");
		if (excelLength != -1) {
			for (int i = 1; i <= excelLength; i++) {
				// page=readExcelData("XPath",i,0);
				mfName = readExcelData("ReportList1", i, 0);
				reportName = readExcelData("ReportList1", i, 1);
				if (!ReportList1.containsKey(mfName)) {
					ReportList1.put(mfName, new ArrayList<>());
				}
				ReportList1.get(mfName).add(reportName);
				// ParamList.put(mfName, reportName);

			}
		}

		int excelLength2 = ExcelSheetLength("ReportList2");
		if (excelLength2 != -1) {
			for (int i = 1; i <= excelLength2; i++) {
				// page=readExcelData("XPath",i,0);
				mfName = readExcelData("ReportList2", i, 0);
				reportName = readExcelData("ReportList2", i, 1);
				if (!ReportList2.containsKey(mfName)) {
					ReportList2.put(mfName, new ArrayList<>());
				}
				ReportList2.get(mfName).add(reportName);
				// ParamList.put(mfName, reportName);

			}
		}

		int account = ExcelSheetLength("AccountList");
		if (account != -1) {
			for (int i = 1; i <= account; i++) {
				// page=readExcelData("XPath",i,0);
				accountName = readExcelData("AccountList", i, 0);

				if (!ArgList.contains(accountName)) {
					ArgList.add(accountName);
				}

			}
		}

		// Populate the HashMap with month names and numbers
		monthNumberMap.put("January", 1);
		monthNumberMap.put("February", 2);
		monthNumberMap.put("March", 3);
		monthNumberMap.put("April", 4);
		monthNumberMap.put("May", 5);
		monthNumberMap.put("June", 6);
		monthNumberMap.put("July", 7);
		monthNumberMap.put("August", 8);
		monthNumberMap.put("September", 9);
		monthNumberMap.put("October", 10);
		monthNumberMap.put("November", 11);
		monthNumberMap.put("December", 12);

	}

	@Test

	public void AutomationTesting() throws IOException {

		// setup();
		ExecuteTestCase_1 testCase = new ExecuteTestCase_1();
		try {
			testCase.Test();
		} catch (InterruptedException e) {
			// Handle the InterruptedException here
			e.printStackTrace();
		}
		tearDown();

	}

	public static WebElement getElement(String locator, String locatorType) {

		if (locatorType.contains("id")) {
			return driver.findElement(By.id(locator));

		} else if (locatorType.contains("name")) {
			;
			return driver.findElement(By.name(locator));
		}

		else if (locatorType.contains("xpath")) {

			return driver.findElement(By.xpath(locator));
		} else if (locatorType.contains("linkText")) {

			return driver.findElement(By.linkText(locator));
		}

		else if (locatorType.contains("partialLinkText")) {
			return driver.findElement(By.partialLinkText(locator));
		}

		else
			throw new NoSuchElementException("No Such Element : " + locator);
	}

	// Get LOcatorType Name
	public static void getvalue(XSSFRow row) {

		getElement(row.getCell(2).getStringCellValue(), row.getCell(1).getStringCellValue());

	}

	public static void Sleep(long Value) throws InterruptedException {

		Thread.sleep(Value);
	}

	public static void Sleep() throws InterruptedException {
		Thread.sleep(5000);
	}

	public static Object executeScript(WebElement ele) {

		JavascriptExecutor executor = (JavascriptExecutor) driver;
		return executor.executeScript("arguments[0].click();", ele);

	}

	public static void waitForElement(String locator, String lo) {

		WebDriverWait wait = new WebDriverWait(driver, 10);


		if (lo.contains("id")) {
			wait.until(ExpectedConditions.presenceOfElementLocated(By.id(locator)));

			return;
		} else if (lo.contains("name")) {
			wait.until(ExpectedConditions.presenceOfElementLocated(By.name(locator)));

			return;
		} else if (lo.contains("xpath")) {
			wait.until(ExpectedConditions.elementToBeClickable(By.xpath(locator)));

			return;
		}

		else if (lo.contains("linkText")) {
			wait.until(ExpectedConditions.presenceOfElementLocated(By.linkText(locator)));

			return;
		}

		else if (lo.contains("partialLinkText")) {
			wait.until(ExpectedConditions.presenceOfElementLocated(By.partialLinkText(locator)));
			return;
		}

		else
			throw new NoSuchElementException("Element Not Found : " + locator);

	}

	// Get LOcatorType Name

	public static void waitUsingKeyword(XSSFRow row) {

		waitForElement(row.getCell(2).getStringCellValue(), row.getCell(1).getStringCellValue());

	}

	@AfterClass
	public void tearDown() {
		driver.quit();
	}

	public static void captureScreen(WebDriver driver, String tname) throws IOException {
		TakesScreenshot ts = (TakesScreenshot) driver;
		File source = ts.getScreenshotAs(OutputType.FILE);
		String timeStamp = new SimpleDateFormat("yyyy.MM.dd.HH.mm.ss").format(new Date());
		File target = new File(System.getProperty("user.dir") + "/Screenshots/" + timeStamp + tname + ".png");
		FileUtils.copyFile(source, target);
		System.out.println("Screenshot taken");
	}

	public static void waitForElement(String xpath) {

		WebDriverWait wait = new WebDriverWait(driver, 120);

		wait.until(ExpectedConditions.elementToBeClickable(By.xpath(xpath)));

	}

	public static WebElement getWebElement(String xpath) {
		return driver.findElement(By.xpath(xpath));
	}

}
