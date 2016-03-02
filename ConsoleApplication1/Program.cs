using DAL;
using DAL.Models;
using sharpPDF;
using sharpPDF.Enumerators;
using System;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            //InsertEmployee();'
            //Pdf();//Skapa och öppna pdf fil
            AsynchronousClient.Start();
            Console.ReadKey();
        }
        private static void Pdf()
        {
            string filePath = @"c:\tfl\test.pdf";
            DateTime dt = DateTime.Now;
            pdfDocument myDoc = new pdfDocument("PDF dokument - Test", "Leif Ödell");
            pdfPage myPage = myDoc.addPage(500, 500);
            myPage.addText("My label!", 100, 250, predefinedFont.csHelveticaBold, 24);
            myPage.addText(dt.ToLongDateString(), 100, 150, predefinedFont.csHelveticaBold, 22);
            myPage.addText(dt.ToLongTimeString(), 100, 50, predefinedFont.csHelveticaBold, 20);
            myDoc.createPDF(filePath);
            myPage = null;
            myDoc = null;
            System.Diagnostics.Process.Start(filePath);
        }
        private static void InsertEmployee()
        {
            using (var repo = new EmployeeRepository())
            {
                var e = repo.Find(1);
                Employee em = new Employee() { FullName = "Wriju" };
                repo.InsertOrUpdate(em);
                repo.Save();
            }
        }
    }
}
