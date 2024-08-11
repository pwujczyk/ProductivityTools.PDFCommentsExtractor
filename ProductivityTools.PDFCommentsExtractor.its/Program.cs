// See https://aka.ms/new-console-template for more information
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using static System.Net.Mime.MediaTypeNames;
using System.Text;
using System.util;
using System.Globalization;

Console.WriteLine("Hello, World!");
string path = @"D:\Trash\X2.pdf";

PdfReader reader = new PdfReader(path);
string text = string.Empty;
for (int i = 1; i <= 1; i++)
{
    PdfDictionary page = reader.GetPageN(i);
    PdfArray annots = page.GetAsArray(iTextSharp.text.pdf.PdfName.ANNOTS);
    if (annots != null)
        foreach (PdfObject annot in annots.ArrayList)
        {
            
            PdfDictionary annotation = (PdfDictionary)PdfReader.GetPdfObject(annot);
            PdfString contents = annotation.GetAsString(PdfName.CONTENTS);
            //ITextExtractionStrategy= strategy = new SimpleTextExtractionStrategy();
            //RectangleJ rect = new System.util.RectangleJ(70, 80, 420, 500);
            PdfArray coordinates = annotation.GetAsArray(PdfName.RECT);

            RectangleJ rect = new RectangleJ(float.Parse(coordinates.ArrayList[0].ToString(), CultureInfo.InvariantCulture.NumberFormat), float.Parse(coordinates.ArrayList[1].ToString(), CultureInfo.InvariantCulture.NumberFormat),
                                        float.Parse(coordinates.ArrayList[2].ToString(), CultureInfo.InvariantCulture.NumberFormat), float.Parse(coordinates.ArrayList[3].ToString(), CultureInfo.InvariantCulture.NumberFormat));

            RenderFilter[] filter = { new RegionTextRenderFilter(rect) };
            ITextExtractionStrategy strategy = new FilteredTextRenderListener(new LocationTextExtractionStrategy(), filter);
            string currentText = PdfTextExtractor.GetTextFromPage(reader, i, strategy);


            currentText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(currentText)));
            text += currentText;
            // now use the String value of contents
            Console.WriteLine(contents);
        }
}
