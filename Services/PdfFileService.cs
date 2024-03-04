using iText.Kernel.Pdf;
using iText.Forms;
using iText.Forms.Fields;
public class PdfFillService
{
    public void FillPdf(string inputFilePath, string outputFilePath, Dictionary<string, string> fieldValues)
    {
        using (PdfReader pdfReader = new PdfReader(inputFilePath))
        using (PdfWriter pdfWriter = new PdfWriter(outputFilePath))
        {
            using (PdfDocument pdfDocument = new PdfDocument(pdfReader, pdfWriter))
            {
                PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDocument, true);

                foreach (var field in fieldValues)
                {
                    form.GetField(field.Key)?.SetValue(field.Value);
                }
            }
        }
    }
}
