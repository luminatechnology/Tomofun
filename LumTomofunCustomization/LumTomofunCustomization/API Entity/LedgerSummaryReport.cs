using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FikaAmazonAPI.ReportGeneration;
using FikaAmazonAPI.ReportGeneration.ReportDataTable;

namespace LumTomofunCustomization.API_Entity
{
    public class LedgerSummaryReport
    {
        public List<LedgerSummaryReportRow> Data { get; set; } = new List<LedgerSummaryReportRow>();

        public LedgerSummaryReport(string path, string refNumber, bool mpIsJP)
        {
            if (string.IsNullOrEmpty(path)) { return; }

            List<LedgerSummaryReportRow> values = new List<LedgerSummaryReportRow>();

            // Since Jananese has special font, a condition for getting the encoding is added.
            var lines = File.ReadAllLines(path, System.Text.Encoding.GetEncoding(mpIsJP == true ? "Shift-JIS" : nameof(System.Text.Encoding.ASCII)));

            var table = new Table(lines.First().Split('\t'));

            lines.Skip(1).ToList().ForEach(a => table.AddRow(a.Split('\t')));

            foreach (var row in table.Rows)
            {
                values.Add(LedgerSummaryReportRow.FromRow(row, refNumber));
            }

            Data = values;
        }
    }

    public class LedgerSummaryReportRow
    {
        public DateTime? Date { get; set; }
        public string FNSKU { get; set; }
        public string ASIN { get; set; }
        public string MSKU { get; set; }
        public string Title { get; set; }
        public string Disposition { get; set; }
        public int? StartingWarehouseBalance { get; set; }
        public int? InTransitBetweenWarehouses{ get; set; }
        public int? Receipts { get; set; }
        public int? CustomerShipments { get; set; }
        public int? CustomerReturns { get; set; }
        public int? VendorReturns { get; set; }
        public int? WarehouseTransferInOrOut { get; set; }
        public int? Found { get; set; }
        public int? Lost { get; set; }
        public int? Damaged { get; set; }
        public int? Disposed { get; set; }
        public int? OtherEvents { get; set; }
        public int? EndingWarehouseBalance { get; set; }
        public int? UnknownEvents { get; set; }
        public string Location { get; set; }
        public string refNumber { get; set; }

        public static LedgerSummaryReportRow FromRow(TableRow rowData, string refNumber)
        {
            var row = new LedgerSummaryReportRow();
            row.Date = DataConverter.GetDate(rowData.GetString("Date"), DataConverter.DateTimeFormat.DATE_LEDGER_FORMAT);
            row.FNSKU = rowData.GetString("FNSKU");
            row.ASIN = rowData.GetString("ASIN");
            row.MSKU = rowData.GetString("MSKU");
            row.Title = rowData.GetString("Title");
            row.Disposition = rowData.GetString("Disposition");
            row.StartingWarehouseBalance = rowData.GetInt32("Starting Warehouse Balance");
            row.InTransitBetweenWarehouses = rowData.GetInt32("In Transit Between Warehouses");
            row.Receipts = rowData.GetInt32("Receipts");
            row.CustomerShipments = rowData.GetInt32("Customer Shipments");
            row.CustomerReturns = rowData.GetInt32("Customer Returns");
            row.VendorReturns = rowData.GetInt32("Vendor Returns");
            row.WarehouseTransferInOrOut = rowData.GetInt32("Warehouse Transfer In/Out");
            row.Found = rowData.GetInt32("Found");
            row.Lost = rowData.GetInt32("Lost");
            row.Damaged = rowData.GetInt32("Damaged");
            row.Disposed = rowData.GetInt32("Disposed");
            row.OtherEvents = rowData.GetInt32Nullable("Other Events");
            row.EndingWarehouseBalance = rowData.GetInt32("Ending Warehouse Balance");
            row.UnknownEvents = rowData.GetInt32Nullable("Unknown Events");
            row.Location = rowData.GetString("Location");
            row.refNumber = refNumber;

            return row;
        }
    }
}
