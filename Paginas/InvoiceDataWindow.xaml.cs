using Herramientas_Factoria.ManipulateExcel;
using Herramientas_Factoria.Paginas;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
        public InvoiceDataWindow  ()
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
            if(this.nombreFactura == null || expediente == null || importe == null)
            {
                MessageBox.Show("No has seleccionado factura o la factura no es válida", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if(this.fechaFactura == null)
            {
                MessageBox.Show("No has seleccionado fecha de factura", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Factura.ReplaceFieldsInWord(expediente, importe, nombreFactura, fechaFactura);
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
                this.nombreFactura = data.NombreFactura;
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
