using System;
using System.IO;
using AspNetCoreHero.ToastNotification.Abstractions;
using Inventario.ConsumeAPI;
using Inventario.Entidades;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;

namespace WebApplication1.Controllers
{
    public class ReporteController : Controller
    {
        public INotyfService _notifyService { get; }
        private string urlApi;
        public ReporteController(IConfiguration configuration, INotyfService notyfService)
        {
            urlApi = configuration.GetValue("ApiUrlBase", "").ToString() + "/AjusteProducto";
            _notifyService = notyfService;
        }

       
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PrevisualizarPDF(string descripcion)
        {
            // Crear un documento PDF
            Document doc = new Document(PageSize.A4);
            MemoryStream stream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(doc, stream);
            doc.Open();

            // Agregar título
            var titulo = new Paragraph("Reporte de Ajustes de Inventario")
            {
                Alignment = Element.ALIGN_CENTER,
                Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16)
            };
            doc.Add(titulo);
            doc.Add(new Paragraph("\n"));

            // Agregar la cabecera del ajuste
            var numeroAjuste = new Paragraph($"Número de Ajuste: AJUS-001");
            var fecha = new Paragraph($"Fecha: {DateTime.Now.ToString("yyyy-MM-dd")}");
            var desc = new Paragraph($"Descripción: {descripcion}");

            doc.Add(numeroAjuste);
            doc.Add(fecha);
            doc.Add(desc);
            doc.Add(new Paragraph("\n"));

            // Detalle del Inventario Antes del Ajuste
            doc.Add(new Paragraph("Detalle del Inventario Antes del Ajuste", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            doc.Add(new Paragraph("\n"));
            PdfPTable table1 = new PdfPTable(4);
            table1.WidthPercentage = 100;
            table1.AddCell("Código de Producto");
            table1.AddCell("Nombre del Producto");
            table1.AddCell("Cantidad Registrada");
            table1.AddCell("Cantidad Física");

            table1.AddCell("P001");
            table1.AddCell("Widget A");
            table1.AddCell("100");
            table1.AddCell("95");

            table1.AddCell("P002");
            table1.AddCell("Widget B");
            table1.AddCell("50");
            table1.AddCell("48");

            table1.AddCell("P003");
            table1.AddCell("Widget C");
            table1.AddCell("200");
            table1.AddCell("198");

            doc.Add(table1);
            doc.Add(new Paragraph("\n"));

            // Razones del Ajuste
            doc.Add(new Paragraph("Razones del Ajuste", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            doc.Add(new Paragraph("1. Producto P001 (Widget A):"));
            doc.Add(new Paragraph("   - 5 unidades dañadas y no utilizables."));
            doc.Add(new Paragraph("2. Producto P002 (Widget B):"));
            doc.Add(new Paragraph("   - 2 unidades perdidas durante el transporte interno."));
            doc.Add(new Paragraph("3. Producto P003 (Widget C):"));
            doc.Add(new Paragraph("   - 2 unidades caducadas."));
            doc.Add(new Paragraph("\n"));

            // Registro del Ajuste
            doc.Add(new Paragraph("Registro del Ajuste", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            doc.Add(new Paragraph("\n"));
            PdfPTable table2 = new PdfPTable(6);
            table2.WidthPercentage = 100;
            table2.AddCell("Código de Producto");
            table2.AddCell("Nombre del Producto");
            table2.AddCell("Cantidad Anterior");
            table2.AddCell("Cantidad Ajustada");
            table2.AddCell("Nueva Cantidad");
            table2.AddCell("Razón del Ajuste");

            table2.AddCell("P001");
            table2.AddCell("Widget A");
            table2.AddCell("100");
            table2.AddCell("-5");
            table2.AddCell("95");
            table2.AddCell("5 unidades dañadas");

            table2.AddCell("P002");
            table2.AddCell("Widget B");
            table2.AddCell("50");
            table2.AddCell("-2");
            table2.AddCell("48");
            table2.AddCell("2 unidades perdidas");

            table2.AddCell("P003");
            table2.AddCell("Widget C");
            table2.AddCell("200");
            table2.AddCell("-2");
            table2.AddCell("198");
            table2.AddCell("2 unidades caducadas");

            doc.Add(table2);
            doc.Close();

            // Guardar el PDF en la carpeta temporal del servidor
            string pdfPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.pdf");
            System.IO.File.WriteAllBytes(pdfPath, stream.ToArray());

            // Guardar la ruta del PDF en TempData para ser utilizada en la vista de previsualización
            TempData["PDFPath"] = pdfPath;

            return RedirectToAction("Previsualizar");
        }

        public IActionResult Previsualizar()
        {
            // Obtener la ruta del PDF desde TempData
            string pdfPath = TempData["PDFPath"] as string;
            if (pdfPath == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.PDFPath = pdfPath;
            return View();
        }

        public IActionResult VisualizarPDF(string pdfPath)
        {
            if (System.IO.File.Exists(pdfPath))
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(pdfPath);
                return File(fileBytes, "application/pdf");
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult DescargarPDF(string pdfPath)
        {
            // Verificar que el archivo existe y devolverlo como un archivo descargable
            if (System.IO.File.Exists(pdfPath))
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(pdfPath);
                return File(fileBytes, "application/pdf", "ReporteAjustesInventario.pdf");
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult ImprimirPDF(int id)
        {
            ViewBag.ID_Ajuste = id;
            var data = CRUD<AjusteProducto>.Read_ById(urlApi,id);
            var newdata = new AjusteProducto
            {
                ID_Ajuste = data.ID_Ajuste,
                NumeroAjuste = data.NumeroAjuste,
                Fecha = data.Fecha,
                Descripcion = data.Descripcion,
                Impreso = true
            };
            CRUD<AjusteProducto>.Update(urlApi, newdata.ID_Ajuste, newdata);
            return RedirectToAction("Index","AjustesProductos");
        }
    }
}

