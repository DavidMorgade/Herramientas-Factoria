using System;
using OfficeOpenXml;
using System.IO;
using System.Globalization;
using System.Collections.Generic;

namespace Herramientas_Factoria.ManipulateExcel
{
    public class ExcelReader
    {
        public static (string Expediente, string Importe, string nombreFactura) ExtractExpedienteAndImporte(string filePath)
        {
            string Expediente = string.Empty;
            string Importe = string.Empty;
            string NombreFactura = string.Empty;

            // Obtener el nombre del archivo desde la ruta completa
            string fileName = System.IO.Path.GetFileName(filePath);

            // Buscar el primer espacio y obtener el texto hasta ahí
            int spaceIndex = fileName.IndexOf(' ');
            if (spaceIndex > 0)
            {
                // Si hay un espacio, devolver la parte antes del primer espacio
                NombreFactura = fileName.Substring(0, spaceIndex);
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
                            Expediente = worksheet.Cells[row, col + 1].Text.Trim();
                        }

                        // Buscar "Importe Factura" y obtener el valor en la celda siguiente
                        if (cellValue.ToLower().Replace(" ", "").Equals("importefactura", StringComparison.OrdinalIgnoreCase))
                        {
                            // Usamos .Value para obtener el valor real
                            var importeCellValue = worksheet.Cells[row, col + 1].Value;

                            // Si el valor es un número, lo convertimos a decimal
                            if (importeCellValue is decimal || importeCellValue is double)
                            {
                                Importe = Convert.ToDecimal(importeCellValue).ToString("C", new CultureInfo("es-ES"));
                            }
                            else if (importeCellValue is string)
                            {
                                Importe = importeCellValue.ToString().Trim();
                            }
                        }
                    }
                }
            }

            return (Expediente, Importe, NombreFactura);
        }
        public static List<Dictionary<string, string>> ExtractTableColumns(string filePath)
        {
            List<Dictionary<string, string>> tableData = new List<Dictionary<string, string>>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];

                int rows = worksheet.Dimension.Rows;
                int cols = worksheet.Dimension.Columns;

                // Mapear nombres de las columnas
                int unorCol = 0, nombreCol = 0, albaranCol = 0, importeCol = 0;
                int headerRow = 0;

                // Buscar dinámicamente la fila que contiene los encabezados
                for (int row = 1; row <= rows; row++)
                {
                    for (int col = 1; col <= cols; col++)
                    {
                        var cellText = worksheet.Cells[row, col].Text.Trim().ToLower();
                        if (cellText.Contains("unor") || cellText.Contains("nombre") ||
                            cellText.Contains("albarán") || cellText.Contains("importe total (iva)"))
                        {
                            headerRow = row; // Encontramos la fila de encabezados
                            break;
                        }
                    }
                    if (headerRow > 0) break;
                }

                // Si no encuentra la fila de encabezado
                if (headerRow == 0)
                    throw new Exception("No se encontró la fila de encabezado.");

                // Identificar las columnas específicas
                for (int col = 1; col <= cols; col++)
                {
                    var header = worksheet.Cells[headerRow, col].Text.Trim().ToLower();
                    if (header.Contains("unor")) unorCol = col;
                    else if (header.Contains("nombre")) nombreCol = col;
                    else if (header.Contains("albarán")) albaranCol = col;
                    else if (header.Contains("importe total (iva)")) importeCol = col;
                }

                // Validación: Si no encuentra las columnas
                if (unorCol == 0 || nombreCol == 0 || albaranCol == 0 || importeCol == 0)
                    throw new Exception("No se encontraron todas las columnas necesarias.");

                // Recorrer filas y extraer datos
                for (int row = headerRow + 1; row <= rows; row++) // Empieza después de la fila de encabezado
                {
                    var rowData = new Dictionary<string, string>();

                    rowData["UNOR"] = worksheet.Cells[row, unorCol].Text.Trim();
                    rowData["Nombre"] = worksheet.Cells[row, nombreCol].Text.Trim();
                    rowData["Albarán"] = worksheet.Cells[row, albaranCol].Text.Trim();

                    // Convertir Importe Total IVA a formato numérico con euros
                    var importeValue = worksheet.Cells[row, importeCol].Value;
                    if (importeValue is double || importeValue is decimal)
                        rowData["Importe Total (IVA)"] = Convert.ToDecimal(importeValue).ToString("C", new CultureInfo("es-ES"));
                    else
                        rowData["Importe Total (IVA)"] = importeValue?.ToString() ?? "0 €";

                    tableData.Add(rowData);
                }
            }

            return tableData;
        }

    }
}


