using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            // Volvemos al login <provisional>
            InvoiceDataWindow invoiceDataWindow = new InvoiceDataWindow();
            invoiceDataWindow.Show();
            // Cerrar la ventana actual
            this.Close();
        }

    }
}
