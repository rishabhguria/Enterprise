package com.nirvana.Utilities;

import java.io.File;
import java.io.FileFilter;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import org.apache.commons.io.FileUtils;
import java.io.IOException;
import java.time.Duration;
import java.time.Month;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.concurrent.TimeUnit;
import java.io.BufferedReader;
import java.io.FileReader;
import java.util.List;

import com.nirvana.Helper.CheckTextHelper;
import com.nirvana.Helper.ClickHelper;
import com.nirvana.Helper.TextBoxHelper;
import com.nirvana.TestCases.BaseClass;
import com.nirvana.TestCases.ExecuteTestCase_1;

import org.apache.commons.io.FileUtils;
import org.apache.poi.ss.usermodel.DataFormatter;
import org.apache.poi.xssf.usermodel.XSSFCell;
import org.apache.poi.xssf.usermodel.XSSFRow;
import org.apache.poi.xssf.usermodel.XSSFSheet;
import org.apache.poi.xssf.usermodel.XSSFWorkbook;
import org.openqa.selenium.Alert;
import org.openqa.selenium.By;
import org.openqa.selenium.JavascriptExecutor;
import org.openqa.selenium.Keys;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.interactions.Actions;
import org.openqa.selenium.support.ui.ExpectedCondition;
import org.openqa.selenium.support.ui.ExpectedConditions;
import org.openqa.selenium.support.ui.Select;
import org.openqa.selenium.support.ui.WebDriverWait;

public class XLUtils extends BaseClass {

	public static FileInputStream fi;
	public static FileOutputStream fo;
	public static XSSFWorkbook wb;
	public static XSSFSheet ws;
	public static XSSFRow row;
	public static XSSFCell cell;

	public static int getRowCount(String xlfile, String xlsheet) throws IOException {

		fi = new FileInputStream(xlfile);
		wb = new XSSFWorkbook(fi);
		ws = wb.getSheet(xlsheet);
		int rowcount = ws.getLastRowNum();
		wb.close();
		fi.close();
		return rowcount;

	}

	public static int getCellCount(String xlfile, String xlsheet, int rownum) throws IOException {
		fi = new FileInputStream(xlfile);
		wb = new XSSFWorkbook(fi);
		ws = wb.getSheet(xlsheet);
		row = ws.getRow(rownum);
		int cellcount = row.getLastCellNum();
		wb.close();
		fi.close();
		return cellcount;
	}

	public static String getCellData(String xlfile, String xlsheet, int rownum, int colnum) throws IOException {
		fi = new FileInputStream(xlfile);
		wb = new XSSFWorkbook(fi);
		ws = wb.getSheet(xlsheet);
		row = ws.getRow(rownum);
		cell = row.getCell(colnum);
		String data;
		try {
			DataFormatter formatter = new DataFormatter();
			String cellData = formatter.formatCellValue(cell);
			return cellData;
		} catch (Exception e) {
			data = "";
		}
		wb.close();
		fi.close();
		return data;
	}

	public static void setCellData(String xlfile, String xlsheet, int rownum, int colnum, String data)
			throws IOException {
		fi = new FileInputStream(xlfile);
		wb = new XSSFWorkbook(fi);
		ws = wb.getSheet(xlsheet);
		row = ws.getRow(rownum);
		cell = row.createCell(colnum);
		cell.setCellValue(data);
		fo = new FileOutputStream(xlfile);
		wb.write(fo);
		wb.close();
		fi.close();
		fo.close();
	}

	public static int getRowNum(String xlfile, String xlsheet, String arg) throws IOException {
		fi = new FileInputStream(xlfile);
		wb = new XSSFWorkbook(fi);
		ws = wb.getSheet(xlsheet);
		int rowcount = ws.getLastRowNum();

		for (int i = 0; i <= rowcount; i++) {
			row = ws.getRow(i);
			if (row != null) {
				if (row.getCell(0) != null && !row.getCell(0).getStringCellValue().isEmpty()) {
					if (row.getCell(0).getStringCellValue().equals(arg)) {
						return i;
					}
				}
			}
		}

		return -1;
	}

	public static void MoveFiles(String arg) {
		String[] arr = arg.split("#");
		String sourcePath = arr[0];
		String targetPath = arr[1] + "\\";
		String fileName="";
		if(arr.length>=3)
		{
			fileName = arr[2];
		}
		

		File source = new File(sourcePath);
		File target = new File(targetPath);

		// Ensure source directory exists and is a directory
		if (!source.exists() || !source.isDirectory()) {
			logger.info(source+" download directory does not exist or is not a directory.");
			return; // Exit the method
		}

		// Ensure target directory exists or create it
		if (!target.exists()) {
			target.mkdirs(); // Creates parent directories if necessary
		}
    boolean isFilefound=false;
		// List files in the source directory
		File[] files = source.listFiles();

		// Move each file from source to target
		for (File file : files) {
			if (file.isFile()) { // Only process files, not directories
				try {
					FileUtils.copyFileToDirectory(file, target, true); // Overwrite existing files if necessary
					logger.info("File moved: " + file.getName());
					file.delete();
					isFilefound=true;
					logger.info("Original file deleted: " + file.getName());
					logger.info("Files moved from " + sourcePath + " to " + targetPath);
				} catch (IOException e) {
					System.out.println("Error moving file: " + e.getMessage());
				}
			}
		}
		if(!isFilefound) {
			logger.info("------------------------------------------------------------------------------");
			logger.info(fileName + " file is NOT DOWNLOADED");
			logger.info("------------------------------------------------------------------------------");
		}
		
	}

	public static void MoveFilesMFWise(String arg) throws IOException {
		// Copying files
		String sourcePath = "E:\\ReportMatchingDestination\\";
		// String targetPath=destPath;
		// String MFName=arg.split("_")[0];
		String targetPath = "E:\\ReportMatchingDestination\\" + ExecuteTestCase_1.module + "\\" + arg + "\\";

		File f = new File(sourcePath);

		FileFilter filter = new FileFilter() {

			public boolean accept(File f) {
				return f.getName().endsWith("xlsx");
			}
		};

		File[] files = f.listFiles(filter);
		String fileName;

		for (File F : files) {
			fileName = F.getName();
			File targetFile = new File(targetPath + fileName);
			FileUtils.copyFile(F, targetFile);
			F.delete();

		}

	}

	public static void renameFile(String arg) {
		try {

			String[] arr = arg.split("#");
			// Copying files
			String sourcePath = arr[0] + "\\";

			File folder = new File(sourcePath);

			// Get a list of files in the folder
			File[] files = folder.listFiles();
			String newFileName = arr[1] + ".csv";
			// Check if there are files in the folder
			if (files != null && files.length > 0) {
				// Loop through the files in the folder
				for (File currentFile : files) {
					// Check if the current item is a file and has the desired name
					if (currentFile.isFile()) {
						// Specify the new file name with the desired name
						File targetFile = new File(sourcePath, newFileName);

						// Attempt to rename the file
						if (currentFile.renameTo(targetFile)) {
							System.out.println("File successfully renamed to " + newFileName);
						} else {
							System.out.println("Failed to rename the file");
						}
						break; // Exit the loop after renaming the file
					}
				}
			} else {
				System.out.println("No files found in the specified folder");
			}
		} catch (Exception e) {
			// Exception handling: Log or handle the exception as needed
			isPassed = false;
			logger.error(e.toString());
		}
	}
	
	public static void renameFile_V2(String arg) {
	    try {
	        arg = replaceDateTimePlaceholders(arg);
	        String[] arr = arg.split("#");
	        String sourcePath = arr[0] + "\\";

	        File folder = new File(sourcePath);
	        File[] files = folder.listFiles();

	        if (files != null && files.length > 0) {
	            for (File currentFile : files) {
	                if (currentFile.isFile()) {
	                    // Extract original file name and extension
	                    String originalFileName = currentFile.getName();
	                    int lastDotIndex = originalFileName.lastIndexOf(".");

	                    // Extract original extension
	                    String extension = (lastDotIndex == -1) ? "" : originalFileName.substring(lastDotIndex);

	                    // Construct new file name with the same extension
	                    String newFileName = arr[1] + extension;

	                    File targetFile = new File(sourcePath, newFileName);

	                    // Attempt to rename the file
	                    if (currentFile.renameTo(targetFile)) {
	                        System.out.println("File successfully renamed to " + newFileName);
	                    } else {
	                        System.out.println("Failed to rename the file");
	                    }
	                    break; // Rename only the first file found
	                }
	            }
	        } else {
	            System.out.println("No files found in the specified folder");
	        }
	    } catch (Exception e) {
	        isPassed = false;
	        logger.error(e.toString());
	    }
	}


	
	public static void clickFidelityCashExportButton(String element, String arg) throws InterruptedException {

		try {
			String FolderName = XPathDict.get("Folder Name Cash");
			String LogFilePath = "";
			if (!FolderName.equals("")) {
				LogFilePath = DownloadLogFilePath.get(FolderName);
			}
			
			String accNoXpath = XPathDict.get("CashAccountNumber");
			WebElement acc = driver.findElement(By.xpath(accNoXpath));
			String reportName = acc.getText();
			if(!reportName.contains(arg))
			{
				isBreakStep = false;
				return;
			}
			else {
				reportName=arg;
			}
			List<String> alreadyDownloadedFiles = new ArrayList<>();
			alreadyDownloadedFiles = readLogFile(LogFilePath);

			String xpath = XPathDict.get(element);
			waitForElement(xpath);
			WebElement ele = getWebElement(xpath);
			if(!isReportAlreadyDownloaded(reportName,alreadyDownloadedFiles)) {

				executeScript(ele);
				LogDownloadedFile(LogFilePath, reportName);
			}

			logger.info(element + " Clicked");
		} catch (Exception e) {
			// ExceptionLog.info(e.toString());
			logger.error(e.toString());
			isPassed = false;
		}

	}


	public static void ConditionBreakFidelityCash(String arg) {
		String FolderName = XPathDict.get("Folder Name Cash");
		String LogFilePath = "";
		if (!FolderName.equals("")) {
			LogFilePath = DownloadLogFilePath.get(FolderName);
		}
		String FolderNamePos = XPathDict.get("Folder Name");
		String LogFilePathPos = "";
		if (!FolderNamePos.equals("")) {
			LogFilePathPos = DownloadLogFilePath.get(FolderNamePos);
		}
		List<String> cashNeededFiles = new ArrayList<>();
		cashNeededFiles=readLogFile(LogFilePathPos);
		List<String> alreadyDownloadedFiles = new ArrayList<>();
		alreadyDownloadedFiles = readLogFile(LogFilePath);
		if (cashAccountList.contains(arg)|| (!isReportAlreadyDownloaded(arg,alreadyDownloadedFiles)&& isCashNeeded(arg,cashNeededFiles))) {

			return;
		}
		isBreakStep = false;
		return;

	}

	
	public static void ConditionBreak(String arg) {
		if (cashAccountList.contains(arg)) {

			return;
		}
		isBreakStep = false;
		return;

	}

	public static boolean isEqual(double value1, double value2) {
		// Round both values to two decimal places
		double roundedValue1 = Math.round(value1 * 100.0) / 100.0;
		double roundedValue2 = Math.round(value2 * 100.0) / 100.0;

		// Compare the rounded values for equality
		return roundedValue1 == roundedValue2;
	}
	
	public static void navigateToFidelityCash(String accountNo) {
		String pos_url = XPathDict.get("Navigate_cash_Url");
		accountNo=accountNo.replace("-", "");
		pos_url+=accountNo;
		driver.navigate().to(pos_url);
		logger.info("Navigated to "+accountNo);
	}
	
	public static void CheckFidelityCashReportUsingUrl(String arg) {
		try {

			String closingQuantity = XPathDict.get("Closing Quantity");
			String cashToAvail = XPathDict.get("Cash Avail to Trade");
			String AccountXpath = XPathDict.get("AccountText");
			String Note = XPathDict.get("AccountText");
			
			
			waitForElement(cashToAvail);
			WebElement ele1 = getWebElement(cashToAvail);
			// WebElement element2 = driver.findElement(By.xpath(cashToAvail));
			String text1 = ele1.getText();
			String stringWithoutDollarSign = text1.replace("$", "").replace(",", "");
			String iFrameXpath = XPathDict.get("iFrame");
			WebElement iframeElement = driver.findElement(By.xpath(iFrameXpath));
			driver.switchTo().frame(iframeElement);
			try {
				String checkPositionXpath = XPathDict.get("IS_PositionAvailable");
				WebElement ele4 = getWebElement(checkPositionXpath);
				isBreakStep = false;
				driver.switchTo().defaultContent();
				return;
			}catch(Exception e){
				
			}
			
			try {
				String checkUnautorizedPositionXpath = XPathDict.get("Is_UnauthorizedAccess");
				WebElement ele5 = getWebElement(checkUnautorizedPositionXpath);
				isBreakStep = false;
				driver.switchTo().defaultContent();
				return;
			}catch(Exception e){
				
			}
			
			
			
			WebElement element2 = driver.findElement(By.xpath(closingQuantity));
			String text2 = element2.getText();

			String stringWithoutDollarSign2 = text2.replace("$", "").replace(",", "");
			WebElement element3 = driver.findElement(By.xpath(AccountXpath));
			String text3 = element3.getText();
			if (!text3.contains(arg)) {

				isBreakStep = false;
				driver.switchTo().defaultContent();
				ClickHelper.cssSelector("Menu_bttn","NA");
				return;
			}
			driver.switchTo().defaultContent();
			double doubleValue1 = Double.parseDouble(stringWithoutDollarSign);

			double doubleValue2 = Double.parseDouble(stringWithoutDollarSign2);

			boolean result = isEqual(doubleValue1, doubleValue2);
			if (!result) {
				cashAccountList.add(arg);
			}
			return;

		} catch (Exception ex) {
			isBreakStep = false;
			driver.switchTo().defaultContent();

			return;

		}

	}
	
	
	public static void navigateToFidelityPostion(String accountNo) {
		try {
		String FolderName=XPathDict.get("Folder Name");
		String LogFilePath="";
		String ReportName=accountNo;
		if(!FolderName.equals(""))
		{
			LogFilePath=DownloadLogFilePath.get(FolderName);
		}
		List<String> alreadyDownloadedFiles = new ArrayList<>();
		alreadyDownloadedFiles=readLogFile(LogFilePath);
		String pos_url = XPathDict.get("Navigate_position_Url");
		accountNo=accountNo.replace("-", "");
		pos_url+=accountNo;
		if(isReportAlreadyDownloaded(ReportName,alreadyDownloadedFiles))
		{
			isBreakStep = false;
			return;
		}
			
		driver.navigate().to(pos_url);
		logger.info("Navigated to "+accountNo);
		}
		catch(Exception e) {
			ExceptionLog.info(e.toString());
			isPassed = false;

		}
	}

	public static void CheckFidelityCashReport(String arg) {
		try {

			String closingQuantity = XPathDict.get("Closing Quantity");
			String cashToAvail = XPathDict.get("Cash Avail to Trade");
			String AccountXpath = XPathDict.get("AccountText");
			String Note = XPathDict.get("AccountText");

			waitForElement(cashToAvail);
			WebElement ele1 = getWebElement(cashToAvail);
			// WebElement element2 = driver.findElement(By.xpath(cashToAvail));
			String text1 = ele1.getText();
			String stringWithoutDollarSign = text1.replace("$", "").replace(",", "");
			String iFrameXpath = XPathDict.get("iFrame");
			WebElement iframeElement = driver.findElement(By.xpath(iFrameXpath));
			driver.switchTo().frame(iframeElement);
			WebElement element2 = driver.findElement(By.xpath(closingQuantity));
			String text2 = element2.getText();

			String stringWithoutDollarSign2 = text2.replace("$", "").replace(",", "");
			WebElement element3 = driver.findElement(By.xpath(AccountXpath));
			String text3 = element3.getText();
			if (!text3.contains(arg)) {

				isBreakStep = false;
				driver.switchTo().defaultContent();
				ClickHelper.cssSelector("Menu_bttn","NA");
				return;
			}
			driver.switchTo().defaultContent();
			double doubleValue1 = Double.parseDouble(stringWithoutDollarSign);

			double doubleValue2 = Double.parseDouble(stringWithoutDollarSign2);

			boolean result = isEqual(doubleValue1, doubleValue2);
			if (!result) {
				cashAccountList.add(arg);
			}
			return;

		} catch (Exception ex) {
			isBreakStep = false;
			driver.switchTo().defaultContent();

			return;

		}

	}

	public static void DownloadFilesFromList(String element) {
		try {
			String xpath = XPathDict.get(element);
			WebDriverWait wait = new WebDriverWait(driver, 10);
			List<WebElement> rows = wait.until(ExpectedConditions.visibilityOfAllElementsLocatedBy(By.xpath(xpath)));
			int rowsize = rows.size();
			logger.info("Total Files Count On Screen : " + rowsize);

			for (WebElement r : rows) {
				// Perform actions on each WebElement, for example, print its text
				executeScript(r);
				logger.info("Downloaded: " + r.getText());
			}

		} catch (Exception e) {
			ExceptionLog.info(e.toString());
			isPassed = false;

		}

	}

	public static void NT_Process(String element) {
		try {
			String date = CheckTextHelper.GetTextInTextArea(element);
			boolean result = true;
			// result=CheckIfDateFileAlreadyExistss(date)
			// Call a script in above function or anything.
			if (!result) {
				logger.info("This" + date + "File already exist");
				isPassed = false;

			} else {

				logger.info("This " + date + " Files are going to download...");

			}

		} catch (Exception e) {
			ExceptionLog.info(e.toString());
			isPassed = false;

		}

	}

	public static List<String> readFilenamesFromFile(String fileName) {
        List<String> filenames = new ArrayList<>();

        try (BufferedReader reader = new BufferedReader(new FileReader(fileName))) {
            String line;
            while ((line = reader.readLine()) != null) {
                filenames.add(line);
            }
        } catch (IOException e) {
            System.err.println("Error reading the file: " + e.getMessage());
        }

        return filenames;
    }
	
	public static void DownloadGSData(String element, String arg) {
		try {
			String FolderName=XPathDict.get(element);
			String LogFilePath="";
			if(!element.equals("NA"))
			{
				LogFilePath=DownloadLogFilePath.get(FolderName);
			}
			String ArgsArr[]=arg.split("#");
			String FileToSkip="NA";
			if(ArgsArr.length>1)
			{
				FileToSkip=ArgsArr[1];
				arg=ArgsArr[0];
			}
		List<String> alreadyDownloadedFiles = new ArrayList<>();
		alreadyDownloadedFiles=readLogFile(LogFilePath);
			String rowListXpath = XPathDict.get("rowsList");
			WebDriverWait wait = new WebDriverWait(driver, 10);

			List<WebElement> rows = wait
					.until(ExpectedConditions.visibilityOfAllElementsLocatedBy(By.xpath(rowListXpath)));

			int rowCount = rows.size();
			String mfName = "";
			String mfNameXpath;
			String reportName;
			String reportNameXpath;
			String checkboxXpath;
			String excelXpath;
			String reportType;
			String reportTypeXpath;
			String portalReportDateXpath;
			String reportDate;
		   HashMap<String, List<String>> temp = new HashMap<>();
		   if(arg.endsWith("1")) {
			   temp=ReportList1;
		   }
		   else if(arg.endsWith("2"))
		   {
			   temp=ReportList2;
		   }

			for (int i = 1; i <= rowCount; i++) {
				WebElement elem = wait
						.until(ExpectedConditions.visibilityOfElementLocated(By.xpath(rowListXpath + "[" + i + "]")));
				String rowId = elem.getAttribute("row-id");
				if (rowId.contains("group")) {
					mfNameXpath = rowListXpath + "[" + i + "]" + "/div[2]/span/span[4]";
					WebElement ele2 = driver.findElement(By.xpath(mfNameXpath));
					mfName = ele2.getText();
					continue;
				} else {
					if (temp.containsKey(mfName)) {
						List<String> reports = temp.get(mfName);
						reportNameXpath = rowListXpath + "[" + i + "]" + "/div[3]";
						WebElement ele3 = driver.findElement(By.xpath(reportNameXpath));
						reportName = ele3.getText();
						boolean containsReport = false;
						for (String report : reports) {
						    if (reportName.contains(report)) {
						        containsReport = true;
						        break; // No need to continue checking once found
						    }
						}
						if (containsReport) {
							reportTypeXpath= rowListXpath + "[" + i + "]" +"/div[4]";
							WebElement ele4 = driver.findElement(By.xpath(reportTypeXpath));
							reportType = ele4.getText();
							if(FileToSkip!="NA" & reportType.contains(FileToSkip))continue;
							checkboxXpath = rowListXpath + "[" + i + "]" + "/div[1]/div/div/div/div[2]/input";
							portalReportDateXpath=rowListXpath + "[" + i + "]" + "/div[5]";
							WebElement dateElement = getWebElement(portalReportDateXpath);
							reportDate =dateElement.getText();
							String ProcessDate=replaceDateTimePlaceholders("(yyyy-MM-dd)");
							if(!ProcessDate.equals(reportDate))continue;
							
							WebElement ele = getWebElement(checkboxXpath);
							
							logger.info(reportName + "Checkbox Selected");
							excelXpath = rowListXpath + "[" + i + "]" + "/div[6]//span/a[text()='Excel']";
							WebElement ele2 = getWebElement(excelXpath);
							Thread.sleep(1000);
							if(!alreadyDownloadedFiles.contains(reportName))
							{
								executeScript(ele);
								executeScript(ele2);
								logger.info(reportName + "Excel Clicked");
								LogDownloadedFile(LogFilePath,reportName);
							}
							
						}

					}
				}

			}
		} catch (Exception e) {
			ExceptionLog.info(e.toString());
			isPassed = false;

		}
	}

	
	
//	 public static boolean isReportAlreadyDownloaded(String reportName,List<String> alreadyDownloadedFiles) {
//	        for (String downloadedFile : alreadyDownloadedFiles) {
//	            if (downloadedFile.contains(reportName)) {
//	                return true;
//	            }
//	        }
//	        return false;
//	    }
	
//	public static void DownloadGSData(String element, String arg) {
//		try {
//			String FolderName=XPathDict.get(element);
//			String LogFilePath="";
//			if(!element.equals("NA"))
//			{
//				LogFilePath=DownloadLogFilePath.get(FolderName);
//			}
//			String ArgsArr[]=arg.split("#");
//			String FileToSkip="NA";
//			if(ArgsArr.length>1)
//			{
//				FileToSkip=ArgsArr[1];
//				arg=ArgsArr[0];
//			}
//		List<String> alreadyDownloadedFiles = new ArrayList<>();
//		alreadyDownloadedFiles=readLogFile(LogFilePath);
//			String rowListXpath = XPathDict.get("rowsList");
//			WebDriverWait wait = new WebDriverWait(driver, Duration.ofSeconds(10));
//
//			List<WebElement> rows = wait
//					.until(ExpectedConditions.visibilityOfAllElementsLocatedBy(By.xpath(rowListXpath)));
//
//			int rowCount = rows.size();
//			String mfName = "";
//			String mfNameXpath;
//			String reportName;
//			String reportNameXpath;
//			String checkboxXpath;
//			String excelXpath;
//			String reportType;
//			String reportTypeXpath;
//			String portalReportDateXpath;
//			String reportDate;
//		   HashMap<String, List<String>> temp = new HashMap<>();
//		   if(arg.endsWith("1")) {
//			   temp=ReportList1;
//		   }
//		   else if(arg.endsWith("2"))
//		   {
//			   temp=ReportList2;
//		   }
//
//			for (int i = 1; i <= rowCount; i++) {
//				WebElement elem = wait
//						.until(ExpectedConditions.visibilityOfElementLocated(By.xpath(rowListXpath + "[" + i + "]")));
//				String rowId = elem.getAttribute("row-id");
//				if (rowId.contains("group")) {
//					mfNameXpath = rowListXpath + "[" + i + "]" + "/div[2]/span/span[4]";
//					WebElement ele2 = driver.findElement(By.xpath(mfNameXpath));
//					mfName = ele2.getText();
//					continue;
//				} else {
//					if (temp.containsKey(mfName)) {
//						List<String> reports = temp.get(mfName);
//						reportNameXpath = rowListXpath + "[" + i + "]" + "/div[3]";
//						WebElement ele3 = driver.findElement(By.xpath(reportNameXpath));
//						reportName = ele3.getText();
//						boolean containsReport = false;
//						for (String report : reports) {
//						    if (reportName.contains(report)) {
//						        containsReport = true;
//						        break; // No need to continue checking once found
//						    }
//						}
//						if (containsReport) {
//							reportTypeXpath= rowListXpath + "[" + i + "]" +"/div[4]";
//							WebElement ele4 = driver.findElement(By.xpath(reportTypeXpath));
//							reportType = ele4.getText();
//							if(FileToSkip!="NA" & reportType.contains(FileToSkip))continue;
//							checkboxXpath = rowListXpath + "[" + i + "]" + "/div[1]/div/div/div/div[2]/input";
//							portalReportDateXpath=rowListXpath + "[" + i + "]" + "/div[5]";
//							WebElement dateElement = getWebElement(portalReportDateXpath);
//							reportDate =dateElement.getText();
//							String ProcessDate=replaceDateTimePlaceholders("(yyyy-MM-dd)");
//							if(!ProcessDate.equals(reportDate))continue;
//							
//							WebElement ele = getWebElement(checkboxXpath);
//							
//							logger.info(reportName + "Checkbox Selected");
//							excelXpath = rowListXpath + "[" + i + "]" + "/div[6]//span/a[text()='Excel']";
//							WebElement ele2 = getWebElement(excelXpath);
//							Thread.sleep(1000);
//							if(!isReportAlreadyDownloaded(reportName,alreadyDownloadedFiles))
//							{
//								executeScript(ele);
//								executeScript(ele2);
//								logger.info(reportName + "Excel Clicked");
//								LogDownloadedFile(LogFilePath,reportName);
//							}
//							
//						}
//
//					}
//				}
//
//			}
//		} catch (Exception e) {
//			ExceptionLog.info(e.toString());
//			isPassed = false;
//
//		}
//	}
//	
	public static void SelectUSBankAccount(String element) {
		try {
			String rowListXpath = XPathDict.get(element);
			WebDriverWait wait = new WebDriverWait(driver, 10);

			List<WebElement> rows = wait.until(ExpectedConditions.visibilityOfAllElementsLocatedBy(By.xpath(rowListXpath)));

			int rowCount = rows.size();


			for (WebElement row : rows) {
				List<WebElement> childElements = row.findElements(By.xpath("*"));
				int childSize=childElements.size();
				for (WebElement child : childElements) {
		            String text = child.getText().trim();
		            if (!text.isEmpty()) {
		                System.out.println("Text of child element: " + text);
		                
		                for (String arg : ArgList) {
		                    if (text.contains(arg)) {
		                        child.click();
		                       
		                        logger.info(text +" is Clicked");	 
		                        }
		                }
		               
		            }
				}
			}
			
		}catch(Exception e) {
			
		}
	}
	
	public static void GetClientDetails(String element, String arg)
	{
        
        // Split the string by #
        String[] splitArray = arg.split("#");
        
        // Create an ArrayList to store the split strings
        ArrayList<String> arrayList = new ArrayList<>(Arrays.asList(splitArray));
		
		 for (String ele : arrayList) {
			 String directoryPath = (System.getProperty("user.dir") + "\\Logs\\" +ele+"\\");
			 DownloadLogFilePath.put(ele, "");
			createDownloadeFilesLog(directoryPath,ele);
	        }
		
	}
	
	 public static boolean isReportAlreadyDownloaded(String reportName,List<String> alreadyDownloadedFiles) {
		    for (String downloadedFile : alreadyDownloadedFiles) {
		        if (downloadedFile.contains(reportName)) {
		            return true;
		        }
		    }
		    return false;
		}
	 
	 public static boolean isCashNeeded(String reportName,List<String> alreadyDownloadedFiles) {
		    for (String downloadedFile : alreadyDownloadedFiles) {
		        if (downloadedFile.contains(reportName+"--- Cash File Also need to be downloaded")) {
		            return true;
		        }
		    }
		    return false;
		}
	
	public static void CheckIfFileAlreadyDownloaded(String element, String arg) {
		try {
			String FolderName = XPathDict.get("Folder Name");
			String LogFilePath = "";
			if (!FolderName.equals("")) {
				LogFilePath = DownloadLogFilePath.get(FolderName);
			}
			
			String reportName = arg;
			List<String> alreadyDownloadedFiles = new ArrayList<>();
			alreadyDownloadedFiles = readLogFile(LogFilePath);
			
			if(isReportAlreadyDownloaded(reportName,alreadyDownloadedFiles)) {
				isBreakStep = false;
				return;
			}
			
		} catch (Exception e) {
			ExceptionLog.info(e.toString());
			isPassed = false;

		}
	
}
	
	
	public static void DownloadOpusFiles(String element, String arg) {
		try {
			String CheckboxXpath = XPathDict.get(element);
			arg=replaceDateTimePlaceholders(arg);
			String FileNameXpath = XPathDict.get("FileXpath");
			FileNameXpath=FileNameXpath.replace("FileName", arg);
			WebDriverWait wait = new WebDriverWait(driver, 10);
			WebElement FileElement = wait.until(ExpectedConditions.visibilityOfElementLocated(By.xpath(FileNameXpath)));
			//WebElement FileElement = getWebElement(FileNameXpath);
			String FileName=FileElement.getText();
			if(!FileName.equals(arg))
			{
				logger.info( "-----File is Not Avilable To Download-----");
				isBreakStep = false;
				return;
			}
		    CheckboxXpath=CheckboxXpath.replace("FileName", arg);
			waitForElement(CheckboxXpath);
			WebElement ele = getWebElement(CheckboxXpath);
        	executeScript(ele);
        	logger.info( " Clear ALL Clicked");
			
			
			
		} catch (Exception e) {
			//ExceptionLog.info(e.toString());
			logger.info(e.toString());
			isPassed = false;

		}
	}
	
	
	public static void BringOwnerToTop(String element, String arg) {
		try {
			String OwnerNameXpath = XPathDict.get(element);
			
			waitForElement(OwnerNameXpath);
			WebElement ele = getWebElement(OwnerNameXpath);
			String OwnerName=ele.getText();
			String OwnerTabXpath=XPathDict.get("OwnerTab");
			WebElement ele2 = getWebElement(OwnerTabXpath);
			int attempt=0;
			while(!OwnerName.equals(arg) && attempt<5)
			{
				executeScript(ele2);
				OwnerName=ele.getText();
				attempt++;
			}
			
        	logger.info( "Promethos Files Owner Moved to top");
			
			
			
		} catch (Exception e) {
			ExceptionLog.info(e.toString());
			isPassed = false;

		}
	}
	
	public static void BringFavouriteToTop(String element, String arg) {
		try {
			String FavouriteXpath = XPathDict.get(element);
			
			waitForElement(FavouriteXpath);
			WebElement ele = getWebElement(FavouriteXpath);
			String FavClassName=ele.getAttribute("class");
			String FavouriteIconXpath=XPathDict.get("FavouriteIcon");
			WebElement ele2 = getWebElement(FavouriteIconXpath);
			int attempt=0;
			while(!FavClassName.equals(arg) && attempt<5)
			{
				executeScript(ele2);
				FavClassName=ele.getAttribute("class");
				attempt++;
			}
			
        	logger.info( "Favourites Moved to top");
			
			
			
		} catch (Exception e) {
			ExceptionLog.info(e.toString());
			isPassed = false;

		}
	}
	
	public static void DownloadUBSTelemarkFutures(String element, String arg) {
		try {
			String rowListXpath = XPathDict.get(element);
			WebDriverWait wait = new WebDriverWait(driver, 10);

			List<WebElement> rows = wait
					.until(ExpectedConditions.visibilityOfAllElementsLocatedBy(By.xpath(rowListXpath)));
			String BusinessDate=replaceDateTimePlaceholders(arg);
			int rowCount = rows.size();

			
			for (int i = 1; i <= rowCount; i++) {
				
				 
				String tempReportXpath=XPathDict.get("FutureReportNameList");
				String reportsXpath =tempReportXpath+"["+i+"]"+"/div[3]/div/a";
				 WebElement ele3 = getWebElement(reportsXpath);
				String reportName=ele3.getText().toString();
				if(!reportName.equals("ETDDailyStatement"))
				{
				String tempBusinessDateXpath=XPathDict.get("FutureReporsdetailsList");
					String BusinessDateXpath =tempBusinessDateXpath+"["+i+"]"+"/div//*[@class='util-components-truncated-label' ]";
					List<WebElement> BD = wait
							.until(ExpectedConditions.visibilityOfAllElementsLocatedBy(By.xpath(BusinessDateXpath)));
					int BDRCount=BD.size();
					boolean isDownloaded=false;
					for(WebElement TBD:BD)
					{
						try {
							WebElement ele2 = getWebElement(BusinessDateXpath);
							String portalBusinessDate=ele2.getText().toString();
							if(portalBusinessDate.equals(BusinessDate.trim())&& !isDownloaded)
							{
								String tempCSVXpath=XPathDict.get("FutureCSVList");
								String CSVXpath =tempCSVXpath+"["+i+"]"+"/div/div/div/div";
								WebElement ele4 = getWebElement(CSVXpath);
								ele4.click();
								logger.info(reportName + " download button is clicked");
								Thread.sleep(15000);
								String MoveFilesArg=XPathDict.get("MoveFiles");
								MoveFiles(MoveFilesArg);
								isDownloaded=true;
							}
						}catch(Exception e){
							
						}
						
					}
					
					

				}
				
				}
			
			

			
		} catch (Exception e) {
		//	ExceptionLog.info(e.toString());
			isPassed = false;

		}
	}
	
	public static void SelectDateFromCalender(String element, String arg) {
		try {
			arg=replaceDateTimePlaceholders(arg);
			String arr[]=arg.split("/");
			String date=arr[1].replace(" ","");
			int monthNumber = Integer.parseInt(arr[0].replace(" ",""));
			int year = Integer.parseInt(arr[2]);
			String decreaseCurYearElementXpath=XPathDict.get("Decrease Year");
			String increaseCurYearElementXpath=XPathDict.get("Increase Year");
			String decreaseCurMonthElementXpath= XPathDict.get("Decrease Month");
			String increaseCurMonthElementXpath=XPathDict.get("Increase Month");
			
			// Select Year
			String currYearxpath=XPathDict.get("getCurrentYear");
			 WebElement ele1 = getWebElement(currYearxpath);
			 int currYear=Integer.parseInt(ele1.getText().toString());
			 
			 if(year!=currYear)
			 {
				 if(year<currYear)
				 {
					 while(year<currYear) {
						 WebElement decreaseCurYearElement = getWebElement(decreaseCurYearElementXpath);
						 decreaseCurYearElement.click();
						 
						 WebElement curYearElement = getWebElement(currYearxpath);
						 currYear=Integer.parseInt(curYearElement.getText().toString());
					 } 
				 }else {
					 while(year>currYear) {
						 WebElement increaseCurYearElement = getWebElement(increaseCurYearElementXpath);
						 increaseCurYearElement.click();
						 
						 WebElement curYearElement = getWebElement(currYearxpath);
						 currYear=Integer.parseInt(curYearElement.getText().toString());
					 } 
				 }
				 
			 }
			 
			 //Select Month
			 
			 String currMonthXpath =XPathDict.get("getCurrentMonth");
			 WebElement ele2 = getWebElement(currMonthXpath);
			String currMonth=ele2.getText().toString();
			int currMonthNumber = monthNumberMap.get(currMonth);
			if(monthNumber!=currMonthNumber)
			{
				if(monthNumber<currMonthNumber)
				{
					while(monthNumber<currMonthNumber&& monthNumber<=12)
					{
						 WebElement decreaseCurMonthElement = getWebElement(decreaseCurMonthElementXpath);
						 decreaseCurMonthElement.click();
						 
						 WebElement curMonthElement = getWebElement(currMonthXpath);
						 currMonth=curMonthElement.getText().toString();
						 currMonthNumber = monthNumberMap.get(currMonth);
					}
				}else {
					while(monthNumber>currMonthNumber && monthNumber<=12) {
						 WebElement increaseCurMonthElement = getWebElement(increaseCurMonthElementXpath);
						 increaseCurMonthElement.click();
						 
						 WebElement curMonthElement = getWebElement(currMonthXpath);
						 currMonth=curMonthElement.getText().toString();
						 currMonthNumber = monthNumberMap.get(currMonth);
					}
				}
			}
			
			// Select Date
			boolean isDateSelected=false;
			int inputDate=Integer.parseInt(date);
			String tempDateRowXpath="";
			String dateXpath= "";
			int DateRowNo=1;
			int dateOnPortal;
			if(inputDate<20 && !isDateSelected)
			{
				while(!isDateSelected)
				{
					try
					{
						tempDateRowXpath = "//*[@id='gfs-neo-client-reporting']/div/div[2]/div[2]/div[3]/div/div/div/div/div/div/div/div/div/div/div/div/div/div/table/tbody/tr["+DateRowNo+"]"+"//td[text()='" + inputDate + "']";
						 WebElement curDateElement = getWebElement(tempDateRowXpath);
						 String dateText1=curDateElement.getText().trim();
						 dateOnPortal=Integer.parseInt(dateText1);
						 if(dateOnPortal==inputDate) {
					   			Actions actions = new Actions(driver);
					   			actions.doubleClick(curDateElement).perform();
					   			isDateSelected=true;
					   			DateRowNo++;
							}
					}
					catch(Exception e){
						DateRowNo++;
						continue;
					}
						
					
					
		   		
			}
			}
				else
				{
					DateRowNo=2;
					while(!isDateSelected)
					{
						try
						{
							tempDateRowXpath = "//*[@id='gfs-neo-client-reporting']/div/div[2]/div[2]/div[3]/div/div/div/div/div/div/div/div/div/div/div/div/div/div/table/tbody/tr["+DateRowNo+"]"+"//td[text()='" + inputDate + "']";
							 WebElement curDateElement = getWebElement(tempDateRowXpath);
							 String dateText1=curDateElement.getText().trim();
							 dateOnPortal=Integer.parseInt(dateText1);
							 if(dateOnPortal==inputDate) {
						   			Actions actions = new Actions(driver);
						   			actions.doubleClick(curDateElement).perform();
						   			isDateSelected=true;
						   			DateRowNo++;
								}
						}
						catch(Exception e){
							DateRowNo++;
							continue;
						}		
				}
			
				
//			if(!isDateSelected)
//			{tempDateRowXpath = "//*[@id='gfs-neo-client-reporting']/div/div[2]/div[2]/div[3]/div/div/div/div/div/div/div/div/div/div/div/div/div/div/table/tbody/tr[3]//td[text()='" + inputDate + "']";
//			 WebElement curDateElement = getWebElement(tempDateRowXpath);
//				
//			String dateText1=curDateElement.getText().trim();
//			int dateOnPortal=Integer.parseInt(dateText1);
//   		if(dateOnPortal==inputDate) {
//   			Actions actions = new Actions(driver);
//   			actions.doubleClick(curDateElement).perform();
//   			isDateSelected=true;
//   			
//   		}
				
			//}
//		 if(inputDate<20 && !isDateSelected)
//			{
//				tempDateRowXpath = "//*[@id='gfs-neo-client-reporting']/div/div[2]/div[2]/div[3]/div/div/div/div/div/div/div/div/div/div/div/div/div/div/table/tbody/tr[2]//td[text()='" + inputDate + "']";
//				 WebElement curDateElement = getWebElement(tempDateRowXpath);
//					
//				String dateText1=curDateElement.getText().trim();
//				int dateOnPortal=Integer.parseInt(dateText1);
//		   		if(dateOnPortal==inputDate) {
//	    			Actions actions = new Actions(driver);
//	    			actions.doubleClick(curDateElement).perform();
//	    			isDateSelected=true;
//	    		}
//			}
//			else if(inputDate>20 && !isDateSelected)
//			{
//				tempDateRowXpath = "//*[@id='gfs-neo-client-reporting']/div/div[2]/div[2]/div[3]/div/div/div/div/div/div/div/div/div/div/div/div/div/div/table/tbody/tr[5]//td[text()='" + inputDate + "']";
//				 WebElement curDateElement = getWebElement(tempDateRowXpath);
//					
//				String dateText1=curDateElement.getText().trim();
//				int dateOnPortal=Integer.parseInt(dateText1);
//		   		if(dateOnPortal==inputDate) {
//	    			Actions actions = new Actions(driver);
//	    			actions.doubleClick(curDateElement).perform();
//	    			isDateSelected=true;
//	    		}
//			}
//			if(!isDateSelected) {
//				 dateXpath= "//td[not(@class) and text()='" + inputDate + "']";
//				 WebElement curDateElement = getWebElement(dateXpath);
//					String dateText1=curDateElement.getText().trim();
//					int dateOnPortal=Integer.parseInt(dateText1);
//			   		if(dateOnPortal==inputDate) {
//		    			Actions actions = new Actions(driver);
//
//		    			// Perform a double click on the element
//		    			actions.doubleClick(curDateElement).perform();
//		    			isDateSelected=true;
//		    		}
//			}
//			
			
				}
			
		} catch (Exception e) {
			//ExceptionLog.info(e.toString());
			logger.error(e.toString());
			isPassed = false;

		}
	}
	
	
	public static void ClearUSBankAccount(String element, String arg) {
		try {
			String rowListXpath = XPathDict.get(element);
			WebDriverWait wait = new WebDriverWait(driver, 10);

			List<WebElement> rows = wait.until(ExpectedConditions.visibilityOfAllElementsLocatedBy(By.xpath(rowListXpath)));

			int rowCount = rows.size();


			for (WebElement row : rows) {
				List<WebElement> childElements = row.findElements(By.xpath("*"));
				int childSize=childElements.size();
				for (WebElement child : childElements) {
		            String text = child.getText().trim();
		            if (!text.isEmpty()) {
		            //    System.out.println("Text of child element: " + text);
		                
		                if (text.toLowerCase().contains(arg.toLowerCase())) {
		                    
		                    row.click();
		                    SelectUSBankAccount("AccountList");
		                    break; // Exit the loop once the desired text is found
		                }
		                else if(text.contains("NOW VIEWING")|| text.contains("Multiple Selected")) {
		                	row.click();
		                	String ClearAllXpath = XPathDict.get("Clear All");
		                	WebElement ele = getWebElement(ClearAllXpath);
		                	executeScript(ele);
		                	logger.info( " Clear ALL Clicked");
		                	SelectUSBankAccount("AccountList");
		                }
		                else {
		                	logger.info(arg + " not found");
		                }
		            }
				}
			}
			
		} catch (Exception e) {
			//ExceptionLog.info(e.toString());
			logger.error(e.toString());
			isPassed = false;

		}
	}
	

	
	public static void SearchAndClick(String element, String arg) {
		try {
			String rowListXpath = XPathDict.get(element);
			WebDriverWait wait = new WebDriverWait(driver, 10);

			List<WebElement> rows = wait
					.until(ExpectedConditions.visibilityOfAllElementsLocatedBy(By.xpath(rowListXpath)));

			int rowCount = rows.size();

			for (WebElement row : rows) {
				List<WebElement> childElements = row.findElements(By.xpath("*"));
				int childSize=childElements.size();;
				for (WebElement child : childElements) {
		            String text = child.getText().trim();
		            if (!text.isEmpty()) {
		                System.out.println("Text of child element: " + text);
		                
		                if (text.contains(arg)) {
		                    
		                    child.click();
		                    
		                  
		                    
		                    // You can add further actions if needed
		                    break; // Exit the loop once the desired text is found
		                }
		            }
	        }
			}

			
		} catch (Exception e) {
			ExceptionLog.info(e.toString());
			isPassed = false;

		}
	}
	
	public static void waitForAnyFileDownload(String element, String downloadDirectory) {
	    long timeout = 180;  // Set timeout to 180 seconds
	    File dir = new File(downloadDirectory);
	    WebDriverWait wait = new WebDriverWait(driver, 10);

	    // Wait until a valid file (non-crdownload) exists in the download directory
	    wait.until(driver -> {
	        File[] files = dir.listFiles();
	        if (files != null) {
	            for (File file : files) {
	                // If the file is not a .crdownload and has a non-zero length, return true
	                if (file.isFile() && file.length() > 0 && !file.getName().endsWith(".crdownload") && !file.getName().endsWith(".tmp")) {
	                    return true;
	                }
	            }
	        }
	        return false;
	    });
	}



	 public static void DownloadUSBankData(String element, String arg)throws InterruptedException {
			try {
				String NoDataCheckxpath=XPathDict.get("Check for No Data");
				
				WebElement ele = driver.findElement(By.xpath(NoDataCheckxpath));
				
				
				
				

			} catch (Exception e) {
				// ExceptionLog.info(e.toString());
				if(!element.equals("Promethos")) {
					ClickHelper.clickButton("CSV radio Button");
				}
				
				TextBoxHelper.typeInTextBox("File Name Input Box", "a");
				ClickHelper.clickButton("Submit Button");
				
				waitForAnyFileDownload("NA", arg);
				BaseClass.Sleep();
				return;
							}
		}

	public static void MoveRenamedFiles(String element, String arg) {
		try {
			String sourcePath = "E:\\ReportMatchingDestination\\";
			// String targetPath=destPath;
			// String MFName=arg.split("_")[0];
			String targetPath = "E:\\ReportMatchingDestination\\" + ExecuteTestCase_1.module + "\\";

			File f = new File(sourcePath);

			FileFilter filter = new FileFilter() {

				public boolean accept(File f) {
					return f.getName().endsWith("csv");
				}
			};

			File[] files = f.listFiles(filter);
			String fileName;
			// String Name[]=arg.split("_");
			String newName = arg;

			for (File F : files) {

				fileName = F.getName();
				fileName = newName + ".csv";

				File targetFile = new File(targetPath + fileName);
				FileUtils.copyFile(F, targetFile);
				F.delete();

			}
		} catch (Exception e) {
			// ExceptionLog.info(e.toString());
			isPassed = false;
			logger.error(e.toString());
		}
	}

	public static void GetTextValueAfterProcessing_MS(String element1, String elements) throws InterruptedException {

		try {
			String label = "";
			String TextBox = "";
			if (elements.contains(",")) {
				label = elements.split(",")[0];
				TextBox = elements.split(",")[1];

			}
			String value = CheckTextHelper.GetTextInTextArea(label);
			while (true) {
				if (value != null && value.contains("progress")) {
					logger.info(value + "...............!!!!!!!!!!!!!!!!!!");
					value = CheckTextHelper.GetTextInTextArea(label);

				} else {
					break;
				}
			}
			String value_textArea = CheckTextHelper.GetTextInTextArea(TextBox);
			logger.info("Download Status =" + value_textArea);
		} catch (Exception e) {
			ExceptionLog.info(e.toString());
			isPassed = false;
		}

	}
	
	public static void SetNTAccounts(String element,String args)
    {
    	try {
    		String [] accounts = args.split(",");
    		Thread.sleep(2000);
    		for(int i=0; i < accounts.length; i++)
    		{
    			String xpath = XPathDict.get(accounts[i]);
    			getWebElement(xpath);
    			WebElement ele = getWebElement(xpath);
    			executeScript(ele);
    			//ele.click();
    			logger.info( element + " Clicked");
    		}
		} catch (Exception e) {
			logger.error(e.toString());
			isPassed = false;
		}
	}
    

    public static void DownloadNTFiles(String element,String args)
    {
    	try {
    		String [] elements = args.split(",");
    		for(int i=0; i < elements.length; i++)
    		{
    			String xpath = XPathDict.get(elements[i]);
    			if(elements[i].equals("View report"))
    			{
    				WebDriverWait wait = new WebDriverWait(driver, 10);
    				wait.until(ExpectedConditions.or(
    		                ExpectedConditions.visibilityOfElementLocated(By.xpath("//span[contains(text(), 'report has no data')]")),
    		                ExpectedConditions.visibilityOfElementLocated(By.xpath(xpath))
    		            ));
    				if (isElementPresent(driver, By.xpath(xpath)))
    				{
        			    waitForElement(xpath);
        			    WebElement ele = getWebElement(xpath);
            			executeScript(ele);    					
    				}
    				else if(isElementPresent(driver, By.xpath("//span[contains(text(), 'report has no data')]")))
    				{
    					
    				}
    			}
    			else
    			{
    				getWebElement(xpath);
    			WebElement ele = getWebElement(xpath);
    			executeScript(ele);
    			}
    			logger.info( element + " Clicked");
    		}
    		Thread.sleep(1000);
		} catch (Exception e) {
			logger.error(e.toString());
			isPassed = false;
		}
    }
    
    public static void CheckIfCheckBoxisSelected(String element,String args)
    {
    	try {
    		String [] elements = args.split(",");
    	for(int i=0;i<elements.length;i++)
    	{
    		String xpath = XPathDict.get(elements[i]);
    		waitForElement(xpath);
		    WebElement checkbox = getWebElement(xpath);

		 // Assuming the <i> element within <td> represents the checkbox
		    WebElement checkboxElement = checkbox.findElement(By.xpath(".//i"));

		    // Check if the checkbox is selected
		    String classAttributeValue = checkboxElement.getAttribute("class");

		    // Determine the state based on class attribute or other indicators
		    boolean isChecked = classAttributeValue.contains("fa-check") || classAttributeValue.contains("fa-check-circle") || classAttributeValue.contains("checkbox__input");
            // Check if the checkbox is selected/checked
            if (isChecked) {
                System.out.println("Checkbox is selected.");
            } else {
                checkbox.click();           }
    	}
        }catch (Exception e) {
			logger.error(e.toString());
			isPassed = false;
		}
    }
    
    public static boolean isElementPresent(WebDriver driver, By locator) {
        try {
            driver.findElement(locator);
            return true;
        } catch (org.openqa.selenium.NoSuchElementException e) {
            return false;
        }
    }
    
    public static void SelectDateFromCalender_UMB(String element, String arg) {
		try {
			arg=replaceDateTimePlaceholders(arg);
			String arr[]=arg.split(",");
			String date=arr[2].replace(" ","");
			String month = arr[0].replace(" ","");
			int monthNumber = (Month.valueOf(arr[0].replace(" ","").toUpperCase())).getValue();
			int year = Integer.parseInt(arr[1].replace(" ",""));
			
			String decreaseCurYearElementXpath=XPathDict.get("Decrease Year");
			String increaseCurYearElementXpath=XPathDict.get("Increase Year");
			String decreaseCurMonthElementXpath= XPathDict.get("Decrease Month");
			String increaseCurMonthElementXpath=XPathDict.get("Increase Month");
			String currYearxpath=XPathDict.get("getCurrentYear");
			
			if(element.equals("ToDate"))
			{
				decreaseCurYearElementXpath = decreaseCurYearElementXpath.replace("FromDate","ToDate");
				increaseCurYearElementXpath = increaseCurYearElementXpath.replace("FromDate","ToDate");
				decreaseCurMonthElementXpath = decreaseCurMonthElementXpath.replace("FromDate","ToDate");
				increaseCurMonthElementXpath = increaseCurMonthElementXpath.replace("FromDate","ToDate");
				currYearxpath = currYearxpath.replace("FromDate","ToDate");
			}
			if(element.equals("As of Date"))
			{
				decreaseCurYearElementXpath = decreaseCurYearElementXpath.replace("FromDate","Date");
				increaseCurYearElementXpath = increaseCurYearElementXpath.replace("FromDate","Date");
				decreaseCurMonthElementXpath = decreaseCurMonthElementXpath.replace("FromDate","Date");
				increaseCurMonthElementXpath = increaseCurMonthElementXpath.replace("FromDate","Date");
				currYearxpath = currYearxpath.replace("FromDate","Date");
			}
			// Select Year
			WebElement ele1 = getWebElement(currYearxpath);
			String currMonthYear = ele1.getText().toString();
			int currYear=Integer.parseInt(currMonthYear.split(", ")[1].toString());			 
			 if(year!=currYear)
			 {
				 if(year<currYear)
				 {
					 while(year<currYear) {
						 WebElement decreaseCurYearElement = getWebElement(decreaseCurYearElementXpath);
						 decreaseCurYearElement.click();
						 
						 WebElement ele2 = getWebElement(currYearxpath);
							currMonthYear = ele2.getText().toString();
						 currYear=Integer.parseInt(currMonthYear.split(", ")[1].toString());
					 } 
				 }else {
					 while(year>currYear) {
						 WebElement increaseCurYearElement = getWebElement(increaseCurYearElementXpath);
						 increaseCurYearElement.click();
						 
						 WebElement ele2 = getWebElement(currYearxpath);
							currMonthYear = ele2.getText().toString();
						 currYear=Integer.parseInt(currMonthYear.split(", ")[1].toString());
					 } 
				 }
				 
			 }
			 
			 //Select Month
			 
			 //String currMonthXpath =XPathDict.get("getCurrentMonth");
			 //WebElement ele2 = getWebElement(currMonthXpath);
			 int currMonthNumber = (Month.valueOf(currMonthYear.split(", ")[0].toUpperCase())).getValue();
			if(monthNumber!=currMonthNumber)
			{
				if(monthNumber<currMonthNumber)
				{
					while(monthNumber<currMonthNumber&& monthNumber<=12)
					{
						 WebElement decreaseCurMonthElement = getWebElement(decreaseCurMonthElementXpath);
						 decreaseCurMonthElement.click();
						 WebElement ele2 = getWebElement(currYearxpath);
							currMonthYear = ele2.getText().toString();
							currMonthNumber = (Month.valueOf(currMonthYear.split(", ")[0].toUpperCase())).getValue();
					}
				}else {
					while(monthNumber>currMonthNumber && monthNumber<=12) {
						 WebElement increaseCurMonthElement = getWebElement(increaseCurMonthElementXpath);
						 increaseCurMonthElement.click();
						 WebElement ele2 = getWebElement(currYearxpath);
							currMonthYear = ele2.getText().toString();
							currMonthNumber = (Month.valueOf(currMonthYear.split(", ")[0].toUpperCase())).getValue();
					}
				}
			}
			
			// Select Date
			boolean isDateSelected=false;
			int inputDate=Integer.parseInt(date);
			String tempDateRowXpath="";
			String dateXpath="";
			dateXpath= "//td[contains(@title, '"+month+" "+date+", "+year+"')]/a[text()='"+inputDate+"']";
			
			if(!isDateSelected) {
				 //dateXpath= "//td[not(@class) and text()='" + inputDate + "']";
				 WebElement curDateElement = getWebElement(dateXpath);
					String dateText1=curDateElement.getText().trim();
					int dateOnPortal=Integer.parseInt(dateText1);
			   		if(dateOnPortal==inputDate) {
		    			Actions actions = new Actions(driver);

		    			// Perform a double click on the element
		    			actions.doubleClick(curDateElement).perform();
		    			isDateSelected=true;
		    		}
			}
			
			Thread.sleep(1000);
			
			
		} catch (Exception e) {
			//ExceptionLog.info(e.toString());
			logger.error(e.toString());
			isPassed = false;

		}
	}
    
    public static void SelectDateFromCalender_SG(String element, String arg)
    {
					try {
						arg=replaceDateTimePlaceholders(arg);
						String arr[]=arg.split("/");
						String date=arr[1].replace(" ","");
						
						// Select Date
									boolean isDateSelected=false;
									int inputDate=Integer.parseInt(date);
									String dateXpath=XPathDict.get(element);
									dateXpath= dateXpath.replace(element, date);
									
									if(!isDateSelected) {
										 //dateXpath= "//td[not(@class) and text()='" + inputDate + "']";
										 WebElement curDateElement = getWebElement(dateXpath);
											String dateText1=curDateElement.getText().trim();
											int dateOnPortal=Integer.parseInt(dateText1);
									   		if(dateOnPortal==inputDate) {
								    			Actions actions = new Actions(driver);

								    			// Perform a double click on the element
								    			actions.doubleClick(curDateElement).perform();
								    			isDateSelected=true;
								    		}
									}
						Thread.sleep(1000);
					} catch (InterruptedException e) {
						// TODO Auto-generated catch block
						e.printStackTrace();
					}
	}
    public static void SelectDateFromList(String element, String arg) {
	    try {
	        arg = replaceDateTimePlaceholders(arg);
	        String xpath = XPathDict.get(element);
	        WebDriverWait wait = new WebDriverWait(driver, 10);
	        
	        // Wait for the dropdown element to be visible and then click to open it
	        wait.until(ExpectedConditions.visibilityOfElementLocated(By.xpath(xpath)));
	        WebElement ele = driver.findElement(By.xpath(xpath));
	        executeScript(ele);
	      
	        
	        
	        // Construct XPath for the specific date in the dropdown
	        String dateXpath = "//span[contains(text(), '" + arg + "')]";
	        
	        // Wait for the date element to be visible and then click it
	        wait.until(ExpectedConditions.visibilityOfElementLocated(By.xpath(dateXpath)));
	        WebElement dateElement = driver.findElement(By.xpath(dateXpath));
	        executeScript(dateElement);
	        
	        
	        logger.info("Selected date " + arg + " from " + element);
	    } catch (Exception e) {
	        logger.error(e.toString());
	        isPassed = false;
	    }
	}
public static void ClearInput(String element,String arg) {
		
		try {
		String xpath=XPathDict.get(element);
		WebDriverWait wait = new WebDriverWait(driver, 10);
		wait.until(ExpectedConditions.visibilityOfElementLocated(By.xpath(xpath)));
		WebElement ele = driver.findElement(By.xpath(xpath));
		try {
            // Use JavaScript to clear the input field
            JavascriptExecutor js = (JavascriptExecutor) driver;
            js.executeScript("arguments[0].value = '';", ele);
            // Trigger the input and change events
            js.executeScript("arguments[0].dispatchEvent(new Event('input'));", ele);
            js.executeScript("arguments[0].dispatchEvent(new Event('change'));", ele);

            logger.info("Cleared input field: " + element);
        } catch (Exception e) {
            logger.error("Error while clearing input field: " + e.toString());
            isPassed = false;
        }
		
		
		logger.info("Typed "+arg+" in "+element);
		
		}
		catch(Exception e)
		{
			//ExceptionLog.info(e.toString());
			logger.error(e.toString());
			isPassed=false;
		}
	
	}
public static void GreenTabsCheck(String element) {
    try {
        String xpath = XPathDict.get(element);
        WebDriverWait wait = new WebDriverWait(driver, 10);
        wait.until(ExpectedConditions.visibilityOfAllElementsLocatedBy(By.xpath(xpath)));
        List<WebElement> elements = driver.findElements(By.xpath(xpath));

        for (WebElement ele : elements) {
            if (!ele.getAttribute("class").contains("aurora-tag-green")) {
                isBreakStep = false;
                return;
            }
        }
       
    } catch (Exception e) {
        logger.error(e.toString());
        isPassed = false;
    }
}
public static void SelectBTIGFund(String element) {
    try {

        // Variable to track if the first element is clicked
        boolean isFirstElementClicked = false;
        for (Map.Entry<String, List<String>> entry : ReportList1.entrySet()) {
            String key = entry.getKey();
            List<String> value = entry.getValue();
            // Create Actions instance
            Actions actions = new Actions(driver);
            // Iterate through the list if needed
            for (String item : value) {
                String fundXpath = item;
                WebElement ele = driver.findElement(By.xpath(fundXpath));
                // Click on the first element without pressing Ctrl key
                if (!isFirstElementClicked) {
                    ele.click();
                    isFirstElementClicked = true;
                } else {
                    // Hold Ctrl key and click on subsequent elements
                	//((JavascriptExecutor) driver).executeScript("arguments[0].scrollIntoView(true);", ele);
                    actions.keyDown(Keys.CONTROL)
                            .click(ele)
                            .keyUp(Keys.CONTROL)
                            .build()
                            .perform();
                }
                // Assuming you want to log that the key is clicked
                logger.info(key + " Clicked");
            }
        }
        
    }catch(Exception e) {
        
    }
}

public static void clickOnSiblingBasedOnChildText(String element,String arg) {
    // Get all rows in the specified column
	String textToFind=arg;
	String columnXpath=XPathDict.get(element);
	String siblingXpath=XPathDict.get("siblingXpath");
	String ancestorXpath=XPathDict.get("ancestorXpath");
	String textToFindXpath=".//*[text() = '" + textToFind + "']";
    List<WebElement> rows = driver.findElements(By.xpath(columnXpath));

    for (WebElement row : rows) {
        // Check if any child of the row contains the specific text
       // List<WebElement> matchingElements = row.findElements(By.xpath(textToFindXpath));
        List<WebElement> matchingElements = row.findElements(By.xpath(".//*[contains(normalize-space(text()), '" + textToFind + "')]"));

     // Iterate through matching elements (if any)
     for (WebElement matchingElement : matchingElements) {
         System.out.println("Found: " + matchingElement.getText());
         // You can interact with the matching element here
     }
        
        if (!matchingElements.isEmpty()) {  // If a matching child is found
            // Find the parent row (ancestor with role='row')
           // WebElement parentRow = row.findElement(By.xpath("./ancestor::div[contains(@class, 'ag-row')]"));
            WebElement parentRow = row.findElement(By.xpath(ancestorXpath));

            // Find the sibling element within the same row
            WebElement siblingElement = parentRow.findElement(By.xpath(siblingXpath));

            // Click on the sibling element
            siblingElement.click();
            break; // Exit loop after clicking the first match
        }
        
    }
    String ancestorXpath2=XPathDict.get("ancestorXpath");
}

public static void SelectCheckbox(String element) {
	try {
		 // Locate the checkbox element
		String elementId=XPathDict.get(element);
        WebElement checkbox = driver.findElement(By.id(elementId));

        // Check if the checkbox is not selected
        if (!checkbox.isSelected()) {
            checkbox.click(); // Tick the checkbox
            logger.info("Checkbox was unticked, now ticked.");
        } else {
        	logger.info("Checkbox is already ticked, no action taken.");
        }

	} catch (Exception e) {
		logger.info(e.toString());
		isPassed = false;

	}
	

}

public static void UnselectCheckbox(String element) {
    try {
        // Locate the checkbox element
        String elementId = XPathDict.get(element);
        WebElement checkbox = driver.findElement(By.id(elementId));

        // Check if the checkbox is selected
        if (checkbox.isSelected()) {
        	// Create an instance of the Actions class
        				Actions actions = new Actions(driver);

        				
        				actions.click(checkbox).perform();
            //checkbox.click(); // Untick the checkbox
            logger.info("Checkbox was ticked, now unticked.");
        } else {
        	logger.info("Checkbox is already unticked, no action taken.");
        }

    } catch (Exception e) {
        logger.info(e.toString());
        isPassed = false;
    }
}

public static void SelectBTIGBCCMFund(String element) {
    try {
        // Variable to track if the first element is clicked
        boolean isFirstElementClicked = false;

        for (Map.Entry<String, List<String>> entry : ReportList1.entrySet()) {
            String key = entry.getKey();
            List<String> value = entry.getValue();
            Actions actions = new Actions(driver);

            for (String fundXpath : value) {
                WebElement ele = driver.findElement(By.xpath(fundXpath));

                // Scroll the element into view
                ((JavascriptExecutor) driver).executeScript("arguments[0].scrollIntoView(true);", ele);
                
                // Click on the first element without pressing Ctrl key
                if (!isFirstElementClicked) {
                    ele.click();
                    isFirstElementClicked = true;
                } else {
                    // Hold Ctrl key and click on subsequent elements
                    actions.keyDown(Keys.CONTROL)
                           .click(ele)
                           .keyUp(Keys.CONTROL)
                           .build()
                           .perform();
                }

                // Log that the key is clicked
                logger.info(key + " Clicked");
            }
        }
        
    } catch (Exception e) {
        logger.error(e.toString());
    }
}


public static void CheckDateInElement(String element,String arg) {
	
	try {
		arg=replaceDateTimePlaceholders(arg);
	String xpath=XPathDict.get(element);
	WebDriverWait wait = new WebDriverWait(driver, 10);
	wait.until(ExpectedConditions.visibilityOfElementLocated(By.xpath(xpath)));
	WebElement ele = driver.findElement(By.xpath(xpath));
	  // Retrieve the value of the element
    String actualValue = ele.getText();
    if (actualValue != null && actualValue.contains(arg)) {
        logger.info("Element " + element + " contains the date: " + arg);
        
    } else {
    	isBreakStep=false;
    	logger.info("Date verification failed. Expected: " + arg + ", but found: " + actualValue);
    }
} catch (Exception e) {
    logger.error(e.toString());
    isPassed = false;
}

}


public static void SelectOptionByVisibleText(String element, String arg) {
    try {
        String xpath = XPathDict.get(element);
        WebDriverWait wait = new WebDriverWait(driver, 10);
        wait.until(ExpectedConditions.visibilityOfElementLocated(By.xpath(xpath)));
        WebElement selectElement = driver.findElement(By.xpath(xpath));
        
        Select dropdown = new Select(selectElement);
        dropdown.selectByVisibleText(arg);
        
        logger.info("Selected option " + arg + " in " + element);
    } catch (Exception e) {
        logger.error(e.toString());
        isPassed = false;
    }
}





//

}
