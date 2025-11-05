package com.nirvana.Utilities;





import java.io.File;
import java.io.FileFilter;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;

import com.nirvana.TestCases.BaseClass;
import com.nirvana.TestCases.ExecuteTestCase_1;

import org.apache.commons.io.FileUtils;
import org.apache.poi.ss.usermodel.DataFormatter;
import org.apache.poi.xssf.usermodel.XSSFCell;
import org.apache.poi.xssf.usermodel.XSSFRow;
import org.apache.poi.xssf.usermodel.XSSFSheet;
import org.apache.poi.xssf.usermodel.XSSFWorkbook;
import org.openqa.selenium.WebElement;

public class XLUtils extends BaseClass{
	
		
		public static FileInputStream fi;
		public static FileOutputStream fo;
		public static XSSFWorkbook wb;
		public static XSSFSheet ws;
		public static XSSFRow row;
		public static XSSFCell cell;

		
		
		public static int getRowCount(String xlfile,String xlsheet) throws IOException 
		{
			
			fi=new FileInputStream(xlfile);
			wb=new XSSFWorkbook(fi);
			ws=wb.getSheet(xlsheet);
			int rowcount=ws.getLastRowNum();
			wb.close();
			fi.close();
			return rowcount;	
			
		}
		
		
		public static int getCellCount(String xlfile,String xlsheet,int rownum) throws IOException
		{
			fi=new FileInputStream(xlfile);
			wb=new XSSFWorkbook(fi);
			ws=wb.getSheet(xlsheet);
			row=ws.getRow(rownum);
			int cellcount=row.getLastCellNum();
			wb.close();
			fi.close();
			return cellcount;
		}
		
		
		public static String getCellData(String xlfile,String xlsheet,int rownum,int colnum) throws IOException
		{
			fi=new FileInputStream(xlfile);
			wb=new XSSFWorkbook(fi);
			ws=wb.getSheet(xlsheet);
			row=ws.getRow(rownum);
			cell=row.getCell(colnum);
			String data;
			try 
			{
				DataFormatter formatter = new DataFormatter();
	            String cellData = formatter.formatCellValue(cell);
	            return cellData;
			}
			catch (Exception e) 
			{
				data="";
			}
			wb.close();
			fi.close();
			return data;
		}
		
		public static void setCellData(String xlfile,String xlsheet,int rownum,int colnum,String data) throws IOException
		{
			fi=new FileInputStream(xlfile);
			wb=new XSSFWorkbook(fi);
			ws=wb.getSheet(xlsheet);
			row=ws.getRow(rownum);
			cell=row.createCell(colnum);
			cell.setCellValue(data);
			fo=new FileOutputStream(xlfile);
			wb.write(fo);		
			wb.close();
			fi.close();
			fo.close();
		}
		
		public static int getRowNum(String xlfile,String xlsheet,String arg) throws IOException
		{
			fi=new FileInputStream(xlfile);
			wb=new XSSFWorkbook(fi);
			ws=wb.getSheet(xlsheet);
			int rowcount=ws.getLastRowNum();
			
			for(int i=0;i<=rowcount;i++)
			{
				row=ws.getRow(i);
				if(row!=null)
					{ if(row.getCell(0)!=null && !row.getCell(0).getStringCellValue().isEmpty()) 
					{
					if(row.getCell(0).getStringCellValue().equals(arg))
				{
					return i;
				}
					}
					}
			}
			
			return -1;
		}
		
		public static void MoveFiles() throws IOException
		{
			//Copying files
			String sourcePath="E:\\ReportMatchingDestination\\";
		       String targetPath="E:\\ReportMatchingDestination\\"+ExecuteTestCase_1.module+"\\";
				
				File f = new File(sourcePath);
				  
		        FileFilter filter = new FileFilter() {

		            public boolean accept(File f)
		            {
		                return f.getName().endsWith("xlsx");
		            }
		        };

		        
		        File[] files = f.listFiles(filter);
		        String fileName;
		        
		        for(File F: files)
		        {
		        	fileName=F.getName();
		        	File targetFile=new File(targetPath+fileName);
		        	FileUtils.copyFile(F, targetFile);
		        	F.delete();
		        	
		        }
		        
		       
		}
		
		
		public static void RenameFiles(String element)
		{
			try
			{
			String xpath=XPathDict.get(element);
			waitForElement(xpath);
			WebElement ele=getWebElement(xpath);
			String text=ele.getText();
			text=text.substring(13);
			String [] date=text.split("/");
			String yy=date[2];
			String dd=date[1];
			String mm=date[0];
			
			if(dd.length()==1)
				dd="0"+dd;
			if(mm.length()==1)
				mm="0"+mm;
			
			String name=yy+mm+dd;
			String sourcePath="E:\\ReportMatchingDestination\\";
		       
				File f = new File(sourcePath);
				  
		        FileFilter filter = new FileFilter() {

		            public boolean accept(File f)
		            {
		                return f.getName().endsWith("xlsx");
		            }
		        };

		        
		        File[] files = f.listFiles(filter);
		        String fileName;
		        
		        for(File F: files)
		        {
		        	fileName=F.getName();
		        	fileName=fileName.substring(0,fileName.length()-5);
		        	if(!fileName.contains(name))
		        	{File oldFile=new File(sourcePath+fileName+".xlsx");
		        	File newFile=new File(sourcePath+fileName+"_"+name+".xlsx");
		        	oldFile.renameTo(newFile);
		        	}
		        	
		        }
			}
			catch(Exception e)
			{
				//ExceptionLog.info(e.toString());
				isPassed=false;
				logger.error(e.toString());
			}
		}
	}



