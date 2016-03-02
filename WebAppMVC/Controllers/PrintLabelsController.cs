using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppMVC.Models;
namespace WebAppMVC.Controllers
{
    public class PrintLabelsController : Controller
    {
        // GET: PrintLabels
        public ActionResult Index()
        {
            Printing model = new Printing() { Text1 = "S7SE-123456", Text2 = "001", Text3 = "text 3", Text4 = "text 4", Text5 = "12345678909999" };
            return View("Index", model);
            /*
            using (OkabSwedenEntities context = new OkabSwedenEntities())
            {
                var model = context.Printing.ToList();
                return View(model);
            }
            */
        }
        [HttpPost]
        [ActionName("Index")]
        public ActionResult Print(Printing prn)
        {
            Printing model = prn;
            LocalReport report = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports"), "Report2.rdlc");
            if (System.IO.File.Exists(path))
            {
                report.ReportPath = path;
            }
            else
            {
                return View("Index");
            }
            List<Printing> dataSource = new List<Printing>();
            dataSource.Add(prn);
            ReportDataSource rd = new ReportDataSource("DataSet1", dataSource);
            report.DataSources.Add(rd);
            Export(report);
            Print();
            return View("Index", model);
        }
        public ActionResult Report(string id)
        {
            LocalReport report = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports"), "Report2.rdlc");
            if (System.IO.File.Exists(path))
            {
                report.ReportPath = path;
            }
            else
            {
                return View("Index");
            }
            List<Printing> cm = new List<Printing>();
            /*using (OkabSwedenEntities context = new OkabSwedenEntities())
            {
                cm = context.Printing.Take(1).ToList();
            }*/
            Printing test = new Printing() { Text1 = "S7SE-123456", Text2 = "001", Text3 = "text 3", Text4 = "text 4", Text5 = "12345678909999" };
            cm.Add(test);
            ReportDataSource rd = new ReportDataSource("DataSet1", cm);
            report.DataSources.Add(rd);

            if (id == "Printer")
            {
                Export(report);
                Print();
                return View("Index");
            }
            else
            {
                string reportType = id;
                string mimeType;
                string encoding;
                string fileNameExtension;
                string deviceInfo =

                "<DeviceInfo>" +
                "  <OutputFormat>" + id + "</OutputFormat>" +
                "  <PageWidth>100mm</PageWidth>" +
                "  <PageHeight>50mm</PageHeight>" +
                "  <MarginTop>0mm</MarginTop>" +
                "  <MarginLeft>0mm</MarginLeft>" +
                "  <MarginRight>0mm</MarginRight>" +
                "  <MarginBottom>0mm</MarginBottom>" +
                "</DeviceInfo>";

                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;

                renderedBytes = report.Render(
                    reportType,
                    deviceInfo,
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);
                return File(renderedBytes, mimeType);
            }
        }
        private int m_currentPageIndex;
        private IList<Stream> m_streams;
        private void Export(LocalReport report)
        {
            string deviceInfo =
              @"<DeviceInfo>
                <OutputFormat>EMF</OutputFormat>
                <PageWidth>100mm</PageWidth>
                <PageHeight>50mm</PageHeight>
                <MarginTop>0mm</MarginTop>
                <MarginLeft>0mm</MarginLeft>
                <MarginRight>0mm</MarginRight>
                <MarginBottom>0mm</MarginBottom>
            </DeviceInfo>";
            Warning[] warnings;
            m_streams = new List<Stream>();
            report.Render("Image", deviceInfo, CreateStream,
               out warnings);
            foreach (Stream stream in m_streams)
                stream.Position = 0;
        }
        private Stream CreateStream(string name, string fileNameExtension, System.Text.Encoding encoding, string mimeType, bool willSeek)
        {
            Stream stream = new MemoryStream();
            m_streams.Add(stream);
            return stream;
        }
        private void Print()
        {
            if (m_streams == null || m_streams.Count == 0)
                throw new Exception("Error: no stream to print.");
            PrintDocument printDoc = new PrintDocument();
            PrinterSettings ps = new PrinterSettings();
            ps.DefaultPageSettings.PaperSize = new PaperSize("Custom",300,500);
            ps.DefaultPageSettings.Landscape = true;
            ps.PrinterName = @"\\Okab-srv-pr1\UtlastningZebraS4M";
            //printDoc.DefaultPageSettings = ps.DefaultPageSettings;
            printDoc.PrinterSettings = ps;
            if (!printDoc.PrinterSettings.IsValid)
            {
                throw new Exception("Error: cannot find the default printer.");
            }
            else
            {
                printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
                m_currentPageIndex = 0;
                printDoc.Print();
            }
        }
        // Handler for PrintPageEvents
        private void PrintPage(object sender, PrintPageEventArgs ev)
        {
            Metafile pageImage = new
               Metafile(m_streams[m_currentPageIndex]);

            // Adjust rectangular area with printer margins.
            Rectangle adjustedRect = new Rectangle(
                ev.PageBounds.Left - (int)ev.PageSettings.HardMarginX,
                ev.PageBounds.Top - (int)ev.PageSettings.HardMarginY,
                ev.PageBounds.Width,
                ev.PageBounds.Height);

            // Draw a white background for the report
            ev.Graphics.FillRectangle(Brushes.White, adjustedRect);

            // Draw the report content
            ev.Graphics.DrawImage(pageImage, adjustedRect);

            // Prepare for the next page. Make sure we haven't hit the end.
            m_currentPageIndex++;
            ev.HasMorePages = (m_currentPageIndex < m_streams.Count);
        }
    }
}