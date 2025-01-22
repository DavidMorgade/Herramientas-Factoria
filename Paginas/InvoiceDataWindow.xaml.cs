using Herramientas_Factoria.ManipulateExcel;
using Herramientas_Factoria.Paginas;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Herramientas_Factoria
{
    /// <summary>
    /// Lógica de interacción para Window2.xaml
    /// </summary>
    public partial class InvoiceDataWindow : Window
    {
        private string nombreFactura;
        private string expediente;
        private string importe;
        private string importeActas;
        private string fechaFactura;
        private string excelFilePath;
        private string pdfFilePath;
        List<Dictionary<string, string>> tableData;
        public InvoiceDataWindow()
        {
            InitializeComponent();
            // Set the default value for the DatePicker to the current date
            DatePicker.SelectedDate = DateTime.Now;
        }

        private void Button_Volver(object sender, RoutedEventArgs e)
        {

            // Volvemos a la pagina princial
            Index index = new Index();
            index.Show();

            // Cerrar la ventana actual
            this.Close();

        }
        private async void Button_Generar(object sender, RoutedEventArgs e)
        {
            if (this.nombreFactura == null || expediente == null || importe == null)
            {
                MessageBox.Show("No has seleccionado factura o la factura no es válida", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (this.fechaFactura == null)
            {
                MessageBox.Show("No has seleccionado fecha de factura", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Guardar Certificados";
            saveFileDialog.Filter = "PDF Files (*.pdf)|*.pdf|Word Documents (*.docx)|*.docx|All files (*.*)|*.*";
            saveFileDialog.FileName = "CERTIFICADO GLOBAL " + this.nombreFactura.Substring(nombreFactura.Length - 3) + " JEFE";

            if (saveFileDialog.ShowDialog() == true)
            {
                ProgressBar.Visibility = Visibility.Visible;
                ProgressBar.IsIndeterminate = true;

                try
                {
                    string folderOfTheFile = Path.GetDirectoryName(saveFileDialog.FileName);
                    string filePath = saveFileDialog.FileName;
                    string sinIvaFilePath = saveFileDialog.FileName.Replace("GLOBAL", "SIN IVA");
                    string[] bothFiles = { filePath, sinIvaFilePath };
                    string[] pdfAndExcel = { pdfFilePath, excelFilePath };
                    bool? isChecked = GenerarSinIVA.IsChecked;

                    await Task.Run(() =>
                    {
                        Factura.GenerarCertificadoGlobal(expediente, importe, nombreFactura, fechaFactura, filePath);
                        if (isChecked == true)
                        {
                            Factura.GenerarCertificadoSinIVA(tableData, sinIvaFilePath, expediente, importe, importeActas, nombreFactura, fechaFactura);
                        }
                        this.CreateDirectoryAndSaveFiles(folderOfTheFile, this.nombreFactura, bothFiles);
                        this.CopyFilesToDirectory(Path.Combine(folderOfTheFile, this.nombreFactura), pdfAndExcel);
                    });
                    MessageBox.Show("Certificado generado correctamente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al generar el certificado: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    ProgressBar.Visibility = Visibility.Collapsed;
                    ProgressBar.IsIndeterminate = false;
                }
            }
        }
        private void CreateDirectoryAndSaveFiles(string directoryPath, string directoryName, params string[] filePaths)
        {
            try
            {
                // Combinamos la ruta con el nombre del directorio
                string fullDirectoryPath = Path.Combine(directoryPath, directoryName);

                // Crear la carpeta si no existe
                if (!Directory.Exists(fullDirectoryPath))
                {
                    Directory.CreateDirectory(fullDirectoryPath);
                    Directory.CreateDirectory(Path.Combine(fullDirectoryPath, "FIRMADOS"));
                    Console.WriteLine($"Directory created at {fullDirectoryPath}");
                }
                else
                {
                    foreach (string filePath in filePaths)
                    {
                        string fileName = Path.GetFileName(filePath);
                        File.Delete(filePath);
                    }
                    MessageBox.Show("La carpeta de la factura ya existe", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                // Copiar cada archivo a la nueva carpeta
                foreach (string filePath in filePaths)
                {
                    if (File.Exists(filePath))
                    {
                        string fileName = Path.GetFileName(filePath);
                        string destinationPath = Path.Combine(fullDirectoryPath, fileName);
                        File.Move(filePath, destinationPath);
                        Console.WriteLine($"File {fileName} copied to {destinationPath}");
                    }
                    else
                    {
                        Console.WriteLine($"File {filePath} does not exist and was not copied.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
        private void Button_Examinar_Click(object sender, RoutedEventArgs e)
        {
            // Crear un cuadro de diálogo para abrir archivos
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Todos los archivos (*.*)|*.*",
                Title = "Seleccionar archivos (Excel y PDF)",
                Multiselect = true // Permitir selección múltiple
            };

            // Mostrar el cuadro de diálogo
            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                // Obtener las rutas de los archivos seleccionados
                string[] filePaths = openFileDialog.FileNames;

                // Filtrar y asignar rutas
                foreach (var filePath in filePaths)
                {
                    if (filePath.EndsWith(".xls") || filePath.EndsWith(".xlsx"))
                    {
                        excelFilePath = filePath;
                    }
                    else if (filePath.EndsWith(".pdf"))
                    {
                        pdfFilePath = filePath;
                    }
                }

                // Verificar que ambos archivos fueron seleccionados
                if (excelFilePath != null && pdfFilePath != null)
                {

                    // Procesar el archivo Excel
                    var data = ExcelReader.ExtractExpedienteAndImporte(excelFilePath);
                    this.tableData = ExcelReader.ExtractTableColumns(excelFilePath);
                    this.nombreFactura = data.nombreFactura;
                    this.expediente = data.Expediente;
                    this.importe = data.Importe;
                    this.importeActas = data.ImporteActas;

                    // Mostrar la información del archivo Excel en los TextBlocks
                    ExpedienteFacturaTextBlock.Text = "Expediente: " + expediente;
                    NombreFacturaTextBlock.Text = "Factura: " + nombreFactura;
                    ImporteFacturaTextBlock.Text = "Importe Factura: " + importe;
                }
                else
                {
                    // Mostrar un mensaje de error si no se seleccionaron ambos archivos
                    MessageBox.Show("Por favor, seleccione un archivo Excel y un archivo PDF.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void FechaFactura_SelectedDateChanged(object e, SelectionChangedEventArgs args)
        {
            // Obtiene la fecha seleccionada del DatePicker
            if (DatePicker.SelectedDate.HasValue)
            {
                DateTime selectedDate = DatePicker.SelectedDate.Value;


                // Formatear la fecha como DD/MM/YYYY
                string formattedDate = selectedDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                this.fechaFactura = formattedDate;

                // Muestra la fecha en consola o úsala como necesites
            }
        }
        private void CopyFilesToDirectory(string targetDirectory, string[] filePaths)
        {
            // Verificar si la carpeta de destino existe, si no, crearla
            if (!Directory.Exists(targetDirectory))
            {
                Directory.CreateDirectory(targetDirectory);
            }

            // Iterar sobre cada archivo en el array de rutas de archivos
            foreach (string filePath in filePaths)
            {
                // Obtener el nombre del archivo
                string fileName = Path.GetFileName(filePath);

                // Crear la ruta completa del archivo de destino
                string destFilePath = Path.Combine(targetDirectory, fileName);

                try
                {
                    // Copiar el archivo a la nueva ubicación
                    File.Copy(filePath, destFilePath, true);
                    Console.WriteLine($"Archivo copiado: {filePath} a {destFilePath}");
                }
                catch (Exception ex)
                {
                    // Manejar cualquier error que ocurra durante la copia de archivos
                    Console.WriteLine($"Error al copiar el archivo {filePath}: {ex.Message}");
                }
            }
        }
    }
}
