using Spire.Doc;
using Spire.Doc.Documents;
using System;
using System.IO;
using System.Windows;
using System.Collections.Generic;
using Spire.Doc.Fields;
using System.Globalization;
using HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment;

public class Factura
{
    public static void GenerarCertificadoGlobal(string expediente, string importe, string nombreFactura, string fechaFactura, string filePath)
    {
        try
        {
            string sourceFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Recursos", "CERTIFICADO GLOBAL.docx");

            // Cargar el documento
            Document document = new Document();
            document.LoadFromFile(sourceFilePath);

            // Reemplazar los campos en el documento
            document.Replace("{{NombreFactura}}", nombreFactura, false, true);
            document.Replace("{{FechaFactura}}", fechaFactura, false, true);
            document.Replace("{{Expediente}}", expediente, false, true);
            document.Replace("{{ImporteFactura}}", importe, false, true);


            // Guardar el documento con los cambios
            document.SaveToFile(filePath, FileFormat.PDF);
        }
        catch (IOException ex)
        {
            MessageBox.Show($"Error accessing file: {ex.Message}", "File Access Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public static void GenerarCertificadoSinIVA(List<Dictionary<string, string>> tableData, string filePath)
    {
        try
        {
            // Crear un nuevo documento Word
            Document document = new Document();
            Section section = document.AddSection();

            // Añadir un título opcional al documento
            Paragraph title = section.AddParagraph();
            title.AppendText("Resumen de Datos");
            title.ApplyStyle(BuiltinStyle.Title);
            title.Format.HorizontalAlignment = HorizontalAlignment.Center;

            // Crear la tabla
            Table table = section.AddTable(true);
            table.ResetCells(tableData.Count + 1, 4); // +1 para la fila de encabezado y 4 columnas

            // --- Insertar el encabezado ---
            string[] headers = { "UNOR", "Nombre", "Albarán", "Importe Total (IVA)" };
            for (int i = 0; i < headers.Length; i++)
            {
                TableCell cell = table.Rows[0].Cells[i];
                Paragraph p = cell.AddParagraph();
                p.AppendText(headers[i]);
                p.Format.HorizontalAlignment = HorizontalAlignment.Center;
                p.ApplyStyle(BuiltinStyle.Heading3); // Aplicar estilo de encabezado
                Console.WriteLine($"Claves en la fila: {string.Join(", ", cell)}");
            }

            // --- Insertar los datos ---
            for (int rowIndex = 0; rowIndex < tableData.Count; rowIndex++)
            {
                var row = tableData[rowIndex];
                table.Rows[rowIndex + 1].Cells[0].AddParagraph().AppendText(row["UNOR"]);
                table.Rows[rowIndex + 1].Cells[1].AddParagraph().AppendText(row["Nombre"]);
                table.Rows[rowIndex + 1].Cells[2].AddParagraph().AppendText(row["Albarán"]);
                table.Rows[rowIndex + 1].Cells[3].AddParagraph().AppendText(row["Importe Total (IVA)"]);
            }
            foreach (TableRow row in table.Rows)
            {
                foreach (TableCell cell in row.Cells)
                {
                    foreach (Paragraph paragraph in cell.Paragraphs)
                    {
                        Console.WriteLine($"Contenido de la celda: {paragraph.Text}"); // Mostrar el texto de cada celda
                    }
                }
            }

            // Ajustar el ancho de la tabla
            table.AutoFit(AutoFitBehaviorType.AutoFitToContents);

            // Guardar el documento Word
            document.SaveToFile(filePath, FileFormat.PDF);
        }
        catch (IOException ex)
        {
            MessageBox.Show($"Error accessing file: {ex.Message}", "File Access Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
