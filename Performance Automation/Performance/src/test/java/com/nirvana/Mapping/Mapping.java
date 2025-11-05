package com.nirvana.Mapping;

import com.nirvana.Helper.*;
import com.nirvana.TestCases.BaseClass;
import com.nirvana.Utilities.XLUtils;

import java.io.File;
import java.io.FileInputStream;

import org.apache.poi.xssf.usermodel.XSSFCell;
import org.apache.poi.xssf.usermodel.XSSFRow;
import org.apache.poi.xssf.usermodel.XSSFSheet;
import org.apache.poi.xssf.usermodel.XSSFWorkbook;

public class Mapping extends BaseClass {

	public static XSSFWorkbook book;
	public static XSSFSheet sheet;
	public static XSSFRow row;
	public static XSSFCell cell;
	public static FileInputStream _readExcel;

	public static void perormActionWithKeyword(String action,String element,String arg) {
		try {
			//_readExcel = new FileInputStream(new File(aFilePath));
			//book = new XSSFWorkbook(_readExcel);
			//sheet = book.getSheet(bSheetName);

			//for(int i = 1; i <= sheet.getLastRowNum(); i++){
			//row = sheet.getRow(i);
//
			//Timer.startTimer();
			switch (action) {


				case "NavigateURL":
					WindowHelper.navigateToPage(arg);
					break;
				case "ClearField":
					TextBoxHelper.Backspace(element);
					break;	

				case "SendKeys":
					TextBoxHelper.typeInTextBox(element, arg);
					break;
				case "CheckRow":
					BaseClass.CheckRow(arg);
					break;
				case "CloseWindow":
					BaseClass.CloseWindow(arg);
					break;
				case "CloseAllWindow":
					BaseClass.CloseAllWindow(arg);
					break;
				case "CloseWindowRTPNL":
					BaseClass.CloseWindowRTPNL(arg);
					break;
				case "Click":
					ClickHelper.clickButton(element);
					break;

				case "Hover":
					HoverActionHelper.Hover(element);
					break;

				case "SelectDropDown":
					TextBoxHelper.SelectDropDown(element, arg);
					break;

				case "ClickDynamicPath":
					ClickHelper.clickDynamicButton(element,arg);
					break;

				case "WaitForText":
					TextBoxHelper.WaitForText(element, arg);
					break;

				case "WaitForCompleteText":
					TextBoxHelper.WaitForCompleteText(element, arg);
					break;

				case "RenameFiles":
					XLUtils.RenameFiles(element);
					break;

				case "Checkbox":
					CheckBoxHelper.Checkbox(row);
					break;

				case "WaitForEle":
					BaseClass.waitElement(element);
					break;

				case "ChangeData":
					HelperClass helperClass = new HelperClass(driver);
					helperClass.CaptureChangeData(element);
					break;
				case "RightClick":
					ClickHelper.rightClick(element);
					break;

				case "CheckElement":
					BaseClass.CheckElement(element);
					break;






					/*
				case "CheckText":
					CheckTextHelper.GetText(row);
					break;

				case "GetDownloadedFileName":
					GetDownloadedFileName.GetNameFile(row);
					break;

 				*/
				case "ExtractFiles":
					ExtractDownloadedFile.Extractzip(arg);
					break;

				case "MoveFiles":
					XLUtils.MoveFiles();
					break;

				case "ConditionalClick":
					ClickHelper.conditionalClick(element);
					break;

				case "Sleep":
					BaseClass.Sleep();
					break;

				case "CanvasClick":
					ClickHelper.CanvasClick(element);
					BaseClass.Sleep();
					break;

				case "SwitchWindow":
					//WindowHelper.SwitchWindowHandles2(arg);
					WindowHelper.SwitchWindowHandles(arg);
					break;
				case "SwitchWindow1":
					WindowHelper.SwitchWindowHandles(arg);
					break;
				case "SwitchToExact":
					WindowHelper.SwitchToExact(arg);
					break;
				case "storeFieldData":
					HelperClass.GetFieldText(element);
					break;
				case "CompareFieldData":
					HelperClass.CompareFieldDataFull(element,arg);
					break;
				case "CompareFieldDataString":
					HelperClass.CompareFieldDataString(element,arg);
					break;
				case "CompareFieldDataPartial":
					HelperClass.CompareFieldData(element, arg);
					break;
				case "StartTimer":
					Timer.startTimer();
					break;
				case "ClickCheckbox":
					ClickHelper.ClickCheckbox(element);
					break;

				case "EndTimer":
					Timer.endTimer();
					break;

				case "TotalTime":
					Timer.calculateTotalTime();
					break;

				case "WriteToExcel":
					Timer.writeToExcel();
					break;





			}


			//Timer.endTimer();



		} catch (Exception e) {
			//ExceptionLog.info(e.toString());
			HelperClass.PrintConsole(e.toString());
			e.printStackTrace();
		}
	}

}
