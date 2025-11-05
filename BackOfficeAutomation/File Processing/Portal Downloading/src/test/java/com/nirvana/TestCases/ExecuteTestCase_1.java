package com.nirvana.TestCases;
import org.apache.poi.ss.usermodel.Cell;
import org.apache.poi.ss.usermodel.CellType;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.IOException;

import org.apache.poi.xssf.usermodel.XSSFCell;
import org.apache.poi.xssf.usermodel.XSSFRow;
import org.apache.poi.xssf.usermodel.XSSFSheet;
import org.apache.poi.xssf.usermodel.XSSFWorkbook;
import org.testng.annotations.DataProvider;
import org.testng.annotations.Test;

import java.util.ArrayList;
import java.util.List;
import com.nirvana.Mapping.Mapping;
import com.nirvana.Utilities.XLUtils;

public class ExecuteTestCase_1 extends BaseClass {
	public static XSSFWorkbook book;
	public static XSSFSheet sheet;
	public static XSSFSheet moduleSheet;
	public static XSSFRow row;
	public static XSSFCell cell;
	public static FileInputStream _readExcel;
	public static String module;

	

	public void Test() throws IOException, InterruptedException {
		
		
		String Path = (System.getProperty("user.dir") + "//Data//Data.xlsx");
		 logger.info("Picking up data sheet from " + Path); // Added logging
		String bnSheetName;
		// _readExcel = new FileInputStream(new File(Path));
		// book = new XSSFWorkbook(_readExcel);
		// sheet = book.getSheet(bnSheetName);

		// for(int i = 1; i <= sheet.getLastRowNum(); i++){
		// row = sheet.getRow(i);

//	String	SheetName= row.getCell(0).getStringCellValue(); 

		// Mapping.perormActionWithKeyword(Filepath, SheetName);

		bnSheetName = "TestCases";
		try {
		_readExcel = new FileInputStream(new File(Path));
		book = new XSSFWorkbook(_readExcel);
		sheet = book.getSheet(bnSheetName);
		 logger.info("Successfully loaded the sheet: " + bnSheetName); // Added logging
        } catch (FileNotFoundException e) {
            logger.error("Data file not found at: " + Path, e); // Added
           isPassed=false;
            return;
          
        } catch (IOException e) {
            logger.error("Error while reading the data file: " + Path, e); // Added
           
          return;
        }

		String testCase = "";
		// List to store business steps
		List<String> stepList = new ArrayList<String>();

		for (int i = 1; i <= sheet.getLastRowNum() + 1; i++) {
			
			row = sheet.getRow(i);
			if (row != null) { // if(!row.getCell(0).getStringCellValue().isEmpty())
				if (row.getCell(0) != null && !row.getCell(0).getStringCellValue().isEmpty()) {
					stepList.clear();
					testCase = row.getCell(0).getStringCellValue();
					logger.info("Loaded test case: " + testCase); // Added logging
					stepList.add(row.getCell(1).getStringCellValue() + "," + row.getCell(2).getStringCellValue());
				}
				// else if(!row.getCell(1).getStringCellValue().isEmpty())
				else if (row.getCell(1) != null && !row.getCell(1).getStringCellValue().isEmpty()) {
					stepList.add(row.getCell(1).getStringCellValue() + "," + row.getCell(2).getStringCellValue());
				}
			} else {
				//System.out.println("Running test case : " + testCase);
		    	logger.info("Running test case : "+ testCase );
				String TCPath = (System.getProperty("user.dir") + "//Data//" + testCase + ".xlsx");

				for (String step : stepList) {
					module = step.split(",")[0];
					String stepName = step.split(",")[1];

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
							//String temp =stepRow.getCell(3).getStringCellValue();
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

					// for(String action :actionList)
					for (int count = 0; count < actionList.size(); count++) {
						isPassed = true;
						String action = actionList.get(count);
						FileInputStream File = new FileInputStream(new File(TCPath));
						XSSFWorkbook wbook = new XSSFWorkbook(File);
						XSSFSheet wsheet = wbook.getSheet(stepName);

						if (action.contains("MultipleSelectionStart")) {
							int endCount = actionList.lastIndexOf("MultipleSelectionEnd;NA;NA");
							// int endCount=actionList.indexOf("MultipleSelectionEnd;NA;NA");

							// int startCount=1;
							count = endCount;
							int argLength = wsheet.getLastRowNum();
							int n = 1;
							// while(wsheet.getRow(n).getCell(2)!=null)
							// {

							boolean isMultiple = false;
							do {
								int startCount = 1;

								for (int loop = 1; loop <= argLength; loop++) {
									if (wsheet.getRow(loop) == null)
										break;
									else if (wsheet.getRow(loop).getCell(0) == null)
										break;
									else if (wsheet.getRow(loop).getCell(0).getStringCellValue().isEmpty())
										break;

									for (int seq = startCount; seq < endCount; seq++) {
										isBreakStep=true;
										isContinueStep=true;
										action = actionList.get(seq);
										String ac = action.split(";")[0];
										if (ac.contains("MultipleSelectionStart")) {
											startCount = seq + 1;
											loop = 0;
											isMultiple = true;
											break;
										}

										String element = action.split(";")[1];
										String arg = "NA";
										if (!action.split(";")[2].equals("NA")) {
											String[] parts = action.split(";");
											String argument = parts[2].substring(3); // Extract the substring after "arg"
											int argNo = Integer.parseInt(argument);
											argNo -= 1;
											if (argNo == 2 && isMultiple == true)
												arg = wsheet.getRow(n).getCell(argNo).getStringCellValue();
											else if (ac.contains("RenameAndMoveFiles")) {
												arg = wsheet.getRow(n).getCell(2).getStringCellValue() + "_"
														+ wsheet.getRow(loop).getCell(argNo).getStringCellValue();
											} else
												arg = wsheet.getRow(loop).getCell(argNo).getStringCellValue();
											if(arg.equals("Continue"))continue;
											
										}

										Mapping.perormActionWithKeyword(ac, element, arg,seq);
										if(isBreakStep.equals(false)) {
                                            isBreakStep=true;
                                            break;
                                        }
										Thread.sleep(5000);
									}
									
								}
								if (!isMultiple || n == argLength)
									break;
								n++;
							} while (wsheet.getRow(n).getCell(2) != null);
						}

						else {
							String ac = action.split(";")[0];
							String element = action.split(";")[1];
							String arg = "NA";
							if (!action.split(";")[2].equals("NA")) {
								String[] parts = action.split(";");
								String argument = parts[2].substring(3); // Extract the substring after "arg"
								int argNo = Integer.parseInt(argument);
				//				 argNo = Integer
				//						.parseInt(action.split(";")[2].substring(action.split(";")[2].length() - 1));
								argNo -= 1;

								int oi = wsheet.getLastRowNum();
								if (wsheet.getLastRowNum() > 1) {
									for (int k = 1; k <= wsheet.getLastRowNum(); k++) {
										if (wsheet.getRow(k) != null) {
											arg = wsheet.getRow(k).getCell(argNo).getStringCellValue();
											Mapping.perormActionWithKeyword(ac, element, arg,k);
										}
										// Mapping.perormActionWithKeyword(ac, element, arg);

									}

								}

								else {
									arg = wsheet.getRow(1).getCell(argNo).getStringCellValue();
									Mapping.perormActionWithKeyword(ac, element, arg,1);
								}
							} else {
								Mapping.perormActionWithKeyword(ac, element, arg,1);
							}

						}
					} // for loop end

					// logger.info(stepName+"completed successfully.");
					if (isPassed)
						logger.info(stepName + " completed successfully.");
					 else {
						logger.info(stepName + " failed.");
					}
				}
			}

		}
		/*
		 
		 * */
	}
	

}
