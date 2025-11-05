package com.nirvana.Helper;

import java.io.File;
import java.io.FileFilter;
import java.io.IOException;

import org.apache.poi.xssf.usermodel.XSSFRow;
import com.nirvana.TestCases.BaseClass;
import net.lingala.zip4j.core.ZipFile;
import net.lingala.zip4j.exception.ZipException;

public class ExtractDownloadedFile extends BaseClass {
	
	
	
	public static void Extractzip(String Folder) throws ZipException, InterruptedException {
		
       try {
		Thread.sleep(10000);
		File f = new File("E:\\ReportMatchingDestination\\");  
        FileFilter filter = new FileFilter() {
            public boolean accept(File f)
            {
                return f.getName().endsWith("zip");
            }
        };

        
        File[] files = f.listFiles(filter);
		String	sour = files[0].getName();
		String destination = Folder;
 		
				ZipFile zipFile = new ZipFile("E:\\ReportMatchingDestination\\"+sour);
			
		     
				zipFile.extractAll(destination);
					Thread.sleep(10000);
					files[0].delete();
					logger.info("Files Extracted");
		}
	
	catch(Exception e)
	{
		//ExceptionLog.info(e.toString());
		isPassed=false;
		logger.error(e.toString());
	}

	}
}