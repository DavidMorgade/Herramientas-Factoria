using System;
using OfficeOpenXml;
using System.IO;
using System.Globalization;

namespace Herramientas_Factoria.ManipulateExcel
{
    public class ExcelReader
    {
        public static (string Expediente, string Importe, string NombreFactura) ExtractExpedienteAndImporte(string filePath)
        {
            string expediente = string.Empty;
            string importe = string.Empty;
            string nombreFactura = string.Empty;

            // Obtener el nombre del archivo desde la ruta completa
            string fileName = System.IO.Path.GetFileName(filePath);

            // Buscar el primer espacio y obtener el texto hasta ahí
            int spaceIndex = fileName.IndexOf(' ');
            if (spaceIndex > 0)
            {
                // Si hay un espacio, devolver la parte antes del primer espacio
                nombreFactura = fileName.Substring(0, spaceIndex);
            }

            // Configurar EPPlus para manejar archivos Excel
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                // Obtener la primera hoja del archivo
                var worksheet = package.Workbook.Worksheets[0];
                package.Workbook.Calculate();

                // Determinar el rango utilizado
                var rows = worksheet.Dimension.Rows;
                var cols = worksheet.Dimension.Columns;

                for (int row = 1; row <= rows; row++)
                {
                    for (int col = 1; col <= cols; col++)
                    {
                        var cellValue = worksheet.Cells[row, col].Text.Trim();  // Trim para eliminar espacios adicionales

                        Console.WriteLine($"Celda ({row}, {col}) - Valor: '{cellValue}'");  // Verifica lo que contiene cada celda

                        // Buscar "Expediente" y obtener el valor en la celda siguiente
                        if (cellValue.Equals("Expediente", StringComparison.OrdinalIgnoreCase))
                        {
                            expediente = worksheet.Cells[row, col + 1].Text.Trim();
                        }

                        // Buscar "Importe Factura" y obtener el valor en la celda siguiente
                        if (cellValue.ToLower().Replace(" ", "").Equals("importefactura", StringComparison.OrdinalIgnoreCase))
                        {
                            // Usamos .Value para obtener el valor real
                            var importeCellValue = worksheet.Cells[row, col + 1].Value;

                            // Si el valor es un número, lo convertimos a decimal
                            if (importeCellValue is decimal || importeCellValue is double)
                            {
                                importe = Convert.ToDecimal(importeCellValue).ToString("C", new CultureInfo("es-ES"));
                            }
                            else if (importeCellValue is string)
                            {
                                importe = importeCellValue.ToString().Trim();
                            }
                        }
                    }
                }
            }

            return (expediente, importe, nombreFactura);
        }
    }
}


