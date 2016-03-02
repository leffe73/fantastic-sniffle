using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAppMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Welcome to Okab system.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact page.";

            return View();
        }
        List<Customer> customers = new List<Customer>();
        private void CreateCustomers()
        {
            customers.Add(new Customer { ID = 1, Name = "Leif" });
            customers.Add(new Customer { ID = 2, Name = "Dan" });
            customers.Add(new Customer { ID = 3, Name = "Peter" });
        }
        public ActionResult LabelsPdf()
        {
            CreateCustomers();

            // Open a new PDF document

            const int pageMargin = 5;
            const int pageRows = 5;
            const int pageCols = 2;

            var doc = new Document();
            doc.SetMargins(pageMargin, pageMargin, pageMargin, pageMargin);
            doc.SetPageSize(new Rectangle(72, 144));// 1 x 2 inch
            var memoryStream = new MemoryStream();

            var pdfWriter = PdfWriter.GetInstance(doc, memoryStream);
            doc.Open();

            // Create the Label table

            PdfPTable table = new PdfPTable(pageCols);
            table.WidthPercentage = 100f;
            table.DefaultCell.Border = 0;

            var baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);

            foreach (var customer in customers)
            {
                #region Label Construction

                PdfPCell cell = new PdfPCell();
                cell.Border = 0;
                cell.FixedHeight = (doc.PageSize.Height - (pageMargin * 2)) / pageRows;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;

                var contents = new Paragraph();
                contents.Alignment = Element.ALIGN_CENTER;

                contents.Add(new Chunk(string.Format("Thing #{0}\n", customer.ID), new Font(baseFont, 12f, Font.BOLD)));
                contents.Add(new Chunk(string.Format("Thing Name: {0}\n", customer.Name), new Font(baseFont, 10f)));

                cell.AddElement(contents);
                table.AddCell(cell);

                #endregion
            }

            table.CompleteRow();
            doc.Add(table);

            // Close PDF document and send

            pdfWriter.CloseStream = false;
            doc.Close();
            memoryStream.Position = 0;

            return File(memoryStream, "application/pdf");
        }

    }
}