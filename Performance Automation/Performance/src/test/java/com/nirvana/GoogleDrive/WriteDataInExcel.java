package com.nirvana.GoogleDrive;

import com.google.api.services.sheets.v4.model.UpdateValuesResponse;
import com.google.api.services.sheets.v4.model.ValueRange;
import com.nirvana.Helper.Timer;
import org.apache.poi.ss.usermodel.Row;


import java.io.IOException;
import java.security.GeneralSecurityException;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;
import java.util.List;

import static com.nirvana.GoogleDrive.HelperClass.service;

public class WriteDataInExcel {
    static HashMap<String, Integer> RowsCount = new HashMap<>();
    public static int GetLastRow(String SpreadSheetId, String SheetName) throws IOException, GeneralSecurityException {
        // Fetch values from column A only
        //getSheetService();
        String range = SheetName; // Fetch column A
        List<List<Object>> values = service.spreadsheets().values().get(SpreadSheetId, range).execute().getValues();

        // Check if no data is present in column A
        if (values == null || values.isEmpty()) {
            RowsCount.put(SheetName,0);
            return 0; // No data in column A

        }

        // Iterate through the rows to find the last non-empty cell
        for (int i = 0; i < values.size(); i++) {
            List<Object> row = values.get(i);
            if (row.isEmpty() || row.get(0).toString().trim().isEmpty()) {
                RowsCount.put(SheetName,i);
                return i; // Found an empty cell in column A
            }
        }

        // If all cells in column A are non-empty, return the total number of rows
        RowsCount.put(SheetName,values.size());
        return values.size();
    }
    public static void WriteDataInSheet(String SpreadSheetId, String SheetName, ArrayList<Object> data,boolean IsFirstRow) throws IOException, GeneralSecurityException {

        int NewRow = GetLastRow(SpreadSheetId, SheetName)+1;
        if(IsFirstRow && NewRow !=1)
            return;
            //ArrayList<Object> data = new ArrayList<Object>(Arrays.asList("Module Name", "Time (In seconds)"));
        List<List<Object>> val = Arrays.asList(data);
        ValueRange body = new ValueRange().setValues(val);

        UpdateValuesResponse updateValuesResponse = service.spreadsheets().values().update(CreateExcelSheet.NewCreatedSheet.getSpreadsheetId(), SheetName+"!A"+NewRow, body).setValueInputOption("USER_ENTERED").execute();
        com.nirvana.Helper.HelperClass.PrintConsole("Total Updated cells :"+updateValuesResponse.getUpdatedCells());





    }



    public static void updateMultipleCells(String sheetId, String sheetName,ArrayList<ArrayList<Object>> data) throws IOException, GeneralSecurityException, InterruptedException {

        int startRow;
        try{
            startRow = RowsCount.get(sheetName)+1;
            }
        catch (Exception e){
            try {
                startRow = GetLastRow(sheetId, sheetName) + 1;
            }
            catch (Exception ex){
                com.nirvana.Helper.HelperClass.PrintConsole(ex);
                return;
            }
        }
        int numRows = data.size();  // Number of rows to update
        if (numRows == 0) {
            return;  // If there's no data, exit the method
        }
        String range = sheetName + "!A" + startRow + ":Z" + (startRow + numRows - 1);
        List<List<Object>> val = new ArrayList<>(data);

        ValueRange body = new ValueRange().setValues(val);
        int retries = 10;
        while (retries > 0) {
            try {
                UpdateValuesResponse updateValuesResponse = service
                        .spreadsheets()
                        .values()
                        .update(sheetId, range, body)
                        .setValueInputOption("USER_ENTERED")
                        .execute();

                com.nirvana.Helper.HelperClass.PrintConsole("Total updated cells: " + updateValuesResponse.getUpdatedCells());
                //RowsCount.put(sheetName, RowsCount.get(sheetName) + 1);
                RowsCount.put(sheetName,RowsCount.get(sheetName)+1);
                break;
            } catch (Exception e) {
                com.nirvana.Helper.HelperClass.PrintConsole("Error during update: " + e.getMessage());
                retries--;
                if (retries == 0) {
                    com.nirvana.Helper.HelperClass.PrintConsole("Failed after 10 retries.");
                } else {
                    Thread.sleep(10000 * (10 - retries));
                }
            }
        }

        /*UpdateValuesResponse updateValuesResponse = service
                .spreadsheets()
                .values()
                .update(sheetId, range, body)
                .setValueInputOption("USER_ENTERED")
                .execute();*/

        //com.nirvana.Helper.HelperClass.PrintConsole("Total updated cells: " + updateValuesResponse.getUpdatedCells());
        //RowsCount.put(sheetName,RowsCount.get(sheetName)+1);
        Thread.sleep(1000);
    }
    public static void updateMultipleCells1(String sheetId, String sheetName,String URL,String MachineUrl) throws IOException, GeneralSecurityException, InterruptedException {
        ArrayList<ArrayList<Object>> data = new ArrayList<>();
        ArrayList<Object> data2 = new ArrayList<Object>();
        data2.add(MachineUrl);
        data2.add(URL);
        ArrayList<Object> cloneDict1 = (ArrayList<Object>) data2.clone();
        data.clear();
        data.add(cloneDict1);

        //AddSheetAndInputData(Timer.calculateTotalTime(), stepName, module, "Logs.xlsx", data1);
        //updateMultipleCells1(sheetid, "Detail",data1);
        int startRow;
        try{
            startRow = RowsCount.get(sheetName)+1;
            //startRow = 1;


        }
        catch (Exception e){
            try {
                startRow = GetLastRow(sheetId, sheetName) + 1;
            }
            catch (Exception ex){
                com.nirvana.Helper.HelperClass.PrintConsole(ex);
                return;
            }
        }
        int numRows = data.size();  // Number of rows to update
        if (numRows == 0) {
            return;  // If there's no data, exit the method
        }
        try{
            String range = sheetName; // Fetch column A
            List<List<Object>> values = service.spreadsheets().values().get(sheetId, range).execute().getValues();
            for (int i = 0; i <values.size(); i++) {
                for(int j = 0; j<values.get(i).size(); j++){
                    if(values.get(i).get(j).toString().equals(MachineUrl)) {
                        startRow = i;
                        startRow+=1;
                        break;
                    }

                }
            }
        }
        catch (Exception ex){
            System.out.println();
        }
        String range = sheetName + "!A" + startRow + ":Z" + (startRow + numRows - 1);
        List<List<Object>> val = new ArrayList<>(data);

        ValueRange body = new ValueRange().setValues(val);
        int retries = 10;
        while (retries > 0) {
            try {
                UpdateValuesResponse updateValuesResponse = service
                        .spreadsheets()
                        .values()
                        .update(sheetId, range, body)
                        .setValueInputOption("USER_ENTERED")
                        .execute();

                com.nirvana.Helper.HelperClass.PrintConsole("Total updated cells: " + updateValuesResponse.getUpdatedCells());
                //RowsCount.put(sheetName, RowsCount.get(sheetName) + 1);
                RowsCount.put(sheetName,RowsCount.get(sheetName)+1);
                break;
            } catch (Exception e) {
                com.nirvana.Helper.HelperClass.PrintConsole("Error during update: " + e.getMessage());
                retries--;
                if (retries == 0) {
                    com.nirvana.Helper.HelperClass.PrintConsole("Failed after 10 retries.");
                } else {
                    Thread.sleep(10000 * (10 - retries));
                }
            }
        }

        /*UpdateValuesResponse updateValuesResponse = service
                .spreadsheets()
                .values()
                .update(sheetId, range, body)
                .setValueInputOption("USER_ENTERED")
                .execute();*/

        //com.nirvana.Helper.HelperClass.PrintConsole("Total updated cells: " + updateValuesResponse.getUpdatedCells());
        //RowsCount.put(sheetName,RowsCount.get(sheetName)+1);
        Thread.sleep(1000);
    }

}
