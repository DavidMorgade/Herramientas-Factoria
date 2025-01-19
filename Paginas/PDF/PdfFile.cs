using System.Windows.Media.Imaging;

public class PdfFile
{
    public string FilePath { get; set; }
    public string FileName => System.IO.Path.GetFileName(FilePath); // Extrae solo el nombre del archivo
    public BitmapSource Thumbnail { get; set; }
}
