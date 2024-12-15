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

namespace Herramientas_Factoria
{
    /// <summary>
    /// Lógica de interacción para Window2.xaml
    /// </summary>
    public partial class InvoiceDataWindow : Window
    {
        public InvoiceDataWindow  ()
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
    }
}
