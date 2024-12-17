using Spire.Doc;
using System;
using System.IO;
using System.Windows;

public class Factura
{
    public static void ReplaceFieldsInWord(string expediente, string importe, string nombreFactura, string fechaFactura, string filePath)
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
}
