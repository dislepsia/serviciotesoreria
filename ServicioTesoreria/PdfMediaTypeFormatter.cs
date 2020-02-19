using ServicioTesoreria.Models;
using System;
using System.IO;
using System.Net;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Reporting.WebForms;
using System.Web;
using System.Collections.Generic;
using Spire.Barcode;
using ServicioTesoreria.Logica;

namespace ServicioTesoreria
{
    class PdfMediaTypeFormatter : MediaTypeFormatter
    {
        private static readonly Type SupportedType = typeof(List<Cupon>);

        public PdfMediaTypeFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/pdf"));
            MediaTypeMappings.Add(new UriPathExtensionMapping("pdf", "application/pdf"));
        }
        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                return ms.ToArray();
            }
        }
        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, System.Net.Http.HttpContent content, TransportContext transportContext)
        {
            var taskSource = new TaskCompletionSource<object>();
            try
            {
                var cupon = (List<Cupon>)value;
                cupon[0].NroFactura = cupon[0].Id.ToString().PadLeft(12, '0');
                //var t = talon.ToList().Take(1).ToList();

                //generacion de codigo de barras


                //if (cupon[0].NroCuota > 1 && cupon[0].FechaVto2 <= DateTime.Now)
                //{
                //    cupon[0].FechaVto2 = DateTime.Now.AddDays(10);
                //    cupon[0].fechaVencimiento2 = DateTime.Now.AddDays(10).ToString("dd/MM/yyyy");
                //}
                BarcodeSettings settings = new BarcodeSettings();
                settings.Type = BarCodeType.Interleaved25;
                var codigoBarras = new CodigoBarrasPagoFacil(
                        "09370489",
                        cupon[0].Importe,
                        cupon[0].FechaVto,
                        cupon[0].Id.ToString(),
                        cupon[0].Importe2,
                        cupon[0].FechaVto2
                            );

                settings.Data = codigoBarras.getCodigoDeBarras();
                //settings.Data = "0937048907720016359002238730077200163599";//cupon[0].Dni.ToString();
                settings.ShowTextOnBottom = true;
                //settings.ImageWidth = 
                
                BarCodeGenerator bc = new BarCodeGenerator(settings);
                cupon[0].barcode = imageToByteArray(bc.GenerateImage());

                //Barcode bc = new Barcode(SymbologyType.Code128);

                //bc.RegistrationName = "demo";
                //bc.RegistrationKey = "demo";

                //bc.DrawCaption = false;
                //bc.Value = cupon[0].Dni.ToString();

                //// Generate the barcode image and store it into the Barcode Column
                //cupon[0].barcode = bc.GetImageBytesPNG();


                String pathReporte ="~/Reports/cuota.rdlc";
                ReportViewer rv = new Microsoft.Reporting.WebForms.ReportViewer();
                rv.ProcessingMode = ProcessingMode.Local;
                rv.LocalReport.ReportPath = HttpContext.Current.Server.MapPath(pathReporte);
                
                ReportDataSource rdsCabecera = new ReportDataSource("TesoreriaDataSet", cupon);
                rv.LocalReport.DataSources.Add(rdsCabecera);
                string reportType = "PDF";
                string mimeType;
                string encoding;
                string fileNameExtension;
                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;

                //Render 
                renderedBytes = rv.LocalReport.Render(
                reportType,
                    //deviceInfo,
                null,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);

                /*
                 * ReportViewer rv = new Microsoft.Reporting.WebForms.ReportViewer();
                    rv.ProcessingMode = ProcessingMode.Local;
                    rv.LocalReport.ReportPath = Server.MapPath("~/Reports/TestReport.rdlc");
                    rv.LocalReport.Refresh();

                    byte[] streamBytes = null;
                    string mimeType = "";
                    string encoding = "";
                    string filenameExtension = "";
                    string[] streamids = null;
                    Warning[] warnings = null;

                    streamBytes = rv.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);

                    return File(streamBytes, mimeType, "TestReport.pdf");*/

                //String nombreArchivo = "publica/Diploma" + dni.ToString() + ".pdf";
                //String filePath = MapPath(nombreArchivo);
                //FileStream fs = new FileStream(filePath, FileMode.Create);
                //fs.Write(renderedBytes, 0, renderedBytes.Length);
                //fs.Close();



                writeStream.Write(renderedBytes, 0, renderedBytes.Length);
                taskSource.SetResult(null);


                //var doc = PdfGenerator.CreatePdf(person.Importe.ToString()); 
                //var ms = new MemoryStream();

                // doc.Save(ms, false); 

                //var bytes = ms.ToArray();
                // writeStream.Write(bytes, 0, bytes.Length);
                // taskSource.SetResult(null);
            }
            catch (Exception e)
            {
                taskSource.SetException(e);
            }
            return taskSource.Task;
        }

        public override bool CanReadType(Type type)
        {
            return SupportedType == type;
        }

        public override bool CanWriteType(Type type)
        {
            return SupportedType == type;
        }
    }
}
