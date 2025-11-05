package com.nirvana.Helper;

import java.time.Duration;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.List;

import com.github.andrewoma.dexx.collection.Set;
import com.google.common.base.Stopwatch;
import com.nirvana.TestCases.BaseClass;
import com.nirvana.TestCases.ExecuteTestCase_1;
import org.apache.poi.xssf.usermodel.XSSFRow;
import org.openqa.selenium.By;
import org.openqa.selenium.JavascriptExecutor;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.support.ui.ExpectedConditions;
import org.openqa.selenium.support.ui.WebDriverWait;
import java.awt.AWTException;
import java.awt.Rectangle;
import java.awt.Robot;
import java.awt.Toolkit;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;
import javax.imageio.ImageIO;

public class WindowHelper extends BaseClass {

	public static void switchTo(int index) {
		try {
			List<String> win = new ArrayList<String>(driver.getWindowHandles());
			driver.switchTo().window(win.get(index));
		} catch (IndexOutOfBoundsException e) {
			throw new IndexOutOfBoundsException("Invalid Window Index : " + index);
		}

	}

	public static void switchToParentWithClose() {
		List<String> win = new ArrayList<String>(driver.getWindowHandles());

		for(int i = 1; i < win.size(); i++){
			driver.switchTo().window(win.get(i));
			driver.close();
		}
		driver.switchTo().window(win.get(0));

	}

	public static void back() {
		driver.navigate().back();

	}

	public static  void forward() {
		driver.navigate().forward();

	}

	public static void refresh() {
		driver.navigate().refresh();

	}

	public static void navigateToPage(String url) {
		driver.navigate().to(url);
		logger.info("Navigated to "+url);
	}

	public static void windowMaximize() {
		driver.manage().window().maximize();
	}

	public static void CaptureImage(String folderName, String subfolderName) throws AWTException, IOException {


		Toolkit toolkit = Toolkit.getDefaultToolkit();
		Rectangle screenRect = new Rectangle(toolkit.getScreenSize());
		Robot robot = new Robot();
		BufferedImage screenshot = robot.createScreenCapture(screenRect);
		String name = LocalDateTime.now().format(DateTimeFormatter.ofPattern("dd-MM-yyyy HH-mm-ss"))+".png";
		File folder = new File(folderName);
		if (!folder.exists()) {
			boolean folderCreated = folder.mkdirs();
			if (folderCreated) {
				HelperClass.PrintConsole("Folder created: " + folder.getAbsolutePath());
			} else {
				HelperClass.PrintConsole("Failed to create folder: " + folder.getAbsolutePath());
			}
		}

		if (!folder.exists()) {
			boolean folderCreated = folder.mkdirs();
			if (folderCreated) {
				com.nirvana.Helper.HelperClass.PrintConsole("Main folder created: " + folder.getAbsolutePath());
			} else {
				System.err.println("Failed to create main folder: " + folder.getAbsolutePath());
			}
		}

		if (!subfolderName.isEmpty()) {
			File subFolder = new File(folder, subfolderName);
			if (!subFolder.exists()) {
				boolean subFolderCreated = subFolder.mkdirs();
				if (subFolderCreated) {
					com.nirvana.Helper.HelperClass.PrintConsole("Subfolder created: " + subFolder.getAbsolutePath());
				} else {
					System.err.println("Failed to create subfolder: " + subFolder.getAbsolutePath());
				}
			}

			File outputFile = new File(subFolder, name + ".png");
			ImageIO.write(screenshot, "PNG", outputFile);
			com.nirvana.Helper.HelperClass.PrintConsole("Screenshot saved in subfolder as " + outputFile.getAbsolutePath());
		} else {

			File outputFile = new File(folder, name + ".png");
			ImageIO.write(screenshot, "PNG", outputFile);
			com.nirvana.Helper.HelperClass.PrintConsole("Screenshot saved in main folder as " + outputFile.getAbsolutePath());
		}

	}

	public static boolean IsSwitched = true;
	public static int SwitchCount = 0;

	public static void SwitchWindowHandles(String WName) throws IOException, AWTException {
		boolean found = false;
		IsSwitched = true;
		String windowName1 = WName;
		com.nirvana.Helper.HelperClass.PrintConsole("Try to switch window to " + windowName1);
		boolean windowFoundOrTimeOut = false;
		Stopwatch sw = Stopwatch.createStarted();
		while (!windowFoundOrTimeOut) {


			if(SwitchCount==2)
			{
				windowFoundOrTimeOut = true;
				//logger.info("Window not found in defined time");
				com.nirvana.Helper.HelperClass.PrintConsole("Logout exceed");
				IsSwitched = false;
				CaptureImage(WName,"Failure");
				ExecuteTestCase_1.IsAnyIssue = false;
				break;
			}
			if(sw.elapsed().getSeconds()>60) {
				ExecuteTestCase_1.IsAnyIssue = true;
				//com.nirvana.Helper.HelperClass.PrintConsole("Total seconds "+sw.elapsed().getSeconds());
				SwitchCount++;
					windowFoundOrTimeOut = true;
				com.nirvana.Helper.HelperClass.PrintConsole("Rerun for count "+SwitchCount);
				CaptureImage(WName,"Failure");
				break;



				/*if (sw.elapsed().getSeconds() > 300) {
					windowFoundOrTimeOut = true;
					logger.info("Window not found in defined time");
					IsSwitched = false;
					CaptureImage();

				}*/
			}

			try {
				for (String handle : driver.getWindowHandles()) {
					driver.switchTo().window(handle);
					String url = (String) ((JavascriptExecutor) driver).executeScript("return window.location.href;");
					if (url.toLowerCase().contains("browser"))
						continue;
					String title = driver.getTitle();

					//com.nirvana.Helper.HelperClass.PrintConsole(title);

					if (title.contains(windowName1)) {
						try {
							com.nirvana.Helper.HelperClass.PrintConsole("Window switched successfully to: " + driver.getTitle());
							IsSwitched = true;
							found = true;
							//SwitchCount = 0;
							windowFoundOrTimeOut = true;
							ExecuteTestCase_1.IsAnyIssue = false;
							((JavascriptExecutor) driver).executeScript("window.focus();");
							List<WebElement> elements = driver.findElements(By.xpath("//*"));
							if (elements.isEmpty()) {
								com.nirvana.Helper.HelperClass.PrintConsole("The DOM is empty.");
								windowFoundOrTimeOut = false;
								IsSwitched = false;
							}
							break;
						} catch (Exception e) {
							//com.nirvana.Helper.HelperClass.PrintConsole("Element not visible in the current window: " + e.getMessage());

						}
					}
				}
			} catch (Exception ex) {
			}
			if (!found) {
				//com.nirvana.Helper.HelperClass.PrintConsole("Window with title '" + windowName1 + "' not found or element not visible.");
			}
		}
	}
	public static void SwitchToExact(String WName) {
		boolean found = false;
		IsSwitched = true;
		String windowName1 = WName;
		com.nirvana.Helper.HelperClass.PrintConsole("Try to switch window to " + windowName1);
		boolean windowFoundOrTimeOut = false;
		Stopwatch sw = Stopwatch.createStarted();
		while (!windowFoundOrTimeOut) {
			if (sw.elapsed().getSeconds() > 60) {
				windowFoundOrTimeOut = true;
				HelperClass.PrintConsole("Window not found in defined time");
				IsSwitched = false;
				SwitchCount++;

			}
			try {
				for (String handle : driver.getWindowHandles()) {
					driver.switchTo().window(handle);

					String title = driver.getTitle();

					//com.nirvana.Helper.HelperClass.PrintConsole(title);

					if (title.equals(windowName1)) {
						try {
							com.nirvana.Helper.HelperClass.PrintConsole("Window switched successfully to: " + driver.getTitle());
							found = true;
							IsSwitched = true;
							windowFoundOrTimeOut = true;
							ExecuteTestCase_1.IsAnyIssue = false;
							((JavascriptExecutor) driver).executeScript("window.focus();");
							break;
						} catch (Exception e) {
							com.nirvana.Helper.HelperClass.PrintConsole("Element not visible in the current window: " + e.getMessage());
						}
					}
				}
			} catch (Exception ex) {
			}

		}
	}


	public static void SwitchWindowHandles2(String WName) {

		boolean found = false;

		String windowName = WName;
		found = false;
		int tryCount=0;

		while (!found & tryCount<11) {

			for (String handle : driver.getWindowHandles()) {
				driver.switchTo().window(handle);
				String title = driver.getTitle();
				//com.nirvana.Helper.HelperClass.PrintConsole("Try to switch window to " + windowName);
				//com.nirvana.Helper.HelperClass.PrintConsole(title);

				if (title.contains(windowName)) {
					com.nirvana.Helper.HelperClass.PrintConsole("Window switched to " + title);
					found = true;
					((JavascriptExecutor) driver).executeScript("window.focus();");
					break;
				}
			}

			tryCount++;
		}

	}

	public static void Navigate(XSSFRow row) {


		navigateToPage(row.getCell(3).getStringCellValue());


	}

}
