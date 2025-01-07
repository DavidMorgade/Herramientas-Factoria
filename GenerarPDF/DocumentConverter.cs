using iText.Kernel.Pdf;
using iText.Kernel.Utils;

public class PdfMergerUtility
{
    public static void MergePdfFiles(string[] pdfFiles, string outputPdfPath)
    {
        using (PdfWriter writer = new PdfWriter(outputPdfPath))
        {
            using (PdfDocument pdfDocument = new PdfDocument(writer))
            {
                PdfMerger merger = new PdfMerger(pdfDocument);

                foreach (string file in pdfFiles)
                {
                    using (PdfReader reader = new PdfReader(file))
                    {
                        PdfDocument document = new PdfDocument(reader);
                        merger.Merge(document, 1, document.GetNumberOfPages());
                        document.Close();
                    }
                }
            }
        }
    }

}
