package com.nirvana.Helper;
import java.util.ArrayList;
import java.util.List;
import com.nirvana.TestCases.BaseClass;
import org.apache.poi.xssf.usermodel.XSSFRow;
import org.apache.poi.ss.usermodel.*;
import org.apache.poi.xssf.usermodel.XSSFWorkbook;
import org.openqa.selenium.By;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.chrome.ChromeDriver;

import java.io.FileOutputStream;
import java.io.IOException;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;

public class Timer extends BaseClass {


	    private static long startTime;
	    private static long endTime;
	    private static long totalTimeTaken;
	

	    public static void startTimer() {
	        endTime = 0;
			startTime = 0;
			startTime = System.currentTimeMillis();
			//com.nirvana.Helper.HelperClass.PrintConsole("start time is "+startTime);
	    }

	    public static void endTimer() {

			endTime = System.currentTimeMillis();
			//com.nirvana.Helper.HelperClass.PrintConsole("end time "+endTime);
	    }

	    public static long calculateTotalTime() {
	     	    
	    	 totalTimeTaken= endTime - startTime;
			 HelperClass.PrintConsole("Total time "+totalTimeTaken);
			//com.nirvana.Helper.HelperClass.PrintConsole("Total time "+totalTimeTaken);
			 return  totalTimeTaken;
	    }

		public static void resetTime(){
			startTime = 0;
			endTime = 0;
			startTimer();
		}
	
	    public static void writeToExcel() {
	        Workbook workbook = new XSSFWorkbook();
	        Sheet sheet = workbook.createSheet("Page Load Times");

	        // Create header row
	        Row headerRow = sheet.createRow(0);
	        headerRow.createCell(0).setCellValue("Timestamp");
	        headerRow.createCell(1).setCellValue("Action Name");
	        headerRow.createCell(2).setCellValue("Total Time Taken (ms)");

	        // Create data row
	        Row dataRow = sheet.createRow(1);
	        DateTimeFormatter dtf = DateTimeFormatter.ofPattern("yyyy/MM/dd HH:mm:ss");
	        LocalDateTime now = LocalDateTime.now();
	        dataRow.createCell(0).setCellValue(dtf.format(now));
	        dataRow.createCell(1).setCellValue("Page Load");
	        dataRow.createCell(2).setCellValue(totalTimeTaken);

	        // Write the output to a file
	        try (FileOutputStream fileOut = new FileOutputStream("E:\\PageLoadTimes.xlsx")) {
	            workbook.write(fileOut);
	        } catch (IOException e) {
	            e.printStackTrace();
	        } finally {
	            try {
	                workbook.close();
	            } catch (IOException e) {
	                e.printStackTrace();
	            }
	        }
	    }
	}

