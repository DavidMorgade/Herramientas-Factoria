using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows;
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
            // Cargar el documento template
            string sourceFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Recursos", "CERTIFICADO SIN IVA.docx");

            Document document = new Document();
            document.LoadFromFile(sourceFilePath);
            // agregar la seccion restante
            Section lastSection = document.Sections[document.Sections.Count - 1];

            int rowsPerPage = 60; // Número de filas por página
            int currentRowCount = 0;

            Table table = CreateTable(lastSection); // Crear la primera tabla

            for (int rowIndex = 0; rowIndex < tableData.Count; rowIndex++)
            {
                var row = tableData[rowIndex];

                // Verificar si la fila contiene datos relevantes
                if (string.IsNullOrWhiteSpace(row["UNOR"]) ||
                    string.IsNullOrWhiteSpace(row["Nombre"]) ||
                    string.IsNullOrWhiteSpace(row["Albarán"]) ||
                    string.IsNullOrWhiteSpace(row["Importe Total (IVA)"]))
                {
                    continue; // Saltar filas vacías
                }

                // Añadir fila de datos
                TableRow newRow = table.AddRow();
                AppendFormattedText(newRow.Cells[0], row["UNOR"]);
                AppendFormattedText(newRow.Cells[1], row["Nombre"]);
                AppendFormattedText(newRow.Cells[2], row["Albarán"]);
                AppendFormattedText(newRow.Cells[3], row["Importe Total (IVA)"]);



                currentRowCount++;

                // Crear nueva tabla si se alcanza el límite de filas
                if (currentRowCount == rowsPerPage)
                {
                    AddSignatureSpace(lastSection); // Añadir espacio para la firma
                    lastSection.AddParagraph().AppendBreak(BreakType.PageBreak); // Salto de página
                    lastSection = document.AddSection(); // Nueva sección
                    table = CreateTable(lastSection); // Nueva tabla en la nueva sección
                    currentRowCount = 0; // Reiniciar el contador de filas
                }
            }

            // Guardar el documento como PDF
            document.SaveToFile(filePath, FileFormat.PDF);
        }
        catch (IOException ex)
        {
            MessageBox.Show($"Error al acceder al archivo: {ex.Message}", "Error de acceso", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    private static void AddEncabezadoCertificado(Section section, string numeroFactura, int dia, string mes, int año)
    {
        // Crear el texto formateado con los parámetros proporcionados
        string text = $"ANEXO. RESUMEN DE SUMINISTROS POR BUI CORRESPONDIENTE A LA FACTURA N° {numeroFactura} DE FECHA {dia} de {mes} de {año} EMITIDA POR LA EMPRESA UTE UCALSA-BAYPORT-BESS NACIONAL NIF U70914627.";

        // Crear un nuevo párrafo
        Paragraph paragraph = section.AddParagraph();

        // Añadir el texto al párrafo
        TextRange textRange = paragraph.AppendText(text);

        // Establecer el formato del texto
        textRange.CharacterFormat.FontName = "Arial";
        textRange.CharacterFormat.FontSize = 12;
        // espaciado entre lineas
        paragraph.Format.LineSpacing = 15;
        // margen abajo
        paragraph.Format.AfterSpacing = 20;

        // Centrar el párrafo
        paragraph.Format.HorizontalAlignment = HorizontalAlignment.Center;
    }

    // Crear una tabla con encabezados
    private static Table CreateTable(Section section)
    {
        Table table = section.AddTable(true);
        table.ResetCells(1, 4); // Encabezado con 4 columnas
        table.AutoFit(AutoFitBehaviorType.AutoFitToContents); // Ajustar la tabla al ancho de la página
                                                              // Calcular el ancho de la tabla como 4/5 del ancho de la página
        float pageWidth = section.PageSetup.PageSize.Width - section.PageSetup.Margins.Left - section.PageSetup.Margins.Right;
        float tableWidth = pageWidth * 4 / 5;

        // Establecer el ancho de la tabla fijo para que todas las tablas sean iguales
        table.PreferredWidth = new PreferredWidth(WidthType.Percentage, 100);

        // Centrar la tabla en la página
        table.TableFormat.HorizontalAlignment = RowAlignment.Center;


        // Centrar la tabla en la página
        table.TableFormat.HorizontalAlignment = RowAlignment.Center;
        string[] headers = { "UNOR", "Nombre", "Albarán", "Importe Total (IVA)" };
        for (int i = 0; i < headers.Length; i++)
        {
            TableCell cell = table.Rows[0].Cells[i];

            Paragraph p = cell.AddParagraph();
            p.Format.HorizontalAlignment = HorizontalAlignment.Center;

            TextRange headerText = p.AppendText(headers[i]);
            // Establecer el color de fondo del texto del encabezado
            headerText.CharacterFormat.HighlightColor = Color.LightBlue;
            headerText.CharacterFormat.Bold = true;
            headerText.CharacterFormat.FontName = "Calibri";
            headerText.CharacterFormat.FontSize = 12;
        }

        return table;
    }

    // Añadir texto formateado a una celda
    private static void AppendFormattedText(TableCell cell, string text)
    {
        Paragraph p = cell.AddParagraph();
        TextRange textRange = p.AppendText(text);
        textRange.CharacterFormat.FontName = "Calibri";
        textRange.CharacterFormat.FontSize = 8;
        p.Format.HorizontalAlignment = HorizontalAlignment.Center; // Centrar horizontalmente
    }

    // Añadir espacio para la firma
    private static void AddSignatureSpace(Section section)
    {
        Paragraph signatureSpace = section.AddParagraph();
        signatureSpace.AppendText(" "); // Añadir un espacio vacío para dejar el margen
        signatureSpace.Format.AfterSpacing = 50; // Espacio adicional para la firma
    }


}
