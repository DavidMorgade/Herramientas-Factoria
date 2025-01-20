using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace Herramientas_Factoria.Paginas.PDF.Unir

{
    /// <summary>
    /// Lógica de interacción para UnirPDF.xaml
    /// </summary>
    public partial class UnirPDF : Window
    {
        public ObservableCollection<string> PdfFiles = new ObservableCollection<string>();
        public UnirPDF()
        {
            InitializeComponent();
        }

        private void Button_Volver(object sender, RoutedEventArgs e)
        {
            // Volvemos a la pagina princial
            ManipularPDF manipularPDF = new ManipularPDF();
            this.CerrarVentanaYAbrir(manipularPDF);
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
                    PdfFiles.Add(file);
                    Console.WriteLine(file);
                }
                Unir_Seleccionados unir_Seleccionados = new Unir_Seleccionados(PdfFiles);
                this.CerrarVentanaYAbrir(unir_Seleccionados);
            }
            if (this.PdfFiles == null)
            {
                MessageBox.Show("No has seleccionado ningún fichero PDF", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


        }
        private void CerrarVentanaYAbrir(Window ventana)
        {
            this.Close();
            ventana.Show();
        }

    }
}
