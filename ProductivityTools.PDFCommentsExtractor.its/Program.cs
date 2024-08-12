// See https://aka.ms/new-console-template for more information
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using static System.Net.Mime.MediaTypeNames;
using System.Text;
using System.util;
using System.Globalization;
using iTextSharp.text;



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
GetRectAnno();

static RectangleJ[] ToRectangle(PdfArray quadricles)
{
    var result = new List<RectangleJ>();
    if (quadricles == null) return null;
    for (var m = 0; m < quadricles.Size; m += 8)
    {
        var dimX = new List<float>();
        var dimY = new List<float>();

        for (var n = 0; n < 8; n += 2)
        {
            var x = quadricles[m + n] as PdfNumber;
            dimX.Add(x.FloatValue);
            var y = quadricles[m + n + 1] as PdfNumber;
            dimY.Add(y.FloatValue);
        }

        result.Add(new RectangleJ(dimX.Min(), dimY.Min(), dimX.Max(), dimY.Max()));
    }

    return result.ToArray();
}

Console.WriteLine("Hello, World!");
string path = @"D:\Trash\2024_33.pdf";

PdfReader reader = new PdfReader(path);
string text = string.Empty;
for (int i = 1; i <= 100; i++)
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

            RectangleJ rect = new RectangleJ(
                float.Parse(coordinates.ArrayList[0].ToString(), CultureInfo.InvariantCulture.NumberFormat),
                float.Parse(coordinates.ArrayList[1].ToString(), CultureInfo.InvariantCulture.NumberFormat),
                float.Parse(coordinates.ArrayList[2].ToString(), CultureInfo.InvariantCulture.NumberFormat), 
                float.Parse(coordinates.ArrayList[3].ToString(), CultureInfo.InvariantCulture.NumberFormat));

            RectangleJ[] rect1 = ToRectangle(annotation.GetAsArray(PdfName.QUADPOINTS));
            if (rect1 != null)
            {
                RenderFilter[] filter = { new RegionTextRenderFilter(rect1[0]) };
                ITextExtractionStrategy strategy = new FilteredTextRenderListener(new LocationTextExtractionStrategy(), filter);
                string currentText = PdfTextExtractor.GetTextFromPage(reader, i, strategy);


                currentText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(currentText)));
                text += currentText;
                // now use the String value of contents
                Console.WriteLine(contents);
            }
        }
}
