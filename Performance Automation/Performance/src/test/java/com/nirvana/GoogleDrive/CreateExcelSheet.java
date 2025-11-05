package com.nirvana.GoogleDrive;

import com.google.api.client.googleapis.javanet.GoogleNetHttpTransport;
import com.google.api.client.http.javanet.NetHttpTransport;
import com.google.api.client.json.JsonFactory;
import com.google.api.client.json.gson.GsonFactory;
import com.google.api.services.drive.Drive;
import com.google.api.services.drive.model.File;
import com.google.api.services.drive.model.Permission;
import com.google.api.services.sheets.v4.Sheets;
import com.google.api.services.sheets.v4.model.*;
import org.apache.jena.base.Sys;

import java.io.BufferedWriter;
//import java.io.File;
import java.io.FileWriter;
import java.io.IOException;
import java.net.InetAddress;
import java.security.GeneralSecurityException;
import java.util.Arrays;
import java.util.Collections;
import java.util.List;

import static com.nirvana.GoogleDrive.HelperClass.getCredentials;

public class CreateExcelSheet {


    public static Spreadsheet NewCreatedSheet = null;
    public static String sheetId = null;
    public static Spreadsheet CreateSheet(String spreadsheetId,String Title) throws IOException, GeneralSecurityException, InterruptedException {
        final NetHttpTransport HTTP_TRANSPORT = GoogleNetHttpTransport.newTrustedTransport();
        Sheets sheetsService = HelperClass.getSheetService();
        /*Sheets sheetsService = new Sheets.Builder(HTTP_TRANSPORT, JSON_FACTORY, getCredentials(HTTP_TRANSPORT))
                .setApplicationName(APPLICATION_NAME)
                .build();*/
        Drive driveService = null;
        try {
            driveService = new Drive.Builder(HTTP_TRANSPORT, JSON_FACTORY, getCredentials(HTTP_TRANSPORT))
                    .setApplicationName(APPLICATION_NAME)
                    .build();
        }
        catch (Exception e){
            com.nirvana.Helper.HelperClass.PrintConsole(e);
            com.nirvana.Helper.HelperClass.PrintConsole("Retry Again for Drive Service Connection");
            boolean isConnected = false;
            long CurrentTime = System.currentTimeMillis();
            while(!isConnected){
                if(System.currentTimeMillis()-CurrentTime>120000){
                    com.nirvana.Helper.HelperClass.PrintConsole("Not Connected");
                    break;
                }
                else{
                    try{
                        driveService = new Drive.Builder(HTTP_TRANSPORT, JSON_FACTORY, getCredentials(HTTP_TRANSPORT))
                                .setApplicationName(APPLICATION_NAME)
                                .build();
                        isConnected = true;
                    }
                    catch (Exception ex){

                    }
                }
            }
        }
        File file = new File();
        file.setName(Title);

        Drive.Files.Copy copyRequest = driveService.files().copy(spreadsheetId, file);

        File copiedFile = copyRequest.execute();
        String duplicatedSpreadsheetId = copiedFile.getId();
        Spreadsheet spreadsheet = sheetsService.spreadsheets().get(duplicatedSpreadsheetId).execute();
        NewCreatedSheet = spreadsheet;
        com.nirvana.Helper.HelperClass.PrintConsole("URL is "+spreadsheet.getSpreadsheetUrl());
        shareSheetWithDomain(spreadsheet.getSpreadsheetId(),"");
        InetAddress inetAddress = InetAddress.getLocalHost();

        // Get the machine name (hostname)
        String machineName = inetAddress.getHostAddress();
        String id = "1nSQR8nGI-hK44-EPwef7Pq9x3QQywKWgEEHEH7zGyNA";
        WriteDataInExcel.updateMultipleCells1(id,"Detail",spreadsheet.getSpreadsheetUrl(),machineName);
        //com.nirvana.Helper.HelperClass.PrintConsole("URL is "+spreadsheet.getSpreadsheetUrl());
        String fileName = "SheetUrl.txt";
        try (BufferedWriter writer = new BufferedWriter(new FileWriter(fileName))) {
            writer.write(spreadsheet.getSpreadsheetUrl());
            com.nirvana.Helper.HelperClass.PrintConsole("Data written to " + fileName);
        } catch (IOException e) {
            e.printStackTrace();
        }
        sheetId = spreadsheet.getSpreadsheetId();
        return spreadsheet;
    }

    private static void shareSheetWithDomain(String spreadsheetId, String s) throws GeneralSecurityException, IOException {
        final NetHttpTransport HTTP_TRANSPORT = GoogleNetHttpTransport.newTrustedTransport();
        Drive driveService = new Drive.Builder(HTTP_TRANSPORT, JSON_FACTORY, getCredentials(HTTP_TRANSPORT))
                .setApplicationName(APPLICATION_NAME)
                .build();

        Permission permission = new Permission()
                .setType("domain")
                .setRole("reader") // "reader", "writer", "commenter" can be used based on needs
                .setDomain("nirvanasolutions.com");

        driveService.permissions().create(spreadsheetId, permission).execute();
    }
    private static void copyFormatting(Sheets service, String sourceSpreadsheetId, String targetSpreadsheetId, Sheet sourceSheet, Sheet targetSheet) throws IOException {
        int sourceSheetId = sourceSheet.getProperties().getSheetId();
        int targetSheetId = targetSheet.getProperties().getSheetId();

        // Log the sheet IDs to ensure they are valid
        //com.nirvana.Helper.HelperClass.PrintConsole("Source Sheet ID: " + sourceSheetId);
        //com.nirvana.Helper.HelperClass.PrintConsole("Target Sheet ID: " + targetSheetId);




        // Dynamically calculate the range based on the sheet size
        int numberOfRows = sourceSheet.getProperties().getGridProperties().getRowCount();
        int numberOfColumns = sourceSheet.getProperties().getGridProperties().getColumnCount();

        // Log the number of rows and columns
        //com.nirvana.Helper.HelperClass.PrintConsole("Number of Rows: " + numberOfRows);
        //com.nirvana.Helper.HelperClass.PrintConsole("Number of Columns: " + numberOfColumns);

        // Create a batch update request to copy the formatting
        Request request = new Request()
                .setCopyPaste(new CopyPasteRequest()
                        .setSource(new GridRange()
                                .setSheetId(sourceSheetId)
                                .setStartRowIndex(0)
                                .setEndRowIndex(numberOfRows)
                                .setStartColumnIndex(0)
                                .setEndColumnIndex(numberOfColumns))
                        .setDestination(new GridRange()
                                .setSheetId(targetSheetId)
                                .setStartRowIndex(0)
                                .setEndRowIndex(numberOfRows)
                                .setStartColumnIndex(0)
                                .setEndColumnIndex(numberOfColumns))
                        .setPasteType("PASTE_NORMAL"));

        // Send the batch update request to copy the formatting
        BatchUpdateSpreadsheetRequest body = new BatchUpdateSpreadsheetRequest()
                .setRequests(Arrays.asList(request));

        service.spreadsheets().batchUpdate(targetSpreadsheetId, body).execute();
    }

    private static final String APPLICATION_NAME = "Google Sheets API Java Quickstart";
    private static final JsonFactory JSON_FACTORY = GsonFactory.getDefaultInstance();
}
