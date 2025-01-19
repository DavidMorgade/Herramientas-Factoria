using Herramientas_Factoria.Paginas.PDF.Unir;
using System.Windows;

namespace Herramientas_Factoria.Paginas.PDF
{
    /// <summary>
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class ManipularPDF : Window
    {
        public ManipularPDF()
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
        private void Button_Unir(object sender, RoutedEventArgs e)
        {
            // Vamos a la pagina unir pdf
            UnirPDF unirPDF = new UnirPDF();
            unirPDF.Show();

            // Cerrar la ventana actual
            this.Close();
        }
    }
}
