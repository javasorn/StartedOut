using ExportToExcel.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace ExportToExcel.Controllers
{
    public class ExportToExcel : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        public ExportToExcel(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Export()
        {
            var employees = GetEmployees();
            FileInfo template = new(_hostingEnvironment.ContentRootPath + "/excel_template/template.xlsx");
            using (var xlPackage = new ExcelPackage(template))
            {
                var worksheet = xlPackage.Workbook.Worksheets["Employees"];
                var row = 4;
                foreach (var emp in employees)
                {
                    worksheet.Cells[row, 1].Value = emp.Id;
                    worksheet.Cells[row, 2].Value = emp.Name;
                    worksheet.Cells[row, 3].Value = emp.Email;
                    row++;
                }
                xlPackage.Save();
                return File(xlPackage.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "export_template.xlsx");
            }
            
        }
        public IActionResult ExportWithOutTemplate()
        {
            var employees = GetEmployees();
            var stream = new MemoryStream();
            using (var xlPackage = new ExcelPackage(stream))
            {
                // Add worksheet
                var worksheet = xlPackage.Workbook.Worksheets.Add("Employees");
                worksheet.Column(2).Width = 30;
                worksheet.Column(3).Width = 30;

                //Title in first row
                worksheet.Cells["A1"].Value = "Sample Export";
                using (var r = worksheet.Cells["A1:C1"])
                {
                    r.Merge = true;
                    r.Style.Font.Color.SetColor(Color.Black);
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.Gray);
                }

                //Title column
                worksheet.Cells["A3"].Value = "Id";
                worksheet.Cells["B3"].Value = "Name";
                worksheet.Cells["C3"].Value = "Email";
                worksheet.Cells["A3:C3"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells["A3:C3"].Style.Fill.BackgroundColor.SetColor(Color.Gray);

                //Put data in a table
                var row = 4;
                foreach (var emp in employees)
                {
                    worksheet.Cells[row, 1].Value = emp.Id;
                    worksheet.Cells[row, 2].Value = emp.Name;
                    worksheet.Cells[row, 3].Value = emp.Email;
                    row++;
                }
                xlPackage.Save();
            }

            stream.Position = 0;
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "export.xlsx");
        }
        private static List<Employee> GetEmployees()
        {
            var result = new List<Employee>()
            {
                new Employee()
                {
                    Id = "001",
                    Email = "001@abc.com",
                    Name = "John Snow"
                },
                new Employee()
                {
                    Id = "002",
                    Email = "002@abc.com",
                    Name = "John Smith"
                },
                new Employee()
                {
                    Id = "003",
                    Email = "003@abc.com",
                    Name = "John Brown"
                },
            };
            return result;
        }
    }
}
