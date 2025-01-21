using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using PdfDocument = PdfiumViewer.PdfDocument;

namespace Herramientas_Factoria.Paginas.PDF.Unir
{
    /// <summary>
    /// Lógica de interacción para Unir_Seleccionados.xaml
    /// </summary>
    public partial class Unir_Seleccionados : Window
    {
        public ObservableCollection<PdfFile> PdfFiles = new ObservableCollection<PdfFile>();
        public Unir_Seleccionados(ObservableCollection<string> pdfFilePaths)
        {
            InitializeComponent();
            foreach (var filePath in pdfFilePaths)
            {
                var firstPage = RenderPdfFirstPage(filePath);
                PdfFiles.Add(new PdfFile { FilePath = filePath, Thumbnail = firstPage });
            }
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
                    PdfFiles.Add(new PdfFile { FilePath = filename, Thumbnail = RenderPdfFirstPage(filename) });
                }
            }
        }
        private void RemovePdf_Click(object sender, MouseButtonEventArgs e)
        {
            // Ensure the event is not handled by other controls
            e.Handled = true;

            // Find the PdfFile associated with the clicked Border
            if (sender is Border border && border.DataContext is PdfFile pdfFile)
            {
                // Remove the PdfFile from the ObservableCollection
                PdfFiles.Remove(pdfFile);
            }
        }
        private void Button_GenerarPDF(object sender, RoutedEventArgs e)
        {
            //TODO: implementar la logica para generar pdf
            if (this.PdfFiles.Count <= 1)
            {
                MessageBox.Show("No has seleccionado mas de un fichero pdf para unir", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Title = "Guardar PDF";
            saveFileDialog.Filter = "PDF Files (*.pdf)|*.pdf|All files (*.*)|*.*";
            saveFileDialog.FileName = "PDF unido" + PdfFiles[0].FileName;

            if (saveFileDialog.ShowDialog() == true)
            {
                string folderOfTheFile = saveFileDialog.FileName;
                string[] filePathsToMerge = PdfFiles.Select(pdf => pdf.FilePath).ToArray();
                PdfMergerUtility.MergePdfFiles(filePathsToMerge, saveFileDialog.FileName);

                // Open the merged PDF with the default PDF viewer
                Process.Start(new ProcessStartInfo(saveFileDialog.FileName) { UseShellExecute = true });
            }

        }
        private void pdfListBox_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Move;
        }
        private void pdfListBox_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(PdfFile)))
            {
                PdfFile data = e.Data.GetData(typeof(PdfFile)) as PdfFile;
                var target = ((FrameworkElement)e.OriginalSource).DataContext as PdfFile;

                int oldIndex = PdfFiles.IndexOf(data);
                int newIndex = PdfFiles.IndexOf(target);

                if (oldIndex != -1 && newIndex != -1 && oldIndex != newIndex)
                {
                    PdfFiles.Move(oldIndex, newIndex);
                }
            }
        }
        private BitmapSource RenderPdfFirstPage(string pdfPath)
        {
            using (var document = PdfDocument.Load(pdfPath))
            {
                using (var image = document.Render(0, 300, 300, true))
                {
                    // Asegurarse de que la imagen sea un Bitmap
                    using (var bitmap = new Bitmap(image))
                    {
                        return ConvertBitmapToBitmapSource(bitmap);
                    }
                }
            }
        }
        private BitmapSource ConvertBitmapToBitmapSource(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }
    }
}
