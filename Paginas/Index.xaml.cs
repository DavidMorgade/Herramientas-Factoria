using Herramientas_Factoria.Paginas.PDF;
using Herramientas_Factoria.Paginas.Productos;
using System.Windows;

namespace Herramientas_Factoria.Paginas
{
    /// <summary>
    /// Lógica de interacción para Index.xaml
    /// </summary>
    public partial class Index : Window
    {
        public Index()
        {
            InitializeComponent();
        }

        private void Button_Volver(object sender, RoutedEventArgs e)
        {
            // Volvemos al login <provisional>
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            // Cerrar la ventana actual
            this.Close();
        }

        private void Button_Facturas(object sender, RoutedEventArgs e)
        {
            // Sacamos la ventana de Facturacion A.M
            InvoiceDataWindow invoiceDataWindow = new InvoiceDataWindow();
            invoiceDataWindow.Show();
            // Cerrar la ventana actual
            this.Close();
        }
        private void Button_PDF(object sender, RoutedEventArgs e)
        {
            // Sacamos la ventana de Facturacion A.M
            ManipularPDF manipularPDFWindow = new ManipularPDF();
            manipularPDFWindow.Show();
            // Cerrar la ventana actual
            this.Close();
        }

        private void Button_Productos(object sender, RoutedEventArgs e)
        {
            // Sacamos la ventana de Productos
            ProductosPagina productosWindow = new ProductosPagina();
            productosWindow.Show();
            // Cerrar la ventana actual
            this.Close();
        }

    }
}
