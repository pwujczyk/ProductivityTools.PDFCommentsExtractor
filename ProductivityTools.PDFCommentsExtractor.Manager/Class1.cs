

using PdfSharp.Pdf;
using PdfSharp.Pdf.Annotations;
using PdfSharp.Pdf.Content;
using PdfSharp.Pdf.Content.Objects;
using System.IO.Pipes;
using System.Reflection.Metadata;
using System.Xml.Linq;

namespace ProductivityTools.PDFCommentsExtractor.Manager
{
    public class Class1
    {
        public IEnumerable<string> ExtractText(CObject cObject)
        {
            if (cObject is COperator)
            {
                var cOperator = cObject as COperator;
                if (cOperator.OpCode.Name == OpCodeName.Tj.ToString() ||
                    cOperator.OpCode.Name == OpCodeName.TJ.ToString())
                {
                    foreach (var cOperand in cOperator.Operands)
                        foreach (var txt in ExtractText(cOperand))
                            yield return txt;
                }
            }
            else if (cObject is CSequence)
            {
                var cSequence = cObject as CSequence;
                foreach (var element in cSequence)
                    foreach (var txt in ExtractText(element))
                        yield return txt;
            }
            else if (cObject is CString)
            {
                var cString = cObject as CString;
                yield return cString.Value;
            }
        }

        public void Read()
        {
            //(
            string path = @"D:\Trash\X1.pdf";
            PdfSharp.Pdf.PdfDocument doc = PdfSharp.Pdf.IO.PdfReader.Open(path);
            for (int i = 0; i < doc.PageCount; i++)
            {
                var page = doc.Pages[i];
                var pagec = page.Contents;
                var annotation = page.Annotations;
                var content = ContentReader.ReadContent(page);
                var text = ExtractText(content);
                string result = string.Empty;
                foreach(var t in text)
                {
                    result += t;
                }

                for (int p = 0; p < doc.Pages[i].Annotations.Elements.Count; p++)
                {
                    PdfItem textAnnot = doc.Pages[i].Annotations.Elements.GetObject(p);
                    //add specific item in "Results View" to CommentList here......
                    Console.WriteLine("fdsa");
                }

                for (int j = 0; j < annotation.Count; j++)
                {
                    var x = annotation[j].Contents;
                    
                    Console.Write(annotation[j].ToString());
                }

            }

            for (int i = 0; i < doc.PageCount; i++)
            {
                var page = doc.Pages[i];

                for (int p = 0; p < doc.Pages[i].Annotations.Elements.Count; p++)
                {
                    PdfAnnotation textAnnot = doc.Pages[i].Annotations[p];

                    string content = textAnnot.Contents;
                    if (content != null)
                    {
                        Console.WriteLine("fdsa");                    }

                }
            }
        }
    }
}
