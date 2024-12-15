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
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Obtener los valores del usuario y contraseña
            string username = Username.Text;
            string password = Password.Password; // PasswordBox usa .Password para obtener el texto

            // Verificar credenciales
            if (username == "admin" && password == "admin")
            {
                // Crear y mostrar la nueva ventana
                InvoiceDataWindow invoiceWindow = new InvoiceDataWindow();
                invoiceWindow.Show();

                // Cerrar la ventana actual
                this.Close();
            }
            else
            {
                // Mostrar mensaje de error
                MessageBox.Show("Usuario o contraseña incorrectos", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
