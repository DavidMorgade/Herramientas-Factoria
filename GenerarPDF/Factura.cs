using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Herramientas_Factoria.GenerarPDF
{
    public class Factura
    {
        public Factura()
        {
        }
        public void GenerarCertificado()
        {
            string pdfPath = "certificado.pdf"; // Ruta donde se guardará el PDF

            // Crear un nuevo documento PDF
            PdfDocument documento = new PdfDocument();
            documento.Info.Title = "Certificado de Pago";

            // Crear una página en el documento
            PdfPage pagina = documento.AddPage();

            // Crear un objeto XGraphics para dibujar en la página
            XGraphics gfx = XGraphics.FromPdfPage(pagina);

            // Crear un objeto XFont para definir la fuente
            XFont fuente = new XFont("Verdana", 20);

            // Dibujar un texto en el PDF
            gfx.DrawString("Certificado de Pago", fuente, XBrushes.Black, new XPoint(200, 100));

            // Dibujar más información en el certificado
            XFont fuenteDetalle = new XFont("Verdana", 12);
            gfx.DrawString("Fecha: " + DateTime.Now.ToString("dd/MM/yyyy"), fuenteDetalle, XBrushes.Black, new XPoint(200, 150));
            gfx.DrawString("Monto: $1000", fuenteDetalle, XBrushes.Black, new XPoint(200, 180));
            gfx.DrawString("Cliente: Juan Pérez", fuenteDetalle, XBrushes.Black, new XPoint(200, 210));

            // Guardar el documento PDF en la ruta especificada
            documento.Save(pdfPath);

            // Mostrar un mensaje indicando que el certificado fue generado
            MessageBox.Show("Certificado generado correctamente", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
