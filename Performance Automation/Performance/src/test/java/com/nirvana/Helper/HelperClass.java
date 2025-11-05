package com.nirvana.Helper;

import com.nirvana.TestCases.ExecuteTestCase_1;
import org.apache.jena.base.Sys;
import org.openqa.selenium.By;
import org.openqa.selenium.Keys;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.interactions.Actions;

import java.io.BufferedWriter;
import java.io.FileWriter;
import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.nio.file.StandardOpenOption;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.List;

import static com.nirvana.TestCases.BaseClass.*;

public class HelperClass {
    static WebDriver driver;
    static Actions action;
	public static boolean skipTime = false;

    // Constructor
    public HelperClass(WebDriver driver) {
        this.driver = driver;
        this.action = new Actions(driver);
    }
    public static void CompareFieldDataString(String element, String IncrementValue){
        try {
            String xpath = XPathDict.get(element);
            System.out.println(xpath);
            Long CurrentTime = System.currentTimeMillis();
            boolean IsChanged = false;
            while (!IsChanged) {
                if (System.currentTimeMillis() - CurrentTime > 120000) {
                    break;
                }
                //String fieldValue = FieldText;
                //System.out.println(fieldValue);
                //int incrementValue = Integer.parseInt(IncrementValue);
                //int abc = (int) Math.round(fieldValue + incrementValue);
                String newVal = (driver.findElement(By.xpath(xpath)).getText().replace(",", ""));
                //int roundedNewVal = (int) Math.round(newVal);
                if (IncrementValue.equals(newVal)) {              // TimerTime = 5Sec;;; TimerTime = 3Sec;Timer.Time = 7Sec
                    IsChanged = true;
                    //com.nirvana.Helper.HelperClass.PrintConsole(abc + "is equal to " + driver.findElement(By.xpath(xpath)).getText());
                }
            }
        }
        catch (Exception e){
            com.nirvana.Helper.HelperClass.PrintConsole(e);
        }
    }
    private static String FieldText = "";
    public static void GetFieldText(String element){
        try{
			skipTime = false;
        String xpath=XPathDict.get(element);
        waitForElement(xpath);
        WebElement ele=getWebElement(xpath);
        FieldText = ele.getText();
        String fieldTextWithoutCommas = FieldText.replace(",", "");
        FieldText = fieldTextWithoutCommas;
        com.nirvana.Helper.HelperClass.PrintConsole("Get value is "+FieldText);
		try{
			double fieldValue = Double.parseDouble(FieldText);
		}
		catch (Exception ex){
			skipTime = true;
		}
		
		}

        catch (Exception e){
            com.nirvana.Helper.HelperClass.PrintConsole(e);
        }
    }
    public static void CompareFieldData(String element, String IncrementValue){
		
        try {
			if(skipTime)
				return;
            System.out.println("Compare field data triggered");
            String xpath = XPathDict.get(element);
			System.out.println(xpath);
            Long CurrentTime = System.currentTimeMillis();
            boolean IsChanged = false;
            while (!IsChanged) {
				
                if (System.currentTimeMillis() - CurrentTime > 120000) {
                    break;
                }
                double fieldValue = Double.parseDouble(FieldText);
                com.nirvana.Helper.HelperClass.PrintConsole(fieldValue);
                int incrementValue = Integer.parseInt(IncrementValue);
                int abc = (int) Math.round(fieldValue + incrementValue);
                double newVal = Double.parseDouble(driver.findElement(By.xpath(xpath)).getText().replace(",", ""));
                com.nirvana.Helper.HelperClass.PrintConsole("field value is "+FieldText);
                System.out.println("parsed field value is "+fieldValue);
                System.out.println("new value is "+fieldValue);
                int roundedNewVal = (int) Math.round(newVal);
                if (fieldValue != roundedNewVal) {              // TimerTime = 5Sec;;; TimerTime = 3Sec;Timer.Time = 7Sec
                    System.out.println("Time taken by Postion change "+(System.currentTimeMillis() - CurrentTime));
                    IsChanged = true;
                    //com.nirvana.Helper.HelperClass.PrintConsole(abc + "is equal to " + driver.findElement(By.xpath(xpath)).getText());
                }
            }
            //Timer.calculateTotalTime();
        }
        catch (Exception e){
            com.nirvana.Helper.HelperClass.PrintConsole(e);
        }
    }

    public static void CompareFieldDataFull(String element, String IncrementValue){
        try {
			if(skipTime)
				return;
            String xpath = XPathDict.get(element);
            Long CurrentTime = System.currentTimeMillis();
            boolean IsChanged = false;
            while (!IsChanged) {
                if (System.currentTimeMillis() - CurrentTime > 120000) {
                    break;
                }
                double fieldValue = Double.parseDouble(FieldText);
                int incrementValue = Integer.parseInt(IncrementValue);
                int abc = (int) Math.round(fieldValue + incrementValue);
                com.nirvana.Helper.HelperClass.PrintConsole(abc);
                double newVal = Double.parseDouble(driver.findElement(By.xpath(xpath)).getText().replace(",", ""));
                int roundedNewVal = (int) Math.round(newVal);
                com.nirvana.Helper.HelperClass.PrintConsole(abc);
                com.nirvana.Helper.HelperClass.PrintConsole(roundedNewVal);
                if (roundedNewVal == abc) {              // TimerTime = 5Sec;;; TimerTime = 3Sec;Timer.Time = 7Sec
                    com.nirvana.Helper.HelperClass.PrintConsole("Time taken by Postion change "+(System.currentTimeMillis() - CurrentTime));
                    IsChanged = true;
                    //com.nirvana.Helper.HelperClass.PrintConsole(abc + "is equal to " + driver.findElement(By.xpath(xpath)).getText());
                }
            }
        }
        catch (Exception e){
            com.nirvana.Helper.HelperClass.PrintConsole(e);
        }
    }


    void ActionClick(String element){
        try {
            com.nirvana.Helper.HelperClass.PrintConsole(element + " To be Click");
            String xpath = XPathDict.get(element);
            HelperClass.PrintConsole("Xpath for "+element+" Is "+xpath);
            waitForElement(xpath);
            WebElement ele = getWebElement(xpath);
            action.click(ele).perform();
            com.nirvana.Helper.HelperClass.PrintConsole(element + " Clicked");
        }
        catch (Exception e){
            com.nirvana.Helper.HelperClass.PrintConsole(e);
        }
        //ele.click();
    }
    public static void CaptureChangeData(String elements) {
        try {
            time.clear();
            time1.clear();
            String[] Elements = elements.split(",");
            List<Thread> threads = new ArrayList<>();
            for(int i = 0; i<Elements.length;i++){
                time.add(300000L);
                time1.add(300000L);
            }
            for (int i = 0; i < Elements.length; i++) {
                final int fieldIndex = i; // Ensure threadId is final or effectively final
                Thread thread = new Thread(() -> {
                    captureChange(Elements[fieldIndex], fieldIndex);
                });
                threads.add(thread);
                thread.start();
            }

            for (Thread thread : threads) {
                try {
                    thread.join();
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
            }
        }
        catch (Exception e){
            com.nirvana.Helper.HelperClass.PrintConsole(e);
        }
    }


    public static List<Long> time = new ArrayList<>();
    public static List<Long> time1 = new ArrayList<>();
    private static void captureChange(String element,int theadId) {
        try {
       com.nirvana.Helper.HelperClass.PrintConsole("Monitoring changes of "+element);

        long currentTime = System.currentTimeMillis();
        String xpath = XPathDict.get(element);
        String data = driver.findElement(By.xpath(xpath)).getText();
        com.nirvana.Helper.HelperClass.PrintConsole("Initial data is for "+element+" is "+data);
        Long SecondDataChangeTime = 0L;
        for(int i = 0; i<3; i++) {                  // i  =0;    TimerTime = 1Sec;;; i = 1; Timer.Time = 0Sec;;; i = 2; Timer.Time = 3Sec
            boolean IsChanged = false;
            while (!IsChanged) {
                if(((System.currentTimeMillis()-currentTime)/60000)>5){
                    com.nirvana.Helper.HelperClass.PrintConsole("Time Over for Fetching Price Update for "+element);
                    isPassed = false;
                    break;
                }

                if (!data.equals(driver.findElement(By.xpath(xpath)).getText())) {              // TimerTime = 5Sec;;; TimerTime = 3Sec;Timer.Time = 7Sec
                    com.nirvana.Helper.HelperClass.PrintConsole("Data is Changed for first time for "+element+" is "+driver.findElement(By.xpath(xpath)).getText());
                    //com.nirvana.Helper.HelperClass.PrintConsole("Previous data is"+data);
					if(i==0) {
                        data = driver.findElement(By.xpath(xpath)).getText();             // True;;;False;;;False
                        currentTime = System.currentTimeMillis();
                        break;
                    }         // TimerTime = 0Sec;;; Not Execute
                    IsChanged = true;
                    if(i==1) {
                        time.set(theadId,System.currentTimeMillis() - currentTime);
                        //if(System.currentTimeMillis() - currentTime> ExecuteTestCase_1.ThresHoldValues.get("ChangeMarketData"))
                          //  WindowHelper.CaptureImage("ChangeMarketData","ThresHoldBreach");
                        SecondDataChangeTime = System.currentTimeMillis();
                        data = driver.findElement(By.xpath(xpath)).getText();
                        com.nirvana.Helper.HelperClass.PrintConsole("2nd Time data changed for"+element+" is "+data);
                    }
                    if(i==2) {
                        time1.set(theadId, System.currentTimeMillis() - SecondDataChangeTime);
                        com.nirvana.Helper.HelperClass.PrintConsole("Third Time data Changed for "+element);
                       // if(System.currentTimeMillis() - SecondDataChangeTime > ExecuteTestCase_1.ThresHoldValues.get("ChangeMarketData"))
                          //  WindowHelper.CaptureImage("ChangeMarketData","ThresHoldBreach");
                    }
                }
            }
        }

        }
        catch (Exception e){
            com.nirvana.Helper.HelperClass.PrintConsole(e);
        }
    }

    void ActionSendKeys(String element, String Text){
        try {
            String xpath = XPathDict.get(element);
            waitForElement(xpath);

            WebElement ele = getWebElement(xpath);
            action.moveToElement(ele).click().perform();
            action.sendKeys(ele, Text).perform();
            action.sendKeys(ele, Keys.TAB).perform();
        }
        catch (Exception e){
            com.nirvana.Helper.HelperClass.PrintConsole(e);
        }
    }
    private static final DateTimeFormatter formatter = DateTimeFormatter.ofPattern("dd-MM-yyyy HH:mm:ss");
    public static void PrintConsole(Object msg){

        String timestamp = LocalDateTime.now().format(formatter);
        System.out.println(timestamp+" "+msg);
        //com.nirvana.Helper.HelperClass.PrintConsole(timestamp + "   "+msg);
        try (BufferedWriter writer = Files.newBufferedWriter(Paths.get("log.txt"),
                StandardOpenOption.CREATE, StandardOpenOption.APPEND)) {
            writer.write(timestamp + "   " + msg);
            writer.newLine();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}
