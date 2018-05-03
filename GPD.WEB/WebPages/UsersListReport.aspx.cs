using OfficeOpenXml;
using System;
using System.Data;
using System.IO;
using System.Web;

namespace GPD.WEB.WebPages
{
    public partial class UsersListReport : System.Web.UI.Page
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable dataTable = GPD.Facade.WebAppFacade.UserDetailsFacade.GetUsersList(DateTime.Now, DateTime.Now);
                this.DumpExcel(dataTable);
            }
            catch (Exception exc)
            {
                log.Error(exc);
            }
        }

        private void DumpExcel(DataTable tbl)
        {
            string fileName = "user-list-report.xlsx";
            FileInfo excelTemplate = new FileInfo(HttpContext.Current.Server.MapPath("./../App_Data/reports/" + fileName));

            using (ExcelPackage pck = new ExcelPackage(excelTemplate))
            {
                //Create the worksheet
                ExcelWorksheet ws = pck.Workbook.Worksheets[1];

                //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
                ws.Cells["A1"].LoadFromDataTable(tbl, true);

                Response.Clear();

                //Write it back to the client
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=" + fileName);
                Response.BinaryWrite(pck.GetAsByteArray());
                Response.End();
            }
        }
    }
}