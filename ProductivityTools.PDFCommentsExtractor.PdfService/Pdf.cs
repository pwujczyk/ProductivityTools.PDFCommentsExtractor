using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Globalization;
using System.Reflection.PortableExecutable;
using System.Text;

namespace ProductivityTools.PDFCommentsExtractor.PdfService
{
    public class Pdf
    {
        void GetRectAnno()
        {
            string path = @"D:\Trash\x2.pdf";
            string appRootDir = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.FullName;

            string filePath = path;
            int pageFrom = 0;
            int pageTo = 0;

            try
            {
                using (PdfReader reader = new PdfReader(filePath))
                {
                    pageTo = reader.NumberOfPages;

                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {


                        PdfDictionary page = reader.GetPageN(i);
                        PdfArray annots = page.GetAsArray(iTextSharp.text.pdf.PdfName.ANNOTS);
                        if (annots != null)
                            foreach (PdfObject annot in annots.ArrayList)
                            {

                                //Get Annotation from PDF File
                                PdfDictionary annotationDic = (PdfDictionary)PdfReader.GetPdfObject(annot);
                                PdfName subType = (PdfName)annotationDic.Get(PdfName.SUBTYPE);
                                //check only subtype is highlight
                                if (subType.Equals(PdfName.HIGHLIGHT))
                                {
                                    // Get Quadpoints and Rectangle of highlighted text
                                    Console.Write("HighLight at Rectangle {0} with QuadPoints {1}\n", annotationDic.GetAsArray(PdfName.RECT), annotationDic.GetAsArray(PdfName.QUADPOINTS));

                                    //Extract Text using rectangle strategy    
                                    PdfArray coordinates = annotationDic.GetAsArray(PdfName.RECT);

                                    Rectangle rect = new Rectangle(float.Parse(coordinates.ArrayList[0].ToString(), CultureInfo.InvariantCulture.NumberFormat), float.Parse(coordinates.ArrayList[1].ToString(), CultureInfo.InvariantCulture.NumberFormat),
                                    float.Parse(coordinates.ArrayList[2].ToString(), CultureInfo.InvariantCulture.NumberFormat), float.Parse(coordinates.ArrayList[3].ToString(), CultureInfo.InvariantCulture.NumberFormat));



                                    RenderFilter[] filter = { new RegionTextRenderFilter(rect) };
                                    ITextExtractionStrategy strategy;
                                    StringBuilder sb = new StringBuilder();


                                    strategy = new FilteredTextRenderListener(new LocationTextExtractionStrategy(), filter);
                                    sb.AppendLine(PdfTextExtractor.GetTextFromPage(reader, i, strategy));

                                    //Show extract text on Console
                                    Console.WriteLine(sb.ToString());
                                    //Console.WriteLine("Page No" + i);

                                }



                            }



                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
