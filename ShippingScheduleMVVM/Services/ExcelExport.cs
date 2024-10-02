
using OfficeOpenXml;
using OfficeOpenXml.Style;
using ShippingScheduleMVVM.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace ShippingScheduleMVVM.Services
{
    public static class ExcelExport
    {
        public static void ExportRecordsBetweenDates(DateTime initialDate, DateTime finalDate, string directory, bool exportParts)
        {
            DatabaseOperations database = new();
            ObservableCollection<Record> scheduleRecords = database.SelectScheduleRecordsBetweenDates(initialDate, finalDate);

            // Set License Context for EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            ExcelPackage package = new ExcelPackage(); 
            var worksheet = package.Workbook.Worksheets.Add("Records");
            
            // Titles
            worksheet.Cells["A1"].Value = "Id";
            worksheet.Cells["B1"].Value = "Category";
            worksheet.Cells["C1"].Value = "ShipTos";
            worksheet.Cells["D1"].Value = "ShipmentType";
            worksheet.Cells["E1"].Value = "TransportMode";
            worksheet.Cells["F1"].Value = "PaleteNumber";
            worksheet.Cells["G1"].Value = "EntryDate";
            worksheet.Cells["H1"].Value = "LeavingDate";
            worksheet.Cells["I1"].Value = "PlannedDate";
            worksheet.Cells["J1"].Value = "Carrier";
            worksheet.Cells["K1"].Value = "Comments";
            worksheet.Cells["L1"].Value = "User";

            // Change the background color of cells from A1 to J1
            System.Drawing.Color themeBlue = System.Drawing.Color.FromArgb(1, 203, 220, 245);
            worksheet.Cells["A1:L1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells["A1:L1"].Style.Fill.BackgroundColor.SetColor(themeBlue);

            int count = 2;
            // Information
            foreach(Record record in scheduleRecords)
            {
                worksheet.Cells["A" + count].Value = record.Id;
                worksheet.Cells["B" + count].Value = record.Category;
                worksheet.Cells["C" + count].Value = record.ShipTo;
                worksheet.Cells["D" + count].Value = record.ShipmentType;
                worksheet.Cells["E" + count].Value = record.TransportMode;
                worksheet.Cells["F" + count].Value = record.PaleteNumber;
                worksheet.Cells["G" + count].Value = record.TransportArrivalDate;
                worksheet.Cells["H" + count].Value = record.ShippedDate;
                worksheet.Cells["I" + count].Value = DateTime.Parse(record.Day.Substring(0, 10) + " " + record.Time);
                worksheet.Cells["J" + count].Value = record.Carrier;
                worksheet.Cells["K" + count].Value = record.Comment;
                worksheet.Cells["L" + count].Value = record.CreatedBy;
                count++;
            }

            worksheet.Cells["A1:L" + (count - 1)].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A1:L" + (count - 1)].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A1:L" + (count - 1)].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["A1:L" + (count - 1)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            // Set column B to date format
            worksheet.Column(7).Style.Numberformat.Format = "dd/mm/yyyy hh:mm";

            // Set column B to date format
            worksheet.Column(8).Style.Numberformat.Format = "dd/mm/yyyy hh:mm";

            // Set column B to date format
            worksheet.Column(9).Style.Numberformat.Format = "dd/mm/yyyy hh:mm";


            // Sort data based on column I, from newest to oldest
            worksheet.Cells["A2:L" + (count - 1)].Sort(8, true);

            // Autofit columns for all cells
            worksheet.Cells.AutoFitColumns();

            if (exportParts)
            {
                ObservableCollection<Part> parts = database.SelectParts(initialDate, finalDate);
                var partsWorksheet = package.Workbook.Worksheets.Add("Parts");

                // Titles
                partsWorksheet.Cells["A1"].Value = "Id";
                partsWorksheet.Cells["B1"].Value = "RecordId";
                partsWorksheet.Cells["C1"].Value = "APN";
                partsWorksheet.Cells["D1"].Value = "CPN";
                partsWorksheet.Cells["E1"].Value = "Designation";
                partsWorksheet.Cells["F1"].Value = "ShipTo";
                partsWorksheet.Cells["G1"].Value = "ShipToDescription";
                partsWorksheet.Cells["H1"].Value = "Unload.Point";
                partsWorksheet.Cells["I1"].Value = "FinalQTY";
                partsWorksheet.Cells["J1"].Value = "ExpectedQTY";
                partsWorksheet.Cells["K1"].Value = "DeliveryNote";
                partsWorksheet.Cells["L1"].Value = "Trans.Number";
                partsWorksheet.Cells["M1"].Value = "Comment";

                count = 2;
                foreach (Part part in parts)
                {
                    partsWorksheet.Cells["A" + count].Value = part.Id;
                    partsWorksheet.Cells["B" + count].Value = part.RecordId;
                    partsWorksheet.Cells["C" + count].Value = part.APN;
                    partsWorksheet.Cells["D" + count].Value = part.CPN;
                    partsWorksheet.Cells["E" + count].Value = part.Designation;
                    partsWorksheet.Cells["F" + count].Value = part.ShipTo;
                    partsWorksheet.Cells["G" + count].Value = part.ShipToName;
                    partsWorksheet.Cells["H" + count].Value = part.UnloadingPoint;
                    partsWorksheet.Cells["I" + count].Value = part.FinalQuantity;
                    partsWorksheet.Cells["J" + count].Value = part.ExpectedQuantity;
                    partsWorksheet.Cells["K" + count].Value = part.DeliveryNote;
                    partsWorksheet.Cells["L" + count].Value = part.TransportNumber;
                    partsWorksheet.Cells["M" + count].Value = part.Comment;
                    count++;
                }

                partsWorksheet.Cells["A1:M" + (count - 1)].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                partsWorksheet.Cells["A1:M" + (count - 1)].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                partsWorksheet.Cells["A1:M" + (count - 1)].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                partsWorksheet.Cells["A1:M" + (count - 1)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                // Change the background color of cells from A1 to M1
                System.Drawing.Color themeOrange = System.Drawing.Color.FromArgb(1, 255, 189, 168);
                partsWorksheet.Cells["A1:M1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                partsWorksheet.Cells["A1:M1"].Style.Fill.BackgroundColor.SetColor(themeOrange);

                // Autofit columns for all cells
                partsWorksheet.Cells.AutoFitColumns();
            }

            var fileInfo = new FileInfo(directory +  "\\" +  @"ShippingScheduleRecords_" + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year + "_" + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".xlsx");
            package.SaveAs(fileInfo);
        } 
    }
}
