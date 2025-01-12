using iText.Kernel.Pdf;
using iText.Kernel.Utils;
using System;
using System.IO;

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
        // delete pdf files after merge
        foreach (string file in pdfFiles)
        {
            try
            {
                File.Delete(file);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting file {file}: {ex.Message}");
            }
        }


    }

}
