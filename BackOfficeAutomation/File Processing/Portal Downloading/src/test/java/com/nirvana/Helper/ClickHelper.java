package com.nirvana.Helper;

import java.awt.Robot;
import java.awt.event.InputEvent;
import java.io.File;
import java.time.Duration;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

import org.apache.poi.xssf.usermodel.XSSFRow;
import org.openqa.selenium.By;
import org.openqa.selenium.Dimension;
import org.openqa.selenium.JavascriptExecutor;
import org.openqa.selenium.Point;
import org.openqa.selenium.TimeoutException;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.interactions.Actions;
import org.openqa.selenium.support.ui.ExpectedConditions;
import org.openqa.selenium.support.ui.WebDriverWait;
import org.sikuli.script.Screen;

import com.nirvana.TestCases.BaseClass;
import com.nirvana.TestCases.ExecuteTestCase_1;

public class ClickHelper extends BaseClass {

	public static void clickButton(String element) throws InterruptedException {

		try {
			String xpath = XPathDict.get(element);
			waitForElement(xpath);
			WebElement ele = getWebElement(xpath);
			executeScript(ele);
			logger.info( element + " Clicked");

		} catch (Exception e) {
			//ExceptionLog.info(e.toString());
			logger.error(e.toString());
			isPassed = false;
		}

	}
	
	public static void clickText(String element,String arg) throws InterruptedException {

		try {
			String xpath = "//*[contains(text(), '" + arg + "')]";
			waitForElement(xpath);
			WebElement ele = getWebElement(xpath);
			executeScript(ele);
			logger.info( arg + " Clicked");

		} catch (Exception e) {
			//ExceptionLog.info(e.toString());
			logger.error(e.toString());
			isPassed = false;
		}

	}

	public static void ConditionalButtonClick(String loginelementname, String arg) throws InterruptedException {
		try {

			String passinfo = arg.split(",")[1];
			String passelementname = passinfo.split("=")[0];
			String password = passinfo.split("=")[1];
			String xpathloginelementname = XPathDict.get(loginelementname);
			String xpathpasselementname = XPathDict.get(passelementname);
			WebElement loginBtn = null;
			WebElement passwordBox = null;
			try {
				passwordBox = getWebElement(xpathpasselementname);
				passwordBox.sendKeys(password);
				loginBtn = getWebElement(xpathloginelementname);
				// WebElement ele2 = driver.findElement(By.xpath(xpathnextelement));

				// Now, you can interact with the element

			} catch (org.openqa.selenium.NoSuchElementException e) {
				// Handle the case where the element is not found
				System.out.println("Element not found: " + passelementname);
				logger.info("Element not found: " + passelementname);
			}

			if (loginBtn != null && loginBtn.isDisplayed()) {
				if (loginBtn.getText().contains("Login")) {
					executeScript(loginBtn);
					logger.info(loginelementname + " Clicked");
				}
				return;
			}

		}

		catch (Exception e) {
			// ExceptionLog.info(e.toString());
			logger.error(e.toString());
			isPassed = false;
		}
	}

	public static void clickButtonConditional(String element, String arg) throws InterruptedException {

		try {
			String css = XPathDict.get(element);
			By cssSelector = By.cssSelector(css);
			WebElement ele = driver.findElement(cssSelector);

			// Click on the element
			ele.click();

			logger.info(element + " Clicked");
		} catch (Exception e) {
			// ExceptionLog.info(e.toString());
			logger.error(e.toString());
			isPassed = false;
		}

	}

	public static void ButtonClickOnTextMatch(String element, String arg) throws InterruptedException {

		try {

			String xpath = XPathDict.get(element);
			waitForElement(xpath);
			WebElement ele = getWebElement(xpath);
			String Text = ele.getText();
			if (Text.contains(arg)) {
				executeScript(ele);
				logger.info(Text + " Clicked");
			} else {
				logger.info(arg + "not found");
				return;
			}
		} catch (Exception e) {
			// ExceptionLog.info(e.toString());
			logger.error(e.toString());
			isPassed = false;
		}

	}
	
	public static void ClientandAccountClearUBS(String element, String arg) throws InterruptedException {

		try {
			String xpath = XPathDict.get(element);
			waitForElement(xpath);
			WebElement parentElement = getWebElement(xpath); // Define your parent element
			String className = "clear-btn"; // Specify the class name you're looking for

			String xpathExpression = ".//*[contains(@class, '" + className + "')]";
			
			WebElement childElement = parentElement.findElement(By.xpath(xpathExpression));
			System.out.println(xpathExpression);
			
			childElement.click();
			
		} catch (Exception e) {
			
		}

	}
	
	public static void ButtonClickOnElementFound(String element, String arg) throws InterruptedException {

		try {

			String xpath = XPathDict.get(element);
			//waitForElement(xpath);
			//xpath=xpath+"/"+arg;
			WebElement ele = getWebElement(xpath);
			executeScript(ele);
			logger.info(element + " Clicked");
		} catch (Exception e) {
			// ExceptionLog.info(e.toString());
			return;
			//isPassed = false;
		}

	}
	
	public static void ButtonClickOnAttributeCheck(String element, String arg) throws InterruptedException {

		try {
            String arr[]= arg.split("#");
            String attributeName=arr[0];
            String Value=arr[1];
			String xpath = XPathDict.get(element);
			WebElement ele = getWebElement(xpath);
			//waitForElement(xpath);
			//xpath=xpath+"/"+arg;
			String attributeValue = ele.getAttribute(attributeName);
			if(!attributeValue.contains(Value)) {
				String ReportDropdownxpath = XPathDict.get(element);
				WebElement ele2 = getWebElement(ReportDropdownxpath);
				ele2.click();
				//ClickWithoutWait(element,"NA");
				logger.info(element + " Clicked");	
			}
			return;
		} catch (Exception e) {
			// ExceptionLog.info(e.toString());
			logger.error(e.toString());
			//return;
			//isPassed = false;
		}

	}
	
	
	
	
	public static boolean isReportAlreadyDownloaded(String reportName, List<String> alreadyDownloadedFiles) {
		for (String downloadedFile : alreadyDownloadedFiles) {
			if (downloadedFile.contains(reportName)) {
				return true;
			}
		}
		return false;
	}
	
	public static void IncrementalDownloadButtonPershing(String element, String arg) throws InterruptedException {

		try {
			String FolderName = XPathDict.get("Folder Name");
			String LogFilePath = "";
			if (!FolderName.equals("")) {
				LogFilePath = DownloadLogFilePath.get(FolderName);
			}
			String accNoXpath = XPathDict.get("Account Number Text");
			WebElement acc = driver.findElement(By.xpath(accNoXpath));
			String reportName = acc.getText();
			if(!reportName.equals(arg))
			{
				isBreakStep = false;
				return;
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
		} catch (Exception e) {
			// ExceptionLog.info(e.toString());
			logger.error(e.toString());
			// return;
			// isPassed = false;
		}

	}


	public static void doubleClickButton(String element) throws InterruptedException {

		try {
			String xpath = XPathDict.get(element);
			waitForElement(xpath);
			WebElement ele = getWebElement(xpath);

			// Create an instance of the Actions class
			Actions actions = new Actions(driver);

			// Perform a double click on the element
			actions.doubleClick(ele).perform();
			logger.info(element + " doubleClicked");
		} catch (Exception e) {
			// ExceptionLog.info(e.toString());
			logger.error(e.toString());
			isPassed = false;
		}

	}



	public static void clickDynamicButton(String element, String arg) throws InterruptedException {

		try {
			String css = XPathDict.get(arg);
			By cssSelector = By.cssSelector(css);
			WebElement ele = driver.findElement(cssSelector);

			// Click on the element
			ele.click();

			logger.info(arg + " Clicked");
		} catch (Exception e) {
			// ExceptionLog.info(e.toString());
			logger.error(e.toString());
			isPassed = false;
		}

	}

	public static void clickDynamicButtonXpath(String element, String arg) throws InterruptedException {

		try {
			String xpath = XPathDict.get(arg);
			WebElement ele = getWebElement(xpath);
			executeScript(ele);

			logger.info(arg + " Clicked");
		} catch (Exception e) {
			// ExceptionLog.info(e.toString());
			logger.error(e.toString());
			isPassed = false;
		}

	}

	public static void SelectNTAccount(String element, String arg) throws InterruptedException {

		try {

			String[] splitArray = arg.split("#");

			String xpath = XPathDict.get(element);
			// Convert the array to a list
			List<String> accountList = Arrays.asList(splitArray);
			// xpath=xpath.replace("arg", arg);
			// waitForElement(xpath);
			WebDriverWait wait = new WebDriverWait(driver, 10);

			List<WebElement> initialRows = wait
					.until(ExpectedConditions.presenceOfAllElementsLocatedBy(By.xpath("//*[@id='listRef']/div")));
			int rowSize = initialRows.size();
			// Identify the dropdown element
			WebElement dropdown = driver.findElement(By.xpath(xpath));

			// Scroll down within the dropdown
			((JavascriptExecutor) driver).executeScript("arguments[0].scrollTop = arguments[0].scrollHeight;",
					dropdown);

			List<WebElement> rows = wait
					.until(ExpectedConditions.presenceOfAllElementsLocatedBy(By.xpath("//*[@id='listRef']/div")));

			int row = rows.size();

			// int count=1;
			for (int i = 1; i <= row; i++) {
				String tempXpath = "//*[@id=\"listRef\"]/div[" + i + "]/div[1]/div/label";
				WebElement elem = driver.findElement(By.xpath(tempXpath));
				String text = elem.getText();
				if (accountList.contains(text)) {
					executeScript(elem);
					logger.info(text + " Clicked");
				}
			}
			
		} catch (Exception e) {
			// ExceptionLog.info(e.toString());
			logger.error(e.toString());
			isPassed = false;
		}

	}

	public static void DownloadComericaData(String element, String arg) throws InterruptedException {

		try {
			String xpath = XPathDict.get(element);


			By parentSelector = By.xpath(xpath);
			// WebElement parentElement = driver.findElement(parentSelector);
			WebDriverWait wait = new WebDriverWait(driver, 10);
			WebElement parentElement = wait.until(ExpectedConditions.visibilityOfElementLocated(parentSelector));

			// Find the button element within the parent
			By buttonSelector = By.xpath("//*[@id=\"expand-collapse-change\"]/mat-card/mat-card-header/button");

			WebElement buttonElement = parentElement.findElement(buttonSelector);

			// Check the aria-label of the button
			String ariaLabel = buttonElement.getAttribute("aria-label");

			if ("Click to expand".equals(ariaLabel)) {
				// If aria-label is "Click to expand", click the button
				buttonElement.click();
				// By aTagSelector = By.xpath(".//a[contains(text(), 'MACOMBRET')]");
				WebElement aTagElement = wait.until(ExpectedConditions.elementToBeClickable(
						parentElement.findElement(By.xpath(".//a[contains(text(), 'MACOMBRET')]"))));
				// WebElement aTagElement = parentElement.findElement(aTagSelector);
				aTagElement.click();
			} else {
				
				WebElement aTagElement = wait.until(ExpectedConditions.elementToBeClickable(
						parentElement.findElement(By.xpath(".//a[contains(text(), 'MACOMBRET')]"))));
				// WebElement aTagElement = parentElement.findElement(aTagSelector);
				aTagElement.click();
			}

			// Click on the element

			logger.info(element + " Clicked");
		} catch (Exception e) {
			// ExceptionLog.info(e.toString());
			logger.error(e.toString());
			isPassed = false;
		}

	}

	public static void cssSelector(String element, String arg) throws InterruptedException {

		try {
			String css = XPathDict.get(element);
			By cssSelector = By.cssSelector(css);
			WebDriverWait wait = new WebDriverWait(driver, 120);
			wait.until(ExpectedConditions.elementToBeClickable(cssSelector));
			WebElement ele = driver.findElement(cssSelector);

			executeScript(ele);

			logger.info(element + " Clicked");
		} catch (Exception e) {
			// ExceptionLog.info(e.toString());
			logger.error(e.toString());
			isPassed = false;
		}

	}
	
	public static void SingleClickUsingAction(String element) throws InterruptedException {

		try {
			String xpath = XPathDict.get(element);
			waitForElement(xpath);
			WebElement ele = getWebElement(xpath);

			// Create an instance of the Actions class
			Actions actions = new Actions(driver);

			// Perform a double click on the element
			actions.click(ele).perform();
			logger.info(element + " doubleClicked");
		} catch (Exception e) {
			// ExceptionLog.info(e.toString());
			logger.error(e.toString());
			isPassed = false;
		}

	}
	
	public static void ClickWithoutWait(String element, String arg) throws InterruptedException {

		try {
			String xpath = XPathDict.get(element);
			WebElement ele = getWebElement(xpath);
			executeScript(ele);
			logger.info(element + " Clicked");
		} catch (Exception e) {
			// ExceptionLog.info(e.toString());
			logger.error(e.toString());
			isPassed = false;
		}

	}
	public static void ClickUsingAction(String element, String arg) throws InterruptedException {

		try {
			String xpath = XPathDict.get(element);
			waitForElement(xpath);
			WebElement ele = getWebElement(xpath);
			Point coordinates = ele.getLocation();
			int xCoordinate = coordinates.getX();
			int yCoordinate = coordinates.getY();
			Robot robot = new Robot();
			//TransactionFilterSearch
			if(element.contains("TransactionFilterSearch")) {
				robot.mouseMove(xCoordinate, yCoordinate);
			}
			else {
			robot.mouseMove(xCoordinate, yCoordinate + 125);
			}
			robot.mousePress(InputEvent.BUTTON1_DOWN_MASK);
			robot.mouseRelease(InputEvent.BUTTON1_DOWN_MASK);
			//Actions actions = new Actions(driver);
			//actions.moveToElement(ele).perform();
			//actions.moveToElement(ele).click().build().perform();
			logger.info(element + " Clicked");
		} catch (Exception e) {
			// ExceptionLog.info(e.toString());
			logger.error(e.toString());
			isPassed = false;
		}

	}



	public static void conditionalClick(String element) throws InterruptedException {
		try {
			String xpath = XPathDict.get(element);
			waitForElement(xpath);
			WebElement ele = getWebElement(xpath);
			if (!ele.getText().contains("Hide")) {
				executeScript(ele);
				logger.info(element + " Clicked");
			}

		}

		catch (Exception e) {
			// ExceptionLog.info(e.toString());
			logger.error(e.toString());
			isPassed = false;
		}
	}

	public static void CanvasClick(String element) throws InterruptedException {
		String xpath = XPathDict.get(element);
		waitForElement(xpath);
		WebElement ele = getWebElement(xpath);
		Actions actions = new Actions(driver);
		// actions.moveToElement(ele,0,0).moveByOffset(500,100).click().build().perform();

		Dimension canvas_dimensions = ele.getSize();

		int center_x = canvas_dimensions.getWidth() / 2;

		int center_y = canvas_dimensions.getHeight() / 2;

		int x = canvas_dimensions.getWidth() / 23;

		int button_x = (int) ((center_x / 3));
		int button_y = (int) ((center_y / 3));

		JavascriptExecutor js = ((JavascriptExecutor) driver);
		js.executeScript("window.scrollTo(0, document.body.scrollHeight)");
		Thread.sleep(5000);
		// actions.moveToElement(ele,button_x,button_y)
		// .click().build().perform();

		actions.moveToElement(ele, 0, 0).moveByOffset(x, 1).click().build().perform();
		Thread.sleep(5000);

	}

	public static void clickonimage(String element) throws InterruptedException {
		Screen screen = new Screen();
		try {
			String Path = (System.getProperty("user.dir") + "\\src\\main\\resources\\images\\");
			// String
			// fpath="C:\\Users\\kislay.kumar\\eclipse-workspace\\AutomationTesting\\src\\main\\resources\\images\\"+XPathDict.get(element);
			String fpath = Path + XPathDict.get(element);
			screen.click(fpath);
			//screen.
			// screen.click("C:\\Users\\vineet.tanwar\\eclipse-workspace\\AutomationTesting\\src\\main\\resources\\images\\menu_bttn_statestreet.png");
			logger.info(element + " Clicked");
		} catch (Exception e) {
			// ExceptionLog.info(e.toString());
			isPassed = false;
		}
	}
	public static void hoveronimage(String element) throws InterruptedException {
		Screen screen = new Screen();
		try {
			String Path = (System.getProperty("user.dir") + "\\src\\main\\resources\\images\\");
			// String
			// fpath="C:\\Users\\kislay.kumar\\eclipse-workspace\\AutomationTesting\\src\\main\\resources\\images\\"+XPathDict.get(element);
			String fpath = Path + XPathDict.get(element);
			screen.hover(fpath);
			//screen.
			// screen.click("C:\\Users\\vineet.tanwar\\eclipse-workspace\\AutomationTesting\\src\\main\\resources\\images\\menu_bttn_statestreet.png");
			logger.info(element + " Clicked");
		} catch (Exception e) {
			// ExceptionLog.info(e.toString());
			isPassed = false;
		}
	}


	private static void simulateMouseScroll(Robot robot, int steps) {
		// Positive steps for scrolling up, negative steps for scrolling down
		robot.mouseWheel(steps);
	}

	public static void ScrollIntoView(String element) throws InterruptedException {

		try {
			Robot robot = new Robot();

			// Simulate a mouse click at (100, 100)
			// fmtc
			robot.mouseMove(300, 400);
			// ss
			// robot.mouseMove(700, 400);
			Thread.sleep(2000);

			robot.mousePress(InputEvent.BUTTON1_DOWN_MASK);
			robot.mouseRelease(InputEvent.BUTTON1_DOWN_MASK);
			Thread.sleep(2000);

			simulateMouseScroll(robot, 300);
			Thread.sleep(2000);
			simulateMouseScroll(robot, 300);
			// simulateMouseScroll(robot, -200);

			// String xpath=XPathDict.get(element);
//			String tableelement="//*[@id=\"tab-pane-1\"]/div/div[2]/div/div/div/div[1]/div[2]/div[2]/div/div[1]/div/div/div/div[2]/div[1]/div[4]/div";
//			waitForElement(tableelement);
//			WebElement ele=getWebElement(tableelement);
//			 executeScript(ele);

//			//*[@id="tab-pane-1"]/div/div[2]/div/div/div/div[1]/div[2]/div[2]/div/div[1]/div/div/div/div[2]/div[1]/div[4]/div
//			WebDriverWait wait = new WebDriverWait(driver, 1);
//			String tablexpath="//*[@id=\"tab-pane-1\"]/div/div[2]/div/div/div/div[1]/div[2]/div[2]";
//			 WebElement tableelement = wait.until(ExpectedConditions.presenceOfElementLocated(By.xpath(tablexpath)));
//			 //((JavascriptExecutor) driver).executeScript("arguments[0].scrollIntoView(true);", tableelement);
//			String xpath=XPathDict.get(element);
//	       // WebElement element1 = wait.until(ExpectedConditions.presenceOfElementLocated(By.xpath(xpath)));
//	        WebElement element1 = tableelement.findElement(By.xpath(xpath));
//	        ((JavascriptExecutor) driver).executeScript("arguments[0].scrollIntoView(true);", tableelement);
//	        // Scroll to the element using JavaScript
//	        //((JavascriptExecutor) driver).executeScript("arguments[0].scrollIntoView(true);", element1);
		} catch (Exception e) {
			logger.error(e.toString());
			isPassed = false;
		}
	}

	public static void ExportforFMTC(String element) throws InterruptedException {

		try {
			//// *[@id="PageContentOuterDiv"]
			String divxpath = "//*[@id=\"PageContentOuterDiv\"]";
			WebElement ele = getWebElement(divxpath);
//			String xpath=XPathDict.get(element);
//			waitForElement(xpath);
//			WebElement ele=getWebElement(xpath);
			JavascriptExecutor js = (JavascriptExecutor) driver;
			js.executeScript("arguments[0].scrollTop = arguments[0].scrollHeight;", ele);

			// js.executeScript("arguments[0].scrollIntoView(true);", ele);

			// # Scroll the inner div to make the target element visible
//			js.executeScript("arguments[0].scrollTop = arguments[0].scrollHeight;", ele);
//	        // Example: Execute JavaScript to change the background color of the body element
//			js.executeScript("document.querySelector('a[title=\"Export to different format\"]').click();");
//			logger.info("first js executed");
//			js.executeScript(
//		            "var elements = document.getElementsByClassName('MenuItemTextCell');" +
//		            "var targetElement = null;" +
//		            "var searchText = 'Excel 2007+';" +
//		            "for (var i = 0; i < elements.length; i++) {" +
//		            "  if (elements[i].textContent.includes(searchText)) {" +
//		            "    targetElement = elements[i];" +
//		            "    break;" +
//		            "  }" +
//		            "}" +
//		            "targetElement.click();"
//		        );
			logger.info("second js executed");

			// document.querySelector('a[title="Export to different format"]').click();

			// logger.info(element +" Clicked");
		} catch (Exception e) {
			// ExceptionLog.info(e.toString());
			logger.error(e.toString());
			isPassed = false;
		}
	}

	public static void scrollandfind(String element) throws InterruptedException {

		try {
			String xpath = XPathDict.get(element);
			WebElement ele = null;
			Robot robot = new Robot();
			robot.mouseMove(700, 400);
			while (ele == null) {
				try {

					ele = getWebElement(xpath);
				} catch (Exception e) {

					robot.mouseMove(700, 400);
					Thread.sleep(2000);

					robot.mousePress(InputEvent.BUTTON1_DOWN_MASK);
					robot.mouseRelease(InputEvent.BUTTON1_DOWN_MASK);
					Thread.sleep(2000);

					simulateMouseScroll(robot, 2);
					// Thread.sleep(2000);

				}
			}
			waitForElement(xpath);
			WebElement ele1 = getWebElement(xpath);
			executeScript(ele1);

			// JavascriptExecutor executor = (JavascriptExecutor)driver;
			// executor.executeScript("arguments[0].click();",ele);

		} catch (Exception e) {
			logger.error(e.toString());
			isPassed = false;
		}
	}

	public static void clickFidelityExportButton(String element, String arg) throws InterruptedException {

		try {
			String FolderName = XPathDict.get("Folder Name");
			String LogFilePath = "";
			if (!FolderName.equals("")) {
				LogFilePath = DownloadLogFilePath.get(FolderName);
			}
			
			String iFrameXpath = XPathDict.get("iFrame");
			WebElement iframeElement = driver.findElement(By.xpath(iFrameXpath));
			driver.switchTo().frame(iframeElement);
			
			
			String accNoXpath = XPathDict.get("AccountText");
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
				if(cashAccountList.contains(reportName))
				{
					reportName=reportName+"--- Cash File Also need to be downloaded ---";
				}
				LogDownloadedFile(LogFilePath, reportName);
			}

			logger.info(element + " Clicked");
			driver.switchTo().defaultContent();
		} catch (Exception e) {
			// ExceptionLog.info(e.toString());
			logger.error(e.toString());
			isPassed = false;
		}

	}
	
	
		
	
	public static void ClickLink(String element)
	{
		try {
			String xpath = XPathDict.get(element);
			// Define the XPath locator for the element
            By xpathLocator = By.xpath(xpath);

            // Use WebDriverWait to wait for the element to be visible and clickable
            WebDriverWait wait = new WebDriverWait(driver, 10);
            WebElement ele = wait.until(ExpectedConditions.elementToBeClickable(xpathLocator));
           
            // Click the element
            ele.click();
		} catch (Exception e) {
			// ExceptionLog.info(e.toString());
			logger.error(e.toString());
			isPassed = false;
		}
	}
	public static void clickOnlyWhenEnabled(String element) throws InterruptedException {

		try {
			String xpath = XPathDict.get(element);
			waitForElement(xpath);
			WebElement ele = getWebElement(xpath);
			
			  // Check if the button is enabled
	        if (!ele.getAttribute("aria-disabled").equals("true")) {
	            // Click the button
	        	ele.click();
	        	logger.info( element + " Clicked");
	        } else {
	        	isBreakStep=false;
	        	logger.info("Download button is disabled, cannot click");
	        }
		

		} catch (Exception e) {
			//ExceptionLog.info(e.toString());
			logger.error(e.toString());
			isPassed = false;
		}

	}
	public static void clickButton2(String element) throws InterruptedException {

		try {
			String xpath = XPathDict.get(element);
			waitForElement(xpath);
			WebElement ele = getWebElement(xpath);
			
				ele.click();
			logger.info( element + " Clicked");

		} catch (Exception e) {
			//ExceptionLog.info(e.toString());
			logger.error(e.toString());
			isPassed = false;
		}

	}

	public static void ClickButtons(String element) throws InterruptedException {

        try {
            // Wait for elements to be present
            String xpath = XPathDict.get(element);
            JavascriptExecutor js = ((JavascriptExecutor) driver);
            js.executeScript("window.scrollTo(0, document.body.scrollHeight)");
            waitForElement(xpath);

            // Get the list of WebElements
            List<WebElement> elements = driver.findElements(By.xpath(xpath));

            // Iterate through the list and click each element with a 500 ms delay
            for (WebElement ele : elements) {
                executeScript(ele);
                logger.info("Element clicked: " + ele.toString());
                Thread.sleep(500); // 500 ms delay
            }

        } catch (Exception e) {
            logger.error("Error clicking elements: " + e.toString());
            isPassed = false;
        }
    }
	
	public static void clickElementAfterScroll(String element) {


	    try {
	    	WebDriverWait wait = new WebDriverWait(driver, 20);
	        JavascriptExecutor js = (JavascriptExecutor) driver;
	        String xpath = XPathDict.get(element);
	        // Wait until the element is present in the DOM
	        wait.until(ExpectedConditions.presenceOfElementLocated(By.xpath(xpath)));

	        WebElement ele = driver.findElement(By.xpath(xpath));

	        // Scroll the element into view
	        js.executeScript("arguments[0].scrollIntoView(true);", ele);

	        // Scroll horizontally if necessary (e.g., if the element is outside the visible area)
	        js.executeScript("window.scrollBy(" + ele.getLocation().getX() + ", 0);");

	        // Wait until the element is visible
	        wait.until(ExpectedConditions.visibilityOf(ele));

	     // Perform a double-click action
	        Actions actions = new Actions(driver);
	        actions.moveToElement(ele).click().perform();

	        logger.info("Clicked on element with XPath: " + xpath);
	    } catch (Exception e) {
	        logger.error("Failed to click on element with XPath: ");
	        logger.error(e.toString());
	        isPassed = false;
	    }
	}
	public static void clickElementsAfterScroll_V2(String element, String elementsToScroll) {
	    try {
	        WebDriverWait wait = new WebDriverWait(driver, 10);
	        JavascriptExecutor js = (JavascriptExecutor) driver;
	        String xpath = XPathDict.get(element);

	        // Split elementsToScroll by "#"
	        String[] scrollElements = elementsToScroll != null ? elementsToScroll.split("#") : new String[]{"", ""};
	        String verticalScrollXpath = scrollElements.length > 0 ? XPathDict.get(scrollElements[0]) : "";
	        String horizontalScrollXpath = scrollElements.length > 1 ? XPathDict.get(scrollElements[1]) : "";

	        WebElement verticalScrollElement = null;
	        WebElement horizontalScrollElement = null;

	        if (verticalScrollXpath != null && !verticalScrollXpath.isEmpty()) {
	            verticalScrollElement = driver.findElement(By.xpath(verticalScrollXpath));
	        }
	        if (horizontalScrollXpath != null && !horizontalScrollXpath.isEmpty()) {
	            horizontalScrollElement = driver.findElement(By.xpath(horizontalScrollXpath));
	        }

	        // **RESET SCROLL POSITION BEFORE SEARCHING** 
	        if (verticalScrollElement != null) {
	            js.executeScript("arguments[0].scrollTop = 0;", verticalScrollElement);
	        } else {
	            js.executeScript("window.scrollTo(0, 0);"); // Reset entire page scroll
	        }

	        if (horizontalScrollElement != null) {
	            js.executeScript("arguments[0].scrollLeft = 0;", horizontalScrollElement);
	        } else {
	            js.executeScript("window.scrollTo(0, 0);");
	        }

	        // **Now Start Progressive Scrolling**
	        boolean elementFound = false;
	        int maxScrollAttempts = 20; // Prevent infinite loops
	        int scrollStep = 100; // Small scroll steps

	        for (int i = 0; i < maxScrollAttempts; i++) {
	            // Check if the target element is found
	            List<WebElement> elements = driver.findElements(By.xpath(xpath));
	            if (!elements.isEmpty()) {
	                elementFound = true;
	                for (WebElement ele : elements) {
	                    js.executeScript("arguments[0].scrollIntoView(true);", ele);
	                    wait.until(ExpectedConditions.visibilityOf(ele));
	                    executeScript(ele);
	                    logger.info("Clicked on element with XPath: " + xpath);
	                    Thread.sleep(1500);
	                }
	                break; // Stop scrolling if found
	            }

	            // Scroll slightly down if a vertical scroll element exists
	            if (verticalScrollElement != null) {
	                js.executeScript("arguments[0].scrollTop += arguments[1];", verticalScrollElement, scrollStep);
	            } else {
	                js.executeScript("window.scrollBy(0, arguments[0]);", scrollStep);
	            }

	            // Scroll slightly right if a horizontal scroll element exists
	            if (horizontalScrollElement != null) {
	                js.executeScript("arguments[0].scrollLeft += arguments[1];", horizontalScrollElement, scrollStep);
	            } else {
	                js.executeScript("window.scrollBy(arguments[0], 0);", scrollStep);
	            }

	            Thread.sleep(500); // Allow time for elements to load
	        }

	        if (!elementFound) {
	            logger.error("Element not found after scrolling. XPath: " + xpath);
	            isPassed = false;
	        }

	    } catch (Exception e) {
	        logger.error("Failed to click on elements with XPath: " + XPathDict.get(element));
	        logger.error(e.toString());
	        isPassed = false;
	    }
	}

	public static void doubleclickElementsAfterScroll_V2(String element, String elementsToScroll) {
	    try {
	        WebDriverWait wait = new WebDriverWait(driver, 10);
	        JavascriptExecutor js = (JavascriptExecutor) driver;
	        String xpath = XPathDict.get(element);

	        // Split elementsToScroll by "#"
	        String[] scrollElements = elementsToScroll != null ? elementsToScroll.split("#") : new String[]{"", ""};
	        String verticalScrollXpath = scrollElements.length > 0 ? XPathDict.get(scrollElements[0]) : "";
	        String horizontalScrollXpath = scrollElements.length > 1 ? XPathDict.get(scrollElements[1]) : "";

	        WebElement verticalScrollElement = null;
	        WebElement horizontalScrollElement = null;

	        if (verticalScrollXpath != null && !verticalScrollXpath.isEmpty()) {
	            verticalScrollElement = driver.findElement(By.xpath(verticalScrollXpath));
	        }
	        if (horizontalScrollXpath != null && !horizontalScrollXpath.isEmpty()) {
	            horizontalScrollElement = driver.findElement(By.xpath(horizontalScrollXpath));
	        }

	        // **RESET SCROLL POSITION BEFORE SEARCHING** 
	        if (verticalScrollElement != null) {
	            js.executeScript("arguments[0].scrollTop = 0;", verticalScrollElement);
	        } else {
	            js.executeScript("window.scrollTo(0, 0);"); // Reset entire page scroll
	        }

	        if (horizontalScrollElement != null) {
	            js.executeScript("arguments[0].scrollLeft = 0;", horizontalScrollElement);
	        } else {
	            js.executeScript("window.scrollTo(0, 0);");
	        }

	        // **Now Start Progressive Scrolling**
	        boolean elementFound = false;
	        int maxScrollAttempts = 20; // Max scroll attempts (can adjust based on your use case)
	        
	        // Dynamically calculate scroll step
	        int scrollStep = 0;
	        int scrollHeight = 0;
	        int scrollWidth = 0;

	        // Calculate scroll step for vertical scroll
	        if (verticalScrollElement != null) {
	            scrollHeight = Integer.parseInt(js.executeScript("return arguments[0].scrollHeight;", verticalScrollElement).toString());
	            scrollStep = scrollHeight / maxScrollAttempts; // Divide scroll height by max attempts
	        } else {
	            // If no vertical scroll element, calculate based on window height
	            scrollHeight = Integer.parseInt(js.executeScript("return window.innerHeight;", verticalScrollElement).toString());
	            scrollStep = scrollHeight / maxScrollAttempts; // Divide window height by max attempts
	        }

	        // Calculate scroll step for horizontal scroll
	        if (horizontalScrollElement != null) {
	            scrollWidth = Integer.parseInt(js.executeScript("return arguments[0].scrollWidth;", horizontalScrollElement).toString());
	        } else {
	            scrollWidth = Integer.parseInt(js.executeScript("return window.innerWidth;", horizontalScrollElement).toString());
	        }

	        for (int i = 0; i < maxScrollAttempts; i++) {
	            // Check if the target element is found
	            List<WebElement> elements = driver.findElements(By.xpath(xpath));
	            if (!elements.isEmpty()) {
	                elementFound = true;
	                for (WebElement ele : elements) {
	                   
	                    wait.until(ExpectedConditions.visibilityOf(ele));
	                    Actions actions = new Actions(driver);

	                    // Perform double-click on the element
	                    actions.doubleClick(ele).perform();
	                    logger.info("Double-clicked on element with XPath: " + xpath);
	                    Thread.sleep(1500);
	                }
	                break; // Stop scrolling if found
	            }

	            // Scroll vertically if a vertical scroll element exists
	            if (verticalScrollElement != null) {
	                js.executeScript("arguments[0].scrollTop += arguments[1];", verticalScrollElement, scrollStep);
	            } else {
	                js.executeScript("window.scrollBy(0, arguments[0]);", scrollStep);
	            }

	            // Scroll horizontally if a horizontal scroll element exists
	            if (horizontalScrollElement != null) {
	                js.executeScript("arguments[0].scrollLeft += arguments[1];", horizontalScrollElement, scrollStep);
	            } else {
	                js.executeScript("window.scrollBy(arguments[0], 0);", scrollStep);
	            }

	            Thread.sleep(500); // Allow time for elements to load
	        }

	        if (!elementFound) {
	            logger.error("Element not found after scrolling. XPath: " + xpath);
	            isPassed = false;
	        }

	    } catch (Exception e) {
	        logger.error("Failed to double-click on elements with XPath: " + XPathDict.get(element));
	        logger.error(e.toString());
	        isPassed = false;
	    }
	}

	
	public static void clickElementAfterScroll_Citi(String element) {


	    try {
	    	WebDriverWait wait = new WebDriverWait(driver, 20);
	        JavascriptExecutor js = (JavascriptExecutor) driver;
	        String xpath = XPathDict.get(element);
	        // Wait until the element is present in the DOM
	        wait.until(ExpectedConditions.presenceOfElementLocated(By.xpath(xpath)));

	        WebElement ele = driver.findElement(By.xpath(xpath));

	        // Scroll the element into view
	        js.executeScript("arguments[0].scrollIntoView(true);", ele);

	        // Scroll horizontally if necessary (e.g., if the element is outside the visible area)
	        js.executeScript("window.scrollBy(" + ele.getLocation().getX() + ", 0);");

	       

	       
			executeScript(ele);
			logger.info(element + " Clicked");
	    } catch (Exception e) {
	        logger.error("Failed to click on element with XPath: ");
	        logger.error(e.toString());
	        isPassed = false;
	    }
	}
	
	public static void clickElementsAfterScroll(String element, String elementsToScroll) {
        try {
        	WebDriverWait wait = new WebDriverWait(driver, 10);
            JavascriptExecutor js = (JavascriptExecutor) driver;
            String xpath = XPathDict.get(element);

            // Split the elementsToScroll argument by "#"
            String[] scrollElements = elementsToScroll != null ? elementsToScroll.split("#") : new String[]{"", ""};
            String verticalScrollXpath = scrollElements.length > 0 ? XPathDict.get(scrollElements[0]) : "";
            String horizontalScrollXpath = scrollElements.length > 1 ? XPathDict.get(scrollElements[1]) : "";

            // Scroll vertically to the maximum extent within the specified element or the DOM if the element is not provided
            if (verticalScrollXpath != null && !verticalScrollXpath.isEmpty()) {
                WebElement verticalScrollElement = driver.findElement(By.xpath(verticalScrollXpath));
                js.executeScript("arguments[0].scrollTop = arguments[0].scrollHeight;", verticalScrollElement);
            } else {
                // Scroll vertically using the DOM to the bottom of the page
                js.executeScript("window.scrollTo(0, document.body.scrollHeight);");
            }

            // Scroll horizontally to the maximum extent within the specified element or the DOM if the element is not provided
            if (horizontalScrollXpath != null && !horizontalScrollXpath.isEmpty()) {
                WebElement horizontalScrollElement = driver.findElement(By.xpath(horizontalScrollXpath));
                js.executeScript("arguments[0].scrollLeft = arguments[0].scrollWidth;", horizontalScrollElement);
            } else {
                // Scroll horizontally using the DOM to the rightmost edge
                js.executeScript("window.scrollBy(document.body.scrollWidth, 0);");
            }

            // Now wait until at least one element is present in the DOM
            wait.until(ExpectedConditions.presenceOfAllElementsLocatedBy(By.xpath(xpath)));

            List<WebElement> elements = driver.findElements(By.xpath(xpath));

            for (WebElement ele : elements) {
                // Ensure the element is visible after scrolling both vertically and horizontally
                js.executeScript("arguments[0].scrollIntoView(true);", ele);

                // Wait until the element is visible
                wait.until(ExpectedConditions.visibilityOf(ele));

                // Click the element
                executeScript(ele);

                logger.info("Clicked on element with XPath: " + xpath);
                Thread.sleep(1500);
            }
        } catch (Exception e) {
            logger.error("Failed to click on elements with XPath: " + XPathDict.get(element));
            logger.error(e.toString());
            isPassed = false;
        }
    }

	public static void clickAndCheckIfFileIsDownloaded(String element, String downloadDirectory) throws InterruptedException {
	    try {
	        String xpath = XPathDict.get(element);
	        waitForElement(xpath);
	        WebElement ele = getWebElement(xpath);
	        long timeout = 20;  // Set timeout to 20 seconds
	        File dir = new File(downloadDirectory);
	        
	        for (int i = 0; i < 4; i++) {  // Loop to click the button 4 times
	            try {
	                executeScript(ele);
	                logger.info(element + " Clicked " + (i + 1) + " time(s)");
	                
	                WebDriverWait wait = new WebDriverWait(driver, timeout);
	                
	                boolean fileDownloaded = wait.until(driver -> {
	                    File[] files = dir.listFiles();
	                    if (files != null) {
	                        for (File file : files) {
	                            if (file.isFile() && file.length() > 0 && !file.getName().endsWith(".crdownload")) {
	                                return true;
	                            }
	                        }
	                    }
	                    return false;
	                });
	                if (fileDownloaded) {
	                    logger.info("File downloaded successfully after click " + (i + 1));
	                    break; // Exit loop if file is downloaded
	                }
	            } catch (TimeoutException e) {
	                logger.warn("Timeout occurred: File not found after click " + (i + 1) + ", retrying...");
	            }
	        }
	    } catch (Exception e) {
	        logger.error("Error encountered: " + e.toString());
	        isPassed = false;
	    }
	}

}
