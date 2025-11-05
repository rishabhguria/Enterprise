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

	public static void perormActionWithKeyword(String action, String element, String arg, int k) {
		try {
			switch (action) {

			case "NavigateURL":
				WindowHelper.navigateToPage(arg);
				break;

			case "NavigateToNextPage":
				WindowHelper.NavigateToNextPage(element, arg);
				break;
			case "NavigateUsingBatchFile":
				WindowHelper.NavigateUsingBatchFile(arg);
				break;
			case "NavigateToLatestPage":
				WindowHelper.NavigateToLatestPage();
				break;
			case "SwitchToIFrame":
				WindowHelper.SwitchToIFrame(element);
				break;
			case "SwitchOutIFrame":
				WindowHelper.SwitchOutIFrame(element);
				break;
			case "refreshpage":
				WindowHelper.refresh();
				break;

			case "SendKeys":
				TextBoxHelper.typeInTextBox(element, arg);
				break;
			case "SendKeysWithoutClear":
				TextBoxHelper.typeInTextBoxWithoutClear(element, arg);
				break;
			case "TypeDateInGs":
				TextBoxHelper.TypeDateInGs(element, arg);
				break;
			case "TypeDateInGsWithClear":
				TextBoxHelper.TypeDateInGsWithClear(element, arg);
				break;
			case "TypeDateInGsWithClear_V2":
				TextBoxHelper.TypeDateInGsWithClear_V2(element, arg);
				break;
			case "TypeDate":
				TextBoxHelper.TypeDate(element, arg);
				break;

			case "Click":
				ClickHelper.clickButton(element);
				break;
			case "clickText":
				ClickHelper.clickText(element, arg);
				break;
			case "SelectNTAccount":
				ClickHelper.SelectNTAccount(element, arg);
				break;
			case "clickButtonConditional":
				ClickHelper.clickButtonConditional(element, arg);
				break;
			case "DoubleClick":
				ClickHelper.doubleClickButton(element);
				break;

			case "clickDynamicButtonXpath":
				ClickHelper.clickDynamicButtonXpath(element, arg);
				break;

			case "Hover":
				HoverActionHelper.Hover(element);
				break;

			case "Scrollup":
				TextBoxHelper.Scrollup();
				break;
				
			case "ScrollDown":
				TextBoxHelper.ScrollDown();
				break;

			case "SelectDropDown":
				TextBoxHelper.SelectDropDown(element, arg);
				break;

			case "ClickDynamicPath":
				ClickHelper.clickDynamicButton(element, arg);
				break;

			case "DownloadComericaData":
				ClickHelper.DownloadComericaData(element, arg);
				break;

			case "ClickCssSelector":
				ClickHelper.cssSelector(element, arg);
				break;
			case "SingleClickUsingAction":
				ClickHelper.SingleClickUsingAction(element);
				break;
			case "ClickWithoutWait":
				ClickHelper.ClickWithoutWait(element, arg);
				break;
			case "ClickUsingAction":
				ClickHelper.ClickUsingAction(element, arg);
				break;

			case "WaitForText":
				TextBoxHelper.WaitForText(element, arg);
				break;

			case "WaitForCompleteText":
				TextBoxHelper.WaitForCompleteText(element, arg);
				break;

			case "WaitForText1":
				TextBoxHelper.WaitForText1(element, arg);
				break;

			case "RenameFiles":
				XLUtils.renameFile(arg);
				break;
			case "RenameFiles_V2":
				XLUtils.renameFile_V2(arg);
				break;

			case "ExtractFiles":
				ExtractDownloadedFile.Extractzip(arg);
				break;

			case "MoveFiles":
				XLUtils.MoveFiles(arg);
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

			case "MoveFilesMFWise":
				XLUtils.MoveFilesMFWise(arg);
				break;

			case "MoveRenamedFiles":
				XLUtils.MoveRenamedFiles(element, arg);
				break;

			case "clickonimage":
				ClickHelper.clickonimage(element);
				break;
			case "hoveronimage":
				ClickHelper.hoveronimage(element);
				break;
				
			case "GetTextValueAfterProcessing":
				XLUtils.GetTextValueAfterProcessing_MS(element, arg);
				break;

			case "scrollinview":
				ClickHelper.ScrollIntoView(element);
				break;
			case "exportforFMTC":
				ClickHelper.ExportforFMTC(element);
				break;

			case "scrollandclick":
				ClickHelper.scrollandfind(element);
				break;
			case "ConditionBreak":
				XLUtils.ConditionBreak(arg);
				break;
			case "ConditionBreakFidelityCash":
				XLUtils.ConditionBreakFidelityCash(arg);
				break;
			case "navigateToFidelityPostion":
				XLUtils.navigateToFidelityPostion(arg);
				break;
			case "CheckFidelityCashReport":
				XLUtils.CheckFidelityCashReport(arg);
				break;
			case "navigateToFidelityCash":
				XLUtils.navigateToFidelityCash(arg);
				break;
			case "CheckFidelityCashReportUsingUrl":
				XLUtils.CheckFidelityCashReportUsingUrl(arg);
				break;
			case "clickFidelityExportButton":
				ClickHelper.clickFidelityExportButton(element,arg);
				break;
			case "clickFidelityCashExportButton":
				XLUtils.clickFidelityCashExportButton(element,arg);
				break;
			case "DownloadFilesFromList":
				XLUtils.DownloadFilesFromList(element);
				break;
			case "NT_Process":
				XLUtils.NT_Process(element);
				break;
			case "ConditionalButtonClick":
				ClickHelper.ConditionalButtonClick(element, arg);
				break;
			case "DownloadGSData":
				XLUtils.DownloadGSData(element,arg);
				break;
			case "DownloadUSBankData":
				XLUtils.DownloadUSBankData(element, arg);
				break;
			
			case "ClearUSBankAccount":
				XLUtils.ClearUSBankAccount(element, arg);
				break;
			case "SearchAndClick":
				XLUtils.SearchAndClick(element, arg);
				break;
			case "waitForAnyFileDownload":
				XLUtils.waitForAnyFileDownload(element, arg);
				break;
			case "ButtonClickOnTextMatch":
				ClickHelper.ButtonClickOnTextMatch(element, arg);
				break;
			case "ClientandAccountClearUBS":
				ClickHelper.ClientandAccountClearUBS(element, arg);
				break;
			case "ButtonClickOnElementFound":
				ClickHelper.ButtonClickOnElementFound(element, arg);
				break;
			case "ButtonClickOnAttributeCheck":
				ClickHelper.ButtonClickOnAttributeCheck(element, arg);
				break;
			case "SelectDateFromCalender":
				XLUtils.SelectDateFromCalender(element, arg);
				break;
			case "DownloadUBSTelemarkFutures":
				XLUtils.DownloadUBSTelemarkFutures(element, arg);
				break;
			case "GetClientDetails":
				XLUtils.GetClientDetails(element, arg);
				break;
			case "DownloadOpusFiles":
				XLUtils.DownloadOpusFiles(element,arg);
				break;
			case "BringOwnerToTop":
				XLUtils.BringOwnerToTop(element,arg);
				break;
			case "BringFavouriteToTop":
				XLUtils.BringFavouriteToTop(element,arg);
				break;
			case "IncrementalDownloadButtonPershing":
				ClickHelper.IncrementalDownloadButtonPershing(element, arg);
				break;
			case "CheckIfFileAlreadyDownloaded":
				XLUtils.CheckIfFileAlreadyDownloaded(element,arg);
				break;
				// My Code
							case "SetNTAccounts":
								XLUtils.SetNTAccounts(element, arg);
								break;
							case "DownloadNTFiles":
								XLUtils.DownloadNTFiles(element, arg);
								break;
							case "SendDate":
								TextBoxHelper.SendDate(element, arg);
								break;
							case "WaitToBecomeInVisible":
								TextBoxHelper.WaitToBecomeInVisible(element, arg);
								break;
							case "ClickLink":
								ClickHelper.ClickLink(element);
								break;
							case "AddStartDate":
								TextBoxHelper.GetStartDate(element, arg);
								break;
							case "TypeDateWithoutSleep":
								TextBoxHelper.TypeDateWithoutSleep(element, arg);
								break;
							case "CheckIfCheckBoxisSelected":
								XLUtils.CheckIfCheckBoxisSelected(element, arg);
								break;
							case "SelectDateFromCalenderUMB":
								XLUtils.SelectDateFromCalender_UMB(element, arg);
								break;
							case "HoverOnLink":
								HoverActionHelper.HoverOnLink(element);
								break;
							case "SelectDateFromCalender_SG":
								XLUtils.SelectDateFromCalender_SG(element, arg);
								break;
								
								//TARUN FUNCTIONS
							
							
							case "ClearInput":
								XLUtils.ClearInput(element, arg);
								break;
							case "SelectDateFromList":
								XLUtils.SelectDateFromList(element, arg);
								break;
							case "GreenTabsCheck":
								XLUtils.GreenTabsCheck(element);
								break;
							case "SelectBTIGFund":
				                XLUtils.SelectBTIGFund(element);
				                break;
							case "CheckDateInElement":
				                XLUtils.CheckDateInElement(element,arg);
				                break;
							case "SelectOptionByVisibleText":
				                XLUtils.SelectOptionByVisibleText(element,arg);
				                break;
							case "clickButton2":
				                ClickHelper.clickButton2(element);
				                break;
							case "SetDateReadOnlyInput":
								TextBoxHelper.SetDateReadOnlyInput(element, arg);
								break;
							case "SetDateReadOnlyInputCITI":
								TextBoxHelper.SetDateReadOnlyInputCITI(element, arg);
								break;
							case "SwitchToAlert":
				                WindowHelper.SwitchToAlert(element);
				                break;
							case "TypeDateAfterClearingInput":
								TextBoxHelper.TypeDateAfterClearingInput(element, arg);
								break;	
							case "clickOnlyWhenEnabled":
				                ClickHelper.clickOnlyWhenEnabled(element);
				                break;
							case "typeInTextBoxAfterJSClear":
								TextBoxHelper.typeInTextBoxAfterJSClear(element,arg); 
								break;
							case "TypeDateFromDatePNC":
								TextBoxHelper.TypeDateFromDatePNC(element, arg);
								break;
							case "TypeDateFromDateCITI":
								TextBoxHelper.TypeDateFromDateCITI(element, arg);
								break;
							case "TypeDateToDateCITI":
								TextBoxHelper.TypeDateToDateCITI(element, arg);
								break;
							case "TypeDateToDateCITI_New":
								TextBoxHelper.TypeDateToDateCITI_New(element, arg);
								break;
							case "closeLatestTab":
								WindowHelper.closeLatestTab();
								break;
							case "TypeDateFromDateStateStreet1monthPrior":
								TextBoxHelper.TypeDateFromDateStateStreet1monthPrior(element,arg);
								break;
							case "TypeDateFromDateStateStreet2monthPrior":
								TextBoxHelper.TypeDateFromDateStateStreet2monthPrior(element, arg);
								break;
							case "TypeDateInBTIG":
								TextBoxHelper.TypeDateInBTIG(element, arg);
								break;
							case "TypeDateInParts_Clearstreet":
				                TextBoxHelper.TypeDateInParts_Clearstreet(element, arg);
				                break;
				            case "clickButtons":
				                ClickHelper.ClickButtons(element);
				                break;
				            case "clickElementAfterScroll":
				                ClickHelper.clickElementAfterScroll(element);
				                break;
				            case "clickElementsAfterScroll":
				                ClickHelper.clickElementsAfterScroll(element,arg);
				                break;
                            case "TypeDateInParts_Clearstreet_V2":
								TextBoxHelper.TypeDateInParts_Clearstreet_V2(element, arg);
								break;
								
                            case "SelectBTIGBCCMFund":
                				XLUtils.SelectBTIGBCCMFund(element);
                				break;
                            case "clickElementAfterScroll_Citi":
				                ClickHelper.clickElementAfterScroll_Citi(element);
				                break;
                            case "SelectCheckbox":
                				XLUtils.SelectCheckbox(element);
                				break;
                            case "UnselectCheckbox":
                				XLUtils.UnselectCheckbox(element);
                				break;
                            case "TypeDateWithClear_CITI":
                				TextBoxHelper.TypeDateWithClear_CITI(element, arg);
                				break;
								
				                
			}

		} catch (Exception e) {
			// ExceptionLog.info(e.toString());
			logger.error(e.toString());
			e.printStackTrace();
		}
	}

}
