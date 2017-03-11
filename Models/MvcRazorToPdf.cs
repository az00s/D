using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace D.Models
{
    public class MvcRazorToPdf
    {
        public byte[] GeneratePdfOutput(ControllerContext context, object model = null, string viewName = null,
            Action<PdfWriter, Document> configureSettings = null)
        {

            if (viewName == null)
            {
                viewName = context.RouteData.GetRequiredString("action");
            }

            context.Controller.ViewData.Model = model;

            byte[] output;
            using (var document = new Document())
            {
                using (var workStream = new MemoryStream())
                {
                    PdfWriter writer = PdfWriter.GetInstance(document, workStream);
                    writer.CloseStream = false;

                    configureSettings?.Invoke(writer, document);
                    document.Open();


                    //using (var reader = new StringReader(RenderRazorView(context, viewName)))
                    //{
                    //    XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, reader);

                    //    document.Close();
                    //    output = workStream.ToArray();
                    //}

                    //using (var msCss = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(RenderRazorView(context, viewName))))
                    //{
                        using (var msHtml = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(RenderRazorView(context, viewName))))
                        {

                            //Parse the HTML
                            XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, msHtml,null,FontFactory.FontImp );
                        }
                    //}
                    document.Close();
                    output = workStream.ToArray();
                }
            }
            return output;
        }

        public string RenderRazorView(ControllerContext context, string viewName)
        {
            IView viewEngineResult = ViewEngines.Engines.FindView(context, viewName, null).View;
            var sb = new StringBuilder();


            using (TextWriter tr = new StringWriter(sb))
            {
                var viewContext = new ViewContext(context, viewEngineResult, context.Controller.ViewData,
                    context.Controller.TempData, tr);
                viewEngineResult.Render(viewContext, tr);
            }
            return sb.ToString();
        }
    }

    public static class MvcRazorToPdfExtensions
    {
        public static byte[] GeneratePdf(this ControllerContext context, object model = null, string viewName = null,
            Action<PdfWriter, Document> configureSettings = null)
        {
            return new MvcRazorToPdf().GeneratePdfOutput(context, model, viewName, configureSettings);
        }
    }
}