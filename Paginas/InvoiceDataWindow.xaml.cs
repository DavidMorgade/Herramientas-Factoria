using Herramientas_Factoria.ManipulateExcel;
using Herramientas_Factoria.Paginas;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace Herramientas_Factoria
{
    /// <summary>
    /// Lógica de interacción para Window2.xaml
    /// </summary>
    public partial class InvoiceDataWindow : Window
    {
        private string filePath;
        private string nombreFactura;
        private string expediente;
        private string importe;
        private string fechaFactura;
        List<Dictionary<string, string>> tableData;
        public InvoiceDataWindow()
        {
            InitializeComponent();
        }

        private void Button_Volver(object sender, RoutedEventArgs e)
        {

            // Volvemos a la pagina princial
            Index index = new Index();
            index.Show();

            // Cerrar la ventana actual
            this.Close();

        }
        private void Button_Generar(object sender, RoutedEventArgs e)
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
            saveFileDialog.FileName = "CERTIFICADO GLOBAL " + this.nombreFactura + ".docx"; // Nombre por defecto

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;
                string sinIvaFilePath = saveFileDialog.FileName.Replace("GLOBAL", " SIN IVA");
                Factura.GenerarCertificadoGlobal(expediente, importe, nombreFactura, fechaFactura, filePath);
                Factura.GenerarCertificadoSinIVA(tableData, sinIvaFilePath, expediente, importe, nombreFactura, fechaFactura);
                MessageBox.Show("Certificado generado correctamente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }
        private void Button_Examinar_Click(object sender, RoutedEventArgs e)
        {
            // Crear un cuadro de diálogo para abrir archivos
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Archivos de Excel (*.xls;*.xlsx)|*.xls;*.xlsx|Todos los archivos (*.*)|*.*",
                Title = "Seleccionar archivo Excel"
            };

            // Mostrar el cuadro de diálogo
            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                // Obtener la ruta del archivo seleccionado
                filePath = openFileDialog.FileName;
                // Mostrar la ruta en un TextBlock

                var data = ExcelReader.ExtractExpedienteAndImporte(filePath);
                this.tableData = ExcelReader.ExtractTableColumns(filePath);
                this.nombreFactura = data.nombreFactura;
                this.expediente = data.Expediente;
                this.importe = data.Importe;

                ExpedienteFacturaTextBlock.Text = "Expediente: " + expediente;
                NombreFacturaTextBlock.Text = "Factura: " + nombreFactura;
                ImporteFacturaTextBlock.Text = "Importe Factura: " + importe;
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
                MessageBox.Show($"Fecha seleccionada: {formattedDate}", "Información");
            }
        }
    }
}
