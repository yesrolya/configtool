using System;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace ConfigTool
{
    public static class ConfigParser
    {
        public static string strSheets = {
        "Design Spec",
        "Dialog",
        "Full Design",
        "Levels",
        "KB, SO and gifts",
        "Economy",
        "Полный дизайн"
    };

        public ConfigParser() {

        }

        public static void TestParsing() {
            Jason ss = new Jason(5);
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(ss));
            using (FileStream fs = new FileStream("test.json", FileMode.OpenOrCreate)) {
                jsonFormatter.WriteObject(fs, ss);
            }
        }
    }

    //public class XLSXReader
    //{
    //    private TRYFUNC() {
    //        Excel.Application xlApp = new Excel.Application();
    //        Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(@"sandbox_test.xlsx");
    //        Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
    //        Excel.Range xlRange = xlWorksheet.UsedRange;
    //    }

    //    public static void getExcelFile() {

    //        Excel.Application xApp = new Excel.Application();
    //        string FullPath = @"D:\GIT";
    //        Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(@"C:\Users\E56626\Desktop\Teddy\VS2012\Sandbox\sandbox_test - Copy - Copy.xlsx");
    //        Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
    //        Excel.Range xlRange = xlWorksheet.UsedRange;

    //        int rowCount = xlRange.Rows.Count;
    //        int colCount = xlRange.Columns.Count;

    //        //iterate over the rows and columns and print to the console as it appears in the file
    //        //excel is not zero based!!
    //        for (int i = 1; i <= rowCount; i++) {
    //            for (int j = 1; j <= colCount; j++) {
    //                //new line
    //                if (j == 1)
    //                    Console.Write("\r\n");

    //                //write the value to the console
    //                if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
    //                    Console.Write(xlRange.Cells[i, j].Value2.ToString() + "\t");
    //            }
    //        }

    //        //cleanup
    //        GC.Collect();
    //        GC.WaitForPendingFinalizers();

    //        //rule of thumb for releasing com objects:
    //        //  never use two dots, all COM objects must be referenced and released individually
    //        //  ex: [somthing].[something].[something] is bad

    //        //release com objects to fully kill excel process from running in the background
    //        Marshal.ReleaseComObject(xlRange);
    //        Marshal.ReleaseComObject(xlWorksheet);

    //        //close and release
    //        xlWorkbook.Close();
    //        Marshal.ReleaseComObject(xlWorkbook);

    //        //quit and release
    //        xlApp.Quit();
    //        Marshal.ReleaseComObject(xlApp);
    //    }
    //}
}