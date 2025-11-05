package com.nirvana.TestCases;

import java.awt.*;
import java.io.*;

import com.google.api.services.sheets.v4.model.Spreadsheet;
import com.google.api.services.sheets.v4.model.UpdateValuesResponse;
import com.google.api.services.sheets.v4.model.ValueRange;
import com.nirvana.GoogleDrive.*;
import com.nirvana.Helper.HelperClass;
import com.nirvana.Helper.Timer;
//import org.apache.commons.math3.stat.descriptive.rank.Percentile;
import com.nirvana.Helper.WindowHelper;
import org.apache.poi.ss.usermodel.*;
import org.apache.poi.ss.util.CellReference;
import org.apache.poi.xssf.usermodel.XSSFCell;
import org.apache.poi.xssf.usermodel.XSSFRow;
import org.apache.poi.xssf.usermodel.XSSFSheet;
import org.apache.poi.xssf.usermodel.XSSFWorkbook;
import org.testng.annotations.Optional;
import org.testng.annotations.Parameters;
import org.testng.annotations.Test;

import java.net.InetAddress;
import java.security.GeneralSecurityException;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Properties;

import com.nirvana.Mapping.Mapping;
import com.nirvana.Utilities.XLUtils;


public class ExecuteTestCase_1 extends BaseClass {
    public static boolean IsAnyIssue  = false;
    public static XSSFWorkbook book;
    public static XSSFSheet sheet;
    static List<String> SheetList = new ArrayList<>();
    public static XSSFSheet moduleSheet;
    public static XSSFRow row;
    public static XSSFCell cell;
    public static HashMap<String,Long> ThresHoldValues = new HashMap<>();
    public static FileInputStream _readExcel;
    public static String module;

    public static ArrayList<ArrayList<Object>> data1 = new ArrayList<>();

    @Test
    @Parameters("Iteration")
    public void Test(@Optional String Iteration) throws IOException, InterruptedException, GeneralSecurityException, AWTException {
        ThresHoldValues.put("TT",4000L);
        ThresHoldValues.put("Blotter",7000L);
        ThresHoldValues.put("Compliance",4000L);
        ThresHoldValues.put("Send",4000L);
        ThresHoldValues.put("Stage",4000L);
        ThresHoldValues.put("Allocate",4000L);
        ThresHoldValues.put("Replace",4000L);
        ThresHoldValues.put("RTPNLPageLoad",4000L);
        ThresHoldValues.put("TradeNewSubRTPNLIncrease",4000L);
        ThresHoldValues.put("TradeNewSubRTPNLUnwind",4000L);
        ThresHoldValues.put("ChangeMarketData",10000L);
        ThresHoldValues.put("TradeNewSub",4000L);
        ThresHoldValues.put("EditOrder",4000L);
        ThresHoldValues.put("ReloadOrder_SubOrder",4000L);
        ThresHoldValues.put("ReloadOrder",4000L);
        ThresHoldValues.put("RTPNLFilterTime",10000L);
        ThresHoldValues.put("PositionChange",10000L);
        ThresHoldValues.put("PositionChangeFirst",10000L);
        ThresHoldValues.put("TTAMT",10000L);
        ThresHoldValues.put("TTBPS",10000L);
        ThresHoldValues.put("TTPST_EditOrder",10000L);
        ThresHoldValues.put("TTPST_EditOrder_BPS",10000L);
        ThresHoldValues.put("TTAMT_AccountChange",10000L);
        ThresHoldValues.put("TTPercent_AccountChange",10000L);
        ThresHoldValues.put("TTBPS_AccountChange",10000L);
        ThresHoldValues.put("TTAMT_PriceChange",10000L);
        ThresHoldValues.put("TTPercent_PriceChange",10000L);
        ThresHoldValues.put("TTBPS_PriceChange",10000L);
        ThresHoldValues.put("TTAMT_CheckCompliance",10000L);
        ThresHoldValues.put("TTPercent_CheckCompliance",10000L);
        ThresHoldValues.put("TTBPS_CheckCompliance",10000L);
        ThresHoldValues.put("TTAMT_Trade",10000L);
        ThresHoldValues.put("TTPercent_Trade",10000L);
        ThresHoldValues.put("TTBPS_Trade",10000L);
        ThresHoldValues.put("TTPercent",10000L);
        ThresHoldValues.put("Login",60000L);
        ThresHoldValues.put("AdjustIncreasePosition",60000L);
        ThresHoldValues.put("AdjustDecreasePosition",60000L);
        ThresHoldValues.put("AdjustSetPosition",60000L);
        ThresHoldValues.put("ExpandGrouping",4000L);


        BaseClass.Iteration = Iteration != null ? Integer.parseInt(Iteration) : 1;

        String Path = (System.getProperty("user.dir") + "//Data//Data.xlsx");
        String bnSheetName;

        bnSheetName = "TestCases";

        boolean flag = false;
        _readExcel = new FileInputStream(new File(Path));
        //CreateSummarySheet("Logs.xlsx","SummaryLog.xlsx");
		//Spreadsheet spreadsheet = CreateExcelSheet.ReuseSheet("13ehuHZTj7Lf29B5AUJBQTjuh1cEzyWHxUy2cq-1TQ7w");
        
        //Spreadsheet spreadsheet = CreateExcelSheet.CreateSheet("Newly Created Sheet");
        InetAddress inetAddress = InetAddress.getLocalHost();

        // Get the machine name (hostname)
        String machineName = inetAddress.getHostAddress();
        Spreadsheet spreadsheet = CreateExcelSheet.CreateSheet("13ehuHZTj7Lf29B5AUJBQTjuh1cEzyWHxUy2cq-1TQ7w",machineName+" Performance Report"+String.valueOf(LocalDate.now()));
        //ArrayList<Object> data = new ArrayList<Object>(Arrays.asList("Module Name", "Time (In seconds)"));

        book = new XSSFWorkbook(_readExcel);
        sheet = book.getSheet(bnSheetName);

        String testCase = "";
        //List to store business steps
        List<String> stepList = new ArrayList<String>();


        for (int i = 1; i <= sheet.getLastRowNum() + 1; i++) {
            row = sheet.getRow(i);
            if (row != null) { //if(!row.getCell(0).getStringCellValue().isEmpty())
                if (row.getCell(0) != null && !row.getCell(0).getStringCellValue().isEmpty()) {
                    stepList.clear();
                    testCase = row.getCell(0).getStringCellValue();
                    stepList.add(row.getCell(1).getStringCellValue() + "," + row.getCell(2).getStringCellValue());
                }
                //else if(!row.getCell(1).getStringCellValue().isEmpty())

                else if (row.getCell(1) != null && !row.getCell(1).getStringCellValue().isEmpty()) {
                    stepList.add(row.getCell(1).getStringCellValue() + "," + row.getCell(2).getStringCellValue());

                }
            } else {
                logger.info("Running test case : " + testCase);
                String TCPath = (System.getProperty("user.dir") + "//Data//" + testCase + ".xlsx");
                int StepsIteration = 1;
                if (BaseClass.Iteration > 0)
                    StepsIteration = BaseClass.Iteration;
                //sw.reset();
                for (int iteration = 0; iteration < StepsIteration; iteration++) {
                    com.nirvana.Helper.HelperClass.PrintConsole("Iteration is "+iteration);
                    IsAnyIssue = false;

                    String timestamp2 = LocalDateTime.now().format(DateTimeFormatter.ofPattern("HH"));
                    if(timestamp2.equals("4") || timestamp2.equals("16")){
                        com.nirvana.Helper.HelperClass.PrintConsole("Not able to run the automation because of 4AM time");
                        com.nirvana.Helper.HelperClass.PrintConsole("Stopping Automation");
                        WindowHelper.CaptureImage("4AM Timeout","");
                        System.exit(1);

                    }
                    for(int steps=0;steps<stepList.size();steps++){
                        WindowHelper.SwitchCount = 0;
                        String step = stepList.get(steps);
                    //for (String step : stepList) {                    //TTLoad
                        module = step.split(",")[0];            //TTLoad
                        String stepName = step.split(",")[1];        //TTLoad


                        // Load Actions and Read the Excel Sheet start
                        List<String> actionList = new ArrayList<String>();
                        moduleSheet = book.getSheet(module);
                        int init = XLUtils.getRowNum(Path, module, stepName);
                        XSSFRow stepRow;

                        for (int j = init; j <= moduleSheet.getLastRowNum(); j++) {
                            stepRow = moduleSheet.getRow(j);
                            if (stepRow != null) {
                                String ele = "NA";
                                String ar = "NA";
                                if (stepRow.getCell(2) != null && !stepRow.getCell(2).getStringCellValue().isEmpty()) {
                                    ele = stepRow.getCell(2).getStringCellValue();
                                }
                                if (stepRow.getCell(3) != null && !stepRow.getCell(3).getStringCellValue().isEmpty()) {
                                    ar = stepRow.getCell(3).getStringCellValue();
                                }

                                if (stepRow.getCell(1) != null && !stepRow.getCell(1).getStringCellValue().isEmpty())
                                    actionList.add(stepRow.getCell(1).getStringCellValue() + ";" + ele + ";" + ar);
                                else
                                    break;
                            } else {
                                break;
                            }
                        }

                        ArrayList<Object> data2 = new ArrayList<Object>();
						String Allactions = "";
                        long sumOfTime = 0L;
                        for (int count = 0; count < actionList.size(); count++) {


							isPassed = true;
                            String action = actionList.get(count);                        //Action = Click;TTLoad;N/A
                            FileInputStream File = new FileInputStream(new File(TCPath));
                            XSSFWorkbook wbook = new XSSFWorkbook(File);
                            XSSFSheet wsheet = wbook.getSheet(stepName);
                            String ac = action.split(";")[0];                //Click
                            String element = action.split(";")[1];                //TTload
							Timer.startTimer();
							if (action.contains("MultipleSelectionStart")) {
                                int endCount = actionList.indexOf("MultipleSelectionEnd;NA;NA");
                                int startCount = count + 1;
                                count = endCount;
                                int argLength = wsheet.getLastRowNum();

                                for (int loop = 1; loop <= argLength; loop++) {
                                    if (wsheet.getRow(loop) == null)
                                        break;
                                    else if (wsheet.getRow(loop).getCell(0) == null)
                                        break;
                                    else if (wsheet.getRow(loop).getCell(0).getStringCellValue().isEmpty())
                                        break;
                                    for (int seq = startCount; seq < endCount; seq++) {
                                        action = actionList.get(seq);
                                        ac = action.split(";")[0];
                                        element = action.split(";")[1];
                                        String arg = "NA";
                                        if (!action.split(";")[2].equals("NA")) {
                                            int argNo = Integer.parseInt(action.split(";")[2].substring(action.split(";")[2].length() - 1));
                                            argNo -= 1;
                                            arg = wsheet.getRow(loop).getCell(argNo).getStringCellValue();
                                        }
                                        Mapping.perormActionWithKeyword(ac, element, arg);
                                    }
                                }

                            }
							else
							{
                                String arg = "NA";

                                if (!action.split(";")[2].equals("NA")) {
                                    int argNo = Integer.parseInt(action.split(";")[2].substring(action.split(";")[2].length() - 1));
                                    String temp = action.split(";")[2];
                                    argNo = Integer.parseInt(temp.substring(3));
                                    argNo -= 1;
                                    int oi = wsheet.getLastRowNum();
                                    if (wsheet.getLastRowNum() > 1) {
                                        for (int k = 1; k <= wsheet.getLastRowNum(); k++) {
                                            if (wsheet.getRow(k) != null) {
                                                arg = wsheet.getRow(k).getCell(argNo).getStringCellValue();
                                                Mapping.perormActionWithKeyword(ac, element, arg);
                                            }
                                        }
                                    }
									else
									{
                                        arg = wsheet.getRow(1).getCell(argNo).getStringCellValue();
                                        Mapping.perormActionWithKeyword(ac, element, arg);
                                    }
                                }
								else
								{
                                    Mapping.perormActionWithKeyword(ac, element, arg);
                                }
                            }

                            Timer.endTimer();
                            Properties properties = new Properties();
                            FileInputStream inputStream = null;

                            try {
                                inputStream = new FileInputStream("config.properties");
                                properties.load(inputStream);
                                String value = properties.getProperty(stepName);

                                if (value.contains(ac + ":" + element)) {
                                    Allactions+=ac;
                                    sumOfTime+=Timer.calculateTotalTime();
                                    System.out.println("Sum of time is "+sumOfTime);

                                }

                            } catch (Exception exception) {
                                com.nirvana.Helper.HelperClass.PrintConsole(exception);
                            }


                            if(count==actionList.size()-1){
                                data2.clear();
                                String timestamp = LocalDateTime.now().format(DateTimeFormatter.ofPattern("dd-MM-yyyy HH:mm:ss"));
                                com.nirvana.Helper.HelperClass.PrintConsole("Entered in condition");
                                data2.add(timestamp);
                                data2.add(module+"("+Allactions+")");
                                data2.add(sumOfTime);
                                ArrayList<Object> cloneDict = (ArrayList<Object>) data2.clone();
                                data1.add(cloneDict);
                                try {
                                    if (sumOfTime > ThresHoldValues.get(stepName)) {
                                        com.nirvana.Helper.HelperClass.PrintConsole("Time taken " + sumOfTime);
                                        WindowHelper.CaptureImage(stepName, "ThresHoldBreach");
                                    }
                                }
                                catch (Exception e){
                                    com.nirvana.Helper.HelperClass.PrintConsole(e);
                                }
                            }
                            if(WindowHelper.SwitchCount>1){
                                HelperClass.PrintConsole("Skipping step after 2 Retry Attempt "+stepName);
                                break;
                            }
                            else if(IsAnyIssue && WindowHelper.SwitchCount<2){
                                count= -1;
                                com.nirvana.Helper.HelperClass.PrintConsole("Retry again "+stepName + "For "+WindowHelper.SwitchCount);
                                sumOfTime = 0;

                            }

                        }
                        if(module.equals("ChangeMarketData")) {
                            ArrayList<Object> data3 = new ArrayList<Object>();
                            data3.add(LocalDateTime.now().format(DateTimeFormatter.ofPattern("dd-MM-yyyy HH:mm:ss")));
                            data3.add(module+"(ChangeData:MarketValuexpath)");
                            if(HelperClass.time.size()>0) {
                                for (int j = 0; j < HelperClass.time.size(); j++) {
                                    try {
										if(HelperClass.time.get(j)<300000)
											data3.add(HelperClass.time.get(j));
                                    }
                                    catch (Exception e){
                                        data3.add("Not Updated");
                                    }
                                }
                            }
                            else{
                                data3.add("Not Updated");
                                data3.add("Not Updated");
                                data3.add("Not Updated");
                                data3.add("Not Updated");
                            }
                            ArrayList<Object> cloneDict1 = (ArrayList<Object>) data3.clone();
                            data1.clear();
                            data1.add(cloneDict1);
                            AddSheetAndInputData(Timer.calculateTotalTime(), stepName, module, "Logs.xlsx", data1);
                            data3.clear();
                            data1.clear();
                            Thread.sleep(2000);
                            data3.add(LocalDateTime.now().format(DateTimeFormatter.ofPattern("dd-MM-yyyy HH:mm:ss")));
                            data3.add(module+"(ChangeData:MarketValuexpath)");
                            if(HelperClass.time1.size()>0) {
                                for (int j = 0; j < HelperClass.time1.size(); j++) {
                                    try {
										if(HelperClass.time1.get(j)<300000)
											data3.add(HelperClass.time1.get(j));
                                    }
                                    catch (Exception e){
                                        data3.add("Not updated");
                                    }
                                }

                            }
                            else{
                                data3.add("Not Updated");
                                data3.add("Not Updated");
                                data3.add("Not Updated");
                                data3.add("Not Updated");
                            }
                            ArrayList<Object> cloneDict2 = (ArrayList<Object>) data3.clone();
                            data1.clear();
                            data1.add(cloneDict2);
							HelperClass.time.clear();
							HelperClass.time1.clear();
                            AddSheetAndInputData(Timer.calculateTotalTime(), stepName, module, "Logs.xlsx", data1);
                        }
                        else
						{
							if(sumOfTime>0L && sumOfTime<120000){
								if(!HelperClass.skipTime)
									AddSheetAndInputData(Timer.calculateTotalTime(), stepName, module, "Logs.xlsx", data1);
							}
						}
                        data1.clear();
                        if (!SheetList.contains(module)) {
                            SheetList.add(module);
                            flag = true;
                            if (isPassed) {
                                com.nirvana.Helper.HelperClass.PrintConsole(stepName + " completed successfully.");
                                //logger.info();
                            } else {
                                com.nirvana.Helper.HelperClass.PrintConsole(stepName + " failed.");
                            }
                        }
                    }
                }
            }
        }


    }


    private void AddSheetAndInputData(long nano, String stepName, String module, String s, ArrayList<ArrayList<Object>> Data) throws IOException, GeneralSecurityException, InterruptedException {
        try {
            WriteDataInExcel.updateMultipleCells(CreateExcelSheet.NewCreatedSheet.getSpreadsheetId(), module, Data);
        } catch (NullPointerException e) {
            com.nirvana.Helper.HelperClass.PrintConsole("Null data");
        }
    }

}