using Microsoft.Win32;
using System.Collections.Generic;
using System.Windows;

namespace Herramientas_Factoria.Paginas.PDF
{
    /// <summary>
    /// Lógica de interacción para UnirPDF.xaml
    /// </summary>
    public partial class UnirPDF : Window
    {
        private List<string> pdfFiles;
        public UnirPDF()
        {
            InitializeComponent();
        }

        private void Button_Volver(object sender, RoutedEventArgs e)
        {
            // Volvemos a la pagina princial
            ManipularPDF manipularPDF = new ManipularPDF();
            manipularPDF.Show();
            // Cerrar la ventana actual
            this.Close();
        }
        private void Button_Seleccionar(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "PDF files (*.pdf)|*.pdf",
                Title = "Select PDF files"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string file in openFileDialog.FileNames)
                {
                    pdfFiles.Add(file);
                }
            }
        }

        public List<string> GetPdfFiles()
        {
            return this.pdfFiles;
        }
    }
}
