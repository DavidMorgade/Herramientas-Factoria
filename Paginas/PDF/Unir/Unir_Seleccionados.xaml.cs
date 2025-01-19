using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Herramientas_Factoria.Paginas.PDF.Unir
{
    /// <summary>
    /// Lógica de interacción para Unir_Seleccionados.xaml
    /// </summary>
    public partial class Unir_Seleccionados : Window
    {
        public ObservableCollection<string> PdfFiles { get; set; }
        public Unir_Seleccionados(ObservableCollection<string> pdfFiles)
        {
            InitializeComponent();
            this.PdfFiles = pdfFiles;
            pdfListBox.ItemsSource = PdfFiles;
        }

        private void Button_Volver(object sender, RoutedEventArgs e)
        {
            // Volvemos a la pagina unir pdf
            UnirPDF unirPDF = new UnirPDF();
            unirPDF.Show();
            // Cerrar la ventana actual
            this.Close();
        }
        private void pdfListBox_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var listBoxItem = ItemsControl.ContainerFromElement(pdfListBox, e.OriginalSource as DependencyObject) as ListBoxItem;
            if (listBoxItem != null)
            {
                DragDrop.DoDragDrop(listBoxItem, listBoxItem.DataContext, DragDropEffects.Move);
            }
        }
        private void Button_AddPDF(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "PDF files (*.pdf)|*.pdf",
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filename in openFileDialog.FileNames)
                {
                    PdfFiles.Add(filename);
                }
            }
        }
        private void pdfListBox_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Move;
        }
        private void pdfListBox_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(string)))
            {
                string data = e.Data.GetData(typeof(string)) as string;
                var target = ((FrameworkElement)e.OriginalSource).DataContext as string;

                int oldIndex = PdfFiles.IndexOf(data);
                int newIndex = PdfFiles.IndexOf(target);

                if (oldIndex != -1 && newIndex != -1 && oldIndex != newIndex)
                {
                    PdfFiles.Move(oldIndex, newIndex);
                }
            }
        }

    }
}
